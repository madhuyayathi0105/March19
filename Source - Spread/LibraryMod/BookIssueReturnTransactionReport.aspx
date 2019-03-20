<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="BookIssueReturnTransactionReport.aspx.cs"
    Inherits="LibraryMod_BookIssueReturnTransactionReport" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="head">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">


        //User
        function CheckBoxListSelectUser(cbControl) {
            var chkBoxList = document.getElementById('<%=checkusers.ClientID %>');
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

        //library
        function CheckBoxListSelectlibrary(cbControl) {
            var chkBoxList = document.getElementById('<%=cbl_library.ClientID %>');
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
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Transaction Report</span>
            </div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="updatepanel3" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="font-weight: normal; height: 25px; font-family: Book Antiqua;
                        font-weight: bold">
                        <tr>
                            <td colspan="6px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                            <fieldset style="height: 64px; width: 270px;">
                                                <asp:Panel ID="Panel24" runat="server" ScrollBars="Auto" Style="height: 70px;">
                                                    <asp:DropDownList ID="ddl_collegename" runat="server" OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged"
                                                        CssClass="textbox ddlstyle ddlheight3" Width="200px" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUser" runat="server" Text="User"></asp:Label>
                                            <fieldset style="height: 64px; width: 177px;">
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 70px;">
                                                    <asp:CheckBox ID="Cb_user" runat="server" Text="All" onclick="CheckBoxListSelectUser(this);" />
                                                    <asp:CheckBoxList ID="checkusers" runat="server" RepeatDirection="vertical" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cbl_users_OnSelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Value="0">Student</asp:ListItem>
                                                        <asp:ListItem Value="1">Staff</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLib" runat="server" Text="Library Name"></asp:Label>
                                            <fieldset style="height: 64px; width: 247px;">
                                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="height: 67px;">
                                                    <asp:CheckBox ID="cb_lib" runat="server" Text="All" onclick="CheckBoxListSelectlibrary(this);" />
                                                    <asp:CheckBoxList ID="cbl_library" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_library_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                            <fieldset id="FS_Dept" style="height: 109px; width: 270px;">
                                                <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectDept(this);" />
                                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSem" runat="server" Text="Semester"></asp:Label>
                                            <fieldset style="height: 109px; width: 176px;">
                                                <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectSem(this);" />
                                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="height: 109px; width: 174px; margin-top: 19px">
                                                <asp:RadioButtonList ID="rbltransactions" runat="server" RepeatDirection="Vertical"
                                                    AutoPostBack="true" ForeColor="Black" OnSelectedIndexChanged="rbltransactions_OnSelectedIndexChanged"
                                                    Style="margin-left: -14px; width: 392px">
                                                    <asp:ListItem Selected="True">Best Book Transactions</asp:ListItem>
                                                    <asp:ListItem>Daily Activity Transactions</asp:ListItem>
                                                    <asp:ListItem>Over Due Members List</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset style="height: 108px; width: 110px; margin-top: 19px; margin-left: -138px">
                                                <asp:RadioButtonList ID="rbldailyacttrans" runat="server" RepeatDirection="Vertical"
                                                    AutoPostBack="true" ForeColor="Black" OnSelectedIndexChanged="rbldailyacttrans_OnSelectedIndexChanged"
                                                    Style="margin-left: -12px; width: 102px; margin-top: 0px" Visible="false">
                                                    <asp:ListItem>Daily</asp:ListItem>
                                                    <asp:ListItem>Weekly</asp:ListItem>
                                                    <asp:ListItem>Monthly</asp:ListItem>
                                                    <asp:ListItem Selected="True">Yearly</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div id="divdatewise" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="cbdate1" runat="server" Text="Date" Enabled="true" AutoPostBack="true"
                                                                OnCheckedChanged="cbdate1_OnCheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblfrom" runat="server" Text="From:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="height: 14px;
                                                                width: 144px;" onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate1" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblto" runat="server" Text="To:"></asp:Label>
                                                            <asp:TextBox ID="txt_todate" runat="server" Enabled="false" Style="height: 14px;
                                                                width: 144px;" onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalenderExtender2" TargetControlID="txt_todate" runat="server"
                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                            </asp:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="cbtime1" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbtime_OnCheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblfromtime" runat="server" Text="From:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <cc1:TimeSelector ID="timerselector1" runat="server" Enabled="false" AllowSecondEditing="true"
                                                                MinuteIncrement="1" SecondIncrement="1" RepeatDirection="Horizondal">
                                                            </cc1:TimeSelector>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbltotime" runat="server" Text="To:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <cc1:TimeSelector ID="timerselector2" runat="server" Enabled="false" AllowSecondEditing="true"
                                                                MinuteIncrement="1" SecondIncrement="1" RepeatDirection="vertical">
                                                            </cc1:TimeSelector>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="upGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" Visible="True"
                                                                        OnClick="btngo_OnClick" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
    <asp:UpdatePanel ID="upGrid" runat="server">
        <ContentTemplate>
            <div id="divtable" runat="server" visible="false">
                <center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:GridView ID="grdBkIssTransReport" Width="1000px" runat="server" ShowFooter="false" ShowHeader="false"
                                        AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                        AllowPaging="true" PageSize="100" OnPageIndexChanging="grdBkIssTransReport_OnPageIndexChanging">
                                       
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    </asp:GridView>
                                    <asp:GridView ID="GrdOverDueMemebersList" Width="1000px" runat="server" ShowFooter="false"  ShowHeader="false"
                                        AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                        AllowPaging="true" PageSize="100" OnPageIndexChanging="GrdOverDueMemebersList_OnPageIndexChanging">
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    </asp:GridView>
                                   
                                </center>
                                <center>
                                    <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                    </asp:Label></center>
                                <div id="div_report" runat="server" visible="false">
                                    <center>
                                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                            CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                            OnClick="btnExcel_Click" />
                                        <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                            OnClick="btn_printmaster_Click" />
                                      <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                    </center>
                                </div>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
         <Triggers>
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="updatepanel2" runat="server">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="updatepanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2" Width="40px"
                                                            OnClick="btnPopAlertClose_Click" Text="Ok" />
                                                    </center>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for upNext--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upGrid">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
