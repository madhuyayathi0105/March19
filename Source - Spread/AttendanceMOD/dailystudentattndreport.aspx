<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="dailystudentattndreport.aspx.cs" Inherits="dailystudentattndreport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }

    </script>

    <script type="text/javascript">
        function SelectAll(id) {
            var dropdownListId = document.getElementById(id);
            var e = document.getElementById(id);
            var strUser = e.options[e.selectedIndex].value;
            var array = id.split("_");
            var position = array[3].toString();

            
            
            var grid = document.getElementById('<%=gview.ClientID%>');
            for (var jk = 3; jk < grid.rows.length; jk++) {
                var ddl = document.getElementById('MainContent_gview_ddlmode_' + position + '_' + jk.toString());
                if (ddl.disabled == false) {
                    for (value = 0; value <= ddl.options.length - 1; value++) {
                        if (ddl.options[value].text.trim() == strUser.trim()) {
                            ddl.options[value].selected = strUser;
                            //ddl.options[value].style.color = colr;
                        }
                    }
                }
            }
        }
    </script>

    <style type="text/css">
        .style2
        {
            width: 729px;
        }
        .cursorptr
        {
            cursor: default;
        }
        .txt
        {
        }
        .style4
        {
            width: auto;
        }
        .style5
        {
            width: auto;
        }
        .style6
        {
            width: 119px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin-top: 10px; margin-bottom: 10px;
            position: relative;">AT09-Individual Student Attendance Report</span>
    </center>
    <div>
        <center>
            <asp:UpdatePanel runat="server" ID="Upanel1">
                <ContentTemplate>
                    <table class="maintablestyle" style="width: auto; height: auto; margin-top: 10px;
                        margin-bottom: 10px; position: relative;">
                        <tr>
                            <td>
                                <asp:Label ID="lblddl" runat="server" Text="Select Option" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="optionddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="147px" AutoPostBack="True" OnSelectedIndexChanged="optionddl_SelectedIndexChanged">
                                    <asp:ListItem>Roll No.</asp:ListItem>
                                    <asp:ListItem>Reg No.</asp:ListItem>
                                    <asp:ListItem>Admission No.</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnTextChanged="txtrollno_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtrollno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblname1" runat="server" Text="Student Name: " Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblname2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="BlueViolet"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="50Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="21px" Width="75px"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                                ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="21px" Width="75px"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                                                ValidChars="/" runat="server" TargetControlID="txtToDate">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="UpGo">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="dateerrlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="Upanel2">
                <ContentTemplate>
                    <div>
                        <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red" Style="margin: 0px;
                            margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
                    </div>
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                        <tr>
                            <td class="style1" align="center">
                                <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pageset_pnl" runat="server" BorderStyle="None" Width="1026px">
                                    <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                        TargetControlID="pageddltxt">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search" Width="95px"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                                    <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:CheckBox ID="viewattendall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="viewattendall_CheckedChanged" Text="View Attendance Header"
                                        AutoPostBack="True" Checked="true" />
                                    <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="19px" Width="496px"></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="std_info" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
                                    margin-top: 10px;">
                                    <fieldset style="border-radius: 6px; background-color: MediumSlateBlue; width: 960px;
                                        height: auto;">
                                        <table style="width: 935px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_name" runat="server" Text="Name" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="name" runat="server" Text="" ForeColor="blue" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_class" runat="server" Text="Class" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="clas" runat="server" Text="" ForeColor="blue" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fullday" runat="server" Text="Full Day Absent" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="fullday" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_halfday" runat="server" Text="Half Day Absent" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="halfday" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_totdays" runat="server" Text="Total Days Absent" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="totdays" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_odapplied" runat="server" Text="OD Applied" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="odapplied" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_leaveapplied" runat="server" Text="Leave Applied" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="leaveapplied" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_lastattndate" runat="server" Text="Last Attended Date" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lastattndate" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblHourPercentage" runat="server" Text="Hour Wise Percentage" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblHrsWisePercentage" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDayPercentage" runat="server" Text="Day Wise Percentage" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDaysWisePercentage" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldiscontinu" runat="server" Text="Discontinue Date" Font-Bold="True"
                                                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbdiscontinue" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblread" runat="server" Text="Readmission Date" Font-Bold="True" Visible="false"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblreadmission" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <div runat="server" id="divNote" visible="false">
                                    <table style="width: auto; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                                        <tr>
                                            <td style="width: 10px; padding: 5px; background-color: #008000;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                P -Present
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #FF0000;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                A -Absent
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #800000;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                H -Holiday
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #0000FF;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                OD -Onduty
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #adff2f;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                SOD
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #E9967A;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                ML -Medical Leave
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10px; padding: 5px; background-color: #DAA520;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                NSS
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #0080ff;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                L -Leave
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #EE82EE;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                NCC
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #708090;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                HS
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #FFC0CB;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                PP
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #32cd32;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                SYOD
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10px; padding: 5px; background-color: #D2B48C;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                COD
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #f5deb3;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                OOD
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #8b4513;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                NJ -Not Join
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #000000;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                S
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #FFFF00;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                RAA
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #FF00FF;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                FH -Free Hour
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10px; padding: 5px; background-color: #432F5C;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                SH -Special Hour
                                            </td>
                                            <td style="width: 10px; padding: 5px; background-color: #7B68EE;">
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                -
                                            </td>
                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                NE -Not Enter
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="18">
                                                <fieldset style="width: auto; height: auto;">
                                                    <legend style="font-weight: bold;">Attendance Status </legend>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 10px; padding: 5px; background-color: #008000;">
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                                -
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                                FP -Full Present
                                                            </td>
                                                            <td style="width: 10px; padding: 5px; background-color: #FFC0CB;">
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                                -
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                                HP -Half Present
                                                            </td>
                                                            <td style="width: 10px; padding: 5px; background-color: #FF0000;">
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                                -
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                                FA -Full Absent
                                                            </td>
                                                            <td style="width: 10px; padding: 5px; background-color: #D2691E;">
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                                -
                                                            </td>
                                                            <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                                HA -Half Absent
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center style="width: 994px">
                                    <asp:UpdatePanel runat="server" ID="Upanelgview">
                                        <ContentTemplate>
                                            <div id="gviewdiv" runat="server" style="height: 1000px; width: 1000px; overflow: auto;">
                                                <asp:GridView ID="gview" runat="server" ShowHeader="false" OnRowDataBound="gview_OnRowDataBound">
                                                    <Columns>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                    <RowStyle ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                </center>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="UpanelPrint">
                <ContentTemplate>
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td class="style2" style="width: 900px;">
                                <center>
                                    <div id="rptprint1" runat="server" visible="false">
                                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                            Height="35px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                            CssClass="textbox textbox1" Visible="false" />
                                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                        <asp:UpdatePanel runat="server" ID="Upanelsave">
                                            <ContentTemplate>
                                                <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="60px" Height="35px" CssClass="textbox textbox1" OnClick="btnsave_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel1" />
                    <asp:PostBackTrigger ControlID="btnprintmaster1" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </div>
    <center>
        <asp:UpdatePanel runat="server" ID="Upanelpop">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 25px; width: 23px; position: absolute; margin-top: 197px; margin-left: 118px;"
                        OnClick="btn_popclose_Click" />
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 276px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                            <center>
                                                <asp:Button ID="btnoldrecord" runat="server" CssClass=" textbox btn2" Width="80px"
                                                    OnClick="btnoldrecord_Click" Text="Old Record" />
                                                <asp:Button ID="btnnewrecord" runat="server" CssClass=" textbox btn2" Width="80px"
                                                    OnClick="btnnewrecord_Click" Text="New Record" />
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
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Upanelgview">
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

        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Upanelsave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
