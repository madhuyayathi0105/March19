<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="bookissue.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_bookissue" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }

        var txtFocus = null;
        window.onload = function () {
            var inputs = document.getElementsByTagName('INPUT');
            for (var i = 0; i < inputs.length; i++) {
                var elem = inputs[i];


                if (elem.type == 'text') {
                    elem.onfocus = function () {
                        txtFocus = this;
                    }
                    elem.onblur = function () {
                        txtFocus = null;
                    }
                }
            }
        }

        function PrintPanel() {
            var panel = document.getElementById("<%=divSpreadReport.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
        document.onkeydown = checkKeycode
        var keycode;
        function checkKeycode(e) {
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            //alert("keycode: " + keycode);
            var rb = document.getElementById("<%=rblissue.ClientID%>");
            var RollNo = document.getElementById("<%=txtRollNo.ClientID%>").value;
            var radio = rb.getElementsByTagName("input");
            if (keycode == "112") {
                radio[0].checked = true;
                if (radio[0].checked) {
                    document.getElementById('<%=lbl_issue.ClientID %>').innerHTML = "Issue Date";
                    document.getElementById('<%=lbl_due.ClientID %>').innerHTML = "Due Date";
                    document.getElementById('<%=lbl_due.ClientID %>').style.visibility = 'visible';
                    document.getElementById('<%= Txtduedate.ClientID %>').style.visibility = 'visible';
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').innerHTML = "Issuing Books";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.forecolor = "Green";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=lbl_issue.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=lbl_due.ClientID %>').style.fontWeight = 'bold';
                }
            }
            if (keycode == "113") {
                radio[1].checked = true;
                if (radio[1].checked) {
                    document.getElementById('<%=lbl_issue.ClientID %>').innerHTML = "Return Date";
                    document.getElementById('<%=lbl_due.ClientID %>').style.visibility = 'hidden';
                    document.getElementById('<%= Txtduedate.ClientID %>').style.visibility = 'hidden';
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').innerHTML = "Returning Books";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.forecolor = "Green";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=lbl_issue.ClientID %>').style.fontWeight = 'bold';
                }
            }
            if (keycode == "115") {
                radio[2].checked = true;
                if (radio[2].checked) {
                    document.getElementById('<%=lbl_issue.ClientID %>').innerHTML = "Renewal Date";
                    document.getElementById('<%=lbl_due.ClientID %>').innerHTML = "Due Date";
                    document.getElementById('<%=lbl_due.ClientID %>').style.visibility = 'visible';
                    document.getElementById('<%= Txtduedate.ClientID %>').style.visibility = 'visible';
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').innerHTML = "Renewaling Books";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.forecolor = "Green";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=btnaccno.ClientID %>').style.disabled = false;
                    document.getElementById('<%=lbl_issue.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=lbl_due.ClientID %>').style.fontWeight = 'bold';
                }
            }
            if (keycode == "119") {
                radio[3].checked = true;
                if (radio[3].checked) {
                    document.getElementById('<%=lbl_issue.ClientID %>').innerHTML = "Issue Date";
                    document.getElementById('<%=lbl_due.ClientID %>').innerHTML = "Lost Date";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').innerHTML = "Lost Books";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.forecolor = "Green";
                    document.getElementById('<%=lblIssSpreadName.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=btnaccno.ClientID %>').style.disabled = false;
                    document.getElementById('<%=lbl_issue.ClientID %>').style.fontWeight = 'bold';
                    document.getElementById('<%=lbl_due.ClientID %>').style.fontWeight = 'bold';
                }
            }
            if (keycode == "13") {
                var AccNo = document.getElementById("<%=Txtaccno.ClientID%>").value;
                var focusedAccNo = document.getElementById("<%=Txtaccno.ClientID%>")
                if (document.activeElement === focusedAccNo) {
                    if (AccNo == "") {
                        document.getElementById('<%=Btnsave.ClientID %>').style.backgroundColor = "LightGreen";
                        document.getElementById("<%=Btnsave.ClientID%>").focus();
                        return false;
                    }
                }
            }
            if (keycode == "37") {
                document.getElementById('<%=BtnYes.ClientID %>').style.backgroundColor = "LightGreen";
                document.getElementById('<%=BtnNo.ClientID %>').style.backgroundColor = "white";
                document.getElementById("<%=BtnYes.ClientID%>").focus();
            }
            if (keycode == "39") {
                document.getElementById('<%=BtnNo.ClientID %>').style.backgroundColor = "LightGreen";
                document.getElementById('<%=BtnYes.ClientID %>').style.backgroundColor = "white";
                document.getElementById("<%=BtnNo.ClientID%>").focus();

            }
            if (keycode == "38") {
                document.getElementById('<%=BtnIssueYesAgain.ClientID %>').style.backgroundColor = "LightGreen";
                document.getElementById('<%=BtnIssueNoAgain.ClientID %>').style.backgroundColor = "white";
                document.getElementById("<%=BtnIssueYesAgain.ClientID%>").focus();
            }
            if (keycode == "40") {
                document.getElementById('<%=BtnIssueNoAgain.ClientID %>').style.backgroundColor = "LightGreen";
                document.getElementById('<%=BtnIssueYesAgain.ClientID %>').style.backgroundColor = "white";
                document.getElementById("<%=BtnIssueNoAgain.ClientID%>").focus();
            }
        }


    </script>
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }

        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }

        .fontcolorb
        {
            color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <asp:Label ID="backvolume" runat="server" Style="margin: 0px; margin-top: 8px; margin-bottom: 8px; position: relative;"
                Text="Book Issue and Return" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
        </div>
        <div id="pnlBranch" runat="server" style="color: Black; font-family: Book Antiqua; height: 240px; width: 965px; margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative; text-align: left;"
            class="maintablestyle">
            <asp:UpdatePanel ID="UpMain" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblclg" runat="server" Text="<b>College</b>" CssClass="commonHeaderFont">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcollege" runat="server" CssClass="dropdown commonHeaderFont"
                                            Width="184px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                            <asp:ListItem Text="All"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 357px; height: 2px; border: 2px solid #ffffff; margin-left: -3px;">
                                    <asp:UpdatePanel ID="UP_issue" runat="server" style="margin-top: -12px;">
                                        <ContentTemplate>
                                            <style>
                                                text-align: center;
                                            </style>
                                            <asp:RadioButtonList ID="rblissue" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblissue_Selected" Enabled="True" Style="margin-left: -10px; font-size: small; font-weight: bold;"
                                                Font-Names=" Book antiqua">
                                                <asp:ListItem Text="Issue(F1)" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Return(F2)" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Renewal(F4)" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Lost(F8)" Value="3"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <fieldset style="width: 254px; height: 2px; margin-left: 7px; border: 2px solid #ffffff;">
                                    <asp:UpdatePanel ID="Up_MemType" runat="server" style="margin-top: -9px;">
                                        <ContentTemplate>
                                            <asp:RadioButtonList ID="RblMemType" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="RblMemType_Selected" Enabled="True"
                                                Font-Names=" Book antiqua" Style="font-family: Book antiqua; margin-left: -16px;">
                                                <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Non member" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <%--  <fieldset style="width: 925px; height: 20px;">--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="LblCardTyp" runat="server" Text="<b>Card Type:</b> " Style="margin-left: -4px;"
                                                        CssClass="commonHeaderFont">
                                                    </asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="LblLibName" runat="server" Text="<b>Library Name</b>" Style="margin-left: 7px;"
                                                        CssClass="commonHeaderFont">
                                                    </asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="Cbo_CardLibrary" runat="server" CssClass="dropdown commonHeaderFont"
                                                        Width="150px" font-colour="black" AutoPostBack="True" OnSelectedIndexChanged="Cbo_CardLibrary_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:Button ID="BtnLib" Style="height: 25px; width: 25px; margin-left: 152px; margin-top: -20px;"
                                                runat="server" CssClass="textbox btn2" Text="#" OnClick="BtnLib_click" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="LblBkType" runat="server" Text="<b>Book Type</b>" Style="margin-left: 15px;"
                                                        CssClass="commonHeaderFont">
                                                    </asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlBookType" runat="server" CssClass="dropdown commonHeaderFont"
                                                        Width="150px" AutoPostBack="True">
                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                        <asp:ListItem Text="Book"></asp:ListItem>
                                                        <asp:ListItem Text="Periodicals"></asp:ListItem>
                                                        <asp:ListItem Text="Project book"></asp:ListItem>
                                                        <asp:ListItem Text="Non-Book Material"></asp:ListItem>
                                                        <asp:ListItem Text="Question Bank"></asp:ListItem>
                                                        <asp:ListItem Text="Back Volume"></asp:ListItem>
                                                        <asp:ListItem Text="Reference Books"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblCardType" runat="server" Text="<b>Card Type:</b> " Style="margin-left: 43px;"
                                                CssClass="commonHeaderFont">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlCardType" runat="server" CssClass="dropdown commonHeaderFont"
                                                        Width="150px" OnSelectedIndexChanged="ddlCardType_OnSelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <%-- </fieldset>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbluser" runat="server" Text="<b>User Entry</b>" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_userentry" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddluserentry" runat="server" CssClass="dropdown commonHeaderFont"
                                            Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddluserentry_SelectedIndexChanged">
                                            <asp:ListItem Text="Roll Number"></asp:ListItem>
                                            <asp:ListItem Text="Library ID"></asp:ListItem>
                                            <asp:ListItem Text="Register Number"></asp:ListItem>
                                            <asp:ListItem Text="Smart Card"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="TxtSmartCard" runat="server" AutoPostBack="true" Visible="false"
                                            Width="70px" Height="12px" TextMode="Password" CssClass="textbox textbox1" OnTextChanged="TxtSmartCard_OnTextChanged"></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblaccno" runat="server" Text="<b>Access No</b>" Style="margin-left: 10px;"
                                    CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <span style="color: Red;">
                                    <blink>*</blink>
                                </span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel50" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txtaccno" runat="server" AutoPostBack="true" Style="height: 12px; width: 145px; margin-left: -15px;"
                                            CssClass="textbox textbox1" OnTextChanged="Txtaccno_OnTextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender_Txtaccno" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetAccNo" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txtaccno"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btnaccno" Width="25px" Height="25px" runat="server" CssClass="textbox btn2"
                                            Text="?" OnClick="btnaccno_Click" Enabled="true" />
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:PostBackTrigger ControlID="Txtaccno" />
                                        <asp:PostBackTrigger ControlID="btnaccno" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                                <%--<asp:Button ID="Btnadd" Width="29px" runat="server" CssClass="textbox btn2" Text="+"
                            OnClick="btnadd_Click" />--%>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblisbook" Style="margin-left: 9px;" runat="server" Text="<b>Issued</b>"
                                    CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="TxtissuedCount" runat="server" AutoPostBack="true" Style="height: 12px; width: 31px; margin-left: 34px;"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                <asp:Label ID="lblReturnCnt" runat="server" Style="margin-left: 10px;" Text="<b>Returned</b>"
                                    CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="txtReturnedCount" runat="server" AutoPostBack="true" Style="height: 12px; width: 31px; margin-left: 28px;"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbllib" runat="server" Text="<b>Library</b>" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddllibrary" runat="server" CssClass="dropdown commonHeaderFont"
                                            Width="179px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                            <asp:ListItem Text="All"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblissuetype" runat="server" Text="<b>Issue Type</b>" Style="margin-left: 10px;"
                                    CssClass="commonHeaderFont">
                                </asp:Label>
                                <style>
                                    blink, .blink
                                    {
                                        animation: blinker 1s linear infinite;
                                    }

                                    @keyframes blinker
                                    {
                                        100%
                                        {
                                            opacity: 0;
                                        }
                                    }
                                </style>
                                <span style="color: Red;">
                                    <blink>*</blink>
                                </span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlissue" runat="server" CssClass="dropdown commonHeaderFont"
                                            Style="width: 184px; margin-left: -14px;" AutoPostBack="True" OnSelectedIndexChanged="ddlissuetype_SelectedIndexChanged">
                                            <asp:ListItem Text="Book"></asp:ListItem>
                                            <asp:ListItem Text="Periodicals"></asp:ListItem>
                                            <asp:ListItem Text="Project book"></asp:ListItem>
                                            <asp:ListItem Text="Non-Book Material"></asp:ListItem>
                                            <asp:ListItem Text="Question Bank"></asp:ListItem>
                                            <asp:ListItem Text="Back Volume"></asp:ListItem>
                                            <asp:ListItem Text="Reference Books"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="LblDueBkCnt" runat="server" Style="margin-left: 10px;" Text="<b>Due Books</b>"
                                    CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="Txt_DueBookCount" runat="server" AutoPostBack="true" Width="31px"
                                    Height="12px" CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                <asp:Label ID="Label5" runat="server" Style="margin-left: 10px;" Text="<b>Fine Amount</b>"
                                    CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="TxtFineAmount" runat="server" AutoPostBack="true" Width="31px" Height="12px"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel18" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUserEntryId" runat="server" Text="<b>Roll No</b>" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                        <span style="color: Red;">
                                            <blink>*</blink>
                                        </span>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel49" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtRollNo" onkeypress="txtRollNo_KeyDown" runat="server" OnTextChanged="txtRollNo_Change"
                                            AutoPostBack="true" Width="140px" Height="12px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRollNo"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btnissutype" Width="25px" Height="25px" runat="server" CssClass="textbox btn2"
                                            Text="?" OnClick="btnissutype_Click" />
                                    </ContentTemplate>
                                   <%-- <Triggers>
                                        <asp:PostBackTrigger ControlID="txtRollNo" />
                                        <asp:PostBackTrigger ControlID="btnissutype" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbl_issue" runat="server" Text="<b>Issue Date</b>" Style="margin-left: 10px;"
                                            CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:TextBox ID="txtissuedate" runat="server" AutoPostBack="true" Style="width: 80px; margin-left: -14px; height: 12px;"
                                    CssClass="textbox textbox1" OnTextChanged="txtissuedate_OnTextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtissuedate"
                                    runat="server" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:TextBox ID="txtreturn" runat="server" AutoPostBack="true" Width="80px" Height="12px"
                                    CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtreturn" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td rowspan="4" colspan="2">
                                <asp:Image ID="imgBook" runat="server" Visible="true" Style="height: 100px; width: 100px; margin-left: 13px;" />
                                <asp:Image ID="img_stud1" runat="server" Visible="true" Style="height: 100px; width: 100px; margin-left: 8px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="<b>Name</b>" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtName" runat="server" AutoPostBack="true" Width="140px" Height="12px"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                <asp:Button ID="btnname" Width="25px" Height="25px" runat="server" CssClass="textbox btn2"
                                    Text="?" OnClick="btnissuname_Click" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbl_due" runat="server" Text="<b>Due Date</b>" Style="margin-left: 10px;"
                                            CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="Txtduedate" runat="server" AutoPostBack="true" Style="width: 80px; height: 12px; margin-left: -16px;"
                                                        CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="Txtduedate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="Chk_SelectedDate" runat="server" Style="margin-left: -3px;" AutoPostBack="false" />
                                            <asp:Label ID="lblSelectDt" runat="server" Style="margin-left: -3px;" Text="<b>SelectedDate</b>"
                                                CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<b>Department</b>" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDept" runat="server" AutoPostBack="true" Width="170px" Height="12px"
                                    CssClass="textbox textbox1"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbleligi" runat="server" Text="<b>Eligibility</b>" CssClass="commonHeaderFont"
                                    Style="margin-left: 10px;" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="txt_elgi" runat="server" AutoPostBack="true" Style="height: 12px; width: 31px; margin-left: 11px;"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblisssued" runat="server" Text="<b>Issued</b>" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="txt_issued" runat="server" AutoPostBack="true" Style="height: 12px; width: 31px; margin-left: 5px;"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcode" runat="server" Text="<b>Token No.</b>" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcodenumber" runat="server" CssClass="dropdown commonHeaderFont"
                                    Width="180px" AutoPostBack="True">
                                    <%--OnSelectedIndexChanged="ddlcodenumber_SelectedIndexChanged"--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblunlocked" runat="server" Text="<b>Unlocked</b>" CssClass="commonHeaderFont"
                                    Style="margin-left: 10px;" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="txt_Unlocked" runat="server" AutoPostBack="true" Style="height: 12px; width: 31px; margin-left: 15px;"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lbllocked" runat="server" Text="<b>Locked</b>" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="txtlocked" runat="server" AutoPostBack="true" Width="31px" Height="12px"
                                    CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <%--<Triggers>
                    <asp:PostBackTrigger ControlID="Txtaccno" />
                </Triggers>--%>
            </asp:UpdatePanel>
        </div>
        <center>
            <div id="divSpreadBookName" runat="server" style="margin-left: -293px;">
                <asp:Label ID="LblSpreadBookName" runat="server" Text="<b>Books In Hand</b>" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua" Visible="true" ForeColor="Green">
                </asp:Label>
                <asp:Label ID="lblcolorOver" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="Red" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label6" runat="server" Text="Over Due" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label7" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="Black" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label8" runat="server" Text="Book" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label9" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="Green" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label10" runat="server" Text="Non Book" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label11" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="Brown" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label14" runat="server" Text="Journal" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label15" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="deepskyblue" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label16" runat="server" Text="Question" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label17" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="mediumpurple" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label18" runat="server" Text="Project" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label34" runat="server" Text="" Width="13px" Height="13px" CssClass="commonHeaderFont"
                    BackColor="deeppink" Font-Names=" Book antiqua">
                </asp:Label>
                <asp:Label ID="Label33" runat="server" Text="Ref Book" CssClass="commonHeaderFont"
                    Font-Names=" Book antiqua">
                </asp:Label>
            </div>
            <div id="BooksInHand" runat="server" style="width: 1000px; overflow: auto; height: auto;">
                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                    <ContentTemplate>
                        <div id="divSpreadBookInHand" runat="server" visible="true">
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="GrdBookInHand" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" Style="background-color: lightyellow;" Width="960px"
                                ShowHeaderWhenEmpty="true" toGenerateColumns="true" OnRowDataBound="GrdBookInHand_OnRowDataBound"
                                OnRowCreated="GrdBookInHand_OnRowCreated" OnSelectedIndexChanged="GrdBookInHand_SelectedIndexChanged">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                <ContentTemplate>
                    <div id="LostAndFineDiv" runat="server" visible="false" style="color: Black; font-family: Book Antiqua; height: 42px; width: 950px; margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative; text-align: left;"
                        class="maintablestyle">
                        <table>
                            <tr>
                                <td id="TdAmt" runat="server" visible="false" style="width: 364px;">
                                    <asp:Label ID="lblDueAmount" runat="server" Text="Amount" CssClass="commonHeaderFont"
                                        Style="font-family: Book antiqua;"> </asp:Label>
                                    <asp:TextBox ID="txt_amount" runat="server" AutoPostBack="true" Width="40px" Height="15px"
                                        CssClass="textbox textbox1" Enabled="false" MaxLength="8"></asp:TextBox>
                                    <%--<asp:FilteredTextBoxExtender ID="Filteredtxt_amount" runat="server" TargetControlID="txt_amount"
                                        FilterType="Numbers" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Label ID="lblDueDays" runat="server" Text="DueDays" CssClass="commonHeaderFont"
                                        Style="font-family: Book antiqua;"> </asp:Label>
                                    <asp:TextBox ID="txt_days" runat="server" AutoPostBack="true" Style="height: 15px; width: 40px;"
                                        CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_days"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lbl_TotalDue" runat="server" Text="TotalDue" CssClass="commonHeaderFont"></asp:Label>
                                    <asp:TextBox ID="txt_TotalDue" runat="server" AutoPostBack="true" Style="height: 15px; width: 40px;"
                                        CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                    <%--   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_TotalDue"
                                        FilterType="Numbers" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>--%>
                                </td>
                                <td id="rbfine" runat="server" visible="false" style="width: 156px;">
                                    <asp:RadioButtonList ID="rblfine" Style="font-family: Book antiqua;" runat="server"
                                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblfine_OnSelectedIndexChanged"
                                        Font-Names=" Book antiqua">
                                        <asp:ListItem Text="Newbook" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Fine" Selected="True" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td id="tdfine" runat="server" visible="false" style="width: 165px;">
                                    <asp:DropDownList ID="ddlFine" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFine_OnSelectedIndexChanged"
                                        CssClass="textbox textbox1 ddlheight4" Style="width: 70px;">
                                        <asp:ListItem Text="Single"></asp:ListItem>
                                        <asp:ListItem Text="Double"></asp:ListItem>
                                        <asp:ListItem Text="Triple"></asp:ListItem>
                                        <asp:ListItem Text="Four"></asp:ListItem>
                                        <asp:ListItem Text="Five"></asp:ListItem>
                                        <asp:ListItem Text="Six"></asp:ListItem>
                                        <asp:ListItem Text="Seven"></asp:ListItem>
                                        <asp:ListItem Text="Eight"></asp:ListItem>
                                        <asp:ListItem Text="Nine"></asp:ListItem>
                                        <asp:ListItem Text="Ten"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblBookPrice" runat="server" Text="price" CssClass="commonHeaderFont"
                                        Style="font-family: Book antiqua;"> </asp:Label>
                                    <asp:TextBox ID="txt_lostprice" runat="server" AutoPostBack="true" Style="height: 15px; width: 40px;"
                                        CssClass="textbox textbox1" Enabled="false"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_lostprice"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td id="Tdfinecnl" runat="server" visible="false">
                                    <asp:CheckBox ID="ChkCancel" runat="server" AutoPostBack="true" Text="Cancel" OnCheckedChanged="ChkCancel_OnCheckedChanged"
                                        Style="font-family: Book antiqua;" />
                                    <asp:Label ID="lblReason" Visible="false" runat="server" Text="Reason" CssClass="commonHeaderFont"></asp:Label>
                                    <asp:Button ID="Btnaddd" Visible="false" runat="server" OnClick="btnadd_Click" CssClass="textbox btn2"
                                        Text="+" Width="20px" />
                                    <asp:DropDownList ID="ddl_Reason" Visible="false" Width="59px" runat="server" AutoPostBack="true"
                                        CssClass="textbox textbox1 ddlheight4">
                                    </asp:DropDownList>
                                    <asp:Button ID="Btndelete" runat="server" Visible="false" OnClick="btndel_Click"
                                        CssClass="textbox btn2" Text="-" Width="20px" />
                                    <asp:TextBox ID="Txt_ActAmount" Visible="false" runat="server" AutoPostBack="true"
                                        Style="height: 15px; width: 47px;" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:TextBox ID="Txt_CurRcptNo" runat="server" AutoPostBack="true" Style="height: 15px; width: 47px;"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblIssSpreadName" runat="server" Text="<b>Issuing Books</b>" Style="margin-left: 55px;"
                                                CssClass="commonHeaderFont" ForeColor="Green" Visible="true" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divIssuingBook" runat="server" visible="true" style="margin-left: 24px; height: auto;"
                                        width="1000px;">
                                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="GrdIssuingBook" runat="server" ShowFooter="false" Width="908px"
                                                    Style="margin-left: 30px; background-color: lightyellow;" AutoGenerateColumns="true"
                                                    Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="true"
                                                    AllowPaging="true" PageSize="10" OnRowDataBound="GrdIssuingBook_OnRowDataBound">
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                            <ContentTemplate>
                                <div id="divBtn" runat="server" style="width: 100px; height: 100px">
                                    <asp:Button ID="BtnRemove" Text="Remove" CssClass="textbox btn1" Style="width: 56px; height: 27px; background-color: violet; font-family: Book Antiqua; font-weight: bolder; margin-top: 48px;"
                                        runat="server" OnClick="BtnRemove_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </center>
        <div style="color: Black; font-family: Book Antiqua; height: 100px; width: 956px; margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative; text-align: left;"
            class="maintablestyle">
            <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblavailable" runat="server" Text="Available:" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="Txtavailable" runat="server" Width="47px" Height="15px" CssClass="textbox textbox1"
                                    Enabled="false"></asp:TextBox>
                                <%--</td>
                    <td>--%>
                                <asp:Label ID="LblIssue" runat="server" Text="Issue:" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="TxtIssue" runat="server" Width="47px" Height="15px" CssClass="textbox textbox1"
                                    Enabled="false"></asp:TextBox>
                                <%--   </td>
                    <td>--%>
                                <asp:Label ID="lblmissing" runat="server" Text="Missing:" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="TxtMissing" runat="server" Width="47px" Height="15px" CssClass="textbox textbox1"
                                    Enabled="false"></asp:TextBox>
                                <%-- </td>
                    <td>--%>
                                <asp:Label ID="lbllost" runat="server" Text="Lost:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="Txtlost" runat="server" Width="47px" Height="15px" CssClass="textbox textbox1"
                                    Enabled="false"></asp:TextBox>
                                <%--</td>
                    <td>--%>
                                <asp:Label ID="LblTotBooks" runat="server" Text="Total:" CssClass="commonHeaderFont"
                                    Font-Names=" Book antiqua">
                                </asp:Label>
                                <asp:TextBox ID="TxtTotBooks" runat="server" Width="47px" Height="15px" CssClass="textbox textbox1"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="ChkissueDet" runat="server" AutoPostBack="true" Style="margin-left: 140px;"
                                            Text="View Current Issue Details" OnCheckedChanged="chkissueDet_OnCheckedChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblview" runat="server" Text="View" CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:DropDownList ID="ddlview" runat="server" CssClass="dropdown commonHeaderFont"
                                    Style="width: 175px; height: 23px; margin-left: 55px;" AutoPostBack="True">
                                    <%--OnSelectedIndexChanged="ddlview_SelectedIndexChanged"--%>
                                    <asp:ListItem Text="View Stack Status"></asp:ListItem>
                                    <asp:ListItem Text="Reservation"></asp:ListItem>
                                    <asp:ListItem Text="Transaction Report"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-left: -80px;"
                                    OnClick="Btngo_Click" />
                                <%--<asp:Button ID="Btngo" Width="29px" runat="server" CssClass="textbox btn2" Text="Go"
                            OnClick="Btngo_Click" />--%>
                            </td>
                            <td colspan="3">
                                <asp:LinkButton ID="Lnksetting" Text="Setting" Font-Name="Book Antiqua" Font-Size="11pt"
                                    OnClick="lnkSetting_Click" runat="server" Width="22px" />
                                <asp:LinkButton ID="LinkButton1" Text="Trace Book" Style="margin-left: 42px;" Font-Name="Book Antiqua"
                                    Font-Size="11pt" OnClick="lnktracebook_Click" runat="server" Width="92px" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="ChkreturnDet" runat="server" AutoPostBack="true" Style="margin-left: 140px;"
                                            Text="View Current Return Details" OnCheckedChanged="ChkreturnDet_OnCheckedChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel42" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblaccnumber" runat="server" Text="Acc No Info:" CssClass="commonHeaderFont"
                                            Style="font-family: Book antiqua; margin-left: 0px;">
                                        </asp:Label>
                                        <asp:TextBox ID="txtaccnumber" runat="server" AutoPostBack="true" Style="height: 12px; width: 170px;"
                                            CssClass="textbox textbox1" OnTextChanged="txtaccnumber_OnTextChanged"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" Text="Reservation" CssClass="textbox btn1"
                                            Style="width: 90px; margin-left: 9px; background-color: violet; font-family: Book Antiqua; font-weight: bolder; height: 27px;"
                                            OnClick="BtnReser_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpSave" runat="server">
                                    <ContentTemplate>
                                        <input type="hidden" id="div_position" name="div_position" />
                                        <asp:Button ID="Btnsave" runat="server" Text="Issue" TabIndex="1" CssClass="textbox btn1"
                                            Style="width: 62px; font-family: Book Antiqua; font-weight: bolder; margin-left: -12px; height: 27px;"
                                            OnClick="Btnsave_Click" />
                                    </ContentTemplate>
                                    <%-- <Triggers>
                                        <asp:PostBackTrigger ControlID="Btnsave" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpClear" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="BtnClear" runat="server" Text="Clear" TabIndex="9" CssClass="textbox btn1"
                                            Style="width: 50px; margin-left: -109px; background-color: violet; font-family: Book Antiqua; font-weight: bolder; height: 27px;"
                                            OnClick="BtnClear_Click" />
                                        <asp:Button ID="BtnHistory" runat="server" Text="History" TabIndex="9" CssClass="textbox btn1"
                                            Style="width: 53px; margin-left: 2px; background-color: Green; font-family: Book Antiqua; font-weight: bolder; height: 27px;"
                                            OnClick="BtnHistory_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="ChkDueDet" runat="server" Text="View Current Due Details" Style="font-family: Book Antiqua; margin-left: 140px;"
                                            AutoPostBack="true" OnCheckedChanged="ChkDueDet_OnCheckedChanged" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </center>
    <br />
    <%--History --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel47" runat="server">
            <ContentTemplate>
                <center>
                    <div id="popwindowdetails" runat="server" visible="false" class="popupstyle popupheight1">
                        <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                            OnClick="imagebtnpop2close_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 750px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span style="color: Green;" class="fontstyleheader">Book Details</span>
                                </div>
                            </center>
                            <center>
                                <table id="colour" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <fieldset id="Fieldset8" runat="server" style="width: 33px; font-family: Book Antiqua; height: 7px; background-color: LightYellow;">
                                                <asp:Label ID="Label35" runat="server" Text="Returned"></asp:Label>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 33px; font-family: Book Antiqua; height: 7px; background-color: LightGreen;">
                                                <asp:Label ID="Label36" runat="server" Text="Issued"></asp:Label>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 33px; font-family: Book Antiqua; height: 7px; background-color: Red;">
                                                <asp:Label ID="Label37" runat="server" Text="Lost"></asp:Label>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel48" runat="server">
                                    <ContentTemplate>
                                        <div id="grid2div" runat="server" style="height: 600px; overflow: auto; width: 850px;">
                                            <asp:GridView ID="grdHistory" runat="server" ShowFooter="false" ShowHeader="true"
                                                Style="height: 600px; width: 850px; font-family: Book Antiqua;" Font-Names="book antiqua"
                                                Visible="false" OnRowDataBound="grdHistory_OnRowDataBound">
                                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- sssss--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel41" runat="server">
            <ContentTemplate>
                <div id="DivBookReservation" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton11" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 300px;"
                        OnClick="imagebtnReservepopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 350px; width: 650px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="LblSpreadReserveName" runat="server" Text="<b>Reservation Details</b>"
                                        CssClass="commonHeaderFont" Font-Names=" Book antiqua" ForeColor="Green">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="DivReservation" runat="server" style="width: 500px; height: 120px;">
                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                            <ContentTemplate>
                                                <div id="divSpreadReservation" runat="server" visible="true" width="500px">
                                                    <asp:GridView ID="grdReservation" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                                        Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="false"
                                                        AllowPaging="true" PageSize="10">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField ItemStyle-Width="100px" DataField="access_number" HeaderText="Access No" />
                                                            <asp:BoundField ItemStyle-Width="100px" DataField="title" HeaderText="Title" />
                                                            <asp:BoundField ItemStyle-Width="200px" DataField="access_date" HeaderText="Requested Date" />
                                                            <asp:BoundField ItemStyle-Width="200px" DataField="access_time" HeaderText="Requested Time" />
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel34" runat="server">
            <ContentTemplate>
                <div id="popupselectlibid" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_popupselectlibid" runat="server" Text="Select Library ID" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Sec">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Visible="true" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <asp:Label ID="lbl_lib_id" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Library Id">
                                    </asp:Label>
                                    <asp:TextBox ID="tx_libid" runat="server" Style="width: 137px; margin-left: 23px"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtendertx_libid" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="tx_libid"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:Label ID="lb_name" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Name">
                                    </asp:Label>
                                    <asp:TextBox ID="tx_libname" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="tx_libname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:ImageButton ID="btn_go_libid" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btn_go_libid_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divRollNo" runat="server" runat="server" style="height: 400px; overflow: auto; width: 800px;">
                            <asp:HiddenField ID="SelectedGridCell" runat="server" Value="-1" />
                            <asp:GridView ID="grdStudent" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStudent_onselectedindexchanged"
                                OnRowCreated="grdStudent_OnRowCreated" Width="840px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_std_exit1" runat="server" CssClass="textbox btn2" Text="Exit"
                                    OnClick="btn_std_exit1_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel35" runat="server">
            <ContentTemplate>
                <div id="DivpopupStaff" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose4_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_staff" runat="server" Text="Select Staff" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staffdept" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_staffdept" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddl_staffdept_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_staffname" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btn_staff_Go" Text="Go" OnClick="btn_staff_Go_Click" CssClass="textbox btn1"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divGrdstaff" runat="server" style="height: 400px; width: 700px; overflow: auto;">
                            <asp:HiddenField ID="HiddenFieldgrdStaff" runat="server" Value="-1" />
                            <asp:GridView ID="grdStaff" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" Width="650px" OnRowCreated="grdStaff_OnRowCreated"
                                OnSelectedIndexChanged="grdStaff_onselectedindexchanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_staff_exit1" runat="server" CssClass="textbox btn2" Text="Exit"
                                    OnClick="btn_staff_exit1_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel27" runat="server">
            <ContentTemplate>
                <div id="popalertsetting" runat="server" visible="false" style="height: 550em; z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%; left: 0%;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 170px; width: 27%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%; right: 5%; top: 25%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label12" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chkrackallocation" runat="server" AutoPostBack="false" OnSelectedIndexChanged="Chkrackallocation_SelectedIndexChanged" />
                                            <asp:Label ID="Label13" runat="server" Text="Auto Rack Allocation" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chkdis" runat="server" AutoPostBack="false" OnSelectedIndexChanged="Chkstudent_SelectedIndexChanged" />
                                            <asp:Label ID="lbldiscontious" runat="server" Text="Inculde Discontinue Student"
                                                CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chkinclude" runat="server" AutoPostBack="false" OnSelectedIndexChanged="Chkinclude_SelectedIndexChanged" />
                                            <asp:Label ID="lblcards" runat="server" Text="Inculde Fine For Merit Cards" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Chkover" runat="server" AutoPostBack="false" OnSelectedIndexChanged="Chkover_SelectedIndexChanged" />
                                            <asp:Label ID="lblover" runat="server" Text="Overnight Issue" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btnover" runat="server" ImageUrl="~/LibImages/close.jpg" OnClick="Btnsettingclose_Click" />
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
    <%--Pop For Set Default  --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
            <ContentTemplate>
                <div id="SureDivSetDefault" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div1" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblDef" runat="server" Text="Do You want to set this as default?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="btn_DeleteDefYes" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                    OnClick="btn_DeleteDefYes_Click" />
                                                <asp:ImageButton ID="btn_DeleteDefNo" runat="server" ImageUrl="~/LibImages/no.jpg"
                                                    OnClick="btn_DeleteDefNo_Click" />
                                            </center>
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
    <%-- Popup for Message--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel37" runat="server">
            <ContentTemplate>
                <div id="DivMess" runat="server" visible="false" style="overflow-y: scroll; height: 200%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblMessage" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="BtnYes" runat="server" TabIndex="2" Text="Yes" CssClass="textbox btn1"
                                                            Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                            OnClick="btn_Yes_Click" />
                                                        <asp:Button ID="BtnNo" runat="server" TabIndex="3" Text="No" CssClass="textbox btn1"
                                                            Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                            OnClick="btn_No_Click" />
                                                        <%-- <asp:ImageButton ID="BtnYes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btn_Yes_Click" />
                                                        <asp:ImageButton ID="BtnNo" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btn_No_Click" />--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel38" runat="server">
            <ContentTemplate>
                <div id="DivMess1" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblMessage1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpMessYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnMessYes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnMessYes_Click" />
                                                        <asp:ImageButton ID="btnMessNo" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnMessNo_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel39" runat="server">
            <ContentTemplate>
                <div id="DivPopName" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="LblPopName" runat="server" Text="Select Name" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="LblNamePop" runat="server" Text="Student Name" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStudentName" runat="server" AutoPostBack="true" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpNameGo" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="BtnNameGo" Text="Go" OnClick="BtnNameGo_Click" CssClass="textbox btn1"
                                                runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="divNameStu" runat="server" style="height: 400px; width: 800px; overflow: auto;">
                            <asp:HiddenField ID="HiddenFieldName" runat="server" Value="-1" />
                            <asp:GridView ID="GrdName" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" Width="800px" OnRowCreated="GrdName_OnRowCreated"
                                OnSelectedIndexChanged="GrdName_onselectedindexchanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <%--<asp:ImageButton ID="BtnStuNameOk" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="BtnStuNameOk_Click" />--%>
                                <asp:ImageButton ID="BtnStuNameExit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="BtnStuNameExit_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- AccessNoLookUp--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel40" runat="server">
            <ContentTemplate>
                <div id="popupselectBook" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <br />
                    <div id="spreadBook" runat="server" style="background-color: White; font-family: Book Antiqua; font-weight: bold; height: 620px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label3" runat="server" Text="Select Access No" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle" width="500px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lb_search" runat="server" Text="Search" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_search" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="dd_search_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search_book" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Gettitle" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search_book"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btn_go_book" runat="server" ImageUrl="~/LibImages/go.jpg" OnClick="btn_go_book_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="grdBook_book" runat="server" style="width: 800px; height: 400px; overflow: auto;">
                            <asp:HiddenField ID="HiddenFieldgrdBook" runat="server" Value="-1" />
                            <asp:GridView ID="grdBook" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdBook_onselectedindexchanged"
                                Width="1067px" OnRowCreated="grdBook_OnRowCreated">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:ImageButton ID="btn_Acc_exit1" runat="server" Visible="false" ImageUrl="~/LibImages/save (2).jpg"
                                    OnClick="btn_Acc_exit1_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--Reports--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="divReports" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose5_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 600px; width: 945px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="LblRptName" runat="server" Text="" class="fontstyleheader" Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <div id="divSpreadReport" runat="server" visible="false" style="width: 900px; overflow: auto; height: 450px; background-color: White; border-radius: 10px;">
                            <asp:GridView ID="grdReport" Width="800px" Height="200px" ShowHeader="false" runat="server"
                                ShowFooter="false" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="false">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        <%--<center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
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
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>--%>
                    </div>
                </div>
            </ContentTemplate>
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed" />
            </Triggers>--%>
        </asp:UpdatePanel>
    </center>
    <%--FineCancelReason Popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel32" runat="server">
            <ContentTemplate>
                <div id="DivFineCnlRea" runat="server" visible="false" style="height: 100%; z-index: 10000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="DivFineReason" runat="server" visible="false" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblFineCancelRea" runat="server" Text="Cancel Reason" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_FineCancelRea" runat="server" Width="200px" Style="font-family: 'Book Antiqua'; margin-left: 13px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox><%--TextMode="MultiLine"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="btn_FineReasonSave" runat="server" ImageUrl="~/LibImages/AddWhite.jpg"
                                            OnClick="btn_FineReasonSave_Click" />
                                        <asp:ImageButton ID="btn_FineReasonExit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="btn_FineReasonExit_Click" />
                                        <%--  <asp:Button ID="btn_FineReasonSave" runat="server" Text="Add" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_FineReasonSave_Click" />
                                <asp:Button ID="btn_FineReasonExit" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_FineReasonExit_Click" />--%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="lblErrNewCardCatoger" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- </asp:Panel>--%>
    </center>
    <%--TraceBook Popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel28" runat="server">
            <ContentTemplate>
                <div id="DivTraceBkUp" runat="server" visible="false" style="height: 100%; z-index: 10000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="DivTrace" runat="server" visible="false" class="table" style="background-color: White; height: 150px; width: 300px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblTraceAccNo" runat="server" Text="Access No" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:TextBox ID="txtTraceAccNo" runat="server" BackColor="Bisque" MaxLength="15"
                                            Style="font-family: Book Antiqua; height: 18px; width: 155px; margin-left: 33px;"
                                            Font-Bold="True" Font-Size="Medium"></asp:TextBox><%--TextMode="MultiLine"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblTraceLib" runat="server" Text="Library" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddlTraceLib" runat="server" BackColor="Bisque" Style="height: 22px; width: 160px; margin-left: 53px;"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblTraceIss" runat="server" Text="Issue Type" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddlTraceIssTyp" runat="server" BackColor="Bisque" Style="height: 22px; width: 161px; margin-left: 30px;"
                                            AutoPostBack="True">
                                            <asp:ListItem Text="Book"></asp:ListItem>
                                            <asp:ListItem Text="Periodicals"></asp:ListItem>
                                            <asp:ListItem Text="Project book"></asp:ListItem>
                                            <asp:ListItem Text="Non-Book Material"></asp:ListItem>
                                            <asp:ListItem Text="Question Bank"></asp:ListItem>
                                            <asp:ListItem Text="Back Volume"></asp:ListItem>
                                            <asp:ListItem Text="Reference Books"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Tdbut" align="center" runat="server">
                                        <asp:ImageButton ID="BtnTraceAdd" runat="server" ImageUrl="~/LibImages/AddWhite.jpg"
                                            Style="margin-top: 10px;" OnClick="BtnTraceAdd_Click" />
                                        <asp:ImageButton ID="BtnTraceDel" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                            OnClick="BtnTraceDel_Click" />
                                        <asp:ImageButton ID="BtnTraceExit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                            OnClick="BtnTraceExit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="Label20" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- </asp:Panel>--%>
    </center>
    <%--Accessed Book Details Popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel45" runat="server">
            <ContentTemplate>
                <div id="DivAccessBookDet" runat="server" visible="false" style="height: 100%; z-index: 10000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <asp:ImageButton ID="ImgBtnAccessBookDet" runat="server" ImageUrl="~/images/close.png"
                        Style="height: 40px; width: 40px; height: 30px; width: 30px; position: absolute; margin-top: 202px; margin-left: 197px;"
                        OnClick="ImgBtnAccessBookDet_Click" />
                    <center>
                        <div id="DivAcessBkDet" runat="server" visible="false" class="table" style="background-color: White; height: 400px; width: 450px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <asp:Label ID="Label24" runat="server" Text="Accessed Book Details" class="fontstyleheader"
                                    Style="color: Green;"></asp:Label>
                            </center>
                            <table border="1">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label19" runat="server" Text="Access No" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccDet" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text="Title" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccTitle" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="Author" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccAuthor" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text="Edition" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccEdition" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label26" runat="server" Text="Price" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccPrice" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label27" runat="server" Text="Department" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccDept" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label28" runat="server" Text="Status" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccStatus" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblAccRoll" runat="server" Text="Roll No" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccRollNo" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblAcStuName" runat="server" Text="Stud Name" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccStuName" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label29" runat="server" Text="Publisher" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccPub" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label30" runat="server" Text="Bill No" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccBill" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label31" runat="server" Text="Book Type" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccBkType" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label32" runat="server" Text="Rack/Shelf No." Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblAccShelf" runat="server" Text="" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- </asp:Panel>--%>
    </center>
    <%-- Popup for Error Message--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
            <ContentTemplate>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpalertMsg" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btn_errorclose" runat="server" TabIndex="8" Text="Ok" CssClass="textbox btn1"
                                                            Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                            OnClick="btn_errorclose_Click" />
                                                        <%-- <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btn_errorclose_Click" />--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
    <%-- Popup for 2ndTimeIssueMessage--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel29" runat="server">
            <ContentTemplate>
                <div id="DivIssue" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblIssuesName" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="BtnIssueYesAgain" runat="server" TabIndex="4" Text="Yes" CssClass="textbox btn1"
                                                    Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                    OnClick="btnIssueYes_Click" />
                                                <asp:Button ID="BtnIssueNoAgain" runat="server" TabIndex="5" Text="No" CssClass="textbox btn1"
                                                    Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                    OnClick="btnIssueNo_Click" />
                                                <%-- <asp:ImageButton ID="BtnIssueYesAgain" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                    OnClick="btnIssueYes_Click" />
                                                <asp:ImageButton ID="BtnIssueNoAgain" runat="server" ImageUrl="~/LibImages/no.jpg"
                                                    OnClick="btnIssueNo_Click" />--%>
                                            </center>
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
    <%-- Popup for Error Message--%>
    <center>
        <div id="DivNocard" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
            <center>
                <div id="Div7" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="LblNocard" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:ImageButton ID="BtnLblNocard" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="BtnLblNocard_Click" />
                                        <%--<asp:Button ID="BtnLblNocard" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="BtnLblNocard_Click" Text="ok" runat="server" />--%>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Popup for Error Message--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel44" runat="server">
            <ContentTemplate>
                <div id="DivReturnLost" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div12" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblReturnLost" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpReturnLost" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btn_ReturnLostclose" runat="server" TabIndex="7" Text="Yes" CssClass="textbox btn1"
                                                            Style="width: 53px; font-weight: bold; font-family: Book Antiqua; height: 27px;"
                                                            OnClick="btn_ReturnLostclose_Click" />
                                                        <%--<asp:ImageButton ID="btn_ReturnLostclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btn_ReturnLostclose_Click" />--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
    <%-- Popup for Already Reserved books--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel30" runat="server">
            <ContentTemplate>
                <div id="DivReservedbk" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div9" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblReservedbkPop" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/LibImages/yes.jpg"
                                                    OnClick="btnReservedbkYes_Click" />
                                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnReservedbkNo_Click" />
                                                <%--<asp:Button ID="btnMessYes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnMessYes_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btnMessNo" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnMessNo_Click" Text="No" runat="server" />--%>
                                            </center>
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
    <%-- Popup for Error Message transfered,weedout books--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel31" runat="server">
            <ContentTemplate>
                <div id="DivErrorMsg" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div8" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="LblErrorMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_errorMsgclose_Click" />
                                                <%-- <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />--%>
                                            </center>
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
    <%-- Popup for AccessNo Lookup Message--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel33" runat="server">
            <ContentTemplate>
                <div id="AccessNoLookup" runat="server" visible="false" style="height: 100%; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                    <center>
                        <div id="Div10" runat="server" class="table" style="background-color: White; height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAccessNoLookup" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="BtnError_AccessNoLookup_Click" />
                                                <%-- <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />--%>
                                            </center>
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
    <%-- ReservedBookNoLookUp--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel46" runat="server">
            <ContentTemplate>
                <div id="ReservedPopup" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton10" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnReservedPopup_Click" />
                    <br />
                    <br />
                    <div id="Div11" runat="server" style="background-color: White; font-family: Book Antiqua; height: 520px; width: 900px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label21" runat="server" Text="Book Reservation Details" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <div>
                            <asp:GridView ID="GrdReservedBkList" Width="800px" Height="400px" runat="server"
                                ShowFooter="false" AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="false"
                                OnRowDataBound="GrdReservedBkList_OnRowDataBound">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </div>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for UpSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpSave">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpClear--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpClear">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpYes">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpMessYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpMessYes">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpReturnLost--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpReturnLost">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for UpNameGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpNameGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
