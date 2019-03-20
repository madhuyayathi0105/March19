<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StudentNewApplicationReportNew.aspx.cs" Inherits="StudentNewApplicationReportNew" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Application</title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 1000px;
            width: 1000px;
        }
    </style>
    <style type="text/css">
        .ajax__myTab
        {
            text-align: center;
        }
        .ajax__myTab .ajax__tab_header
        {
            font-family: Book Antiqua;
            text-align: initial;
            font-size: 16px;
            font-weight: bold;
            color: White;
            border-left: solid 1px #666666;
            border-bottom: thin 1px #666666;
        }
        .ajax__myTab .ajax__tab_outer
        {
            border: 1px solid black;
            width: 220px;
            height: 35px;
            border-top: 3px solid transparent;
        }
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 4px;
            background-color: indigo;
            width: 275px;
            height: 35px;
        }
        
        .ajax__myTab .ajax__tab_tab
        {
            height: 22px;
            padding: 4px;
            margin: 0;
            text-align: center;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            border-top: 3px solid #00527D;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #A1C344;
            color: White;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
            background-color: #A1C344;
            cursor: pointer;
            color: White;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            border-top: 2px solid white;
            border-bottom: transparent;
            color: #B0E0E6;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #F36200;
            border-bottom: transparent;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
            background-color: #F36200;
            cursor: inherit;
            width: 160px;
        }
        .ajax__myTab .ajax__tab_body
        {
            border: 1.5px solid #F36200;
            padding: 6px;
            background-color: #EFEBEF;
        }
        .ajax__myTab .ajax__tab_disabled
        {
            color: #F1F1F1;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            //Added by Idhris 13-05-2016
            function checkApplicationDetailssave(caller) {
                var err = "";
                var eduleve = "";
                
                err += checkselectedval(document.getElementById("<%=ddl_courseone.ClientID %>"), "Choice 1 required");
                
                err += checkselectedval(document.getElementById("<%=ddl_seattype2.ClientID %>"), "Seat type required");
                err += checkEmpty(document.getElementById("<%=txt_appfromno.ClientID %>"), "Application No required");
                err += checkEmpty(document.getElementById("<%=txt_FName_app.ClientID %>"), "First Name required");

                //Age check
                var date = document.getElementById("<%=txt_dob.ClientID %>").value;
                if (date.trim().toUpperCase() == "" || date.trim().toUpperCase() == "00/00/0000") {
                    document.getElementById("<%=txt_age.ClientID %>").value = "";
                }
                err += checkEmpty(document.getElementById("<%=txt_age.ClientID %>"), "Age required");
                err += checkMandselectedval(document.getElementById("<%=ddlcountry.ClientID %>"), document.getElementById("<%=txt_othernationality.ClientID %>"), "Nationality required");
                err += checkreligValreq(document.getElementById("<%=ddlreligion.ClientID %>"), document.getElementById("<%=ddlsubreligion.ClientID %>"), document.getElementById("<%=txt_otherreligion.ClientID %>"), "Religion required");
                err += checkselectedval(document.getElementById("<%=ddlcoummunity.ClientID %>"), "Community required");
                err += checkEmpty(document.getElementById("<%=txt_parentsname.ClientID %>"), "Father Name required");
                err += checkEmpty(document.getElementById("<%=txt_mothername.ClientID %>"), "Mother Name required");
                err += checkEmpty(document.getElementById("<%=txt_caddress1.ClientID %>"), "Address 1 required");
                err += checkEmpty(document.getElementById("<%=txt_cpostelcode.ClientID %>"), "Pincode required");
                err += checkEmpty(document.getElementById("<%=txt_ccity.ClientID %>"), "City required");
                err += checkMandselectedval(document.getElementById("<%=ddlcstate.ClientID %>"), document.getElementById("<%=txt_cotherstate.ClientID %>"), "State required");
                err += checkMandselectedval(document.getElementById("<%=ddlccountry.ClientID %>"), document.getElementById("<%=txt_ccountry.ClientID %>"), "Country required");
                if (eduleve == "UG") {
                    var element = document.getElementById('ugtable');
                    if (element != null) {
                        err += checkMandselectedval(document.getElementById("<%=ddlexaminationug.ClientID %>"), document.getElementById("<%=txt_qulifyexam.ClientID %>"), "Qualified Exam required");
                        err += checkEmpty(document.getElementById("<%=txt_board.ClientID %>"), "Board required");
                        err += checkEmpty(document.getElementById("<%=txt_Name_of_school.ClientID %>"), "School Name required");
                    }
                }
                if (eduleve == "PG") {
                    var element = document.getElementById('pgtable');
                    if (element != null) {
                        err += checkEmpty(document.getElementById("<%=txt_allsubjectpercentage.ClientID %>"), "Percentage required");
                        err += checkEmpty(document.getElementById("<%=txt_majorsubjectpercentage.ClientID %>"), "Major Subjects % required");
                        err += checkEmpty(document.getElementById("<%=txt_majorandalliedsubjectpercentage.ClientID %>"), "Major/Allied/Ancillary % required");
                        err += checkselectedval(document.getElementById("<%=ddlexamination.ClientID %>"), " Qualifying Examination required");
                        err += checkselectedval(document.getElementById("<%=ddltypeofmajor.ClientID %>"), " Type Of Major required");
                        err += checkselectedval(document.getElementById("<%=ddltypeofsemester.ClientID %>"), " Type of Semester required");
                        err += checkselectedval(document.getElementById("<%=DropDownList1.ClientID %>"), "Pass Month required");
                        err += checkselectedval(document.getElementById("<%=DropDownList2.ClientID %>"), "Pass Year required");
                        err += checkEmpty(document.getElementById("<%=txt_major.ClientID %>"), "Major required");
                        err += checkEmpty(document.getElementById("<%=txt_mediumofstudy.ClientID %>"), "Medium Of UG required");
                        err += checkEmpty(document.getElementById("<%=txt_regnomarksheet.ClientID %>"), "Register No required");
                    }
                }
                else {
                    // err += checkForSameSubject();
                }
                //err += checkForcertificateno();
                if (caller.value == "Save") {
                }

                if (err.trim() != "") {
                    alert(err); return false;
                }
                else {
                    return true;
                }
            }
            function checkForcertificateno() {
                var subOk = false;
                var subid;
                var tbl = document.getElementById("<%=GV1.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                for (var i = 0; i < (tbl.rows.length - 1); i++) {
                    subid = document.getElementById('MainContent_TabContainer1_tabpanel2_GV1_txt_certificate_' + i.toString());
                    if (subid.value != "") {
                        subOk = true;
                    }
                    else {
                        subOk = false;
                    }
                }
                if (subOk == false) {
                    return "* - Certificate No Should Not Empty\n";
                } else {
                    return "";
                }
            }
            function checkMandselectedval(id, id2, msg) {
                var g = id.value;
                id.style.borderColor = '';
                var idtext = id.options[id.selectedIndex].innerHTML;
                if (idtext.trim().toUpperCase() == "SELECT") {
                    id.style.borderColor = 'Red';
                    return "* - " + msg + "\n";
                }
                else if (idtext.trim().toUpperCase() == "OTHERS") {
                    var subid = id2.value;
                    id2.style.borderColor = '';
                    if (subid.trim().toUpperCase() == "") {
                        id2.style.borderColor = 'Red';
                        return "* - " + msg + "\n";
                    }
                    else {
                        id.style.borderColor = 'White';
                        return "";
                    }
                }
                else {
                    id.style.borderColor = 'White';
                    return "";
                }
            }
            function checkselectedval(id, msg) {
                var g = id.value;
                id.style.borderColor = '';
                var idtext = id.options[id.selectedIndex].innerHTML;
                if (idtext.trim().toUpperCase() == "SELECT" || idtext.trim() == "" || idtext.trim() == "0" || idtext.trim() == "Month" || idtext.trim() == "Year") {
                    id.style.borderColor = 'Red';
                    return "* - " + msg + "\n";
                }
                else {
                    id.style.borderColor = 'White';
                    return "";
                }
            }
            function checkEmpty(id, msg) {
                var g = id.value;
                id.style.borderColor = '';
                if (g.trim() == "") {
                    id.style.borderColor = 'Red';
                    return "* - " + msg + "\n";
                }
                else {
                    id.style.borderColor = 'White';
                    return "";
                }
            }
            function checkreligValreq(id, id2, id3, msg) {
                var g = id.value;
                var value1 = id.options[id.selectedIndex].innerHTML;
                id.style.borderColor = '';
                if (value1.trim().toUpperCase() == "SELECT") {
                    id.style.borderColor = 'Red';
                    return "* - " + msg + "\n";
                }
                else if (value1.trim().toUpperCase() == "OTHERS") {
                    var subid = id3.value;
                    id3.style.borderColor = '';
                    if (subid.trim().toUpperCase() == "") {
                        id3.style.borderColor = 'Red';
                        return "* - " + msg + "\n";
                    }
                    else {
                        id.style.borderColor = 'White';
                        return "";
                    }
                }
                else if (value1.trim().toUpperCase() == "CHRISTIAN") {
                    //                var subid1 = id2.value;
                    var subid1 = id2.options[id2.selectedIndex].innerHTML;
                    id2.style.borderColor = '';
                    if (subid1.trim().toUpperCase() == "OTHERS") {
                        var subid = id3.value;
                        id3.style.borderColor = '';
                        if (subid.trim().toUpperCase() == "") {
                            id3.style.borderColor = 'Red';
                            return "* - " + msg + "\n";
                        }
                        else {
                            id.style.borderColor = 'White';
                            return "";
                        }
                    }
                    else if (subid1.trim().toUpperCase() == "SELECT") {
                        id2.style.borderColor = 'Red';
                        return "* - " + msg + "\n";
                    }
                    else {
                        id.style.borderColor = 'White';
                        return "";
                    }
                }
                else {
                    id.style.borderColor = 'White';
                    return "";
                }
            }
            function subrelig(id) {
                var course = id.value;
                if (course.trim().toUpperCase() == "OTHERS") {
                    var ug = document.getElementById('<%=txt_otherreligion.ClientID%>');
                    ug.value = "";
                    ug.style.display = "block";
                    ug.focus();
                }
                else {
                    var ug = document.getElementById('<%=txt_otherreligion.ClientID%>');
                    ug.style.display = "none";
                }
            }
         
            function checkForSameSubject() {
                //var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                // var gridViewControls = tbl.getElementsByTagName("input");
                // for (var i = 0; i < (tbl.rows.length - 1); i++) {
                //var ddl = document.getElementById('TabContainer1_tabpanel1_UgGrid_ddlugsubject_' + i.toString());
                // }
                var subOk = false;
                var ddl = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_ddlugsubject_0');
                var subid = ddl.options[ddl.selectedIndex].innerHTML.trim();
                if (subid == "Others") {
                    subid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_0').value.trim();
                }
                var txt = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_1');
                var subid1 = txt.value.trim();
                var ddl2 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_ddlugsubject_2');
                var subid2 = ddl2.options[ddl2.selectedIndex].innerHTML.trim();
                if (subid2 == "Others") {
                    subid2 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_2').value.trim();
                }
                var ddl3 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_ddlugsubject_3');
                var subid3 = ddl3.options[ddl3.selectedIndex].innerHTML.trim();
                if (subid3 == "Others") {
                    subid3 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_3').value.trim();
                }
                var ddl4 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_ddlugsubject_4');
                var subid4 = ddl4.options[ddl4.selectedIndex].innerHTML.trim();
                if (subid4 == "Others") {
                    subid4 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_4').value.trim();
                }
                var ddl5 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_ddlugsubject_5');
                var subid5 = ddl5.options[ddl5.selectedIndex].innerHTML.trim();
                if (subid5 == "Others") {
                    subid5 = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_subject56_5').value.trim();
                }
                if (subid != subid1 && subid != subid2 && subid != subid3 && subid != subid4 && subid != subid5 && subid1 != subid2 && subid1 != subid3 && subid1 != subid4 && subid1 != subid5 && subid2 != subid3 && subid2 != subid4 && subid2 != subid5 && subid3 != subid4 && subid3 != subid5 && subid4 != subid5 && subid1 != "---Select---" && subid2 != "---Select---" && subid3 != "---Select---" && subid4 != "---Select---" && subid5 != "---Select---") {
                    subOk = true;
                }
                if (subOk) {
                    return "";
                } else {
                    return "* - Subject Should Not Repeat\n";
                }
            }
            function Attemptsfill(id) {
                var row = id.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                if (rowIndex == 0) {
                    var regno = id.value;
                    if (id.value != "") {
                        var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                        var gridViewControls = tbl.getElementsByTagName("input");
                        for (var i = 0; i < (tbl.rows.length - 1); i++) {
                            var txtid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_ugnoofattmpts_' + i.toString());
                            txtid.value = regno;
                        }
                    }
                }
            }
            function Yearfill(id) {
                var row = id.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                if (rowIndex == 0) {
                    var regno = id.value;
                    if (id.value != "") {
                        var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                        var gridViewControls = tbl.getElementsByTagName("input");
                        for (var i = 0; i < (tbl.rows.length - 1); i++) {
                            var txtid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_ugyear_' + i.toString());
                            txtid.value = regno;
                        }
                    }
                }
            }
            function Monthfill(id) {
                var row = id.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                if (rowIndex == 0) {
                    var regno = id.value;
                    if (id.value != "") {
                        var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                        var gridViewControls = tbl.getElementsByTagName("input");
                        for (var i = 0; i < (tbl.rows.length - 1); i++) {
                            var txtid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_ugmonth_' + i.toString());
                            txtid.value = regno;
                        }
                    }
                }
            }
            function MaxMarksfill(id) {
                var row = id.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                if (rowIndex == 0) {
                    var regno = id.value;
                    if (id.value != "") {
                        var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                        var gridViewControls = tbl.getElementsByTagName("input");
                        for (var i = 0; i < (tbl.rows.length - 1); i++) {
                            var txtid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_rg_max_' + i.toString());
                            txtid.value = regno;
                        }
                    }
                }
            }
            function Regnofill(id) {
                var row = id.parentNode.parentNode;
                var rowIndex = row.rowIndex - 1;
                if (rowIndex == 0) {
                    var regno = id.value;
                    if (id.value != "") {
                        var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                        var gridViewControls = tbl.getElementsByTagName("input");
                        for (var i = 0; i < (tbl.rows.length - 1); i++) {
                            var txtid = document.getElementById('MainContent_TabContainer1_tabpanel1_UgGrid_txt_ugregno_' + i.toString());
                            txtid.value = regno;
                        }
                    }
                }
            }
            function validateCaseSensitiveEmail(email) {
                var reg = /^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;
                if (reg.test(email.value)) {
                }
                else {
                    email.value = "";
                }
            }
            function referChange() {
                var divstaff = document.getElementById("<%=divReferStaff.ClientID %>");
                var divOther = document.getElementById("<%=divReferOther.ClientID %>");
                var divstud = document.getElementById("<%=divReferStudent.ClientID %>");
                var cblid = document.getElementById("<%=rblReferredBy.ClientID %>");
                var options = cblid.getElementsByTagName('input');
                divstaff.style.display = "none";
                divOther.style.display = "none";
                divstud.style.display = "none";
                if (options[0].checked == true) {
                }
                else if (options[1].checked == true) {
                    divstaff.style.display = "block";
                    // txtsearch.focus();
                } else if (options[2].checked == true) {
                    divstud.style.display = "block";
                } else {
                    divOther.style.display = "block";
                }
            }
            function ddlRefStat() {
                var ddl = document.getElementById('<%=ddlRefState.ClientID%>').value;
                var txt = document.getElementById('<%=txtRefState.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlRefAgen() {
                var ddl = document.getElementById('<%=ddlRefAgent.ClientID%>').value;
                var txt = document.getElementById('<%=txtRefAgent.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlrefConsNam() {
                var ddl = document.getElementById('<%=ddlrefConsName.ClientID%>').value;
                var txt = document.getElementById('<%=txtrefConsName.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlBnkNam() {
                var ddl = document.getElementById('<%=ddlBnkName.ClientID%>').value;
                var txt = document.getElementById('<%=txtBnkName.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlBnkAccFo() {
                var ddl = document.getElementById('<%=ddlBnkAccFor.ClientID%>').value;
                var txt = document.getElementById('<%=txtBnkAccFor.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlAcaBra() {
                var ddl = document.getElementById('<%=ddlAcaBran.ClientID%>').value;
                var txt = document.getElementById('<%=txtAcaBran.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlAcaUni() {
                var ddl = document.getElementById('<%=ddlAcaUniv.ClientID%>').value;
                var txt = document.getElementById('<%=txtAcaUniv.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlAcaCours() {
                var ddl = document.getElementById('<%=ddlAcaCourse.ClientID%>').value;
                var txt = document.getElementById('<%=txtAcaCourse.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlFatherQua() {
                var ddl = document.getElementById('<%=ddlfaterquali.ClientID%>').value;
                var txt = document.getElementById('<%=txt_faterquali.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlMotherQua() {
                var ddl = document.getElementById('<%=ddlmotherqual.ClientID%>').value;
                var txt = document.getElementById('<%=txt_motherqual.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlPart1() {

                var ddl = "";
                var txt = "";
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlPart2() {
                var ddl = document.getElementById('<%=ddlPartLang2.ClientID%>').value;
                var txt = document.getElementById('<%=txtLangPart2.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function ddlPart3() {
                var ddl = document.getElementById('<%=ddlPartLang3.ClientID%>').value;
                var txt = document.getElementById('<%=txtLangPart3.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            //added by saranyadevi
            function ddlscholar() {
                var ddl = document.getElementById('<%=ddlscholarship.ClientID%>').value;
                var txt = document.getElementById('<%=txt_subscholar.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            //end by saranyadevi 23.2.2018
            function nation1change() {
                var ddl = document.getElementById('<%=ddlccountry.ClientID%>').value;
                var txt = document.getElementById('<%=txt_ccountry.ClientID%>');
                //txt.value = "";
                if (ddl.trim() == "Others") {
                    txt.style.display = "block";
                } else {
                    txt.style.display = "none";
                }
            }
            function licenseChange(rblVal) {
                //document.getElementById("<%=txtDrivLic.ClientID %>").value = "";
                var txtIns = document.getElementById("<%=txtDrivLic.ClientID %>");
                var cblid = document.getElementById("<%=rblDrivLic.ClientID %>");
                var options = cblid.getElementsByTagName('input');
                if (options[0].checked == true) {
                    txtIns.style.display = "block";
                }
                else {
                    txtIns.style.display = "none";
                }
            }
            function insuranceChange(rblVal) {
                var trow = document.getElementById("trInsurance");
                //document.getElementById("<%=txtInsureAmt.ClientID %>").value = "";
                //document.getElementById("<%=txtInsureBy.ClientID %>").value = "";
                //document.getElementById("<%=txtInsureNominee.ClientID %>").value = "";
                //document.getElementById("<%=txtInsureRelat.ClientID %>").value = "";
                var txtIns = document.getElementById("<%=txtInsureRelat.ClientID %>");
                var ddlIns = document.getElementById("<%=ddlInsureNominee.ClientID %>");
                var cblid = document.getElementById("<%=rblInsurance.ClientID %>");
                var options = cblid.getElementsByTagName('input');
                if (options[0].checked == true) {
                    document.getElementById("<%=txtInsureAmt.ClientID %>").style.display = "block";
                    document.getElementById("<%=txtInsureBy.ClientID %>").style.display = "block";
                    document.getElementById("<%=txtInsureNominee.ClientID %>").style.display = "block";
                    document.getElementById("<%=txtInsureRelat.ClientID %>").style.display = "block";
                    ddlIns.style.display = "block";
                }
                else {
                    document.getElementById("<%=txtInsureAmt.ClientID %>").style.display = "none";
                    document.getElementById("<%=txtInsureBy.ClientID %>").style.display = "none";
                    document.getElementById("<%=txtInsureNominee.ClientID %>").style.display = "none";
                    document.getElementById("<%=txtInsureRelat.ClientID %>").style.display = "none";
                    ddlIns.style.display = "none";
                    txtIns.style.display = "none";
                }
                if (ddlIns.value.trim() == "Others" && ddlIns.style.display == "block") {
                    txtIns.style.display = "block";
                } else {
                    txtIns.style.display = "none";
                }
            }
            function StfChildChange(rdbVal) {
                var cblid = document.getElementById("<%=rdbstfChild.ClientID %>");
                var options = cblid.getElementsByTagName('input');
                if (options[0].checked == true) {
                    document.getElementById("<%=tdStfDet.ClientID %>").style.display = "block";
                }
                else {
                    document.getElementById("<%=tdStfDet.ClientID %>").style.display = "none";
                }
            }
            function ddlInsureNomi(ddlval) {
                document.getElementById("<%=txtInsureRelat.ClientID %>").value = "";
                var txtIns = document.getElementById("<%=txtInsureRelat.ClientID %>");
                var ddlIns = document.getElementById("<%=ddlInsureNominee.ClientID %>");
                if (ddlIns.value.trim() == "Others") {
                    txtIns.style.display = "block";
                } else {
                    txtIns.style.display = "none";
                }
            }
            //Last modified by Idhris 25-05-2016
            function display(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function printdisp() {
                document.getElementById('<%=lbl_norec.ClientID%>').innerHTML = "";
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function blurFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function Changetotal(id) {
                if (parseFloat(id) > 1200) {
                    document.getElementById('<%=txtmark.ClientID%>').value = "";
                    document.getElementById('<%=txtmark.ClientID%>').style.borderColor = 'Red';
                }
                else {
                }
            }
            function othersport(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_sports.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_sports.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function otherextra(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_extraactivites.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_extraactivites.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function checkEmail(id) {
                var filter = /^([a-z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }
            function Save() {
                var id = "";
                var empty = true;
                //var year = document.getElementById('<%=lblyear.ClientID%>').value;
                var year = document.getElementById('<%=ddlBatch_year.ClientID%>').value;
                var clg = document.getElementById('<%=ddlclg.ClientID%>').value;
                var applno = document.getElementById('<%=txtapplno.ClientID%>').value;
                //            var course = document.getElementById('<%=ddlcourse.ClientID%>').value;
                var name = document.getElementById('<%=txtapplname.ClientID%>').value;
                var coursefirst = document.getElementById('<%=ddlchoice1.ClientID%>').value;
                var mblno = document.getElementById('<%=txtmblno.ClientID %>').value;
                var fname = document.getElementById('<%=txtfather.ClientID %>').value;
                if (year == "0") {
                    empty = false;
                }
                if (clg == "0") {
                    empty = false;
                }
                if (applno == "") {
                    applno = document.getElementById('<%=txtapplno.ClientID%>');
                    applno.style.borderColor = 'Red';
                    empty = false;
                }
                if (name == "") {
                    name = document.getElementById('<%=txtapplname.ClientID%>');
                    name.style.borderColor = 'Red';
                    empty = false;
                }
                if (fname == "") {
                    name = document.getElementById('<%=txtfather.ClientID%>');
                    name.style.borderColor = 'Red';
                    empty = false;
                }
                //            if (course == "0") {
                //                empty = false;
                //            }
                if (coursefirst == "0") {
                    empty = false;
                }
                if (mblno == "") {
                    mblno = document.getElementById('<%=txtmblno.ClientID %>');
                    mblno.style.borderColor = 'Red';
                    empty = false;
                }
                if (empty == false) {
                    return false;
                }
                else {
                    return confirm('Are Sure Want To Continue');
                    // return true;
                }
            }
            function getled(txt) {
                $.ajax({
                    type: "POST",
                    url: "StudentNewApplicationReportNew.aspx/checkApplno",
                    data: '{applno: "' + txt + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function Success(response) {
                var mesg1 = $("#lblerr")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "Not Exist";
                        break;
                    case "1":
                        mesg1.style.color = "Red";
                        document.getElementById('<%=txtapplno.ClientID %>').value = "";
                        mesg1.innerHTML = "Exist";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Enter Application No";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error Occurred";
                        break;
                }
            }
            function dateFormat() {
                var check = false;
                var txtpass = document.getElementById('<%=txtpassyr.ClientID%>').value;
                if (txtpass.length = "8") {
                    var value = txtpass.split(" ");
                    var mnth = value[0];
                    var year = value[1];
                    if (mnth.length == "3") {
                        check = true;
                    }
                    if (year == "4") {
                        check = true;
                    }
                    if (check == true) {
                        return true;
                    }
                    else {
                        document.getElementById('<%=txtpassyr.ClientID%>').value = "";
                        return false;
                    }
                }
                else {
                    document.getElementById('<%=txtpassyr.ClientID%>').value = "";
                }
            }
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
                var fvalue = "";
                var tovalue = "";
                fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
                toDate = document.getElementById('<%=txt_todate.ClientID%>').value;
                fvalue = document.getElementById('<%=txt_fromdate.ClientID %>');
                tovalue = document.getElementById('<%=txt_todate.ClientID %>');
                var today = new Date();
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);
                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
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
                    fvalue.value = currentDate;
                    tovalue.value = currentDate;
                    alert("To date should be greater than from date ");
                    return false;
                }
            }
            function educationDivShow(id) {
                var course = id.value;
                if (course.trim().toUpperCase() == "OTHERS") {
                    var ug = document.getElementById('<%=txtugcourse.ClientID%>');
                    ug.value = "";
                    ug.style.display = "block";
                    ug.focus();
                }
                else {
                    var ug = document.getElementById('<%=txtugcourse.ClientID%>');
                    ug.style.display = "none";
                }
            }
            //        /////9/5/2016
            function universityTxt(id) {
                var course = id.value;
                if (course.trim().toUpperCase() == "OTHERS") {
                    var uniname = document.getElementById('<%=txtuniname.ClientID%>');
                    uniname.value = "";
                    uniname.style.display = "block";
                    uniname.focus();
                }
                else {
                    var uniname = document.getElementById('<%=txtuniname.ClientID%>');
                    uniname.style.display = "none";
                }
            }
            function ValidateValue(idval) {
                if (idval != "" && idval != "0") {
                    if (parseFloat(idval) > 0 && parseFloat(idval) <= 100) {
                        var id = document.getElementById('<%=txtugper.ClientID%>');
                        id.value = parseFloat(idval).toFixed(1).toString();
                        return true;
                    }
                    else {
                        var id = document.getElementById('<%=txtugper.ClientID%>');
                        id.value = "";
                        id.style.borderColor = "red";
                        return false;
                    }
                }
            }
            function ValidateTancetMark(idval) {
                if (idval != "" && idval != "0") {
                    if (parseFloat(idval) > 0 && parseFloat(idval) <= 100) {
                        var id = document.getElementById('<%=txttanmark.ClientID%>');
                        id.value = parseFloat(idval).toFixed(1).toString();
                        return true;
                    }
                    else {
                        var id = document.getElementById('<%=txttanmark.ClientID%>');
                        id.value = "";
                        id.style.borderColor = "red";
                        return false;
                    }
                }
            }

            function FnautoAddressFill() {
                var ChkBxAutoFill = document.getElementById("<%=rdbsameyes.ClientID %>");

                var value = document.getElementById("<%=rdbsameno.ClientID %>");
                var value1 = document.getElementById("<%=communicationdiv.ClientID %>");
                if (ChkBxAutoFill.checked == true) {
                    document.getElementById("<%=txt_address1.ClientID %>").value = document.getElementById("<%=txt_caddress1.ClientID %>").value;
                    document.getElementById("<%=txt_address2.ClientID %>").value = document.getElementById("<%=txt_caddress2.ClientID %>").value;
                    document.getElementById("<%=txt_address3.ClientID %>").value = document.getElementById("<%=txt_caddress3.ClientID %>").value;
                    document.getElementById("<%=txt_potherstate.ClientID %>").value = document.getElementById("<%=txt_cotherstate.ClientID %>").value;
                    document.getElementById("<%=txt_postelcode.ClientID %>").value = document.getElementById("<%=txt_cpostelcode.ClientID %>").value;
                    document.getElementById("<%=txt_std.ClientID %>").value = document.getElementById("<%=txt_cstd.ClientID %>").value;
                    document.getElementById("<%=txt_phonenumber.ClientID %>").value = document.getElementById("<%=txt_cphonenumber.ClientID %>").value;
                    document.getElementById("<%=txt_Pcity.ClientID %>").value = document.getElementById("<%=txt_ccity.ClientID %>").value;

                    
                    if (document.getElementById("<%=ddlcstate.ClientID %>").options[document.getElementById("<%=ddlcstate.ClientID %>").selectedIndex].text == "Others") {
                        document.getElementById("<%=ddlpstate.ClientID %>").options[document.getElementById("<%=ddlpstate.ClientID %>").selectedIndex].text = document.getElementById("<%=ddlcstate.ClientID %>").options[document.getElementById("<%=ddlcstate.ClientID %>").selectedIndex].text;
                        document.getElementById("<%=txt_cotherstate.ClientID %>").style.display = "block";
                        document.getElementById("<%=txt_cotherstate.ClientID %>").value = document.getElementById("<%=txt_cotherstate.ClientID %>").value;
                    }
                    else {
                        document.getElementById("<%=ddlpstate.ClientID %>").options[document.getElementById("<%=ddlpstate.ClientID %>").selectedIndex].text = document.getElementById("<%=ddlcstate.ClientID %>").options[document.getElementById("<%=ddlcstate.ClientID %>").selectedIndex].text;
                        document.getElementById("<%=txt_potherstate.ClientID %>").style.display = "none";
                    }
                    document.getElementById("<%=ddlpcountry.ClientID %>").options[document.getElementById("<%=ddlpcountry.ClientID %>").selectedIndex].text = document.getElementById("<%=ddlccountry.ClientID %>").options[document.getElementById("<%=ddlccountry.ClientID %>").selectedIndex].text;
                }
            }
        
        </script>
        <script type="text/javascript">
            function txtSemChangePG(txt) {
                var value = 1;
                if (txt.value != null && txt.value != "" && txt.value != "0") {
                    value = txt.value.toString();
                } else {
                    txt.value = "1";
                }
            }
            function PgAvgMark() {
                var txtMark1 = document.getElementById("<%=txt_SecMrk1.ClientID %>");
                var txtMark2 = document.getElementById("<%=txt_SecMrk2.ClientID %>");
                var txtMark3 = document.getElementById("<%=txt_SecMrk3.ClientID %>");
                var txtMark4 = document.getElementById("<%=txt_SecMrk4.ClientID %>");
                var txtMark5 = document.getElementById("<%=txt_SecMrk5.ClientID %>");
                var txtMax1 = document.getElementById("<%=txt_maxmark1.ClientID %>");
                var txtMax2 = document.getElementById("<%=txt_maxmark2.ClientID %>");
                var txtMax3 = document.getElementById("<%=txt_maxmark3.ClientID %>");
                var txtMax4 = document.getElementById("<%=txt_maxmark4.ClientID %>");
                var txtMax5 = document.getElementById("<%=txt_maxmark5.ClientID %>");
                var txtAvg1 = document.getElementById("<%=txt_avg1.ClientID %>");
                var txtAvg2 = document.getElementById("<%=txt_avg2.ClientID %>");
                var txtAvg3 = document.getElementById("<%=txt_avg3.ClientID %>");
                var txtAvg4 = document.getElementById("<%=txt_avg4.ClientID %>");
                var txtAvg5 = document.getElementById("<%=txt_avg5.ClientID %>");
                var ovrAvg = 0;
                //            if (txtMark1.value == "") {
                //                txtMark1.value = "0";
                //            }
                //            if (txtMark2.value == "") {
                //                txtMark2.value = "0";
                //            }
                //            if (txtMark3.value == "") {
                //                txtMark3.value = "0";
                //            }
                //            if (txtMark4.value == "") {
                //                txtMark4.value = "0";
                //            }
                //            if (txtMark5.value == "") {
                //                txtMark5.value = "0";
                //            }
                //            if (txtMax1.value == "") {
                //                txtMax1.value = "0";
                //            }
                //            if (txtMax2.value == "") {
                //                txtMax2.value = "0";
                //            }
                //            if (txtMax3.value == "") {
                //                txtMax3.value = "0";
                //            }
                //            if (txtMark4.value == "") {
                //                txtMark4.value = "0";
                //            }
                //            if (txtMax5.value == "") {
                //                txtMax5.value = "0";
                //            }
                //            if (txtAvg1.value == "") {
                //                txtAvg1.value = "0";
                //            }
                //            if (txtAvg2.value == "") {
                //                txtAvg2.value = "0";
                //            }
                //            if (txtAvg3.value == "") {
                //                txtAvg3.value = "0";
                //            }
                //            if (txtAvg4.value == "") {
                //                txtAvg4.value = "0";
                //            }
                //            if (txtAvg5.value == "") {
                //                txtAvg5.value = "0";
                //            }
                txtAvg1.value = ((parseInt(txtMark1.value) / parseInt(txtMax1.value)) * 100).toString();
                ovrAvg = ovrAvg + parseInt(txtMark1.value);
                txtMark1.value = (parseInt(txtMark1.value));
                txtMark2.value = (parseInt(txtMark2.value));
                txtMark3.value = (parseInt(txtMark3.value));
                txtMark4.value = (parseInt(txtMark4.value));
                txtMark5.value = (parseInt(txtMark5.value));
                txtMax1.value = (parseInt(txtMax1.value));
                txtMax2.value = (parseInt(txtMax2.value));
                txtMax3.value = (parseInt(txtMax3.value));
                txtMax4.value = (parseInt(txtMax4.value));
                txtMax5.value = (parseInt(txtMax5.value));
                txtAvg2.value = ((parseInt(txtMark2.value) / parseInt(txtMax2.value)) * 100).toString();
                ovrAvg = ovrAvg + parseInt(txtMark2.value);
                txtAvg3.value = ((parseInt(txtMark3.value) / parseInt(txtMax3.value)) * 100).toString();
                ovrAvg = ovrAvg + parseInt(txtMark3.value);
                txtAvg4.value = ((parseInt(txtMark4.value) / parseInt(txtMax4.value)) * 100).toString();
                ovrAvg = ovrAvg + parseInt(txtMark4.value);
                txtAvg5.value = ((parseInt(txtMark5.value) / parseInt(txtMax5.value)) * 100).toString();
                ovrAvg = ovrAvg + parseInt(txtMark5.value);
                ovrAvg = ovrAvg / 5;
            }
            function univ(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_univ.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_univ.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function onreli(id) {
                var value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_otherreligion.ClientID %>");
                    idvalue.style.display = "block";
                    idvalue.value = "Other Religion";
                    idvalue.style.color = "Gray";
                    var idvalue1 = document.getElementById("<%=ddlsubreligion.ClientID %>");
                    idvalue1.style.display = "none";
                }
                else if (value1.trim().toUpperCase() == "CHRISTIAN") {
                    var idvalue2 = document.getElementById("<%=txt_otherreligion.ClientID %>");
                    idvalue2.style.display = "none";
                    var idvalue3 = document.getElementById("<%=ddlsubreligion.ClientID %>");
                    idvalue3.style.display = "block";
                }
                else {
                    var idvalue1 = document.getElementById("<%=ddlsubreligion.ClientID %>");
                    idvalue1.style.display = "none";
                    var idvalue = document.getElementById("<%=txt_otherreligion.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
          
            function agecal(id) {
                //05.06.17 barath
                var date = document.getElementById("<%=txt_dob.ClientID %>").value;
                var date_txt = document.getElementById("<%=txt_dob.ClientID %>");
                var age = document.getElementById("<%=txt_age.ClientID %>");
                age.value = "";
                var valid = true;
                var month = parseInt(date.substring(3, 5), 10);
                var day = parseInt(date.substring(0, 2), 10);
                var year = parseInt(date.substring(6, 10), 10);
                if (isNaN(month) || isNaN(day) || isNaN(year)) { date_txt.style.borderColor = "red"; age.style.borderColor = "red"; return false; }
                if ((month < 1) || (month > 12)) valid = false;
                else if ((day < 1) || (day > 31)) valid = false;
                else if (((month == 4) || (month == 6) || (month == 9) || (month == 11)) && (day > 30)) valid = false;
                else if ((month == 2) && (((year % 400) == 0) || ((year % 4) == 0)) && ((year % 100) != 0) && (day > 29)) valid = false;
                else if ((month == 2) && ((year % 100) == 0) && (day > 29)) valid = false;
                else if ((month == 2) && (day > 28)) valid = false;
                if (valid == true) {
                    if ((date.trim() != "" || date.trim() != "00/00/0000") && date.length == 10) {
                        var dd = date.substring(0, 2);
                        var mm = date.substring(3, 5);
                        var yy = date.substring(6, 10);
                        if (yy != null) {
                            var today = new Date();
                            var yyyy = today.getFullYear();
                            var today1 = yyyy - yy;
                            age.value = (today1 == 0) ? '' : today1;
                        }
                    } else { date_txt.style.borderColor = "red"; age.style.borderColor = "red"; }
                    return true;
                }
                else {
                    date_txt.style.borderColor = "red"; age.style.borderColor = "red";
                    return false;
                }  
            }
            function otherable(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_otherdifferentlyable.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_otherdifferentlyable.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function abled1(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=ddldiferentltyable.ClientID %>");
                    idvalue.style.display = "none";
                    var idvalue1 = document.getElementById("<%=txt_otherdifferentlyable.ClientID %>");
                    idvalue1.style.display = "none";
                }
            }
            function otherable1(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_otherbloodgroup.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_otherbloodgroup.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function abled(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=ddldiferentltyable.ClientID %>");
                    idvalue.style.display = "block";
                }
            }
            function radiochange(valId) {
                var rdbexy = document.getElementById("<%=rdbexy.ClientID %>");
                var idval = document.getElementById("<%=txtExservNum.ClientID %>");
                var idval1 = document.getElementById("<%=txtExservPlace.ClientID %>");
                var idval2 = document.getElementById("<%=txtExservRank.ClientID %>");
                //idval.value = "";
                //idval1.value = "";
                //idval2.value = "";
                idval.style.display = "none";
                idval1.style.display = "none";
                idval2.style.display = "none";
                if (rdbexy.checked == true) {
                    idval.style.display = "block";
                    idval1.style.display = "block";
                    idval2.style.display = "block";
                }
            }
            function extra_fun(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=DropDownList4.ClientID %>");
                    idvalue.style.display = "block";
                    var idvalue1 = document.getElementById("<%=txt_specifyNcc.ClientID %>");
                    idvalue1.style.display = "block";
                }
            }
            function extra_fun1(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=DropDownList4.ClientID %>");
                    idvalue.style.display = "none";
                    var idvalue1 = document.getElementById("<%=txt_extraactivites.ClientID %>");
                    idvalue1.style.display = "none";
                    var idvalue2 = document.getElementById("<%=txt_specifyNcc.ClientID %>");
                    idvalue2.style.display = "none";
                }
            }
            function sport_fun(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=ddldistinctionsports.ClientID %>");
                    idvalue.style.display = "block";
                    var idvalue1 = document.getElementById("<%=txt_specify.ClientID %>");
                    idvalue1.style.display = "block";
                }
            }
            function sport1_fun(id) {
                if (id.checked == true) {
                    var idvalue = document.getElementById("<%=ddldistinctionsports.ClientID %>");
                    idvalue.style.display = "none";
                    var idvalue1 = document.getElementById("<%=txt_sports.ClientID %>");
                    idvalue1.style.display = "none";
                    var idvalue2 = document.getElementById("<%=txt_specify.ClientID %>");
                    idvalue2.style.display = "none";
                }
            }
            function chekpassvalue() {
                //alert('jai');
                var value = document.getElementById("<%=txt_percentage.ClientID %>");
                if (parseFloat(value) >= 35) {
                    return true;
                }
                else {
                    return false;
                }
            }
            function DisableBackButton() {
                window.history.forward()
            }
            DisableBackButton();
            window.onload = DisableBackButton;
            window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
            window.onload = function () { void (0) }
            function checkShortcut() {
                if (event.keyCode == 8 || event.keyCode == 13) {
                    return false;
                }
            }
            function checkEmail(id) {
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    alert('Please Enter Valid email address');
                    email.focus;
                    return false;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }
            function change(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_potherstate.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_potherstate.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function change1(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_cotherstate.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_cotherstate.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function AddNewRecord() {
                alert('jairam');
                var grd = document.getElementById('<%=Marksgrid.ClientID%>');
                if (grid.rows.length > 0) {
                }
            }
            function minmax(value, min, max) {
                var empty = '';
                if (parseInt(value) < min) {
                    return empty;
                }
                else if (parseInt(value) > max) {
                    alert('Please Enter the Mark Lesserthan or Equal ' + max + '');
                    return empty;
                }
                else {
                    return value;
                }
            }
            function checkmonth(value) {
                var txt = value.value;
                var temp = "true";
                var empty = "";
                if (txt != "") {
                    var month = new Array();
                    month[0] = "JAN";
                    month[1] = "FEB";
                    month[2] = "MAR";
                    month[3] = "APR";
                    month[4] = "MAY";
                    month[5] = "JUN";
                    month[6] = "JUL";
                    month[7] = "AUG";
                    month[8] = "SEP";
                    month[9] = "OCT";
                    month[10] = "NOV";
                    month[11] = "DEC";
                    for (i = 0; i < month.length; i++) {
                        if (txt.trim().toUpperCase() == month[i]) {
                            temp = false;
                        }
                    }
                    if (temp == "true") {
                        value.style.borderColor = 'Red';
                        return empty;
                    }
                    else {
                        return txt;
                    }
                }
                else {
                    return empty
                }
            }
            function checkyear(value) {
                var txt = value.value;
                var temp = "true";
                var empty = "";
                if (txt != "") {
                    var month = new Array();
                    var d = new Date();
                    var n = d.getFullYear();
                    for (i = n; i > n - 20; i--) {
                        month.push(i);
                    }
                    if (month.length > 0) {
                        for (j = 0; j < month.length; j++) {
                            if (txt == month[j]) {
                                temp = "false";
                            }
                        }
                    }
                    if (temp == "true") {
                        value.style.borderColor = 'Red';
                        return empty;
                    }
                    else {
                        return txt;
                    }
                }
                else {
                    return empty
                }
            }
            function oncomm(id) {
                var value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS" || value1.trim().toUpperCase() == "OTHER") {
                    var idvalue = document.getElementById("<%=txtCommunity.ClientID %>");
                    idvalue.style.display = "block";
                    idvalue.value = "Other Community";
                    idvalue.style.color = "Gray";
                }
                else {
                    var idvalue = document.getElementById("<%=txtCommunity.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function funation(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_othernationality.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_othernationality.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function occupat(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_occupation.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_occupation.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function occupat1(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_motheroccupation.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_motheroccupation.ClientID %>");
                    idvalue.style.display = "none";
                }
            }

            function Quata(id) {
                var Quata = id.value;

                if (Quata.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_quata.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_quata.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function noshow(id) {
                var idvalue = document.getElementById("<%=communicationdiv.ClientID %>");
                idvalue.style.display = "block";
                document.getElementById("<%=txt_address1.ClientID %>").value = "";
                document.getElementById("<%=txt_address2.ClientID %>").value = "";
                document.getElementById("<%=txt_address3.ClientID %>").value = "";
                document.getElementById("<%=txt_cotherstate.ClientID %>").value = "";
                document.getElementById("<%=txt_postelcode.ClientID %>").value = "";
                document.getElementById("<%=txt_std.ClientID %>").value = "";
                document.getElementById("<%=txt_phonenumber.ClientID %>").value = "";
                document.getElementById("<%=txt_Pcity.ClientID %>").value = "";
                document.getElementById("<%=ddlpstate.ClientID %>").options[document.getElementById("<%=ddlpstate.ClientID %>").selectedIndex].text = "Select";
                document.getElementById("<%=txt_ccountry.ClientID %>").value = "";
            }
            function showid(id) {
                var idvalue = document.getElementById("<%=communicationdiv.ClientID %>");
                idvalue.style.display = "block";

                FnautoAddressFill();



            }
            
            function meidum(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_mediumstudy.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_mediumstudy.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function meidum1(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_ddlmediumofStudyXII.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_ddlmediumofStudyXII.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function addinternalexternal(id) {
                var finalvalue = 0;
                var inter = 0;
                var exter = 0;
                var tbl = document.getElementById("<%=GridView_mphil.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var len = tbl.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    var txtid = document.getElementById('GridView_mphil_txt_marks_' + i.toString());
                    inter = document.getElementById('GridView_mphil_txt_inter_' + i.toString());
                    exter = document.getElementById('GridView_mphil_txt_exter_' + i.toString());
                    if (inter.value != "" && exter.value != "") {
                        finalvalue = parseInt(exter.value) + parseInt(inter.value);
                    }
                    else if (inter.value != "" && exter.value == "") {
                        finalvalue = parseInt(inter.value);
                    }
                    else {
                        finalvalue = parseInt(exter.value);
                    }
                    txtid.value = finalvalue;
                }
            }
            function addavgmphil(id) {
                var addvalue = id.value;
                var finalvalue = 0;
                var totalvalue = 0;
                var checkvalue = 0;
                var maxcheckvalue = 0;
                var count = 0;
                var avg = 0;
                var ch = "s";
                var em = "";
                var tbl = document.getElementById("<%=GridView_mphil.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var len = tbl.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_marks") > 1) {
                        if (gridViewControls[i].value != "") {
                            checkvalue = parseInt(gridViewControls[i].value);
                            finalvalue = parseInt(finalvalue) + parseInt(gridViewControls[i].value);
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_maximum_marks") > 1) {
                        if (gridViewControls[i].value != "") {
                            maxcheckvalue = parseInt(gridViewControls[i].value);
                            totalvalue = parseInt(totalvalue) + parseInt(gridViewControls[i].value);
                            var count = count + 1;
                        }
                    }
                    if (maxcheckvalue != "") {
                        if (checkvalue > maxcheckvalue) {
                            ch = "N";
                            if (gridViewControls[i].name.indexOf("txt_maximum_marks") > 1) {
                                if (gridViewControls[i].value != "") {
                                    gridViewControls[i].value = em;
                                }
                            }
                        }
                    }
                }
                if (ch == "N") {
                    alert('Maxmimum Mark less Then of actual Mark. Please Enter Correct Mark');
                }
            }
            function addmarks(id) {
                var addvalue = id.value;
                var finalvalue = 0;
                var totalvalue = 0;
                var maxtotalvalue = 0;
                var avg = 0;
                var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var len = tbl.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_ugmagrs") > 1) {
                        if (gridViewControls[i].value != "") {
                            finalvalue = parseInt(finalvalue) + parseInt(gridViewControls[i].value);
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                        if (gridViewControls[i].value != "") {
                            totalvalue = parseInt(totalvalue) + parseInt(gridViewControls[i].value);
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                        if (gridViewControls[i].value != "") {
                            maxtotalvalue = parseInt(maxtotalvalue) + parseInt(gridViewControls[i].value);
                        }
                    }
                }
                var second = document.getElementById("<%=ugsubgrid.ClientID %>");
                if (second != null) {
                    var secondlen = second.rows.length;
                    if (secondlen > 0) {
                        var lengthvalue = second.getElementsByTagName("input");
                        for (var j = 0; j < lengthvalue.length; j++) {
                            if (lengthvalue[j].name.indexOf("txt_subugmagrs") > 1) {
                                if (lengthvalue[j].value != "") {
                                    finalvalue = parseInt(finalvalue) + parseInt(lengthvalue[j].value);
                                }
                            }
                            if (lengthvalue[j].name.indexOf("txt_subug_max") > 1) {
                                if (lengthvalue[j].value != "") {
                                    totalvalue = parseInt(totalvalue) + parseInt(lengthvalue[j].value);
                                }
                            }
                            if (lengthvalue[j].name.indexOf("txt_subug_max") > 1) {
                                if (lengthvalue[j].value != "") {
                                    maxtotalvalue = parseInt(maxtotalvalue) + parseInt(lengthvalue[j].value);
                                }
                            }
                        }
                    }
                }
                document.getElementById("<%=txt_totalmark.ClientID %>").value = finalvalue;
                document.getElementById("<%=avgtext.ClientID %>").value = totalvalue;
                if (maxtotalvalue != 0) {
                    avg = finalvalue / maxtotalvalue * 100;
                    var check = avg % 1;
                    if (check == 0) {
                        document.getElementById("<%=txt_percentage.ClientID %>").value = avg;
                    }
                    else {
                        var n = check.toString();
                        var n1 = n.length;
                        if (n1 > 3) {
                            document.getElementById("<%=txt_percentage.ClientID %>").value = avg.toFixed(3);
                        }
                        else {
                            document.getElementById("<%=txt_percentage.ClientID %>").value = avg;
                        }
                    }
                }
            }
            function addavg(id) {
                MaxMarksfill(id);
                var addvalue = id.value;
                var finalvalue = 0;
                var totalvalue = 0;
                var checkvalue = 0;
                var maxcheckvalue = 0;
                var count = 0;
                var avg = 0;
                var ch = "s";
                var em = "";
                var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var len = tbl.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_ugmagrs") > 1) {
                        if (gridViewControls[i].value != "") {
                            checkvalue = parseInt(gridViewControls[i].value);
                            finalvalue = parseInt(finalvalue) + parseInt(gridViewControls[i].value);
                        }
                    }
                    if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                        if (gridViewControls[i].value != "") {
                            maxcheckvalue = parseInt(gridViewControls[i].value);
                            totalvalue = parseInt(totalvalue) + parseInt(gridViewControls[i].value);
                            var count = count + 1;
                        }
                    }
                    if (maxcheckvalue != "") {
                        if (checkvalue > maxcheckvalue) {
                            ch = "N";
                            if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                                if (gridViewControls[i].value != "") {
                                    gridViewControls[i].value = em;
                                }
                            }
                        }
                    }
                }
                var second = document.getElementById("<%=ugsubgrid.ClientID %>");
                if (second != null) {
                    var secondlen = second.rows.length;
                    if (secondlen > 0) {
                        var lengthvalue = second.getElementsByTagName("input");
                        for (var j = 0; j < lengthvalue.length; j++) {
                            if (lengthvalue[j].name.indexOf("txt_subugmagrs") > 1) {
                                if (lengthvalue[j].value != "") {
                                    finalvalue = parseInt(finalvalue) + parseInt(lengthvalue[j].value);
                                }
                            }
                            if (lengthvalue[j].name.indexOf("txt_subug_max") > 1) {
                                if (lengthvalue[j].value != "") {
                                    totalvalue = parseInt(totalvalue) + parseInt(lengthvalue[j].value);
                                }
                            }
                        }
                    }
                }
                if (ch == "N") {
                    alert('Maxmimum Mark less Then of actual Mark. Please Enter Correct Mark');
                }
                else {
                    if (totalvalue > 0) {
                        avg = finalvalue / totalvalue * 100;
                    }
                    document.getElementById("<%=txt_totalmark.ClientID %>").value = finalvalue;
                    document.getElementById("<%=avgtext.ClientID %>").value = totalvalue;
                    // document.getElementById("<%=txt_percentage.ClientID %>").value = Math.round(avg);
                    var check = avg % 1;
                    if (check == 0) {
                        document.getElementById("<%=txt_percentage.ClientID %>").value = avg;
                    }
                    else {
                        var n = check.toString();
                        var n1 = n.length;
                        if (n1 > 3) {
                            document.getElementById("<%=txt_percentage.ClientID %>").value = avg.toFixed(3);
                        }
                        else {
                            document.getElementById("<%=txt_percentage.ClientID %>").value = avg;
                        }
                    }
                }
            }
            function qualifyexam(id) {
                var dateObj = new Date();
                var month = dateObj.getUTCMonth(); //months from 1-12
                var day = dateObj.getUTCDate();
                var year = dateObj.getUTCFullYear();
                var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                var mon = monthNames[month].toString();
                var non = "1";
                var nation = id.value;
                var checkvalue = id.options[id.selectedIndex].text;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_qulifyexam.ClientID %>");
                    idvalue.style.display = "block";
                }
                else if (checkvalue.trim().toUpperCase() == "HSC") {
                    var max = 200;
                    var maxtotal = 0;
                    var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                    var gridViewControls = tbl.getElementsByTagName("input");
                    var len = tbl.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                            gridViewControls[i].value = max;
                            maxtotal = parseInt(maxtotal) + parseInt(max);
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugmonth") > 1) {
                            gridViewControls[i].value = mon;
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugyear") > 1) {
                            gridViewControls[i].value = year;
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugnoofattmpts") > 1) {
                            gridViewControls[i].value = non;
                        }
                    }
                    document.getElementById("<%=avgtext.ClientID %>").value = maxtotal;
                    var idvalue = document.getElementById("<%=txt_qulifyexam.ClientID %>");
                    idvalue.style.display = "none";
                }
                else if (checkvalue.trim().toUpperCase() == "CBSE") {
                    var max = 100;
                    var maxtotal = 0;
                    var cn = 0;
                    var tbl = document.getElementById("<%=UgGrid.ClientID %>");
                    var gridViewControls = tbl.getElementsByTagName("input");
                    var len = tbl.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txt_rg_max") > 1) {
                            cn++;
                            if (parseInt(cn) != parseInt(len - 1)) {
                                gridViewControls[i].value = max;
                                maxtotal = parseInt(maxtotal) + parseInt(max);
                            }
                            else {
                                gridViewControls[i].value = "";
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugmonth") > 1) {
                            if (parseInt(cn) != parseInt(len - 1)) {
                                gridViewControls[i].value = mon;
                            }
                            else {
                                gridViewControls[i].value = "";
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugyear") > 1) {
                            if (parseInt(cn) != parseInt(len - 1)) {
                                gridViewControls[i].value = year;
                            }
                            else {
                                gridViewControls[i].value = "";
                            }
                        }
                        if (gridViewControls[i].name.indexOf("txt_ugnoofattmpts") > 1) {
                            if (parseInt(cn) != parseInt(len - 1)) {
                                gridViewControls[i].value = non;
                            }
                            else {
                                gridViewControls[i].value = "";
                            }
                        }
                    }
                    document.getElementById("<%=avgtext.ClientID %>").value = maxtotal;
                    var idvalue = document.getElementById("<%=txt_qulifyexam.ClientID %>");
                    idvalue.style.display = "none";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_qulifyexam.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function monthertoung(id) {
                var nation = id.value;
                if (nation.trim().toUpperCase() == "OTHERS") {
                    var idvalue = document.getElementById("<%=txt_mothertounge.ClientID %>");
                    idvalue.style.display = "block";
                }
                else {
                    var idvalue = document.getElementById("<%=txt_mothertounge.ClientID %>");
                    idvalue.style.display = "none";
                }
            }
            function myFunction1(id) {
                id.value = "";
            }       
        </script>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green; font-size: x-large;">Student Application
                            Manager</span>                            
                    </div>
                </center>
            </div>
        </center>
        <center>
            <%-- <div class="maindivstyle maindivstylesize">--%>
            <div>
                <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lbl_degreeT" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lbl_branchT" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lbl_semT" runat="server" Visible="false"></asp:Label>
                <center>               
                    <table class="maintablestyle">
                        <tr>
                            <td> 
                                <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                    AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Stream" Width="100px" runat="server" Text="Stream"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stream" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 100px; height: 130px;">
                                            <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_stream_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_stream"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_graduation" runat="server" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="width: 130px; height: 130px;">
                                            <asp:CheckBox ID="cb_graduation" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_graduation_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_graduation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_graduation_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_graduation"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch" Width="100px" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch" runat="server" CssClass="ddlheight textbox textbox1"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p3" runat="server" runat="server" BackColor="White" BorderColor="Black"
                                            BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px"
                                            Style="position: absolute;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_degree_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="p3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="180px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_branch_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="p4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="Panel11" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_religion" Text="Religion" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_religion" runat="server" CssClass="textbox textbox1 txtheight"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="150px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_religion" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_religion_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_religion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_religion"
                                            PopupControlID="Panel7" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_comm" Text="Community" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_comm" runat="server" CssClass="textbox textbox1 txtheight3"
                                            Width="90px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" Width="90px"
                                            BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_comm" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_comm_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_comm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_comm"
                                            PopupControlID="Panel8" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_searchstudname" runat="server" Text="Student Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchstudname" runat="server" CssClass="textbox textbox1 txtheight2"
                                    Width="165px" AutoPostBack="true" OnTextChanged="txt_searchstudname_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                    ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchstudname" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_searchstudname"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_searchappno" runat="server" Text="Application No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchappno" runat="server" CssClass="textbox textbox1 txtheight1"
                                    Width="135px" AutoPostBack="true" OnTextChanged="txt_searchappno_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_searchmobno" runat="server" Text="Mobile No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchmobno" runat="server" CssClass="textbox textbox1 txtheight2"
                                    Width="135px" MaxLength="13" AutoPostBack="true" OnTextChanged="txt_searchmobno_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender01" runat="server" TargetControlID="txt_searchmobno"
                                    FilterType="numbers,custom" ValidChars=" +">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="getmob" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchmobno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFormat" runat="server" Text="Format"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAppFormat" CssClass="ddlheight textbox textbox1" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlAppFormat_SelectedIndexChanged">
                                   <%-- <asp:ListItem Value="0">Format1</asp:ListItem>
                                    <asp:ListItem Value="1">Format2</asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:CheckBox ID="cbl_datewise" AutoPostBack="true" OnCheckedChanged="cb_selectedchange_Click"
                                    runat="server" Text="Date Wise" />
                            </td>
                            <%-- <td>
                            <asp:Button ID="btn_go" runat="server" Text="Search" CssClass="textbox textbox1 btn2"
                                OnClick="btn_go_OnClick" />
                        </td>
                        <td>
                            <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                                OnClick="btn_addnew_OnClick" />
                        </td>--%>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" onchange="return checkDate()"
                                                Style="height: 20px; width: 75px; margin-left: 52px;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todate" runat="server" Enabled="false" onchange="return checkDate()"
                                                Style="height: 20px; width: 75px; margin-left: 6px;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="Label1" Text="Report Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreportTye" CssClass="ddlheight4 textbox textbox1" runat="server"
                                    AutoPostBack="true" Width="145px" OnSelectedIndexChanged="ddlreportTye_SelectedIndexChanged">
                                    <asp:ListItem>Applied</asp:ListItem>
                                    <asp:ListItem>Shortlist</asp:ListItem>
                                    <asp:ListItem>Wait to Admitted</asp:ListItem>
                                    <asp:ListItem>Admitted</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_seat" Text="Seat Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_seat" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true" Enabled="false">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_seat_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                             
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_go" runat="server" Text="Search" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_go_OnClick" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                                    OnClick="btn_addnew_OnClick" />
                            </td>
                            <td>
                                <asp:Label ID="Label6" Text="Quota/Category" runat="server"></asp:Label>
                            </td>
                             <td>
                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtQuota" runat="server" CssClass="textbox textbox1 txtheight2"
                                        ReadOnly="true" Width="100px" Enabled="false">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel14" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: 190px;">
                                        <asp:CheckBox ID="cbQuota" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbQuota_checkedchange" /><%----%>
                                        <asp:CheckBoxList ID="cblQuota" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblQuota_SelectedIndexChange">
                                        </asp:CheckBoxList>
                                        <%----%>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtQuota"
                                        PopupControlID="Panel14" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                            <%-- <td><asp:RadioButton ID="rdb_paid" Visible="false" runat="server" Text="Paid" GroupName="d" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_notpaid" Visible="false" runat="server" Text="Not Paid"
                                GroupName="d" />
                        </td>--%>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter0" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="850px" Style="margin-top: -0.1%;">
                                <asp:Label ID="lbl_st" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Image7" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <asp:Panel ID="pcolumnorder0" runat="server" CssClass="maintablestyle" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column0" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column0_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton8" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove0_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborder0" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder0" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder0_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="date_applied">Date Applied</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="app_formno">Application No</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="stud_name">Applicant Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="sex">Gender</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Batch_Year">Batch</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Course_Name">Course</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Dept_Name">Department (Option1)</asp:ListItem>
                                        <%-- <asp:ListItem Selected="True" Value="PassMonth">Month Of Passing</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="PassYear">Year Of Passing</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="Course">Board </asp:ListItem>
                                    <asp:ListItem Selected="True" Value="branch_code">Major </asp:ListItem>--%>
                                        <%-- <asp:ListItem Selected="True" Value="securedmark">Total Marks </asp:ListItem>--%>
                                        <asp:ListItem Selected="True" Value="Alternativedegree_code">Department (Option2)</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Student_Mobile">Mobile No</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="religion">Religion</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="community">Community</asp:ListItem>
                                        <%--<asp:ListItem Selected="True" Value="Institute_Name">Institute Name</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="instaddress">Institute Address</asp:ListItem>--%>
                                        <asp:ListItem Selected="True" Value="remarks">Remarks </asp:ListItem>
                                        <asp:ListItem Selected="True" Value="seattype">Seat Type</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="stuper_id">Email ID </asp:ListItem>
                                        <asp:ListItem Selected="True" Value="date_paid">Date of Payment</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="quota">Quota/Category</asp:ListItem><%--Abarna--%>

                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder0" runat="server" TargetControlID="pcolumnorder0"
                        CollapseControlID="pheaderfilter0" ExpandControlID="pheaderfilter0" Collapsed="true"
                        TextLabelID="lbl_st" CollapsedSize="0" ImageControlID="Image7" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                    <asp:Label ID="lbl_errormsg" ForeColor="Red" Visible="false" Style="font-weight: bold;"
                        runat="server"></asp:Label>
                </center>
                <center>
                    <%--<div id="div1" runat="server" visible="false" style="width: 890px; height: 500px;
                    overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                        BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px"
                        OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <%--</div>--%>
                </center>
                <br />
                <asp:Label ID="lbl_norec" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress=" return printdisp()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                            AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                            CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
                <br />
                <div id="popview" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 471px;"
                        OnClick="btn_popclose_Click" />
                    <%--  style="height: 300em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">--%>
                    <br />
                    <div style="background-color: White; height: 634px; width: 960px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <span class="fontstyleheader" style="color: #008000;">Student Application</span>
                        <div>
                            <center>
                                <fieldset id="studdetail" runat="server" style="height: 190px; width: 850px;">
                                    <center>
                                        <span style="color: Blue; font-size: larger; font-weight: bold;">Personal Details</span>
                                    </center>
                                    <table width="840px">
                                    <tr>
                                    <td>
                                    <asp:CheckBox ID="IsEnquiry" runat="server" Text="Enquiry Student" AutoPostBack="true" OnCheckedChanged="Chk_Changed_StudentEnquiry"/> 
                                    </td>
                                     </tr>
                                        <tr>
                                            <td>
                                                Academic Year
                                            </td>
                                            <td>
                                                <%--  <asp:DropDownList ID="ddlyear" runat="server" Enabled="false" Style="width: 66px;
                                    height: 30px;" CssClass="textbox3 textbox1">
                                </asp:DropDownList>--%>
                                                <asp:Label ID="lblyear" runat="server" Enabled="false" Visible="false" Style="width: 66px;
                                                    font-size: larger; font-weight: bold; height: 30px;"></asp:Label>
                                                <asp:DropDownList ID="ddlBatch_year" runat="server" Style="width: 100px; height: 30px;"
                                                    CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_clg1" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlclg_OnSelectedIndexChanged"
                                                    Style="width: 150px; height: 30px;" CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <%--<span style="color: Red;">*</span>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Enq_No" runat="server"  Text="Application No"/>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtAcrApplno" runat="server" Style="width: 32px;" CssClass="textbox textbox1"
                                                    onblur="return getled(this.value)" Enabled="false"></asp:TextBox>
                                                <asp:TextBox ID="txtapplno" runat="server" ontextchanged="txtapplno_TextChanged" AutoPostBack="true" Style="width: 98px;" CssClass="textbox textbox1"
                                                    onkeypress="display(this)" onblur="return getled(this.value)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtapplno"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblerr" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                Student Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtapplname" runat="server" placeholder="Name" Style="width: 260px;"
                                                    onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_applname" runat="server" TargetControlID="txtapplname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Father Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtfather" runat="server" placeholder="Father Name" Style="width: 260px;"
                                                    onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender113" runat="server" TargetControlID="txtfather"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Gender
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsex" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                    onfocus="myFunction(this)" Style="width: 160px;">
                                                </asp:DropDownList>
                                                <%--<asp:RadioButton ID="rbmale" runat="server"  Text="Male"/>
                                            <asp:RadioButton ID="rbfemale" runat="server" Text="Female" />--%>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </td>
                                            <td id="tdreg" runat="server" visible="false">
                                                Religion
                                                <%-- </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlregl" runat="server" Style="width: 150px; height: 30px;"
                                                    CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdcom" runat="server" visible="false">
                                                Community
                                                <%-- </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlcomm" runat="server" Style="width: 150px; height: 30px;"
                                                    CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <%-- <td>
                                            Education level
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcourse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged"
                                                Style="width: 150px; height: 30px;" CssClass="textbox3 textbox1">
                                            </asp:DropDownList>
                                          
                                            <span style="color: Red;">*</span>
                                        </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="2" id="tdedu" runat="server" visible="false">
                                                Education level
                                                <%-- </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlcourse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged"
                                                    Style="width: 150px; height: 30px;" CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <%--onchange="return educationDivShow(this)"--%>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Class
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlchoice1" runat="server" Style="width: 150px; height: 30px;"
                                                    CssClass="textbox3 textbox1" OnSelectedIndexChanged="ddlchoice1_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <%-- OnSelectedIndexChanged="ddlchoice1_SelectedIndexChanged"
                                                AutoPostBack="true"--%>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" id="tdchoice2" runat="server" visible="false">
                                                Choice 2
                                                <%-- </td>
                                        <td>--%>
                                                <asp:DropDownList ID="ddlchoice2" runat="server" Style="width: 150px; height: 30px;"
                                                    CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <%-- <span style="color: Red;">*</span>--%>
                                            </td>
                                        </tr>
                                        <tr id="trscl" runat="server" visible="false">
                                            <td>
                                                <asp:CheckBox ID="cbsamclg" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="cbsamclg_OnCheckedChanged" />
                                                <asp:Label ID="lblsclname" runat="server" Text=" School Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtsclname" runat="server" placeholder="School Name" Style="width: 260px;"
                                                    onkeypress="display()" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtsclname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblscladd" runat="server" Text="  School Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtscladd" runat="server" placeholder="School Address" Style="width: 260px;"
                                                    onkeypress="display()" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtscladd"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars="/ ,-  ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset id="edut" runat="server" visible="false" style="height: 125px; width: 850px;">
                                    <center>
                                        <span style="color: Blue; font-size: larger; font-weight: bold;">Education Details</span></center>
                                    <table id="tbledut" runat="server" style="float: left;">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbmarkmark" Text="Marks" GroupName="ww" Checked="true" runat="server"
                                                    AutoPostBack="true" OnCheckedChanged="rbmarkmark_OnCheckedChanged"></asp:RadioButton>
                                                <asp:RadioButton ID="rbmarkgrade" Text="Grade" GroupName="ww" runat="server" AutoPostBack="true"
                                                    OnCheckedChanged="rbmarkgrade_OnCheckedChanged"></asp:RadioButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Board
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbhsc" runat="server" GroupName="bd" Text="HSC" AutoPostBack="true"
                                                    OnCheckedChanged="rbhsc_OnCheckedChanged" Checked="true" />
                                                <asp:RadioButton ID="rbcbse" runat="server" GroupName="bd" Text="CBSE/Except T.N"
                                                    AutoPostBack="true" OnCheckedChanged="rbcbse_OnCheckedChanged" />
                                                <asp:RadioButton ID="rbother" runat="server" GroupName="bd" Text="Other State/Country"
                                                    AutoPostBack="true" OnCheckedChanged="rbother_OnCheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblboard" runat="server" Text="Enter The Board" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtotherscl" runat="server" placeholder="Board Name" Visible="false"
                                                    Style="width: 130px;" onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtotherscl"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="/ - . ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblmark" runat="server" Text="Total Marks" Visible="false"></asp:Label>
                                                <asp:Label ID="lblgrade" runat="server" Text="Grade" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmark" runat="server" MaxLength="4" placeholder="Total Marks"
                                                    Visible="false" Style="width: 130px;" onkeypress="display(this)" onblur="return Changetotal(this.value)"
                                                    CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender66" runat="server" TargetControlID="txtmark"
                                                    FilterType="Numbers" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:TextBox ID="txtgrade" runat="server" placeholder="Grade" Visible="false" Style="width: 130px;"
                                                    onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender77" runat="server" TargetControlID="txtgrade"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="+ -">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Year Of Passing
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpassyr" runat="server" Visible="false" MaxLength="8" placeholder="jan 2015"
                                                    onblur="return dateFormat()" Style="width: 130px;" onkeypress="display(this)"
                                                    CssClass="textbox textbox1"></asp:TextBox>
                                                <%--onblur="return dateFormat()"--%>
                                                <%--OnTextChanged="txtpassyr_OnTextChanged"--%>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtpassyr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:DropDownList ID="ddlmonth" CssClass="textbox3 textbox1" runat="server" Style="width: 80px;"
                                                    onfocus="myFunction(this)">
                                                    <asp:ListItem Value="1">JAN</asp:ListItem>
                                                    <asp:ListItem Value="2">FEB</asp:ListItem>
                                                    <asp:ListItem Value="3">MAR</asp:ListItem>
                                                    <asp:ListItem Value="4">APR</asp:ListItem>
                                                    <asp:ListItem Value="5">MAY</asp:ListItem>
                                                    <asp:ListItem Value="6">JUN</asp:ListItem>
                                                    <asp:ListItem Value="7">JUL</asp:ListItem>
                                                    <asp:ListItem Value="8">AUG</asp:ListItem>
                                                    <asp:ListItem Value="9">SEP</asp:ListItem>
                                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlyear" CssClass="textbox3 textbox1" runat="server" onfocus="myFunction(this)"
                                                    Style="width: 100px;">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="float: right; display: none;">
                                        <tr>
                                            <td>
                                                <asp:Image ID="studimg" runat="server" Style="height: 100px; width: 100px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:FileUpload ID="studphoto" runat="server" Style="height: 30px; width: 80px;" />
                                            </td>
                                            <td>
                                                <asp:Button ID="upload" runat="server" Text="Upload" OnClick="upload_OnClick" Style="height: 30px;
                                                    width: 80px;" />
                                                <%--onchange='prvimg.UpdatePreview(this)'--%>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset id="edutpg" runat="server" visible="false" style="height: 117px; width: 850px;">
                                    <center>
                                        <span style="color: Blue; font-size: larger; font-weight: bold;">Education Details</span>
                                    </center>
                                    <table style="float: left;">
                                        <tr>
                                            <td>
                                                University
                                            </td>
                                            <%--9/5/2016--%>
                                            <td>
                                                <asp:DropDownList ID="ddlpg" runat="server" Style="width: 150px; height: 30px;" onchange="return universityTxt(this)"
                                                    CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                                <asp:TextBox ID="txtuniname" runat="server" placeholder="University Name" Style="width: 110px;
                                                    float: right; display: none;" onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtuniname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=".  ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <%--9/5/2016--%>
                                            <td>
                                                <asp:Label ID="lbltan" runat="server" Visible="false" Text=" Tancet Mark"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttanmark" runat="server" placeholder="TANCET MARK" MaxLength="5"
                                                    onblur="return ValidateTancetMark(this.value)" Visible="false" Style="width: 84px;"
                                                    onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txttanmark"
                                                    FilterType="Numbers,custom" ValidChars=".  ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_cource" runat="server" Text="UG Course"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlugcourse" runat="server" onchange="return educationDivShow(this)"
                                                    Style="width: 150px; height: 30px;" CssClass="textbox3 textbox1">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtugcourse" runat="server" placeholder="PG Course" Style="width: 110px;
                                                    float: right; display: none;" onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtugcourse"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=".  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_totalpercentage" runat="server" Text="Total Percentage Obtained in UG"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtugper" runat="server" placeholder="Percentage PG" onblur="return ValidateValue(this.value)"
                                                    Style="width: 88px;" onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtugper"
                                                    FilterType="Numbers,custom" ValidChars=".  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Major
                                            </td>
                                            <td>
                                                <%-- <asp:DropDownList ID="ddlugdept" runat="server" Style="width: 150px; height: 30px;"
                                                CssClass="textbox3 textbox1">
                                            </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtugmaj" runat="server" placeholder="Major" Style="width: 260px;"
                                                    onkeypress="display(this)" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtugmaj"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="- , ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Year Of Passing
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox1" runat="server" Visible="false" MaxLength="8" placeholder="jan 2015"
                                                    onblur="return dateFormat()" Style="width: 130px;" onkeypress="display(this)"
                                                    CssClass="textbox textbox1"></asp:TextBox>
                                                <%--onblur="return dateFormat()"--%>
                                                <%--OnTextChanged="txtpassyr_OnTextChanged"--%>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtpassyr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:DropDownList ID="ddlugmnth" CssClass="textbox3 textbox1" runat="server" Style="width: 80px;"
                                                    onfocus="myFunction(this)">
                                                    <asp:ListItem Value="1">JAN</asp:ListItem>
                                                    <asp:ListItem Value="2">FEB</asp:ListItem>
                                                    <asp:ListItem Value="3">MAR</asp:ListItem>
                                                    <asp:ListItem Value="4">APR</asp:ListItem>
                                                    <asp:ListItem Value="5">MAY</asp:ListItem>
                                                    <asp:ListItem Value="6">JUN</asp:ListItem>
                                                    <asp:ListItem Value="7">JUL</asp:ListItem>
                                                    <asp:ListItem Value="8">AUG</asp:ListItem>
                                                    <asp:ListItem Value="9">SEP</asp:ListItem>
                                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlugyr" CssClass="textbox3 textbox1" runat="server" onfocus="myFunction(this)"
                                                    Style="width: 100px;">
                                                </asp:DropDownList>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                <fieldset id="remarks" runat="server" style="height: 193px; width: 850px;">
                                    <table style="float: left; width: 850px;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Enquiry_date" runat="server" Text=" Application Date"/>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtappldate" runat="server" Style="width: 72px;" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtappldate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                            <td>
                                                Mobile No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtmblno" runat="server" MaxLength="10" placeholder="Mobile No"
                                                    Style="width: 141px;" CssClass="textbox textbox1" onkeypress="display(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtmblno"
                                                    FilterType="Numbers,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                                <span style="color: Red;">*</span>
                                            </td>
                                        </tr>
                                        <tr id="trseat" runat="server" visible="false">
                                            <td>
                                                Seat Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_seattype" runat="server" CssClass="ddlheight3 textbox textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Student Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMode" runat="server" CssClass="ddlheight3 textbox textbox1"
                                                    OnSelectedIndexChanged="ddlMode_Change" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpsem" runat="server" Text="Semester"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSemester1" runat="server" Style="width: 80px;" CssClass="ddlheight3 textbox textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbincludercpt" Checked="true" Visible="false" runat="server" Text="Make Payment" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                Remarks
                                                <%-- </td>
                                        <td colspan="4">--%>
                                                <asp:TextBox ID="txtrm" runat="server" TextMode="MultiLine" placeholder="Remarks"
                                                    Style="width: 700px; height: 40px;" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender87" runat="server" TargetControlID="txtrm"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <%--<asp:CheckBox ID="IsEnquiry" runat="server" Text="Enquiry Student"  delsi2401/>--%>
                                    <asp:Button ID="btnUpdate" runat="server" Visible="false" Text="Update" OnClientClick="return Save()"
                                        OnClick="btnUpdate_OnClick" />
                                    <asp:Button ID="btnDelete" runat="server" Visible="false" Text="Delete" OnClientClick="return Save()"
                                        OnClick="btnDelete_OnClick" />
                                    <asp:Button ID="btnsave" runat="server" Visible="false" Text="Save" OnClientClick="return Save()"
                                        OnClick="btnsaves_OnClick" />
                                    <%--  <asp:Button ID="btnsaves" runat="server" Text="Save" OnClick="btnsaves_OnClick" OnClientClick="return Save()" />--%>
                                    <asp:Button ID="btncancel" runat="server" Visible="false" Text="Reset" OnClick="btncancel_OnClick" />
                                    <asp:LinkButton ID="lbtn_smsContent" runat="server" Visible="true" Text="Set SMS Content" OnClick="lbtn_smsContent_Click" />
                                </fieldset>
                            </center>
                        </div>
                        <br />
                    </div>
                </div>

                 <center>
                    <div id="Alreadyreg_div" runat="server" visible="false" style="height: 300em; z-index: 100000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                         <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute;margin-top: 164px; margin-left: 427px;"
                        OnClick="Alreadyreg_div_Click" />
                        <br />
                        <center>
                            <div id="Div13" runat="server" class="table" style="background-color: White; height: 400px; overflow:scroll;
                                width: 885px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 153px;
                                border-radius: 10px;">
                                 <asp:GridView ID="Alreadyregstudent_grid" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                                            CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="600px" Font-Size="Small">
                                           <Columns> <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="Gray">
                                            <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle BackColor="gray"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField></Columns> 
                                            <RowStyle BackColor="#E3EAEB" />
                                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <AlternatingRowStyle BackColor="White" />
                                 </asp:GridView>
                                 <br />
                             </div>
                        </center>
                    </div>
                 </center>
                <%--delsi0702--%>
                <div id="popSendSms" class="popupstyle popupheight1" runat="server" style="height: 100em; z-index: 1000; width: 100%;
              position: absolute; top: 0; left: 0;" visible="false">
               
            <center>
                <div style="background-color: #FFFFFF; height: 300px; margin-top: 180px; width: 500px;">
                 <asp:ImageButton ID="Imgbtn_close" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -2px; margin-left: 220px;"
                        OnClick="btn_Close_Click" />
                    <br />
                    <table>
                        <tr>
                            <td style="color: Green; text-align: center; font-size: 20px; font-weight: bold;">
                               SMS Content
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Type your message here :
                                <br />
                                <asp:TextBox ID="txt_SmsMsgPop" runat="server" Width="400px" TextMode="MultiLine"
                                    Rows="10" Placeholder="New Message"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                       
                        <td>
                         <center>
                        <asp:Button ID="btn_SaveTxt" runat="server" Text="Save" OnClick="Sava_txtClick" />
                        </center>
                        </td>
                        
                        </tr>
                       
                       
                    </table>
                </div>
            </center>
        </div>

                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
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
                    <div id="imgdel" runat="server" visible="false" style="height: 300em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbldelalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btndelok" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btndelok_Click" Text="ok" runat="server" />
                                                    <asp:Button ID="btndelcan" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btndelcan_Click" Text="Cancel" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <div id="popaddnewF2" runat="server" class="popupstyle popupheight1" visible="false">
                    <asp:ImageButton ID="btn_popupclose2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; left: 95%; top: 0%; right=0px;"
                        OnClick="btn_popupclose2_Click" />
                    <div style="background-color: White; width=100%; height: auto; overflow: auto; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">

                          <%--delsi--%>
                                                              <center>
                <asp:Panel ID="panelphoto" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 460px; top: 220px; position: absolute;"
                    Height="454px" Width="700px">
                    <div class="PopupHeaderrstud2" id="Div14" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            <asp:Label ID="lblcaption" runat="server" Text="Photos Details" Font-Bold="True"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </caption>
                    </div>
                    <br />
                    <br />
                     <fieldset style="left: 25px; top: 37px; width: 232px; height: 130px; position: absolute;">
                        <asp:Label ID="Label3" runat="server" Text="Student Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 3px;"></asp:Label>
                        <asp:Image ID="imgstudp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; left: 5px;
                            top: 130px;" />
                             <asp:Button ID="BtnsaveStud" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="BtnsaveStud_Click" Style="position: absolute; left: 175px; top: 100px;" />
                             <asp:Button ID="btnrmv_student" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvStud_Click" Style="position: absolute; left: 178px; top: 130px;" />
                            
                       
                    </fieldset>
                    
                     <fieldset style="left: 300px; top: 37px; width: 232px; height: 130px; position: absolute;">
                        <asp:Label ID="Label2" runat="server" Text="Father Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 3px;"></asp:Label>
                        <asp:Image ID="imgfatp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulfatp" runat="server"  Style="position: absolute;
                            left: 5px; top: 130px;" />
                             <asp:Button ID="Btnsavfather" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="Btndownloadfather_Click" Style="position: absolute; left: 175px; top: 100px;" />
                             <asp:Button ID="btnrmv_father" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvfather_Click" Style="position: absolute; left: 178px; top: 130px;" />
                       
                    </fieldset>

                    <fieldset style="width: 232px; height: 130px; position: absolute; left: 25px; top: 200px;">
                        <asp:Label ID="Label4" runat="server" Text="Mother Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imgmotp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulmp" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />

                             <asp:Button ID="btnsavemother" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="btnsavemother_Click" Style="position: absolute; left: 175px; top: 100px;" />
                              <asp:Button ID="btnrmv_mother" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmvmother_Click" Style="position: absolute; left: 178px; top: 130px;" />
                      
                    </fieldset>
                     <fieldset style="width: 232px; height: 130px; position: absolute; left: 300px; top: 200px;">
                        <asp:Label ID="Label5" runat="server" Text="Guardian Photo" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute; left: 50px;
                            top: 2px;"></asp:Label>
                        <asp:Image ID="imggurp" runat="server" Style="width: 100px; height: 100px; position: absolute;
                            left: 35px; top: 20px;" />
                        <asp:FileUpload ID="fulguar" runat="server" onchange="callme(this)" Style="position: absolute;
                            left: 5px; top: 130px;" />
                               <asp:Button ID="Btnsavegau" runat="server" Text="Save" Width="80px" Font-Bold="true"
                            OnClick="Btnsavegau_Click" Style="position: absolute; left: 175px; top: 100px;" />

                               <asp:Button ID="btnrmv_gua" runat="server" Text="Remove" Width="75px" Font-Bold="true"
                            OnClick="btnrmv_gua_Click" Style="position: absolute; left: 178px; top: 130px;" />
                    
                    </fieldset>

                    <asp:Label ID="lblphotoerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Style="position: absolute; left: 5px; top: 380px;"></asp:Label>
                    <fieldset style="width: 150px; height: 12px; position: absolute; left: 167px; top: 400px;">
                        
                        <asp:Button ID="btnstuph" runat="server" Text="Ok" Width="75px" Font-Bold="true"
                            OnClick="btnstuph_Click" Style="position: absolute; left: 10px; top: 7px;" />
                        <asp:Button ID="Button2" runat="server" Text="Exit" Width="75px" Font-Bold="true"
                            OnClick="btnexit_Click" Style="position: absolute; left: 88px; top: 7px;" />
                    </fieldset>
                   

                </asp:Panel>
            </center>


                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Student Application</span></center>
                        <asp:TabContainer ID="TabContainer1" runat="server" Visible="true" Style="height: auto;"
                            CssClass="ajax__myTab" BackColor="Lavender" Width="100%" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged"
                            AutoPostBack="true">
                            <asp:TabPanel ID="tabpanel1" runat="server" HeaderText="Personal Details" Font-Names="Book Antiqua"
                                CssClass="ajax__myTab1" Font-Size="Medium" Visible="true" TabIndex="1">
                                <ContentTemplate>
                                    <center>
                                        <fieldset>
                                            <%-- <legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Course</legend>--%>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold;">Course</span>
                                            </div>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_clg2" runat="server" Style="font-family: Sans-Serif; font-size: small;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_college" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged"
                                                            CssClass="textbox textbox1 ddlheight4" Width="220px">
                                                        </asp:DropDownList>
                                                        <%--<span style="color: Red;">*</span>--%>
                                                    </td>
                                                    
                                                    <td>
                                                        <span style="font-family: Sans-Serif; font-size: small;">Choice 1</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_courseone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_courseone_OnSelectedIndexChanged"
                                                            CssClass="textbox3 textbox1 ddlheight4">
                                                        </asp:DropDownList>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span style="font-family: Sans-Serif; font-size: small;">Application No</span> 
                                                        <asp:TextBox ID="txt_appfromno" ontextchanged="txt_appfromno_TextChanged"  AutoPostBack="true" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight4"
                                                            MaxLength="50" Style="text-trnasform: none;"></asp:TextBox>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                    
                                      
                                                    <td colspan="2">
                                                        <span style="font-family: Sans-Serif; font-size: small;">Applied Date</span>
                                                        <asp:TextBox ID="txt_appliaddate" runat="server" Enabled="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_appliaddate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                                         <td colspan="2">
                                                        <span style="font-family: Sans-Serif; font-size: small;">Quota/Category</span>
                                                        <asp:DropDownList ID="ddl_quata" runat="server"  onchange="return  Quata(this)" CssClass="textbox textbox1 ddlheight2">
                                                        </asp:DropDownList>
                                                       
                                                    </td>

                                                     <td>
                                                     <asp:TextBox ID="txt_quata" CssClass="textbox textbox1" Visible="true"
                                                                    MaxLength="50" runat="server" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Other Quota" Style="width: 150px;
                                                                    float: left; display: none;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender162" runat="server" TargetControlID="txt_quata"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                                                                            
                                                              
                                                            </td>
                                                    <td>
                                                        <span style="font-family: Sans-Serif; font-size: small; padding-top: 7px;">Batch
                                                        </span>
                                                        <asp:DropDownList ID="ddlbatch2" CssClass="textbox textbox2" Width="70px" Height="30px"
                                                            runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2">
                                                        <span style="font-family: Sans-Serif; font-size: small;">SeatType</span>
                                                        <asp:DropDownList ID="ddl_seattype2" runat="server" CssClass="textbox textbox1 ddlheight2" AutoPostBack="true"   OnSelectedIndexChanged="ddseattype_change">
                                                        </asp:DropDownList>
                                                        <span style="color: Red;">*</span>
                                                    </td>
                                                                                                        <%--krishhna kumar.r--%>
                                                      </td>
                               
                                                     <%--krishhna kumar.r--%>

                                                    <td colspan="2">
                                                    <asp:Label ID="lbl_enquiry" runat="server" Text="Enquiry"  style="font-family: Sans-Serif; font-size: small;"/>
                                                     <asp:Button ID="btn_question" Visible="true" runat="server" CssClass="textbox btn" Text="?" OnClick="btnQ_Click" />
                                                       <asp:LinkButton ID="lnkbtn_studentphoto" runat="server"  Text="Student's Photo Upload" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lbkBtn_studphoto_Click">
                            </asp:LinkButton>

                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <%-- <fieldset>--%><%--delsi--%>
                                        <center>
                                <div id="popenquiry" runat="server" visible="false" class="popupstyle popupheight">
                                    <br />
                                    <div class="subdivstyle" style="background-color: White; height: 610px; width: 800px;">
                                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: -37px; margin-left: 385px;"
                                            OnClick="imagebtnpopclose1_Click" />
                                        <br />
                                        <center>
                                            <asp:Label ID="lbl_select" runat="server" Font-Bold="true" Style="font-size: large;
                                                color: Green;" Text="Enquiry Student Details"></asp:Label>
                                        </center>
                                        <br />
                                        <div>
                                            <center>
                                                <table class="maintablestyle">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_collegename2" runat="server" Text="College"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_collegename2" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlcollege2_SelectedIndexChanged" CssClass="textbox textbox1 ddlheight5">
                                                            </asp:DropDownList>
                                                        </td>
                                                        
                             <td>
                                <asp:Label ID="lbldegree" Text="Degree" runat="server"></asp:Label>
                            </td>
                            

                                 <td>
                                <asp:UpdatePanel ID="Updegree1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdegree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="Pdegree2" runat="server" BackColor="White" BorderColor="Black"  BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px"
                                            Style="position: absolute;">
                                            <asp:CheckBox ID="cbdegree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbdegree_checkedchange" />
                                            <asp:CheckBoxList ID="cbldegree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbldegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdegree"
                                            PopupControlID="Pdegree2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>

                            </tr>
                            <tr>
                            <td>
                                <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" runat="server" OnTextChanged="txtfromdate_TextChanged"
                                     Width="80px" CssClass="textbox textbox1 txtheight1"></asp:TextBox>  
                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                                <asp:TextBox ID="txttodate" runat="server" CssClass="textbox textbox1 txtheight1"
                                    OnTextChanged="txttodate_TextChanged" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txttodate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                                                <td>
                                                        <asp:Button ID="btn_view" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btnView_go_Click" />
                                                        </td>
                                                </tr>
                                                </table>
                                                <div>
                                                    <p>
                                                        <asp:Label ID="lbl_search3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="lbl_error3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    </p>
                                
                                                    <FarPoint:FpSpread ID="Fp_Enquery" runat="server" Visible="false" Width="600px" ActiveSheetViewIndex="0" Style="overflow: auto;
                                                        height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                                        box-shadow: 0px 0px 8px #999999;" OnCellClick="Cell_Click" OnPreRender="Fp_Enquery_SelectedIndexChanged" >
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </div>
                                                <br />
                                                <center>
                                                    <div>
                                                        <asp:Button ID="btn_save2" Visible="false" runat="server" CssClass="textbox btn2" Text="Ok" Width="100" OnClick="btnsav_Click" />
                                                       
                                                    </div>
                                                </center>
                                            </center>
                                        </div>
                                    </div>
                                </div>
                            </center>


                                        <div id="Div1" runat="server" visible="false" style="width: 100%; height: 25px; background-color: Brown;">
                                            <span style="font-size: large; font-weight: bold; color: White;">Part-I Language &nbsp;<span
                                                style="color: White; font-size: 20px;">*</span></span>
                                        </div>
                                        <center>
                                            <div>
                                                <table id="tt" runat="server" visible="false">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList ID="rdblistlanguage" runat="server" Style="width: 700px; font-family: Sans-Serif;
                                                                font-size: small;" RepeatDirection="Horizontal">
                                                                <asp:ListItem>Arabic</asp:ListItem>
                                                                <asp:ListItem>French</asp:ListItem>
                                                                <asp:ListItem>Hindi</asp:ListItem>
                                                                <asp:ListItem Selected="True">Tamil</asp:ListItem>
                                                                <asp:ListItem>Urdu</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                        <%--</fieldset>--%>
                                        <fieldset>
                                            <%-- <legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Student Details</legend>--%>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold;">Student Details</span>
                                            </div>
                                            <div style="width: 100%;">
                                                <div style="width: 50%; border: 1; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <span style="font-family: Sans-Serif; font-size: small;">Name of the Applicant</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_FName_app" runat="server" Text="" placeholder="First Name" CssClass="textbox textbox1"></asp:TextBox>
                                                                <asp:TextBox ID="TextBox3" runat="server" Text="" Visible="false" placeholder="First Name"
                                                                    MaxLength="50" CssClass="textbox textbox1"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_FName_app"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_LName_app" runat="server" Text="" placeholder="Last Name" CssClass="textbox textbox1"
                                                                    MaxLength="50"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_LName_app"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <span style="font-family: Sans-Serif; font-size: small;">Place of birth</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_placeofbirth" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Place of birth" Style="width: 159px;
                                                                    float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender64" runat="server" TargetControlID="txt_placeofbirth"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td colspan="2" style="float: left;">
                                                                <span style="font-family: Sans-Serif; font-size: small;">&nbsp;&nbsp;Gender</span>
                                                                <asp:DropDownList ID="ddlsex2" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onfocus="myFunction(this)" Style="width: 100px;">
                                                                </asp:DropDownList>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Nationality</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlcountry" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onchange="return funation(this)" onfocus="myFunction(this)" Style="width: 160px;
                                                                    float: left;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_othernationality" CssClass="textbox textbox1" Visible="true"
                                                                    MaxLength="50" runat="server" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Other Nationality" Style="width: 150px;
                                                                    float: left; display: none;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_othernationality"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Community</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlcoummunity" CssClass="textbox3 textbox1" runat="server"
                                                                    onblur="blurFunction(this)" onchange="return oncomm(this)" onfocus="myFunction(this)"
                                                                    Style="width: 160px; float: left;">
                                                                    <asp:ListItem>Select</asp:ListItem>
                                                                    <asp:ListItem>Others</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCommunity" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtCommunity"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Blood Group</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlbloodgroup" CssClass="textbox3 textbox1" runat="server"
                                                                    onblur="blurFunction(this)" onchange="return otherable1(this)" onfocus="myFunction(this)"
                                                                    Style="width: 100px; float: left;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_otherbloodgroup" CssClass="textbox textbox1" Visible="true"
                                                                    MaxLength="7" runat="server" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Blood group" Style="width: 150px; display: none;
                                                                    float: left; text-transform: none"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender63" runat="server" TargetControlID="txt_otherbloodgroup"
                                                                    FilterType="Custom" ValidChars=" aboveABOVE+-12">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Identification
                                                                    Mark 1</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtIdMark1" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="100" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Identification Mark" Style="width: 314px;
                                                                    float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender83" runat="server" TargetControlID="txtIdMark1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">EMIS Number</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtemis_no" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="100" onkeydown="return (event.keyCode!=13);" placeholder="EMIS Number"
                                                                    Style="width: 314px; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender114" runat="server" TargetControlID="txtemis_no"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 50%; float: right;">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <span style="font-family: Sans-Serif; font-size: small;">DOB</span>
                                                            </td>
                                                            <td colspan="3">
                                                              <asp:TextBox ID="txt_dob" runat="server" width="103px" height="25px" MaxLength="10"  onchange="return agecal(this)"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender160" runat="server" TargetControlID="txt_dob"
                                                    FilterType="numbers,custom" ValidChars="/"> </asp:FilteredTextBoxExtender>
                                                    <asp:ImageButton runat="Server" ID="imgdob" 
AlternateText="show" Height="22px" Width="22px"   ImageUrl="../images/calendar_icon.png" />
                                                <asp:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txt_dob"
PopupButtonID="imgdob" Format="dd/MM/yyyy">
</asp:CalendarExtender>
                                                                <%--<asp:DropDownList ID="ddldobdate" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onchange="return agecal(this)" onfocus="myFunction(this)" Style="width: 60px;">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddldobMonth" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onchange="return agecal(this)" onfocus="myFunction(this)" Style="width: 80px;">
                                                                    <asp:ListItem Value="00">Month</asp:ListItem>
                                                                    <asp:ListItem Value="01">JAN</asp:ListItem>
                                                                    <asp:ListItem Value="02">FEB</asp:ListItem>
                                                                    <asp:ListItem Value="03">MAR</asp:ListItem>
                                                                    <asp:ListItem Value="04">APR</asp:ListItem>
                                                                    <asp:ListItem Value="05">MAY</asp:ListItem>
                                                                    <asp:ListItem Value="06">JUN</asp:ListItem>
                                                                    <asp:ListItem Value="07">JUL</asp:ListItem>
                                                                    <asp:ListItem Value="08">AUG</asp:ListItem>
                                                                    <asp:ListItem Value="09">SEP</asp:ListItem>
                                                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddldobYear" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onchange="return agecal(this)" onfocus="myFunction(this)" Style="width: 80px;">
                                                                </asp:DropDownList>--%>
                                                                <span style="color: Red;">*</span> <span style="font-family: Sans-Serif; font-size: small;">
                                                                    Age</span> &nbsp;&nbsp;
                                                                <asp:TextBox ID="txt_age" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="3" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 50px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_age"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Mother Tongue
                                                                </span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlmothertounge" CssClass="textbox3 textbox1" runat="server"
                                                                    onblur="blurFunction(this)" onfocus="myFunction(this)" onchange="return monthertoung(this)"
                                                                    Style="width: 160px; float: left;">
                                                                </asp:DropDownList>
                                                                <%-- <span style="color: Red;">*</span>--%>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_mothertounge" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Mother Tongue" Style="width: 150px; float: left; display: none;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender150" runat="server" TargetControlID="txt_mothertounge"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Religion</span>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlreligion" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onchange="return onreli(this)" onfocus="myFunction(this)" Style="width: 160px;">
                                                                </asp:DropDownList>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlsubreligion" CssClass="textbox3 textbox1" runat="server"
                                                                    onchange="return subrelig(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                    Style="width: 160px; display: none; float: left;">
                                                                    <asp:ListItem>Select</asp:ListItem>
                                                                    <asp:ListItem>Others</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td colspan="3" align="right">
                                                                <asp:TextBox ID="txt_otherreligion" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction1(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender164" runat="server" TargetControlID="txt_otherreligion"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;">Caste</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txt_Caste" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" Text="" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txt_Caste"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Aadhar Card No</span>
                                                            </td>
                                                            <td align="left" colspan="2">
                                                                <asp:TextBox ID="txt_Aadharcardno" runat="server" Width="314px" CssClass="textbox textbox1"
                                                                    MaxLength="25" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" AutoPostBack="true" ontextchanged="txtAadharcardno"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender32" runat="server" TargetControlID="txt_Aadharcardno"
                                                                    FilterType="Custom,Numbers,LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Identification
                                                                    Mark 2</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtIdMark2" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="100" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" placeholder="Identification Mark" Style="width: 314px;
                                                                    float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender84" runat="server" TargetControlID="txtIdMark2"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Height</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_height" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="4" placeholder="Height" Style="float: left; width: 100px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender115" runat="server" TargetControlID="txt_height"
                                                                    FilterType="numbers,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left; margin-top: 8px;">
                                                                    Weight</span>
                                                                <asp:TextBox ID="txt_weight" CssClass="textbox textbox1" Visible="true" runat="server"
                                                                    MaxLength="3" placeholder="Weight" Style="float: left; width: 80px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender116" runat="server" TargetControlID="txt_weight"
                                                                    FilterType="numbers,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <%--  <legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Parents's Details</legend>--%>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold;">Parents's Details</span>
                                            </div>
                                            <div style="width: 100%;">
                                                <div style="width: 50%; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Name of the Father
                                                                    (In English)</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txt_parentsname" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="text-transform: uppercase; width: 280px;" placeholder="Father's Name"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_parentsname"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fathernametamil" Visible="false" CssClass="textbox textbox1"
                                                                    MaxLength="50" runat="server" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                    onkeydown="return (event.keyCode!=13);" Style="width: 280px;"></asp:TextBox>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Qualification</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlfaterquali" CssClass="textbox textbox1" Width="100px" Height="30px"
                                                                    runat="server" Style="float: left;" onchange="return ddlFatherQua();">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_faterquali" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                    MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Annual Income</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_annualincome" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="10" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Father's Income" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" runat="server" TargetControlID="txt_annualincome"
                                                                    FilterType="Numbers,Custom" ValidChars=". ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">E-mail ID</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fatheremail" CssClass="textbox textbox1" runat="server" onfocus="myFunction(this)"
                                                                    MaxLength="60" onkeydown="return (event.keyCode!=13);" placeholder="Father's E-mail ID"
                                                                    onblur="return checkEmail(this)" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender53" runat="server" TargetControlID="txt_fatheremail"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars="@._ ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Occupation</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddloccupation" CssClass="textbox3 textbox1" runat="server"
                                                                    onblur="blurFunction(this)" onfocus="myFunction(this)" onchange="return occupat(this)"
                                                                    Style="width: 130px; float: left;">
                                                                </asp:DropDownList>
                                                                <%-- </td>
                                                <td>--%>
                                                                <asp:TextBox ID="txt_occupation" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Occupation" Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_occupation"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;">Contact No 1</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fathercontactno" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="16" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Father Contact No" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender52" runat="server" TargetControlID="txt_fathercontactno"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Name of the guardian
                                                                    (if living with guardian)</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_guardiananme" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Name of guardian" Style="width: 280px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_guardiananme"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <%--Deepali on 7.4.18--%>
                                                          <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;float:left;">Contact No 2</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fathercontactno2" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="16" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Father Contact No 2" Style="width: 150px;float:left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender161" runat="server" TargetControlID="txt_fathercontactno2"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 50%; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;">Name of the Mother</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txt_mothername" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Mother's Name" Style="text-transform: uppercase; width: 280px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender54" runat="server" TargetControlID="txt_mothername"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" .">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Qualification</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlmotherqual" CssClass="textbox textbox1" Width="100px" Height="30px"
                                                                    runat="server" Style="float: left;" onchange="return ddlMotherQua();">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_motherqual" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                    MaxLength="50" Style="float: left; display: none;"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;">Mother's Annual Income</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_motherannualincome" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="10" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Mother's Income" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txt_motherannualincome"
                                                                    FilterType="Numbers,Custom" ValidChars=". ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small;">E-mail ID</span>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txt_motheremail" CssClass="textbox textbox1" runat="server" onfocus="myFunction(this)"
                                                                    MaxLength="60" onkeydown="return (event.keyCode!=13);" placeholder="Mother's E-mail ID"
                                                                    Style="width: 280px;" onblur="return checkEmail(this)"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender59" runat="server" TargetControlID="txt_motheremail"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars="@._ ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Occupation</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlmotheroccupation" CssClass="textbox3 textbox1" runat="server"
                                                                    onblur="blurFunction(this)" onfocus="myFunction(this)" onchange="return occupat1(this)"
                                                                    Style="width: 130px; float: left;">
                                                                </asp:DropDownList>
                                                                <%--</td>
                                                <td>--%>
                                                                <asp:TextBox ID="txt_motheroccupation" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Occupation" Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderF19" runat="server" TargetControlID="txt_motheroccupation"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Contact No</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_mothercontactno" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="16" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Mother Contact No" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_mothercontactno"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Guardian Contact
                                                                    No</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_guardianconatactno" CssClass="textbox textbox1" runat="server"
                                                                    MaxLength="16" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    placeholder="Guardian Contact No" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender60" runat="server" TargetControlID="txt_guardianconatactno"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Guardian E-mail
                                                                    ID</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_guardianemail" CssClass="textbox textbox1" runat="server" onfocus="myFunction(this)"
                                                                    MaxLength="60" onkeydown="return (event.keyCode!=13);" onblur="return checkEmail(this)"
                                                                    placeholder="E-mail ID" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender61" runat="server" TargetControlID="txt_guardianemail"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars="@._ ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <fieldset>
                                            <%-- <legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Same as Address for Communication</legend>--%>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold;">Permanent Address</span>
                                            </div>
                                            <%--<center>
                                                    <div style="width: 350px;">
                                                        <span style="font-family: Sans-Serif; font-size: small; float: left;">Same as Address
                                                            for Communication</span><asp:RadioButton ID="rdbsameyes" runat="server" onclick="return FnautoAddressFill()"
                                                                Text="Yes" GroupName="Same9" Checked="true" onchange="return showid(this)" Style="font-family: Sans-Serif;
                                                                font-size: small; float: left;" />
                                                        <asp:RadioButton ID="rdbsameno" runat="server" onchange="return noshow(this)" Text="No"
                                                            GroupName="Same9" Style="font-family: Sans-Serif; font-size: small; float: left;" />
                                                    </div>
                                                </center>--%>
                                            <br />
                                            
                                                <div style="width: 50%; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line1</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_caddress1" CssClass="textbox textbox1" runat="server" placeholder="House / Flat Number"
                                                                    MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 300px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" TargetControlID="txt_caddress1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line3</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_caddress3" CssClass="textbox textbox1" runat="server" placeholder="Area / Village / District"
                                                                    MaxLength="80" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 250px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender35" runat="server" TargetControlID="txt_caddress3"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">State(Foreign
                                                                    Students Choose 'Others')</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlcstate" CssClass="textbox3 textbox1" runat="server" onchange="change1(this)"
                                                                    onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)"
                                                                    Style="width: 160px; float: left;">
                                                                </asp:DropDownList>
                                                                <%-- </td>
                                                <td>--%>
                                                                <asp:TextBox ID="txt_cotherstate" CssClass="textbox textbox1" runat="server" placeholder="Specify state"
                                                                    MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" Style="width: 129px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender37" runat="server" TargetControlID="txt_cotherstate"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">PIN Code</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_cpostelcode" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="8" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender39" runat="server" TargetControlID="txt_cpostelcode"
                                                                    FilterType="Numbers" ValidChars="">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 50%; float: right;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line2</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_caddress2" CssClass="textbox textbox1" runat="server" placeholder="Name of street / Road"
                                                                    MaxLength="80" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 250px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" TargetControlID="txt_caddress2"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">City</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_ccity" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 200px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender36" runat="server" TargetControlID="txt_ccity"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()- ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Country</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlccountry" CssClass="textbox3 textbox1" runat="server" onchange="nation1change()"
                                                                    onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)"
                                                                    Style="width: 200px; float: left;">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_ccountry" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    Style="display: none; float: left;" placeholder="Other" MaxLength="50" onfocus="myFunction(this)"
                                                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" TargetControlID="txt_ccountry"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <span style="color: Red;">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Phone Number with
                                                                    (Landline) STD/ISD code</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_cstd" CssClass="textbox textbox1" runat="server" MaxLength="5"
                                                                    onkeydown="return (event.keyCode!=13);" Style="width: 60px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender40" runat="server" TargetControlID="txt_cstd"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:TextBox ID="txt_cphonenumber" CssClass="textbox textbox1" runat="server" MaxLength="8"
                                                                    onkeydown="return (event.keyCode!=13);" Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" TargetControlID="txt_cphonenumber"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                           
                                        </fieldset>
                                        <div style="width: 100%; background-color: brown; height: 25px;">
                                            <span style="color: White; font-weight: bold;">Same as Address for Communication</span>
                                            <asp:RadioButton ID="rdbsameyes" runat="server" ForeColor="White" Text="Yes" GroupName="Same9"
                                                onchange="return showid(this)" />
                                            <asp:RadioButton ID="rdbsameno" runat="server" ForeColor="White" onchange="return noshow(this)"
                                                Text="No" GroupName="Same9" />
                                        </div>
                                         <div style="width: 100%;" id="communicationdiv" runat="server">
                                        <fieldset>
                                            <%--  <legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Address For Communication</legend>--%>
                                        
                                            <div style="width: 100%;">
                                                <div style="width: 50%; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line1</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_address1" CssClass="textbox textbox1" runat="server" placeholder="House / Flat Number"
                                                                    MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 280px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderf20" runat="server" TargetControlID="txt_address1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <%-- <span style="color: Red;">*</span>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line2</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_address2" CssClass="textbox textbox1" runat="server" placeholder="Name of street / Road"
                                                                    MaxLength="80" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 250px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender210" runat="server" TargetControlID="txt_address2"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Address Line3</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_address3" CssClass="textbox textbox1" runat="server" placeholder="Area / Village / District"
                                                                    MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 250px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender119" runat="server" TargetControlID="txt_address3"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=",./()-@& ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">City</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_Pcity" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" Text="Chennai" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 250px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender33" runat="server" TargetControlID="txt_Pcity"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <%--<span style="color: Red;">*</span>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">State(Foreign
                                                                    Students Choose 'Others')</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlpstate" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onfocus="myFunction(this)" onchange="return change(this)" Style="width: 160px;
                                                                    float: left;">
                                                                </asp:DropDownList>
                                                                <%--  </td>
                                                <td>--%>
                                                                <asp:TextBox ID="txt_potherstate" CssClass="textbox textbox1" runat="server" placeholder="Specify state"
                                                                    MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    onfocus="myFunction(this)" Style="width: 110px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" TargetControlID="txt_potherstate"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <%-- <span style="color: Red; float: left;">*</span>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">PIN Code</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_postelcode" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onfocus="myFunction(this)" MaxLength="6" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 150px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_postelcode"
                                                                    FilterType="Numbers" ValidChars="">
                                                                </asp:FilteredTextBoxExtender>
                                                                <%--<span style="color: Red;">*</span>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div style="width: 50%; float: left;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Country</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlpcountry" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                                                    onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)" Style="width: 200px;
                                                                    float: left;">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txt_pcountry" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                                    MaxLength="50" Text="India" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                    Style="width: 200px; display: none; float: left;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender34" runat="server" TargetControlID="txt_pcountry"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <%--  <span style="color: Red; float: right;">*</span>--%>
                                                            </td>
                                                        </tr>
                                                       
                                                       
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">Phone Number with
                                                                    (Landline)<br />
                                                                    STD/ISD code</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_std" CssClass="textbox textbox1" runat="server" MaxLength="5"
                                                                    onkeydown="return (event.keyCode!=13);" Style="width: 60px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" TargetControlID="txt_std"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:TextBox ID="txt_phonenumber" CssClass="textbox textbox1" runat="server" MaxLength="8"
                                                                    onkeydown="return (event.keyCode!=13);" Style="width: 125px;"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" TargetControlID="txt_phonenumber"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                       
                                                    </table>
                                                </div>
                                            </div>
                                        </fieldset>
                                        </div>
                                        <asp:UpdatePanel ID="upExamDet" runat="server">
                                            <ContentTemplate>
                                                <fieldset id="educationdet" runat="server">
                                                    <%--<legend style="font-family: Sans-Serif; color: Blue; font-size: medium; font-weight: 400;">
                                    Educational Details</legend>--%>
                                                    <div style="width: 100%; background-color: brown; height: 25px;">
                                                        <span style="color: White; font-weight: bold;">Educational Details</span>
                                                    </div>
                                                    <center>
                                                        <div style="width: 100%;" runat="server" id="pgtable" visible="false">
                                                            <div style="width: 50%; float: left;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Qualifying Examination
                                                                                Passed</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlexamination" CssClass="textbox3 textbox1" runat="server"
                                                                                onchange="return qualifyexam(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                                Style="width: 160px; float: left;">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtqulifyexam" CssClass="textbox textbox1" runat="server" Width="150px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left; display: none;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender49" runat="server" TargetControlID="txtqulifyexam"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <span style="color: Red; float: left;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Mention Major</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_major" CssClass="textbox textbox1" runat="server" Width="150px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender45" runat="server" TargetControlID="txt_major"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Type of Major</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddltypeofmajor" CssClass="textbox3 textbox1" runat="server"
                                                                                onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px;">
                                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                                <asp:ListItem Value="1">Single</asp:ListItem>
                                                                                <asp:ListItem Value="2">Double</asp:ListItem>
                                                                                <asp:ListItem Value="3">Triple</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Medium of Study
                                                                                at UG level</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_mediumofstudy" CssClass="textbox textbox1" runat="server" Width="150px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender46" runat="server" TargetControlID="txt_mediumofstudy"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Registration No.
                                                                                as Mentioned on your Mark Sheet</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_regnomarksheet" CssClass="textbox textbox1" runat="server" Width="150px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender47" runat="server" TargetControlID="txt_regnomarksheet"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span style="color: Red; float: left;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Marks/Grade</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rdbugmarks" runat="server" AutoPostBack="true" Text="Marks"
                                                                                GroupName="samegrade" Style="font-family: Sans-Serif; font-size: small; float: left;
                                                                                height: 25px;" Checked="true"></asp:RadioButton><%--OnCheckedChanged="ugmarks_changed"--%>
                                                                            <asp:RadioButton ID="rdbuggrade" runat="server" AutoPostBack="true" Text="Grade"
                                                                                GroupName="samegrade" Style="font-family: Sans-Serif; font-size: small; float: left;">
                                                                            </asp:RadioButton><%--OnCheckedChanged="uggrade_changed"--%>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div style="width: 50%; float: right;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Name of the college
                                                                                last studied</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_nameofcollege" CssClass="textbox textbox1" runat="server" Width="300px"
                                                                                MaxLength="80" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txt_nameofcollege"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Location of the
                                                                                College</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_locationofcollege" CssClass="textbox textbox1" runat="server"
                                                                                MaxLength="100" Width="300px" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender43" runat="server" TargetControlID="txt_locationofcollege"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Name of University</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlUniv" CssClass="textbox3 textbox1" runat="server" onchange="return univ(this)"
                                                                                onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px; float: left;">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txt_univ" CssClass="textbox textbox1" runat="server" Width="140px"
                                                                                onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)"
                                                                                Style="display: none; float: left;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender92" runat="server" TargetControlID="txt_univ"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Type of Semester</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddltypeofsemester" CssClass="textbox3 textbox1" runat="server"
                                                                                onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px;">
                                                                                <asp:ListItem>Select</asp:ListItem>
                                                                                <asp:ListItem Value="0">Semester</asp:ListItem>
                                                                                <asp:ListItem Value="1">Non Semester</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Year Of Passing</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="DropDownList1" CssClass="textbox3 textbox1" runat="server"
                                                                                Style="width: 80px;" onfocus="myFunction(this)">
                                                                                <asp:ListItem Value="1">JAN</asp:ListItem>
                                                                                <asp:ListItem Value="2">FEB</asp:ListItem>
                                                                                <asp:ListItem Value="3">MAR</asp:ListItem>
                                                                                <asp:ListItem Value="4">APR</asp:ListItem>
                                                                                <asp:ListItem Value="5">MAY</asp:ListItem>
                                                                                <asp:ListItem Value="6">JUN</asp:ListItem>
                                                                                <asp:ListItem Value="7">JUL</asp:ListItem>
                                                                                <asp:ListItem Value="8">AUG</asp:ListItem>
                                                                                <asp:ListItem Value="9">SEP</asp:ListItem>
                                                                                <asp:ListItem Value="10">OCT</asp:ListItem>
                                                                                <asp:ListItem Value="11">NOV</asp:ListItem>
                                                                                <asp:ListItem Value="12">DEC</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:DropDownList ID="DropDownList2" CssClass="textbox3 textbox1" runat="server"
                                                                                onfocus="myFunction(this)" Style="width: 100px;">
                                                                            </asp:DropDownList>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span id="sp_cutofpg" runat="server" style="font-family: Sans-Serif; font-size: small;
                                                                                float: left;">Cut Of Mark</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_cutofpg" runat="server" CssClass="textbox1 textbox txtheight"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender110" runat="server" TargetControlID="txt_cutofpg"
                                                                                FilterType="Numbers,Custom" ValidChars=" .">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <span id="cutof" runat="server" style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <br />
                                                        <div style="width: 100%;" runat="server" id="ugtable" visible="true">
                                                            <div style="width: 50%; float: left;">
                                                                <table>
                                                                    <tr style="line-height: 30px;">
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Vocational</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButton ID="rbVocation" runat="server" Text="Yes" AutoPostBack="true" GroupName="vocRb"
                                                                                Style="font-family: Sans-Serif; font-size: small; float: left;" /><%--OnCheckedChanged="rbVocation_Changed"--%>
                                                                            <asp:RadioButton ID="rbVocation1" runat="server" Text="No" AutoPostBack="true" GroupName="vocRb"
                                                                                Checked="true" Style="font-family: Sans-Serif; font-size: small; float: left;" /><%--OnCheckedChanged="rbVocation_Changed"--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Qualifying Boards
                                                                                & State</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_board" CssClass="textbox textbox1" runat="server" Width="200px"
                                                                                MaxLength="50" placeholder="STATE BOARD/CBSE" onkeydown="return (event.keyCode!=13);"
                                                                                onblur="blurFunction(this)" onfocus="myFunction(this)" Style="float: left; text-transform: uppercase"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender65" runat="server" TargetControlID="txt_board"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:TextBox ID="txt_otherstatequalify" CssClass="textbox textbox1" placeholder="Specify state"
                                                                                MaxLength="50" runat="server" Width="100px" onkeydown="return (event.keyCode!=13);"
                                                                                onblur="blurFunction(this)" onfocus="myFunction(this)" Style="float: left; display: none;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender67" runat="server" TargetControlID="txt_otherstatequalify"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlstatequalify" CssClass="textbox3 textbox1" runat="server"
                                                                                onchange="return qualifyboard(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                                Style="width: 110px; float: left;">
                                                                            </asp:DropDownList>
                                                                            <td>
                                                                                <span style="color: Red; float: right;">*</span>
                                                                            </td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Language Studied
                                                                                in X-std</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="ddlmediumstudy" CssClass="textbox3 textbox1" runat="server"
                                                                                onchange="return meidum(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                                Style="width: 160px; float: left;">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txt_mediumstudy" CssClass="textbox textbox1" runat="server" Width="148px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left; display: none;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender70" runat="server" TargetControlID="txt_mediumstudy"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <%-- <span style="color: Red; float: left;">*</span>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Language Studied
                                                                                in XII-std</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="ddlmediumofstudyXII" CssClass="textbox3 textbox1" runat="server"
                                                                                onchange="return meidum1(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                                Style="width: 160px; float: left;">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txt_ddlmediumofStudyXII" CssClass="textbox textbox1" runat="server"
                                                                                MaxLength="50" Width="148px" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left; display: none;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender71" runat="server" TargetControlID="txt_ddlmediumofStudyXII"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <%-- <span style="color: Red; float: left;">*</span>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div style="width: 50%; float: right;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Qualifying Examination
                                                                                Passed</span>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:DropDownList ID="ddlexaminationug" CssClass="textbox3 textbox1" runat="server"
                                                                                onchange="return qualifyexam(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                                Style="width: 120px; float: left;">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txt_qulifyexam" CssClass="textbox textbox1" runat="server" Width="150px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)" Style="float: left; display: none;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender62" runat="server" TargetControlID="txt_qulifyexam"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <span style="color: Red; float: left;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Institution last
                                                                                Attended</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_Name_of_school" CssClass="textbox textbox1" runat="server" Width="300px"
                                                                                MaxLength="50" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender68" runat="server" TargetControlID="txt_Name_of_school"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td>
                                                                            <span style="color: Red;">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Institution address</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_location_scholl" CssClass="textbox textbox1" runat="server"
                                                                                MaxLength="50" Width="300px" onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                                                onfocus="myFunction(this)"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender69" runat="server" TargetControlID="txt_location_scholl"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="line-height: 30px;">
                                                                        <td>
                                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">Marks/Grade</span>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="rbMarksGrade" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbMarksGrade_IndexChange"
                                                                                CssClass="textbox3 textbox1" Style="width: 120px;">
                                                                                <asp:ListItem Selected="True">Marks</asp:ListItem>
                                                                                <asp:ListItem>Grade</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <span id="sp_cutof" runat="server" style="font-family: Sans-Serif; font-size: small;">
                                                                                Cut Of</span>
                                                                            <asp:TextBox ID="txt_cutof" runat="server" CssClass="textbox1 textbox txtheight"></asp:TextBox><span
                                                                                id="cutoug" runat="server" style="color: Red;">*</span>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender109" runat="server" TargetControlID="txt_cutof"
                                                                                FilterType="Numbers,Custom" ValidChars=" .">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </center>
                                                </fieldset>
                                                <center>
                                                    <fieldset id="ExmarkstQua" runat="server">
                                                        <div style="width: 100%; background-color: brown; height: 25px;">
                                                            <span style="color: White; font-weight: bold;">Extract of the Mark Statement/s of the
                                                                Qualifying Examination Passed(Attach Attested Copies)</span>
                                                        </div>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <center>
                                                                        <asp:UpdatePanel ID="upUg" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="UgGrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                                    Visible="true" OnRowDataBound="ugbindgrid_Bound" GridLines="None" OnRowCommand="uggirdrowcommand"
                                                                                    OnDataBound="uggridDatabound" Width="600px">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Language" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Width="102px" Text='<%# Eval("Type") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="& Subjects" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlugsubject" CssClass="textbox3 textbox1" runat="server" OnSelectedIndexChanged="ddlugsubject_Change" AutoPostBack="true" onblur="blurFunction(this)"
                                                                                                    onfocus="myFunction(this)" Style="width: 160px; height: 27px;">
                                                                                                </asp:DropDownList>
                                                                                                <asp:TextBox ID="txt_subject56" runat="server" CssClass="text7" onkeydown="return (event.keyCode!=13);"
                                                                                                    MaxLength="50" Style="text-align: left; text-transform: none;" Width="150px"
                                                                                                    Visible="false">
                                                                                                </asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1415" runat="server" TargetControlID="txt_subject56"
                                                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Register No / Roll No" HeaderStyle-BackColor="#13F2DF">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ugregno" runat="server" CssClass="text7" Text='<%# Eval("reg") %>'
                                                                                                    MaxLength="25" onkeydown="return (event.keyCode!=13);" onblur="return Regnofill(this)"
                                                                                                    Style="text-align: center;" Width="130px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender145" runat="server" TargetControlID="txt_ugregno"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="300px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Marks Obtained" HeaderStyle-BackColor="#13F2DF">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ugmagrs" runat="server" Text='<%# Eval("actmark") %>' CssClass="text7"
                                                                                                    MaxLength="3" onkeydown="return (event.keyCode!=13);" Style="text-align: center;"
                                                                                                    onkeyup="this.value = minmax(this.value, 0, 200)" onblur="return addmarks(this)"
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender141" runat="server" TargetControlID="txt_ugmagrs"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ug_grade" runat="server" CssClass="text7" onkeydown="return (event.keyCode!=13);"
                                                                                                    MaxLength="2" Style="text-align: center;" Width="60px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender142" runat="server" TargetControlID="txt_ug_grade"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="+">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maximum Marks" HeaderStyle-BackColor="#13F2DF">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_rg_max" runat="server" CssClass="text7 " Text='<%# Eval("maximark") %>'
                                                                                                    MaxLength="3" onblur="return addavg(this)" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                                    onkeydown="return (event.keyCode!=13);" Style="text-align: center;" Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender147" runat="server" TargetControlID="txt_rg_max"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Month (Of" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="Right">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ugmonth" runat="server" Text='<%# Eval("passmonth") %>' CssClass="text7"
                                                                                                    MaxLength="3" onkeydown="return (event.keyCode!=13);" placeholder="eg:May" Style="text-align: center;
                                                                                                    text-transform: none;" onblur="this.value=checkmonth(this)" onfocus="myFunction(this)"
                                                                                                    onchange="Monthfill(this)" Width="60px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender143" runat="server" TargetControlID="txt_ugmonth"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText=" & Year Passing)" HeaderStyle-BackColor="#13F2DF"
                                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ugyear" runat="server" Text='<%# Eval("passyear") %>' CssClass="text7"
                                                                                                    MaxLength="4" onkeydown="return (event.keyCode!=13);" placeholder="eg:2016" Style="text-align: center;"
                                                                                                    onfocus="myFunction(this)" onblur="this.value=checkyear(this)" onchange="Yearfill(this)"
                                                                                                    Width="60px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender144" runat="server" TargetControlID="txt_ugyear"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="No.of Attempts" HeaderStyle-BackColor="#13F2DF">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_ugnoofattmpts" Text='<%# Eval("noofatm") %>' runat="server"
                                                                                                    CssClass="text7" MaxLength="2" onkeyup="this.value = minmax(this.value, 0, 20)"
                                                                                                    onkeydown="return (event.keyCode!=13);" onblur="return Attemptsfill(this);" Style="text-align: center;"
                                                                                                    Width="50px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender146" runat="server" TargetControlID="txt_ugnoofattmpts"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                        <table id="pgaddmark" runat="server" visible="false">
                                                                            <tr>
                                                                                <td>
                                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Total Number of
                                                                                        Papers in All Semesters</span><br />
                                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left; color: #ff0000;">
                                                                                        (Applicant should type all the papers Studied)</span>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtTotSem" CssClass="textbox textbox1" runat="server" Width="50px"
                                                                                        onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)"
                                                                                        MaxLength="2" onchange="return txtSemChangePG(this)" Text="1" Style="float: left;"></asp:TextBox>
                                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender108" runat="server" TargetControlID="txtTotSem"
                                                                                        FilterType="NUmbers">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                    <span style="color: Red; float: left;">*</span>
                                                                                    <asp:Button ID="btnGrid" runat="server" Text="Generate" OnClick="btnGrid_OnClick"
                                                                                        CssClass="textbox textbox1 type" BackColor="Brown" ForeColor="White" Width="90px"
                                                                                        Height="30px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <asp:UpdatePanel ID="upPg" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:GridView ID="Marksgrid" runat="server" Visible="False" AutoGenerateColumns="False"
                                                                                    CellPadding="4" OnRowDataBound="Marksgrid_Bound" OnRowCommand="Marksgrid_Rowcommand"
                                                                                    OnRowDeleting="OnRowDeleting" OnDataBound="Marksgrid_pg_DataBound" GridLines="None">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Sem/Year" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_semoryear" runat="server" CssClass="text7" MaxLength="50" Style="text-align: center;
                                                                                                    text-transform: capitalize;" onkeydown="return (event.keyCode!=13);" Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1411" runat="server" TargetControlID="txt_semoryear"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars="/ ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_pgsubject" runat="server" CssClass="text7" MaxLength="50" Style="text-align: center;
                                                                                                    text-transform: none;" onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                                                    Width="150px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender141" runat="server" TargetControlID="txt_pgsubject"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddltypeofsubject" CssClass="textbox3 textbox1" runat="server"
                                                                                                    onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px; height: 27px;">
                                                                                                </asp:DropDownList>
                                                                                                <asp:TextBox ID="txt_typeofsubject" runat="server" CssClass="text7" MaxLength="50"
                                                                                                    Style="text-align: center; text-transform: capitalize;" onkeydown="return (event.keyCode!=13);"
                                                                                                    Width="150px" Height="17px" Visible="false">
                                                                                                </asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1415" runat="server" TargetControlID="txt_typeofsubject"
                                                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Marks" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_marks" runat="server" CssClass="text7" MaxLength="3" Style="text-align: center;"
                                                                                                    onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);" Width="60px"
                                                                                                    onkeyup="this.value = minmax(this.value, 0, 200)"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender143" runat="server" TargetControlID="txt_marks"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_Grade" runat="server" CssClass="text7" MaxLength="2" Style="text-align: center;"
                                                                                                    onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);" Width="60px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender144" runat="server" TargetControlID="txt_Grade"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="+ ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_pgmonth" runat="server" CssClass="text7" MaxLength="5" onkeydown="return (event.keyCode!=13);"
                                                                                                    onblur="this.value=checkmonth(this)" Style="text-align: center; text-transform: capitalize;"
                                                                                                    Width="50px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender145" runat="server" TargetControlID="txt_pgmonth"
                                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_pgyear" runat="server" CssClass="text7" MaxLength="4" onkeydown="return (event.keyCode!=13);"
                                                                                                    onblur="this.value=checkyear(this)" Style="text-align: center;" Width="50px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender146" runat="server" TargetControlID="txt_pgyear"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Maximum Marks" HeaderStyle-BackColor="LightBlue">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txt_maximum_marks" runat="server" CssClass="text7" MaxLength="3"
                                                                                                    onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                                    Style="text-align: center;" Width="80px"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender147" runat="server" TargetControlID="txt_maximum_marks"
                                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </center>
                                                                    <center>
                                                                        <asp:GridView ID="GridView_mphil" runat="server" Visible="true" AutoGenerateColumns="False"
                                                                            CellPadding="4" OnRowDataBound="GridView_mphil_Bound" OnRowCommand="GridView_mphil_Rowcommand"
                                                                            OnRowDeleting="GridView_mphil_OnRowDeleting" OnDataBound="GridView_mphil_DataBound"
                                                                            GridLines="None">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Sem/Year" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_semoryear" runat="server" CssClass="text7" Style="text-align: center;
                                                                                            text-transform: capitalize;" onkeydown="return (event.keyCode!=13);" Width="50px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1411" runat="server" TargetControlID="txt_semoryear"
                                                                                            FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars="/ ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_pgsubject" runat="server" CssClass="text7" Style="text-align: center;
                                                                                            text-transform: none;" onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                                            Width="100px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender141" runat="server" TargetControlID="txt_pgsubject"
                                                                                            FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddltypeofsubject" CssClass="textbox3 textbox1" runat="server"
                                                                                            onblur="blurFunction(this)" onfocus="myFunction(this)" Style="width: 160px; height: 27px;">
                                                                                        </asp:DropDownList>
                                                                                        <asp:TextBox ID="txt_typeofsubject" runat="server" CssClass="text7" Style="text-align: center;
                                                                                            text-transform: capitalize;" onkeydown="return (event.keyCode!=13);" Width="150px"
                                                                                            Height="17px" Visible="false">
                                                                                        </asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1415" runat="server" TargetControlID="txt_typeofsubject"
                                                                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Internal Marks" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_inter" runat="server" CssClass="text7" MaxLength="3" Style="text-align: center;"
                                                                                            onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);" Width="60px"
                                                                                            onkeyup="this.value = minmax(this.value, 0, 100)" onblur="return addinternalexternal(this)"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1432" runat="server" TargetControlID="txt_inter"
                                                                                            FilterType="Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="External Marks" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_exter" runat="server" CssClass="text7" MaxLength="3" Style="text-align: center;"
                                                                                            onkeydown="return (event.keyCode!=13);" Width="60px" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                            onblur="return addinternalexternal(this)"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1436" runat="server" TargetControlID="txt_exter"
                                                                                            FilterType="Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Marks" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_marks" runat="server" CssClass="text7" MaxLength="3" Style="text-align: center;"
                                                                                            onkeydown="return (event.keyCode!=13);" Width="60px" onkeyup="this.value = minmax(this.value, 0, 100)"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender143" runat="server" TargetControlID="txt_marks"
                                                                                            FilterType="Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_Grade" runat="server" CssClass="text7" MaxLength="2" Style="text-align: center;"
                                                                                            onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);" Width="60px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender144" runat="server" TargetControlID="txt_Grade"
                                                                                            FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="+ ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_pgmonth" runat="server" CssClass="text7" onkeydown="return (event.keyCode!=13);"
                                                                                            onblur="this.value=checkmonth(this)" Style="text-align: center; text-transform: capitalize;"
                                                                                            Width="50px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender145" runat="server" TargetControlID="txt_pgmonth"
                                                                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_pgyear" runat="server" CssClass="text7" MaxLength="4" onkeydown="return (event.keyCode!=13);"
                                                                                            onblur="this.value=checkyear(this)" Style="text-align: center;" Width="50px"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender146" runat="server" TargetControlID="txt_pgyear"
                                                                                            FilterType="Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Maximum Marks" HeaderStyle-BackColor="LightBlue">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txt_maximum_marks" runat="server" CssClass="text7" MaxLength="3"
                                                                                            onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                            Style="text-align: center;" Width="50px" onblur="return addavgmphil(this)"></asp:TextBox>
                                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender147" runat="server" TargetControlID="txt_maximum_marks"
                                                                                            FilterType="Numbers" ValidChars=" ">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle BackColor="#13F2DF"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                </asp:TemplateField>
                                                                                <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </center>
                                                                </td>
                                                                <td style="vertical-align: text-top;">
                                                                    <span style="color: Red;">*</span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <center>
                                                            <div style="margin-left: 30px;">
                                                                <asp:GridView ID="ugsubgrid" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                    Visible="true" ShowHeader="False" OnRowDataBound="ugsubbindgrid_Bound" GridLines="None"
                                                                    Width="600px" OnRowDeleting="OnRowDeleting_ugsubgrid">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Language & Subjects" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblsno" runat="server" ForeColor="Brown" Width="102px" Text='<%# Eval("Type") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Subjects" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subsubject567" runat="server" CssClass="text7" MaxLength="50"
                                                                                    Style="text-align: center; text-transform: none;" onkeydown="return (event.keyCode!=13);"
                                                                                    Width="150px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14555" runat="server" TargetControlID="txt_subsubject567"
                                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Register No" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subugregno" runat="server" CssClass="text7" MaxLength="25" Style="text-align: center;"
                                                                                    Width="130px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14511" runat="server" TargetControlID="txt_subugregno"
                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="200px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Marks" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subugmagrs" runat="server" CssClass="text7" MaxLength="3" onblur="return addmarks(this)"
                                                                                    Style="text-align: center;" onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                    Width="80px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1417" runat="server" TargetControlID="txt_subugmagrs"
                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Grade" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subug_grade" runat="server" CssClass="text7" MaxLength="2" Style="text-align: center;"
                                                                                    Width="60px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1428" runat="server" TargetControlID="txt_subug_grade"
                                                                                    FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars="+ ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Maximum Marks" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subug_max" runat="server" CssClass="text7" MaxLength="3" onblur="return addavg(this)"
                                                                                    onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 200)"
                                                                                    Style="text-align: center;" Width="80px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14713" runat="server" TargetControlID="txt_subug_max"
                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subugmonth" runat="server" CssClass="text7" MaxLength="3" Style="text-align: center;
                                                                                    text-transform: capitalize;" onkeydown="return (event.keyCode!=13);" onblur="this.value=checkmonth(this)"
                                                                                    Width="60px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1439" runat="server" TargetControlID="txt_subugmonth"
                                                                                    FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subugyear" runat="server" CssClass="text7" MaxLength="4" Style="text-align: center;"
                                                                                    onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);" onblur="this.value=checkyear(this)"
                                                                                    Width="60px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14410" runat="server" TargetControlID="txt_subugyear"
                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="No.of Attempts" HeaderStyle-BackColor="LightBlue">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txt_subugnoofattmpts" runat="server" CssClass="text7" MaxLength="2"
                                                                                    onkeydown="return (event.keyCode!=13);" onkeyup="this.value = minmax(this.value, 0, 20)"
                                                                                    Style="text-align: center;" Width="50px"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14612" runat="server" TargetControlID="txt_subugnoofattmpts"
                                                                                    FilterType="Numbers" ValidChars=" ">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="LightBlue"></HeaderStyle>
                                                                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                                        </asp:TemplateField>
                                                                        <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="totalfinddiv" runat="server" visible="true" style="height: 50px; text-align: right;">
                                                                <span>Total Marks Obtained :</span><asp:TextBox ID="txt_totalmark" CssClass="text7"
                                                                    MaxLength="5" runat="server" Width="80px"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="fiteler1" runat="server" TargetControlID="txt_totalmark"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;<span>Maximum Marks :</span><asp:TextBox ID="avgtext" runat="server"
                                                                    MaxLength="5" CssClass="text7" Width="80px"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender211" runat="server" TargetControlID="avgtext"
                                                                    FilterType="Numbers" ValidChars=" ">
                                                                </asp:FilteredTextBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;&nbsp; <span>Percentage :</span><asp:TextBox ID="txt_percentage"
                                                                    MaxLength="5" CssClass="text7" runat="server" Width="80px"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender48" runat="server" TargetControlID="txt_percentage"
                                                                    FilterType="Numbers,Custom" ValidChars=". ">
                                                                </asp:FilteredTextBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </div>
                                                            <br />
                                                            <div style="text-align: right; width: 90%;">
                                                                <asp:Button ID="btnugaddrows" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                                                    ForeColor="White" Visible="true" Width="145px" Height="26px" OnClick="btnugaddrows_Click"
                                                                    Text="Add Another Subject" />
                                                            </div>
                                                            <div id="ugdiv" runat="server" visible="true">
                                                                <table>
                                                                </table>
                                                            </div>
                                                        </center>
                                                        <div style="text-align: right; width: 90%;">
                                                            <asp:Button ID="btn_addrows" runat="server" CssClass="textbox textbox1 type" BackColor="Brown"
                                                                ForeColor="White" Visible="false" Width="81px" Height="26px" OnClick="btnaddrows_Click"
                                                                Text="Add Rows" />
                                                        </div>
                                                        <table id="pgmarkvalue" runat="server" visible="true">
                                                            <tr>
                                                                <td>
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Total percentage
                                                                        of marks in all subjects (Language/major/Allied/Ancillary/Elective inclusive ofTheory
                                                                        and Practical</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_allsubjectpercentage" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender122" runat="server" TargetControlID="txt_allsubjectpercentage"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <span style="color: Red;">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Total % of Marks
                                                                        in Major subjects alone (Including theory & Practicals)</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_majorsubjectpercentage" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender123" runat="server" TargetControlID="txt_majorsubjectpercentage"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <span style="color: Red;">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Total percentage
                                                                        marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_majorandalliedsubjectpercentage" runat="server" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        MaxLength="4" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender124" runat="server" TargetControlID="txt_majorandalliedsubjectpercentage"
                                                                        FilterType="Numbers,Custom" ValidChars=".  ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <span style="color: Red;">*</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="INTERNAL" runat="server" visible="false" style="font-family: Sans-Serif;
                                                                        font-size: small; float: left;">Total percentage of Internal Mark</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_internal" runat="server" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                        Width="80px" Visible="false" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        MaxLength="4" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender111" runat="server" TargetControlID="txt_internal"
                                                                        FilterType="Numbers,Custom" ValidChars=".  ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <%--<span style="color: Red;">*</span>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span id="external" runat="server" visible="false" style="font-family: Sans-Serif;
                                                                        font-size: small; float: left;">Total percentage of External Mark</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_external" runat="server" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                                        Width="80px" Visible="false" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        MaxLength="4" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender112" runat="server" TargetControlID="txt_external"
                                                                        FilterType="Numbers,Custom" ValidChars=".  ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <%--<span style="color: Red;">*</span>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table id="pgnewmark" runat="server" visible="false">
                                                            <tr>
                                                                <th align="center">
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Parts</span>
                                                                </th>
                                                                <th align="center">
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Marks Secured
                                                                    </span>
                                                                </th>
                                                                <th align="center">
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Maximum Marks</span>
                                                                </th>
                                                                <th align="center">
                                                                    <span style="font-family: Sans-Serif; font-size: small; float: left;">Percentage</span>
                                                                </th>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span>Part - I</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_SecMrk1" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender93" runat="server" TargetControlID="txt_SecMrk1"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_maxmark1" runat="server" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender94" runat="server" TargetControlID="txt_maxmark1"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_avg1" runat="server" MaxLength="6" onblur="blurFunction(this)"
                                                                        Width="80px" CssClass="text7" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender95" runat="server" TargetControlID="txt_avg1"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span>Part - II</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_SecMrk2" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender96" runat="server" TargetControlID="txt_SecMrk2"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_maxmark2" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender97" runat="server" TargetControlID="txt_maxmark2"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_avg2" runat="server" MaxLength="6" onblur="blurFunction(this)"
                                                                        Width="80px" CssClass="text7" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender98" runat="server" TargetControlID="txt_avg2"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span>Part - III</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_SecMrk3" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender99" runat="server" TargetControlID="txt_SecMrk3"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_maxmark3" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender100" runat="server" TargetControlID="txt_maxmark3"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_avg3" runat="server" MaxLength="6" onblur="blurFunction(this)"
                                                                        Width="80px" CssClass="text7" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender101" runat="server" TargetControlID="txt_avg3"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span>Part - IV</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_SecMrk4" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender102" runat="server" TargetControlID="txt_SecMrk4"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_maxmark4" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender103" runat="server" TargetControlID="txt_maxmark4"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_avg4" runat="server" MaxLength="6" onblur="blurFunction(this)"
                                                                        Width="80px" CssClass="text7" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender104" runat="server" TargetControlID="txt_avg4"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <span>Part - V</span>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_SecMrk5" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender105" runat="server" TargetControlID="txt_SecMrk5"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_maxmark5" runat="server" MaxLength="4" onblur="blurFunction(this)"
                                                                        onfocus="myFunction(this)" Width="80px" CssClass="text7"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender106" runat="server" TargetControlID="txt_maxmark5"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_avg5" runat="server" MaxLength="6" onblur="blurFunction(this)"
                                                                        Width="80px" CssClass="text7" onkeyup="this.value = minmax(this.value, 0, 100)"
                                                                        onfocus="myFunction(this)"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender107" runat="server" TargetControlID="txt_avg5"
                                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                       
                                                                  <br />
                                        <fieldset>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold; float: left; width: 950px; text-align: center;">
                                                    Referred By</span>
                                            </div>
                                            <div style="height: 150px; overflow: auto;">
                                                <table>
                                                    <tr>
                                                        <td colspan="6">
                                                            <asp:RadioButtonList ID="rblReferredBy" runat="server" RepeatDirection="Horizontal"
                                                                onchange="return referChange();">
                                                                <asp:ListItem Selected="True">Direct</asp:ListItem>
                                                                <asp:ListItem>Referred By Staff</asp:ListItem>
                                                                 <asp:ListItem>Referred By Student</asp:ListItem>
                                                                  <asp:ListItem>Others</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <div id="divReferStaff" runat="server" style="display: none;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblStaffId" runat="server" Text="" Visible="false"></asp:Label>
                                                                            StaffName
                                                                            <asp:DropDownList ID="txtname_staff" CssClass="textbox textbox1" Width="300px" Height="30px"
                                                                                runat="server">
                                                                            </asp:DropDownList>
                                                                            <%--<asp:TextBox ID="txtname_staff" runat="server" placeholder="Name" CssClass="textbox txtheight4"
                                                                            MaxLength="60" OnTextChanged="txtname_staff_Changed" AutoPostBack="true"></asp:TextBox>
                                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_staff"
                                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                            CompletionListItemCssClass="panelbackground">
                                                                        </asp:AutoCompleteExtender>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                      <td colspan="6">
                                                            <div id="divReferStudent" runat="server" style="display: none;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            StudentName
                                                                            <asp:DropDownList ID="ddl_ReferStudent" CssClass="textbox textbox1" Width="300px" Height="30px"
                                                                                runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <div id="divReferOther" runat="server" style="display: none;">
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="6">
                                                                            <span style="float: left; padding-top: 5px;">Consultant/ Name</span>
                                                                            <asp:DropDownList ID="ddlrefConsName" CssClass="textbox textbox1" Width="120px" Height="30px"
                                                                                runat="server" Style="float: left;" onchange="return ddlrefConsNam();">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtrefConsName" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                                            <span style="float: left; padding-top: 5px;">Agent Name</span>
                                                                            <asp:DropDownList ID="ddlRefAgent" CssClass="textbox textbox1" Width="120px" Height="30px"
                                                                                runat="server" Style="float: left;" onchange="return ddlRefAgen();">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtRefAgent" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                                            <span style="float: left; padding-top: 5px;">Amount Rs</span>
                                                                            <asp:TextBox ID="txtRefAmt" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="8" Style="float: left;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender91" runat="server" TargetControlID="txtRefAmt"
                                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                                            </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="6">
                                                                            <span style="float: left; padding-top: 5px;">Address</span>
                                                                            <asp:TextBox ID="txtRefAddres" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="150" placeholder="Address" Style="float: left;"></asp:TextBox>
                                                                            <asp:TextBox ID="txtRefCity" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="50" placeholder="City" Style="float: left;"></asp:TextBox>
                                                                            <asp:TextBox ID="txtRefDist" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="50" placeholder="District" Style="float: left;"></asp:TextBox>
                                                                            <span style="float: left; padding-top: 5px;">State</span>
                                                                            <asp:DropDownList ID="ddlRefState" CssClass="textbox textbox1" Width="120px" Height="30px"
                                                                                runat="server" Style="float: left;" onchange="return ddlRefStat();">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtRefState" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="50" placeholder="State" Style="display: none; float: left;"></asp:TextBox>
                                                                            <asp:TextBox ID="txtRefPhone" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                                MaxLength="16" placeholder="Phone No" Style="float: left;"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender89" runat="server" TargetControlID="txtRefPhone"
                                                                                FilterType="Numbers,Custom" ValidChars="">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:TextBox ID="txtRefEmail" runat="server" CssClass="textbox textbox1 txtheight4"
                                                                                MaxLength="60" onchange="return validateCaseSensitiveEmail(this)" placeholder="Email ID"
                                                                                Style="float: left; text-transform: none;"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                                    ID="FilteredTextBoxExtender90" runat="server" TargetControlID="txtRefEmail" FilterType="Numbers, LowercaseLetters, Custom"
                                                                                    ValidChars=".@_">
                                                                                </asp:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                        <br />
                                        <center>
                                            <asp:Button ID="btnOpenAca" runat="server" Text="Add Academic Info" BackColor="Brown"
                                                ForeColor="White" Style="width: 140px; height: 40px;" OnClick="openAcademic" visible="false" />
                                            <asp:Button ID="btnOpenBnk" runat="server" Text="Add Bank Info" BackColor="Brown"
                                                ForeColor="White" Style="width: 140px; height: 40px;" OnClick="openBank" visible="false" />
                                            <asp:Button ID="btnOpenRel" runat="server" Text="Add Relation Info" BackColor="Brown"
                                                ForeColor="White" Style="width: 140px; height: 40px;" OnClick="openRelation" visible="false"/>
                                            <asp:Button ID="btn_addcertificate" runat="server" Text="Add Certificate" BackColor="Brown"
                                                ForeColor="White" Style="width: 140px; height: 40px;" OnClick="btn_addcertificate_Click" visible="false"/></center>
                                        <br />
                                        <%-- <fieldset>--%>
                                        <%--<div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold;">
                                                List of Enclosures</span>
                                                </div>--%>
                                        <div id="Div3" runat="server" visible="false">
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">(i) Community
                                                                Certificate</span> &nbsp;&nbsp;
                                                            <asp:FileUpload ID="Communityupload" runat="server" />
                                                        </td>
                                                        <td>
                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">(ii) Sports Certificate</span>
                                                            &nbsp;&nbsp;
                                                            <asp:FileUpload ID="sportupload" runat="server" />
                                                        </td>
                                                        <td>
                                                            <span style="font-family: Sans-Serif; font-size: small; float: left;">(iii) Differently
                                                                abled Certificate</span> &nbsp;&nbsp;
                                                            <asp:FileUpload ID="differentlyabledupload" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(iv) Ex-Serviceman
                                                                    Certificate</span> &nbsp;&nbsp;
                                                                <asp:FileUpload ID="exupload" runat="server" />
                                                            </td>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(v) HSC/Equivalent
                                                                    Mark List</span> &nbsp;&nbsp;
                                                                <asp:FileUpload ID="marksheetupload" runat="server" />
                                                            </td>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(vi) SSLC Certificate</span>
                                                                &nbsp;&nbsp;
                                                                <asp:FileUpload ID="sslcupload" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(vii) Student
                                                                    Name Proof(English & Tamil)</span> &nbsp;&nbsp;
                                                                <asp:FileUpload ID="studentproof" runat="server" />
                                                            </td>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(viii) Father
                                                                    Name Proof(English & Tamil)</span> &nbsp;&nbsp;
                                                                <asp:FileUpload ID="fatherProof" runat="server" />
                                                            </td>
                                                            <td>
                                                                <span style="font-family: Sans-Serif; font-size: small; float: left;">(ix) Applicant's
                                                                    Photo</span> &nbsp;&nbsp;
                                                                <asp:FileUpload ID="photoupload" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Button ID="btn_upload" CssClass="textbox textbox1 type" runat="server" BackColor="Brown"
                                                                    ForeColor="White" onkeyup="return chekpassvalue()" Style="width: 100px; height: 35px;"
                                                                    Text="Upload Files" OnClick="btn_upload_Click" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <p style="font-family: Sans-Serif; font-size: small; float: left;">
                                                                The following certificates should be in pdf or doc format<br />
                                                                (i) Community Certificate (ii) Sports Certificate (iii) Differently abled Certificate
                                                                (iv) Ex-Serviceman Certificate (v) HSC/Equivalent Mark List (vi) SSLC Certificate
                                                                (vii) Student Name Proof(English & Tamil) (viii) Father Name Proof(English & Tamil)<br>
                                                                (ix) Applicant's Photo should be in jpeg format and is mandatory(*)</p>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                        <%-- </fieldset>--%>
                                        <div id="div_cer" runat="server" visible="false" style="width: 100%; background-color: brown;
                                            height: 25px;">
                                            <span style="color: White; font-weight: bold;">Certificate Details</span>
                                        </div>
                                        <br />
                                        <center>
                                            <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                                                OnRowDataBound="OnRowDataBound_gv1">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Certificate" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_certificate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                            <asp:Label ID="lbl_certval" Visible="false" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                                            <asp:Label ID="Label1" Visible="false" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>'></asp:Label>
                                                            <asp:Label ID="Label2" Visible="false" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Orginal" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk_orginal" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duplicate" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chk_Duplicate" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Certificate No" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_certificate" ReadOnly="false" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="dsd" runat="server" TargetControlID="txt_certificate"
                                                                FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars="  ">
                                                            </asp:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Browse" HeaderStyle-BackColor="#13F2DF" HeaderStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:FileUpload ID="file_uplod" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </center>
                                        <br />
                                        <asp:Button ID="btn_fromate2update" CssClass="textbox textbox1 type" runat="server"
                                            BackColor="Brown" ForeColor="White" Visible="false" OnClientClick="return checkForcertificateno()"
                                            onkeyup="return chekpassvalue()" Style="width: 140px; height: 40px;" Text="Update"
                                            OnClick="btn_fromate2update_click" />



                                        <asp:Button ID="btnstept3" CssClass="textbox textbox1 type" runat="server" BackColor="Brown"
                                            ForeColor="White" OnClientClick="return checkApplicationDetailssave(this); " onkeyup="return chekpassvalue()"
                                            Style="width: 140px; height: 40px;" Text="Save" OnClick="btnstept3_click" />
                                        <asp:Button ID="btnDeleteStud" CssClass="textbox textbox1 type" runat="server" BackColor="Brown"
                                            Visible="false" ForeColor="White" Style="width: 140px; height: 40px;" Text="Delete"
                                            OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnExit" CssClass="textbox textbox1 type" runat="server" BackColor="Brown"
                                            ForeColor="White" Style="width: 100px; height: 40px;" Text="Exit" OnClick="btnExit2_Click" />
                                    </center>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tabpanel2" runat="server" HeaderText="Additional Information" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="false" TabIndex="2">
                                <ContentTemplate>
                                    <div id="step2" runat="server">
                                        <div style="width: 100%; background-color: brown; height: 25px;">
                                            <span style="color: White; font-weight: bold;">Additional Information</span>
                                        </div>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Are
                                                        You a Tamil Origin From Andaman and Nicobar Islands ?</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdbtaany" runat="server" Text="Yes" GroupName="Same" /><asp:RadioButton
                                                        ID="rdbtaanN" runat="server" Text="No" GroupName="Same" Checked="true" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Are
                                                        You a Child of an Ex-serviceman ?</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdbexy" runat="server" Text="Yes" GroupName="Same1" onchange="return radiochange(this.value)"
                                                        onfocus="myFunction(this)" /><asp:RadioButton ID="rdbexN" runat="server" Text="No"
                                                            GroupName="Same1" Checked="true" onchange="return radiochange(this.value)" onfocus="myFunction(this)" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExservRank" CssClass="textbox textbox1" runat="server" Style="width: 60px;
                                                        display: none; float: left;" MaxLength="4" Placeholder="Rank"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender85" runat="server" TargetControlID="txtExservRank"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtExservPlace" CssClass="textbox textbox1" runat="server" Style="width: 120px;
                                                        display: none; float: left;" MaxLength="32" Placeholder="Place"></asp:TextBox>
                                                    <asp:TextBox ID="txtExservNum" CssClass="textbox textbox1" runat="server" Style="width: 100px;
                                                        display: none; float: left;" MaxLength="20" Placeholder="Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Are
                                                        You Differently abled</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Yes" GroupName="Same2" onclick="return abled(this)"
                                                        Style="float: left;" />
                                                    <asp:RadioButton ID="RadioButton2" runat="server" Text="No" GroupName="Same2" Checked="true"
                                                        onclick="return abled1(this)" Style="float: left;" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldiferentltyable" CssClass="textbox3 textbox1" runat="server"
                                                        onchange="return otherable(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                        Style="width: 160px; display: none; float: left;">
                                                        <asp:ListItem>Select</asp:ListItem>
                                                        <asp:ListItem Value="1">Visually Challanged</asp:ListItem>
                                                        <asp:ListItem Value="2">Physically Challanged</asp:ListItem>
                                                        <asp:ListItem Value="3">Learning Disability</asp:ListItem>
                                                        <asp:ListItem>Others</asp:ListItem>
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:TextBox ID="txt_otherdifferentlyable" CssClass="textbox textbox1" runat="server"
                                                        MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                        Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender74" runat="server" TargetControlID="txt_otherdifferentlyable"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Are
                                                        you a First generation learner ?</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButton3" runat="server" Text="Yes" GroupName="Same4" />
                                                    <asp:RadioButton ID="RadioButton4" runat="server" Text="No" Checked="true" GroupName="Same4" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Is
                                                        Hostel accommodation required?</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="RadioButton5" runat="server" Text="Yes" GroupName="Same5" />
                                                    <asp:RadioButton ID="RadioButton6" runat="server" Text="No" Checked="true" GroupName="Same5" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Distinction
                                                        in Sports</span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdbdistinctionsportsYes" runat="server" Text="Yes" GroupName="Same6"
                                                        onclick="return sport_fun(this)" Style="float: left;" /><asp:RadioButton ID="rdbdistinctionsportsNo"
                                                            runat="server" Checked="true" Text="No" GroupName="Same6" onclick="return sport1_fun(this)"
                                                            Style="float: left;" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldistinctionsports" CssClass="textbox3 textbox1" runat="server"
                                                        onchange="return othersport(this)" onblur="blurFunction(this)" onfocus="myFunction(this)"
                                                        Style="width: 160px; display: none; float: left;">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_sports" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                        MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                        Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender75" runat="server" TargetControlID="txt_sports"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txt_specify" CssClass="textbox textbox1" runat="server" placeholder="Specify Sports"
                                                        MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                        Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender76" runat="server" TargetControlID="txt_specify"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Extra
                                                        Curricular Activites / Co-Curricular Activites </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdbextraactivitesYes" runat="server" Text="Yes" GroupName="Same7"
                                                        Style="float: left;" onclick="return extra_fun(this)" />
                                                    <asp:RadioButton ID="rdbextraactivitesNo" runat="server" Checked="true" Text="No"
                                                        GroupName="Same7" Style="float: left;" onclick="return extra_fun1(this)" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="DropDownList4" CssClass="textbox3 textbox1" runat="server"
                                                        onchange="return otherextra(this)" Style="width: 160px; display: none; float: left;">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_extraactivites" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                                        MaxLength="50" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                        Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender78" runat="server" TargetControlID="txt_extraactivites"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txt_specifyNcc" CssClass="textbox textbox1" runat="server" placeholder="Specify Activites"
                                                        MaxLength="50" onblur="blurFunction(this)" onfocus="myFunction(this)" onkeydown="return (event.keyCode!=13);"
                                                        Style="width: 150px; display: none; float: left;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender79" runat="server" TargetControlID="txt_specifyNcc"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Blood
                                                        Donor </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rdb_bloodyes" runat="server" Text="Yes" GroupName="Sameblood" /><asp:RadioButton
                                                        ID="rdb_bloodno" runat="server" Text="No" Checked="true" GroupName="Sameblood" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Missionary
                                                        Child </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblMissionChild" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Reserved
                                                        Caste </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblReservCast" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Economic
                                                        Backward </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblEcoBack" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Is
                                                        Parents Old Student </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblparentOld" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;
                                                        padding-top: 7px;">Do you have Driving License </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblDrivLic" runat="server" RepeatDirection="Horizontal"
                                                        Style="float: left;" onchange="return licenseChange(this);">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDrivLic" runat="server" Style="float: left; display: none;" Placeholder="License No"
                                                        MaxLength="30" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Tuition
                                                        Fee Waiver </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblFeeWaiver" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Insurance
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblInsurance" runat="server" RepeatDirection="Horizontal"
                                                        onchange="return insuranceChange(this);">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtInsureAmt" CssClass="textbox textbox1" MaxLength="10" runat="server"
                                                        Style="width: 70px; display: none; float: left;" Placeholder="Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderInsAmt" runat="server" TargetControlID="txtInsureAmt"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtInsureBy" CssClass="textbox textbox1" MaxLength="50" runat="server"
                                                        Style="width: 100px; display: none; float: left;" Placeholder="Insured By"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender80" runat="server" TargetControlID="txtInsureBy"
                                                        FilterType="UppercaseLetters, LowercaseLetters, Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtInsureNominee" CssClass="textbox textbox1" runat="server" Style="width: 100px;
                                                        float: left; display: none;" MaxLength="50" Placeholder="Nominee"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender81" runat="server" TargetControlID="txtInsureNominee"
                                                        FilterType="UppercaseLetters, LowercaseLetters, Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:DropDownList ID="ddlInsureNominee" runat="server" CssClass="textbox textbox1 txtheight"
                                                        Height="30px" onchange="return ddlInsureNomi(this);" Style="float: left; display: none;">
                                                        <asp:ListItem>Father</asp:ListItem>
                                                        <asp:ListItem>Mother</asp:ListItem>
                                                        <asp:ListItem>Guardian</asp:ListItem>
                                                        <asp:ListItem>Others</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtInsureRelat" CssClass="textbox textbox1" MaxLength="50" runat="server"
                                                        Style="width: 100px; display: none; float: left;" Placeholder="Relationship"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender82" runat="server" TargetControlID="txtInsureRelat"
                                                        FilterType="UppercaseLetters, LowercaseLetters, Custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        
                                            <tr id="trStfDet" runat="server" visible="false">
                                              <td align="left">
                                                    <span style="align: left; font-family: Sans-Serif; font-size: small; float: left;">Is Staff's Child
                                                    </span>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rdbstfChild" runat="server" RepeatDirection="Horizontal"
                                                        onchange="return StfChildChange(this);">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="true">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td id="tdStfDet" runat="server" style="display:none;">
                                                <div style="position:relative;">
                                                  <asp:Label ID="lblStfDet" runat="server" style="font-family: Book Antiqua; font-size: medium;" Text="Staff Name"></asp:Label>
                                                  <asp:DropDownList ID="ddlStfDet" runat="server" CssClass="textbox1 ddlheight3" Width="250px"></asp:DropDownList>
                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <span style="font-family: Sans-Serif; font-size: small;">Language</span>
                                                </td>
                                                <td colspan="6">
                                                    <span style="float: left; padding-top: 5px;">Part 2</span>
                                                    <asp:DropDownList ID="ddlPartLang2" CssClass="textbox textbox1" Width="100px" Height="30px"
                                                        runat="server" Style="float: left;" onchange="return ddlPart2();">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtLangPart2" runat="server" CssClass="textbox textbox1 txtheight2"
                                                        MaxLength="50" Style="display: none; float: left;"></asp:TextBox><span style="float: left;
                                                            padding-top: 5px;">Part 3</span>
                                                    <asp:DropDownList ID="ddlPartLang3" CssClass="textbox textbox1" Width="100px" Height="30px"
                                                        runat="server" Style="float: left;" onchange="return ddlPart3();">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtLangPart3" runat="server" CssClass="textbox textbox1 txtheight2"
                                                        MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                               <%-- Added By Saranyaevi 21.2.2018--%>
                                            <tr>
                                               <td>
                                                    <span style="font-family: Sans-Serif; font-size: small;">Scholarship</span>
                                                </td>
                                                            <td>
                                                           
                                                    <asp:DropDownList ID="ddlscholarship" CssClass="textbox textbox1" Width="100px" Height="30px"
                                                        runat="server" Style="float: left;" onchange="return ddlscholar();">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_subscholar" runat="server" CssClass="textbox textbox1 txtheight2"
                                                        MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                            </td>
                                            
                                            </tr>
                                        </table>
              
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </div>
                </div>
            </div>
        </center>
        <%--Delete Confirmation Popup --%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete The Student?"
                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
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
        <%-- Pop Academic--%>
        <center>
            <div id="popAcademic" runat="server" visible="false" style="height: 100em; z-index: 100000;
                width: 98%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div7" runat="server" class="table" style="background-color: White; height: 450px;
                        width: 100%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <fieldset>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold; float: left; width: 900px; text-align: center;">
                                                    Academic Details</span>
                                            </div>
                                            <div style="height: 250px; overflow: auto;">
                                                <table>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span style="float: left; padding-top: 5px;">Course Last Studied</span>
                                                            <asp:DropDownList ID="ddlAcaCourse" CssClass="textbox textbox1" Width="80px" Height="30px"
                                                                runat="server" Style="float: left;" onchange="return ddlAcaCours();">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtAcaCourse" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">Board/ University</span>
                                                            <asp:DropDownList ID="ddlAcaUniv" CssClass="textbox textbox1" Width="80px" Height="30px"
                                                                runat="server" Style="float: left;" onchange="return ddlAcaUni();">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtAcaUniv" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">Branch/ Group</span>
                                                            <asp:DropDownList ID="ddlAcaBran" CssClass="textbox textbox1" Width="80px" Height="30px"
                                                                runat="server" Style="float: left;" onchange="return ddlAcaBra();">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtAcaBran" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span style="float: left; padding-top: 5px;">Institute</span>
                                                            <asp:TextBox ID="txtAcaInstitute" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="100" Style="float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">SecuredMarks</span>
                                                            <asp:TextBox ID="txtAcaSecMarks" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="6" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender50" runat="server" TargetControlID="txtAcaSecMarks"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span style="float: left; padding-top: 5px;">MaximumMarks</span>
                                                            <asp:TextBox ID="txtAcaMaxMarks" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="6" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender72" runat="server" TargetControlID="txtAcaMaxMarks"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span>
                                                                <asp:Button ID="btnAcaAdd" runat="server" OnClick="addAcaDetails" Text="Add" CssClass="textbox textbox1"
                                                                    Style="width: 60px; border: 1px solid black; font-weight: bold; float: left;
                                                                    height: 30px;" />
                                                                <asp:Button ID="btnAcaupdate" runat="server" OnClick="btnAcaupdate_Click" Text="Update"
                                                                    CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black; font-weight: bold;
                                                                    float: left; height: 30px;" />
                                                                <asp:Button ID="btnAcaClear" runat="server" OnClick="btnAcaClear_Click" Text="Clear"
                                                                    CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black; font-weight: bold;
                                                                    float: left; height: 30px;" /></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <FarPoint:FpSpread ID="spreadAcaDet" runat="server" Height="170px" Width="950px"
                                                                ShowHeaderSelection="false">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnCloseAca" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="closeAcademic" Text="Exit" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- Pop Bank--%>
        <center>
            <div id="popBank" runat="server" visible="false" style="height: 100em; z-index: 100000;
                width: 98%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div8" runat="server" class="table" style="background-color: White; height: 450px;
                        width: 100%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <fieldset>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold; float: left; width: 900px; text-align: center;">
                                                    Bank Details</span>
                                            </div>
                                            <%-- <asp:UpdatePanel ID="upBnkDet" runat="server"><ContentTemplate>--%>
                                            <div style="height: 250px; overflow: auto;">
                                                <table>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span style="float: left; padding-top: 5px;">Account For</span>
                                                            <asp:DropDownList ID="ddlBnkAccFor" CssClass="textbox textbox1" Width="80px" Height="30px"
                                                                runat="server" Style="float: left;" onchange="return ddlBnkAccFo();">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtBnkAccFor" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">Bank Name</span>
                                                            <asp:DropDownList ID="ddlBnkName" CssClass="textbox textbox1" Width="80px" Height="30px"
                                                                runat="server" Style="float: left;" onchange="return ddlBnkNam();">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtBnkName" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                MaxLength="50" Style="display: none; float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">Branch</span>
                                                            <asp:TextBox ID="txtBnkBran" runat="server" CssClass="textbox textbox1 txtheight3"
                                                                MaxLength="100" Style="float: left;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span style="float: left; padding-top: 5px;">Account No</span>
                                                            <asp:TextBox ID="txtBnkAccNo" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="40" Style="float: left;"></asp:TextBox>
                                                            <span style="float: left; padding-top: 5px;">DebitCard No</span>
                                                            <asp:TextBox ID="txtBnkDebit" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="25" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender73" runat="server" TargetControlID="txtAcaSecMarks"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span style="float: left; padding-top: 5px;">IFSC Code</span>
                                                            <asp:TextBox ID="txtBnkIFSC" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                MaxLength="25" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender86" runat="server" TargetControlID="txtAcaMaxMarks"
                                                                FilterType="Numbers,Custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span>
                                                                <asp:Button ID="btnAddBnkDet" runat="server" OnClick="addBnkDetails" Text="Add" CssClass="textbox textbox1"
                                                                    Style="width: 60px; border: 1px solid black; font-weight: bold; float: left;
                                                                    height: 30px;" />
                                                                <asp:Button ID="btnbankupdate" runat="server" OnClick="btnbankupdate_Click" Text="Update"
                                                                    CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black; font-weight: bold;
                                                                    float: left; height: 30px;" />
                                                                <asp:Button ID="btnBnkClear" runat="server" OnClick="btnBnkClear_Click" Text="Clear"
                                                                    CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black; font-weight: bold;
                                                                    float: left; height: 30px;" /></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <FarPoint:FpSpread ID="spreadBankDet" runat="server" Height="170px" Width="950px"
                                                                ShowHeaderSelection="false">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <%--            </ContentTemplate>
                                        
                                        </asp:UpdatePanel>--%>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnCloseBnk" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="closeBank" Text="Exit" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- Pop Relation--%>
        <center>
            <div id="popRel" runat="server" visible="false" style="height: 100em; z-index: 100000;
                width: 98%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div9" runat="server" class="table" style="background-color: White; height: 450px;
                        width: 100%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <fieldset>
                                            <div style="width: 100%; background-color: brown; height: 25px;">
                                                <span style="color: White; font-weight: bold; float: left; width: 900px; text-align: center;">
                                                    Relationship</span>
                                            </div>
                                            <div style="height: 250px; overflow: auto;">
                                                <table>
                                                    <tr>
                                                        <td colspan="6">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRelType" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                        <asp:DropDownList ID="ddlRelType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddlRelType_indexchange">
                                                                            <asp:ListItem Selected="True">Student</asp:ListItem>
                                                                            <asp:ListItem>Staff</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddl_collegeRel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddl_collegeRel_SelectedIndexchange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblBatRel" runat="server" Text="Batch :" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                        <asp:DropDownList ID="ddlrelBatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlrelBatch_IndexChange" Font-Size="Medium"
                                                                            Width="80px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbldegRel" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                        <asp:DropDownList ID="ddldegRel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddldegRel_Indexchange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbldeptRel" runat="server" Text="Dept" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                        <asp:DropDownList ID="ddldeptRel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddldeptRel_indexchange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <span style="float: left; padding-top: 5px;">Name</span>
                                                            <asp:DropDownList ID="ddlRelName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Width="200px" Style="float: left;">
                                                            </asp:DropDownList>
                                                            <span style="float: left; padding-top: 5px;">Phone No</span>
                                                            <asp:TextBox ID="txtRelPhone" runat="server" CssClass="textbox textbox1 txtheight3"
                                                                MaxLength="16" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender88" runat="server" TargetControlID="txtRelPhone"
                                                                FilterType="Numbers,Custom" ValidChars="">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span style="float: left; padding-top: 5px;">Enter the Relationship</span>
                                                            <asp:TextBox ID="txtrelRelship" runat="server" CssClass="textbox textbox1 txtheight3"
                                                                MaxLength="30" Style="float: left;"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderR92" runat="server" TargetControlID="txtrelRelship"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <span>
                                                                <asp:Button ID="btnAddRelDet" runat="server" OnClick="addRelDetails" Text="Add" CssClass="textbox textbox1"
                                                                    Style="width: 60px; border: 1px solid black; font-weight: bold; float: left;
                                                                    height: 30px;" />
                                                                <asp:Button ID="btnreldetupdate" runat="server" OnClick="btnreldetupdater_Click"
                                                                    Text="Update" CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black;
                                                                    font-weight: bold; float: left; height: 30px;" />
                                                                <asp:Button ID="btnRelClear" runat="server" OnClick="btnRelClear_Click" Text="Clear"
                                                                    CssClass="textbox textbox1" Style="width: 60px; border: 1px solid black; font-weight: bold;
                                                                    float: left; height: 30px;" /></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="6">
                                                            <FarPoint:FpSpread ID="spreadRelation" runat="server" Height="170px" Width="950px"
                                                                ShowHeaderSelection="false">
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnCloseRel" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="closeRelation" Text="Exit" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
    </html>
</asp:Content>
