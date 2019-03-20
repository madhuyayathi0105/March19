<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ConvocationRegistration.aspx.cs" Inherits="ConvocationRegistration" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            $(document).ready(function () {
                $('#<%=btnExcel.ClientID %>').click(function () {
                    var txtval = $('#<%=txtexcelname.ClientID %>').val();
                    if (txtval == null || txtval == "") {
                        $('#<%=lblvalidation1.ClientID %>').show();
                        $('#<%=lblvalidation1.ClientID %>').text("Please Enter Your  Report Name");
                        return false;
                    }
                });
            });

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
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

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

                    var dt = today.getDate();
                    var mnth = today.getMonth() + 1;
                    var yr = today.getFullYear();
                    if (dt < 10)
                        dt = '0' + dt;
                    if (mnth < 10)
                        mnth = '0' + mnth;
                    currentDate = dt + '/' + mnth + '/' + yr;
                    document.getElementById('<%=txt_fromdate.ClientID%>').value = currentDate;
                    document.getElementById('<%=txt_todate.ClientID%>').value = currentDate;
                    alert("To date should be greater than from date ");
                    return false;
                }
            }
            function columnOrderCbl() {
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
        </script>
        <script type="text/javascript">


            function myFunction(x) {

                x.style.borderColor = "#c4c4c4";

            }
            function blurFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function click1() {
                var error = "";
                var email = document.getElementById("<%=txtemailid.ClientID %>");
                var Phone = document.getElementById("<%=txtmobileno.ClientID %>").value;
                var Address = document.getElementById("<%=txtaddress.ClientID %>").value;
                var confirm = document.getElementById("<%=confirm.ClientID %>");
                var confirm1 = document.getElementById("<%=Cancel.ClientID %>");
                //                var feedback = document.getElementById("<%=cbfeedback.ClientID %>");
                var amount = document.getElementById("<%=txtamount.ClientID %>").value;
                var rollNo = document.getElementById("<%=txt_roll.ClientID %>").value;
                if (rollNo == "") {
                    error = error + "* -Please Enter Student Details \n";
                    document.getElementById("<%=txt_roll.ClientID %>").style.borderColor = 'Red';
                }
                if (confirm.checked == false && confirm1.checked == false) {
                    document.getElementById("<%=confirm.ClientID %>").style.borderColor = 'Red';
                    document.getElementById("<%=Cancel.ClientID %>").style.borderColor = 'Red';
                    error = error + "* -Confirmation Required \n";
                }
                var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (email.value != "") {
                    if (!filter.test(email.value)) {
                        email.style.borderColor = 'Red';
                        error = error + "* -Please Enter Valid email address \n";
                    }
                    else {
                        email.style.borderColor = '#c4c4c4';
                    }
                }
                else {
                    error = error + "* -Please Enter email \n";
                    document.getElementById("<%=txtemailid.ClientID %>").style.borderColor = 'Red';
                }

                if (Phone == "") {
                    error = error + "* -Please Enter Phone Number \n";
                    document.getElementById("<%=txtmobileno.ClientID %>").style.borderColor = 'Red';
                }

                if (Address == "") {

                    error = error + "* -Please Enter Address \n";
                    document.getElementById("<%=txtaddress.ClientID %>").style.borderColor = 'Red';
                }

                //                if (feedback.checked == false) {
                //                    error = error + "* -Please Check Feedback \n";


                //                }
                if (amount == "") {
                    if (!confirm1.checked) {
                        error = error + "* -Please Enter Amount \n";
                        document.getElementById("<%=txtamount.ClientID %>").style.borderColor = 'Red';
                    }
                    else {
                        document.getElementById("<%=txtamount.ClientID %>").style.borderColor = "#c4c4c4";
                    }
                }

                if (error == "") {
                    return true;
                }
                else {
                    alert(error);
                    return false;
                }



            }

            function checkvalue() {
                var fl = 0;
                var id = document.getElementById("<%=feedbackgrid.ClientID %>");
                var len = id.rows.length;
                for (var ak = 0; ak < id.rows.length; ak++) {
                    if (id.rows[ak].getElementsByTagName("input")[0] != null
                    && id.rows[ak].getElementsByTagName("input")[0].type == "radio") {
                        if (id.rows[ak].getElementsByTagName("input")[0].checked == false
                    && id.rows[ak].getElementsByTagName("input")[1].checked == false
                    && id.rows[ak].getElementsByTagName("input")[2].checked == false
                    && id.rows[ak].getElementsByTagName("input")[3].checked == false) {
                            fl = 1;
                        }
                    }
                }

                if (fl == 1) {
                    alert('Please Fill All Values');
                    return false;
                }
                else {
                    return true;
                }
            }

            function cancel() {

                var id = document.getElementById("<%=btncancel.ClientID %>");
                id.checked = false;
            }

            function change(id) {

                var value1 = id.value;

                if (value1.trim().toUpperCase() == "OTHERS") {

                    var idval = document.getElementById("<%=txt_state.ClientID %>");
                    idval.style.display = "block";

                }
                else {
                    var idval = document.getElementById("<%=txt_state.ClientID %>");
                    idval.style.display = "none";


                }

            }

            function change(id) {

                var value1 = id.value;

                if (value1.trim().toUpperCase() == "OTHERS") {

                    var idval = document.getElementById("<%=txt_state.ClientID %>");
                    idval.style.display = "block";

                }
                else {
                    var idval = document.getElementById("<%=txt_state.ClientID %>");
                    idval.style.display = "none";


                }

            }

            function change_Value(id) {

                var value1 = id.value;

                if (value1.trim().toUpperCase() == "OTHERS") {

                    var idval = document.getElementById("<%=txt_officestate.ClientID %>");
                    idval.style.display = "block";

                }
                else {
                    var idval = document.getElementById("<%=txt_officestate.ClientID %>");
                    idval.style.display = "none";


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
                    $("[id*=cblcolumnorder]").removeAttr('checked');
                    $('#<%=cb_column.ClientID %>').removeAttr('checked');
                    return false;
                });

                $('#<%=imgcolumn.ClientID %>').click(function () {
                    $('#<%=divcolorder.ClientID %>').hide();
                    return false;
                });
                $('#<%=btnGo.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlMainreport.ClientID %>').find('option:selected').text();
                    if (rptText.trim() == null || rptText == "Select") {
                        alert("Please select any one report type!");
                        return false;
                    }
                });

                $('#<%=btncolorderOK.ClientID %>').click(function () {
                    var rptText = $('#<%=ddlreport.ClientID %>').find('option:selected').text();
                    var textval = $('#<%=txtcolorder.ClientID %>').val();
                    if (rptText.trim() == null || rptText == "Select") {
                        alert("Please select any one report type!");
                        return false;
                    }
                    if (textval == "" || textval == null) {
                        alert("Please Select columns!");
                        return false;
                    }
                });
            });

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
                    else {
                        //                    if (oldval != null && oldval != "") {
                        //                        var result = oldval.includes(getval);
                        //                        if (result) {
                        //                            oldval = oldval.replace(getval, " ");
                        //                        }
                        //                    }
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
       
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Convocation
                        Registration</span>
                </div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
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
                                <span>Stream</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" Width="120px" Height="30px" OnSelectedIndexChanged="type_Change"
                                    AutoPostBack="true" CssClass="textbox textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="panel_batch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degrees" runat="server" Style="height: 20px; width: 100px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degrees"
                                            PopupControlID="panel_degree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: 300px;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="panel_dept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:RadioButton ID="Rbapply" runat="server" Font-Size="14px" Text="Attending Student"
                                    GroupName="same" />
                                <asp:RadioButton ID="Rbnotapply" runat="server" Text="Not Attending Student" GroupName="same" />
                                <asp:RadioButton ID="RbBoth" Checked="true" runat="server" Text="Both" GroupName="same" />
                            </td>
                            <td colspan="2">
                                <div id="divdatewise" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                    onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="textbox btn2" OnClick="btnGo_Click" />
                                <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btnAddNew_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblrpt" runat="server" Text="Report"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                    Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="lnkcolorder" runat="server" Width="30px" Height="30px" Text="All"
                                    ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="lnkcolorder_Click" />
                                <%-- <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>--%>
                                <%-- </td>
                        <td>--%>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 83px; margin-left: 403px;" />
                            <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                            <center>
                                <div id="Div2" runat="server" class="table" style="background-color: White; height: 380px;
                                    width: 885px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="885px"
                                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                        RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <center>
                                                        <asp:Button ID="btncolorderOK" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btncolorderOK_Click" Text="OK" runat="server" />
                                                        <%--   </center>
                                </td>
                                <td>
                                    <center>--%>
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
                    <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder" OnPreRender="spreadDet_SelectedIndexChanged" OnCellClick="spreadDet_CellClick">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <FarPoint:FpSpread ID="spreadNotAttend" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <center>
                        <div id="print" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                Height="32px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmasterhed" runat="server" Visible="false" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                            <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                        </div>
                    </center>
                </div>
                <%--add new popup--%>
                <div id="div_addNew" runat="server" visible="false" class="popupstyle popupheight1 ">
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 440px;"
                        OnClick="imgAddNewclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 1252px; width: 920px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Enter Your Details</span></div>
                        </center>
                        <br />
                        <table style="width: 300px; text-align: left;">
                            <%--class="maintablestyle"--%>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <asp:Label ID="Label1" Text="College" runat="server"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddladdclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddladdclg_SelectedIndexChanged" AutoPostBack="true" Width="325px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <asp:Label ID="lblroll" runat="server"></asp:Label>
                                    <%-- </td>
                                <td>--%>
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1"
                                        OnTextChanged="txt_roll_TextChanged" AutoPostBack="true" Width="325px"></asp:TextBox>
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
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Name(English)</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="gen" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox textbox1" Width="325px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Name(Tamil)</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="genf" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_studNameTamil" runat="server" CssClass="textbox textbox1" Width="325px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Degree</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox textbox1" Width="100px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                                <td>
                                    <span>Subject</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_depatment" runat="server" CssClass="textbox textbox1" Width="200px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Years of Study</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_batchyear" runat="server" CssClass="textbox textbox1" Width="100px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Father Name</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span2" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtfatherName" runat="server" CssClass="textbox textbox1" Width="325px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: lightblue;">
                                <td>
                                    <span>Attending Graduation Day</span>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButton ID="confirm" runat="server" Text="Yes" GroupName="first" />
                                    <asp:RadioButton ID="Cancel" runat="server" Text="No" GroupName="first" />
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Amount</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span3" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtamount" runat="server" CssClass="textbox textbox1" Width="90px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtamount"
                                        FilterType="numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Paid Date</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span4" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtPaidDt" runat="server" Style="height: 20px; width: 90px;" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtPaidDt" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Passing Month</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span5" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtpassmnth" runat="server" CssClass="textbox textbox1" Width="90px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Passing Year</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span6" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtpassyear" runat="server" CssClass="textbox textbox1" Width="90px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="background-color: #5CE6D1;">
                                <td>
                                    <span>Remark</span> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span
                                        id="Span7" runat="server" style="text-align: right;"></span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtremark" runat="server" CssClass="textbox textbox1" Width="372px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Residential Address Line1</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtaddress" runat="server" CssClass="textbox textbox1" Width="290px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)" Placeholder="Permanent Address Line1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtaddress"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Address Line2</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtaddressline2" runat="server" Placeholder="Permanent Address Line2"
                                        CssClass="textbox textbox1" Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtaddressline2"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Address Line3</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtaddressline3" runat="server" Placeholder="Permanent Address Line3"
                                        CssClass="textbox textbox1" Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtaddressline3"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>City</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_City" runat="server" Placeholder="Permanent City" CssClass="textbox textbox1"
                                        Width="200px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_City"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>State</span>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlstate" runat="server" onchange="return change(this)" Style="float: left;"
                                        CssClass="textbox textbox1" Height="28px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="txt_state" runat="server" CssClass="textbox textbox1" Width="200px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)" Style="display: none;
                                        float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" TargetControlID="txt_state"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Email ID</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtemailid" runat="server" Placeholder="Email Id" CssClass="textbox textbox1"
                                        Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtemailid"
                                        FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars="@._- ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Mobile Number</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtmobileno" runat="server" Placeholder="Mobile No" CssClass="textbox textbox1"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtmobileno"
                                        FilterType="Numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Phone Number(With STD Code)</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_residentstd" runat="server" CssClass="textbox textbox1" Width="80px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_residentstd"
                                        FilterType="Numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="txt_residentphone" runat="server" CssClass="textbox textbox1" onblur="blurFunction(this)"
                                        onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_residentphone"
                                        FilterType="Numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Office Name</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officename" runat="server" CssClass="textbox textbox1" Width="290px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_officename"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Designation</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_designation" runat="server" CssClass="textbox textbox1" Width="290px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_designation"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Office Address Line1</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officeaddressline1" runat="server" CssClass="textbox textbox1"
                                        Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_officeaddressline1"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Address Line2</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officeaddressline2" runat="server" CssClass="textbox textbox1"
                                        Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_officeaddressline2"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Address Line3</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officeaddressline3" runat="server" CssClass="textbox textbox1"
                                        Width="290px" onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_officeaddressline3"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars="/.@& ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Office City</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officecity" runat="server" CssClass="textbox textbox1" Width="200px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_officecity"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Office State</span>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_officestate" runat="server" onchange="return change_Value(this)"
                                        CssClass="textbox textbox1" Height="28px" Style="float: left;">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_officestate" runat="server" CssClass="textbox textbox1" Width="200px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)" Style="display: none;
                                        float: left;"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txt_officestate"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Office Phone Number(With STD Code)</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_officestd" runat="server" CssClass="textbox textbox1" Width="80px"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_officestd"
                                        FilterType="Numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:TextBox ID="txt_officephonenumber" runat="server" CssClass="textbox textbox1"
                                        onblur="blurFunction(this)" onfocus="myFunction(this)"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_officephonenumber"
                                        FilterType="Numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr id="trfeed" runat="server" visible="false">
                                <td>
                                    <span>Student Feedback</span>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBox ID="cbfeedback" runat="server" Text="Click Here" AutoPostBack="true"
                                        OnCheckedChanged="click_Change" />
                                    <span>("Kindly take a few minutes to fill in the IQAC feedback form")</span> <span
                                        style="color: Red;">*</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <div>
                            <asp:Button ID="Button1" runat="server" Text="Submit" Visible="true" Height="30px"
                                ForeColor="White" BackColor="Blue" Width="90px" CssClass="textbox textbox1" OnClientClick="return click1()"
                                OnClick="save_click" />
                            <asp:Button ID="Button2" runat="server" Text="Exit" Visible="true" Height="30px"
                                ForeColor="White" BackColor="Blue" Width="90px" CssClass="textbox textbox1" OnClick="Exit_click" />
                        </div>
                    </div>
                </div>
                <div>
                    <%--<div id="step7" runat="server" visible="false" style="z-index: 10000; width: 900px;
                        height: 50em; border: 1px solid black; background-color: White;">--%>
                    <div id="step7" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <div id="Div1" runat="server" style="z-index: 10000; width: 900px; height: 50em;
                            border: 1px solid black; background-color: White;">
                            <br />
                            <br />
                            <center>
                                <span style="font-size: 20px; color: Green;">Student's Feedback</span>
                            </center>
                            <br />
                            <div style="width: 617px; text-align: left;">
                                <span>Please provide a feedback on your course on the following attributes using the
                                    four point scale. Please Select [<asp:Image ID="image" runat="server" ForeColor="Blue"
                                        ImageUrl="~/image/dotimage.png" Width="10px" Height="10px" />] the appropriate
                                    box.</span>
                            </div>
                            <br />
                            <center>
                                <asp:GridView ID="feedbackgrid" runat="server" AutoGenerateColumns="false" Style="line-height: 25px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Parameters">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltype" runat="server" Text='<%# Eval("type") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Very Good">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdbverygood" runat="server" GroupName="same" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Good">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdbgood" runat="server" GroupName="same" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Satisfactory">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdbsatisfactory" runat="server" GroupName="same" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unsatisfactory">
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rdbunsatisfactoty" runat="server" GroupName="same" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="130px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <asp:Button ID="btnsubmit1" runat="server" Text="Submit" Height="30px" Width="90px"
                                    CssClass="textbox textbox1" OnClientClick="return checkvalue()" OnClick="Ok_Click" />
                                <asp:Button ID="btncancel" runat="server" Text="Cancel" Height="30px" Width="90px"
                                    CssClass="textbox textbox1" OnClientClick="return cancel()" OnClick="Cancel_Click" />
                            </center>
                            <asp:HiddenField runat="server" ID="HiddenField1" />
                            <asp:ModalPopupExtender ID="mptancet1" runat="server" BackgroundCssClass="modalPopup"
                                TargetControlID="HiddenField1" PopupControlID="step7">
                            </asp:ModalPopupExtender>
                        </div>
                    </div>
                </div>
            </center>
        </div>
        </center>
    </body>
</asp:Content>
