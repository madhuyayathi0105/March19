<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="UserInOutEntry.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_UserInOutEntry" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--    <script src="../FinanceModScripts/FinanceUniversalScript.js" type="text/javascript"></script>--%>
    <style>
        .backColor
        {
            border-color: Red;
        }
        .style2
        {
            width: 191px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
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

            function SelLedgers() {
                var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
                var tbl = document.getElementById("<%=grdVisit_Details.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 1; i < (tbl.rows.length - 1); i++) {
                    var chkSelectid = document.getElementById('MainContent_grdVisit_Details_selectchk_' + i.toString());

                    if (chkSelAll.checked == false) {
                        chkSelectid.checked = false;
                    } else {
                        chkSelectid.checked = true;
                    }
                }

            }
        </script>
        <div>
            <center>
                <div>
                    <span id="sphd" runat="server" class="fontstyleheader" style="color: Green;">User IN/OUT
                        Entry Report</span>
                </div>
            </center>
        </div>
        <center>
            <asp:UpdatePanel ID="updatepanel15" runat="server">
                <ContentTemplate>
                    <table class="maintablestyle" style="font-weight: normal; font-family: Book Antiqua;
                        font-weight: bold; height: 25px;">
                        <tr>
                            <td colspan="6">
                                <%--<fieldset style="width: 500px;">--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                            <fieldset style="height: 64px; width: 270px;">
                                                <asp:Panel ID="Panel24" runat="server" ScrollBars="Auto" Style="height: 70px;">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblUser" runat="server" Text="User"></asp:Label>
                                            <fieldset style="height: 64px; width: 177px;">
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 70px;">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="Cb_user" runat="server" Text="All" onclick="CheckBoxListSelectUser(this);" />
                                                            <asp:CheckBoxList ID="checkusers" runat="server" RepeatDirection="vertical" AutoPostBack="true"
                                                                OnSelectedIndexChanged="cbl_users_OnSelectedIndexChanged">
                                                                <asp:ListItem Selected="True" Value="0">Student</asp:ListItem>
                                                                <asp:ListItem Value="1">Staff</asp:ListItem>
                                                                <asp:ListItem Value="2">Visitor</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLib" runat="server" Text="Library Name"></asp:Label>
                                            <fieldset style="height: 64px; width: 197px;">
                                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Style="height: 67px;">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="cb_lib" runat="server" Text="All" onclick="CheckBoxListSelectlibrary(this);" />
                                                            <asp:CheckBoxList ID="cbl_library" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_library_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblHit" runat="server" Text="Hit Status"></asp:Label>
                                            <fieldset style="height: 64px; width: 180px;">
                                                <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:Label ID="lblHitStu" runat="server" Text="Student"></asp:Label>
                                                    <asp:TextBox ID="studtxt" runat="server" Style="width: 50px; height: 12px; margin-left: 10px"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="lblHitStaff" runat="server" Text="Staff"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="stafftxt" runat="server" Style="width: 50px; height: 12px; margin-left: 17px"></asp:TextBox>
                                                    <br />
                                                    <asp:Label ID="lblHitVisit" runat="server" Text="Visitor"></asp:Label>&nbsp;&nbsp;
                                                    <asp:TextBox ID="visitortxt" runat="server" Style="width: 50px; height: 12px; margin-left: 9px"></asp:TextBox>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                            <fieldset id="FS_Dept" style="height: 109px; width: 270px;">
                                                <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectDept(this);" />
                                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSem" runat="server" Text="Semester"></asp:Label>
                                            <fieldset style="height: 109px; width: 176px;">
                                                <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="All" onclick="CheckBoxListSelectSem(this);" />
                                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRpt" runat="server" Text="Reports"></asp:Label>
                                            <fieldset style="height: 109px; width: 201px;">
                                                <asp:Panel ID="Panel6" runat="server" ScrollBars="Auto" Style="height: 109px;">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:RadioButtonList ID="reports" runat="server" RepeatDirection="vertical" AutoPostBack="true"
                                                                OnSelectedIndexChanged="reports_changed">
                                                                <asp:ListItem Selected="True">Best Members Visit</asp:ListItem>
                                                                <asp:ListItem>Members Entry List</asp:ListItem>
                                                                <asp:ListItem>Visitor Entry Statistics</asp:ListItem>
                                                                <asp:ListItem>Visit Details</asp:ListItem>
                                                                <asp:ListItem>Visit Details With Time</asp:ListItem>
                                                                <asp:ListItem>Batch Year Wise</asp:ListItem>
                                                                <asp:ListItem>Visitor Entry Statistics With Batch Year</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                        <td style="padding-top: 21px;">
                                            <asp:Label ID="Label9" runat="server" Text="Report" Visible="false"></asp:Label>
                                            <fieldset style="height: 109px; width: 180px;">
                                                <asp:Panel ID="Panel7" runat="server" ScrollBars="Auto" Style="height: 100px;">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr id="BestMem" runat="server" visible="true">
                                                                    <td>
                                                                        <asp:Label ID="toplabel" runat="server" Text="Top"></asp:Label>
                                                                        <asp:TextBox ID="Topitem" runat="server" Style="width: 50px; height: 12px;"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="MemEntry" runat="server" visible="false">
                                                                    <td>
                                                                        <asp:RadioButton ID="daywise" runat="server" GroupName="mementry" Text="DayWise"
                                                                            Checked="true" AutoPostBack="true" OnCheckedChanged="MementryDayWise_OnCheckedChanged" /><br />
                                                                        <asp:RadioButton ID="DeptWise" runat="server" GroupName="mementry" Text="DepartmentWise"
                                                                            AutoPostBack="true" OnCheckedChanged="MementryDeptwise_OnCheckedChanged" />
                                                                        <fieldset id="mementrylist" runat="server" visible="false" style="height: 30px; width: 100px;">
                                                                            <asp:RadioButton runat="server" ID="Individualdept" GroupName="mementrydept" Text="Individual"
                                                                                Visible="false" /><br />
                                                                            <asp:RadioButton runat="server" ID="Commondept" GroupName="mementrydept" Text="Common"
                                                                                Checked="true" Visible="false" /><br />
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr id="VisitDet" runat="server" visible="false">
                                                                    <td>
                                                                        <asp:Label ID="rollstaff" runat="server" Text="Roll No/Staff Code"></asp:Label>
                                                                        <asp:TextBox ID="txtroll" runat="server" CssClass="textbox  txtheight2" Height="12px"></asp:TextBox><br />
                                                                        <asp:Label ID="Lblname" runat="server" Text="Name"></asp:Label><br />
                                                                        <asp:TextBox ID="Nametxt" runat="server" CssClass="textbox  txtheight2" Height="12px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr id="VisitEntry" runat="server" visible="false">
                                                                    <td>
                                                                        <asp:RadioButton runat="server" ID="rbdaily" GroupName="visitentry" Text="Daily"
                                                                            Checked="true" /><br />
                                                                        <asp:RadioButton runat="server" ID="rbweekly" GroupName="visitentry" Text="Weekly" /><br />
                                                                        <asp:RadioButton runat="server" ID="rbmonthly" GroupName="visitentry" Text="Monthly" /><br />
                                                                        <asp:RadioButton runat="server" ID="rbyearly" GroupName="visitentry" Text="Yearly" /><br />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td id="Batchwise" runat="server" visible="false">
                                                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                            height: auto;">
                                                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                            PopupControlID="panel_batch" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div id="divdatewise" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="cbdate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="cbdate_Changed" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 12px; width: 75px;"
                                                                        onchange="return checkDate()"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                    </asp:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 12px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                    </asp:CalendarExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td style="padding-left: 15px;">
                                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="cbtime" runat="server" Text="Time" AutoPostBack="true" OnCheckedChanged="cbtime_Changed" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="fromtime" runat="server" Text="From"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                <ContentTemplate>
                                                                    <cc1:TimeSelector ID="TimeSelector1" runat="server" RepeatDirection="Horizondal"
                                                                        Enabled="false" AllowSecondEditing="true" MinuteIncrement="1" SecondIncrement="1">
                                                                    </cc1:TimeSelector>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="totime" runat="server" Text="To"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                <ContentTemplate>
                                                                    <cc1:TimeSelector ID="TimeSelector2" runat="server" RepeatDirection="Horizondal"
                                                                        Enabled="false" AllowSecondEditing="true" MinuteIncrement="1" SecondIncrement="1">
                                                                    </cc1:TimeSelector>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td colspan="2" style="padding-left: 78px;">
                                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                                                    <asp:ImageButton ID="btnViewCurrIn" runat="server" ImageUrl="~/LibImages/view currently in.jpg"
                                                                        OnClick="btnViewCurrIn_Click" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btngo" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <%--</fieldset>--%>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <br />
        <center>
            <asp:UpdatePanel ID="upReport" runat="server">
                <ContentTemplate>
                    <div id="divSpreadReport" runat="server" visible="false" style="overflow: auto;"
                        width="1000px">
                        <asp:GridView ID="grdUserReport" Width="1000px" runat="server" ShowHeader="true"
                            ShowFooter="false" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                            OnRowDataBound="grdUserReport_OnRowDataBound">
                            <%--AllowPaging="true" PageSize="50"  OnPageIndexChanging="grdUserReport_OnPageIndexChanged"--%>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                    <div id="divSpreadVisitWithTime" runat="server" visible="false" style="overflow: auto;"
                        width="1000px">
                        <asp:GridView ID="GrdVisitWithTime" Width="1000px" runat="server" ShowFooter="false"
                            ShowHeader="true" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true">
                            <%--AllowPaging="true" PageSize="50" OnPageIndexChanging="GrdVisitWithTime_OnPageIndexChanged"--%>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                <ContentTemplate>
                    <div id="divVisit_Details" runat="server" visible="false" style="overflow: auto;"
                        width="1000px">
                        <center>
                            <span style="padding-right: 100px; margin-left: 442px; margin-top: 3px;">
                                <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Visible="false"
                                    onchange="return SelLedgers();" />
                            </span>
                        </center>
                        <asp:GridView ID="grdVisit_Details" Width="1000px" runat="server" ShowFooter="false"
                            ShowHeader="true" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                            OnRowDataBound="grdVisit_Details_OnRowDataBound">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                <ContentTemplate>
                    <div id="div1" runat="server" visible="false" style="overflow: auto;" width="1000px">
                        <asp:GridView ID="GridView1" Width="1000px" runat="server" ShowFooter="false" ShowHeader="false"
                            AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="updatepanel18" runat="server">
                <ContentTemplate>
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel_Click" />
                        <asp:ImageButton ID="btnprintmasterhed" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed" Visible="false" />
                        <%--  <asp:ImageButton ID="btn_delete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                            Visible="false" OnClick="btn_delete_Click" />--%>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel" />
                    <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="updatepanel16" runat="server">
                <ContentTemplate>
                    <div id="print1" runat="server" visible="false">
                        <asp:Label ID="lblvalidation2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" Width="180px" onkeypress="display(this)"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:ImageButton ID="btnexcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel1_Click" />
                        <asp:ImageButton ID="btnprint1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster1_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed1" Visible="false" />
                        <%--<asp:ImageButton ID="btndelete1" runat="server" ImageUrl="~/LibImages/delete.jpg"
                            Visible="false" OnClick="btn_delete_Click" />--%>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnexcel1" />
                    <asp:PostBackTrigger ControlID="btnprint1" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="updatepanel17" runat="server">
                <ContentTemplate>
                    <div id="print2" runat="server" visible="false">
                        <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname2" runat="server" Width="180px" onkeypress="display(this)"
                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname2"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:ImageButton ID="btnexcel2" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel2_Click" />
                        <asp:ImageButton ID="btnprint2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster2_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                        <asp:ImageButton ID="btndelete2" runat="server" ImageUrl="~/LibImages/delete.jpg"
                            Visible="false" OnClick="btndelete2_Click" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnexcel2" />
                    <asp:PostBackTrigger ControlID="btnprint2" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="updatepanel19" runat="server">
                <ContentTemplate>
                    <div id="divPhoto" runat="server" visible="false" width="1000px">
                        <%--style="overflow: auto;" --%>
                        <table class="maintablestyle" style="font-weight: normal; height: 500px; width: 1000px">
                            <tr>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%--<asp:Panel ID="PanelPhoto1" runat="server" Style="height:200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo1" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud1" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName1" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%-- <asp:Panel ID="PanelPhoto2" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo2" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud2" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName2" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%-- <asp:Panel ID="PanelPhoto3" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo3" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud3" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName3" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%--<asp:Panel ID="PanelPhoto4" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo4" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud4" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName4" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--      </asp:Panel>--%>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="Lst_PhotoList" runat="server" Visible="false" Width="170px"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%--<asp:Panel ID="PanelPhoto1" runat="server" Style="height:200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo5" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud5" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName5" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%-- <asp:Panel ID="PanelPhoto2" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo6" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud6" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName6" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%-- <asp:Panel ID="PanelPhoto3" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo7" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud7" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName7" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </asp:Panel>--%>
                                    </fieldset>
                                </td>
                                <td>
                                    <fieldset style="height: 200px; width: 200px;">
                                        <%--<asp:Panel ID="PanelPhoto4" runat="server" Style="height: 200px;">--%>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblRollNo8" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="img_stud8" runat="server" Height="141px" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblStudName8" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--</asp:Panel>--%>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="align: left">
                                    <asp:Button ID="BtnFirst" runat="server" CssClass="textbox btn2" Text="First" OnClick="BtnFirst_Click" />
                                    <asp:Button ID="BtnPrev" runat="server" CssClass="textbox btn2" Text="Previous" OnClick="BtnPrev_Click" />
                                    <asp:Button ID="BtnNext" runat="server" CssClass="textbox btn2" Text="Next" OnClick="BtnNext_Click" />
                                    <asp:Button ID="BtnLast" runat="server" CssClass="textbox btn2" Text="Last" OnClick="BtnLast_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <%--PopUp for Visit Details With Time--%>
        <center>
            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                <ContentTemplate>
                    <div id="DivVisitDetWithTime" runat="server" visible="false" class="popupstyle popupheight1 ">
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 230px;"
                            OnClick="imageVisitDetWithTime_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; font-family: Book Antiqua; height: 300px; width: 500px;
                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                                    </td>
                                    <td>
                                        <cc1:TimeSelector ID="TimeSelector3" runat="server" RepeatDirection="Horizondal"
                                            Enabled="false" AllowSecondEditing="true" MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                    </td>
                                    <td>
                                        <cc1:TimeSelector ID="TimeSelector4" runat="server" RepeatDirection="Horizondal"
                                            Enabled="false" AllowSecondEditing="true" MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                    </td>
                                    <td>
                                        <asp:Button ID="BtnAddTime" runat="server" CssClass="textbox btn2" Text="Add" OnClick="BtnAddTime_Click"
                                            Style="width: 40px" />
                                    </td>
                                    <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto" Style="height: 100px;">
                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:ListBox ID="LstBoxTime" runat="server" Height="121px" Width="170px"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upTimeSave" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="BtnTimeSave" runat="server" CssClass="textbox btn2" Text="Save" OnClick="BtnTimeSave_Click"
                                                    Style="width: 40px" />
                                                <asp:Button ID="BtnRemoveTime" runat="server" CssClass="textbox btn2" Text="Remove"
                                                    OnClick="BtnRemoveTime_Click" Style="width: 60px" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <%-- </asp:Panel>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <div>
            <asp:UpdatePanel ID="updatepanel21" runat="server">
                <ContentTemplate>
                    <center>
                        <div id="Diveleterecord" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                    <asp:Label ID="lbl_Diveleterecord" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upSureDel" runat="server">
                                                        <ContentTemplate>
                                                            <center>
                                                                <asp:ImageButton ID="btn_detele_yes__record" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                                    OnClick="btn_detele_yes__record_Click" />
                                                                <asp:ImageButton ID="btn_detele_no__record" runat="server" ImageUrl="~/LibImages/no (2).jpg"
                                                                    OnClick="btn_detele_no__recordClick" />
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
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%-- Popup for Error Message--%>
        <center>
            <asp:UpdatePanel ID="updatepanel20" runat="server">
                <ContentTemplate>
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
                                                <asp:UpdatePanel ID="updatepanelbtn2" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                                OnClick="btn_errorclose_Click" />
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
    </body>
    <%--progressBar for Go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <%--progressBar for upTimeSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upTimeSave">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upReport">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upSureDel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
