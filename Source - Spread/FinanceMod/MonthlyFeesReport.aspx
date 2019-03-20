<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MonthlyFeesReport.aspx.cs" Inherits="MonthlyFeesReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
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


        });
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">Monthly
                    Fees Report</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 875px; height: auto">
                <table class="maintablestyle" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 175px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                        height: 120px;">
                                        <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbclg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtclg"
                                        PopupControlID="pnlclg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 112px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 200px; height: 180px;">
                                        <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                        PopupControlID="pnl_studhed" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ledger" runat="server" Text="Ledger" Style="width: 50px;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 75px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 200px; height: 180px;">
                                        <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_studled"
                                        PopupControlID="pnl_studled" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
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
                         <td>
                            <asp:LinkButton ID="lnkcolorder" runat="server" Text="Column Order" OnClick="lnkcolorder_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMainreport" runat="server" CssClass="textbox textbox1 ddlheight4"
                                Width="119px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel28" runat="server">
                                <ContentTemplate>
                                    <asp:RadioButton ID="rdbpaid" runat="server" AutoPostBack="true" Text="Paid" OnCheckedChanged="rdbpaid_checkedChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:RadioButton ID="rdbduelist" runat="server" AutoPostBack="true" Text="Due List"
                                        OnCheckedChanged="rdbduelist_checkedChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:RadioButton ID="rdbCumulative" runat="server" AutoPostBack="true" Text="Cumulative"
                                        OnCheckedChanged="rdbCumulative_checkedChanged" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                       
                        <td style="text-align: right">
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
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
                <br />
                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Text="Please Enter Your Report Name"
                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Red" Style="display: none;"></asp:Label>
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
                        <%--added by deepali 02.11.2017--%>
                        <%-- <asp:Button ID="btn_print" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Print" OnClick="btn_print_Click" Height="32px" Style="margin-top: 10px;"
                            CssClass="textbox textbox1" Width="60px" />--%>
                        <%--------------------------------------------%>
                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divcolorder" runat="server" style="height: 100%; display: none; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="imgcolumn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 304px;" />
                <%--   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>--%>
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 322px;
                        width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
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
                                        <asp:TextBox ID="txtcolorder" runat="server" Columns="20" Style="height: 70px; width: 600px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="600px"
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
    </div>
</asp:Content>
