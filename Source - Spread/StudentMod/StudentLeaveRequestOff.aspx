<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentLeaveRequestOff.aspx.cs"
    MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" Inherits="StudentLeaveRequestOff" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Leave Request</title>
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .alter
        {
            top: 120;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .pHeader
        {
            color: white;
            font-size: 11px;
            cursor: pointer;
            padding: 4px;
            font-style: italic;
            font-variant: small-caps;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
            width: 904px;
        }
        .pBody
        {
            background-color: #9DF0E8;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        
        .cpimage
        {
            vertical-align: middle;
            background-color: transparent;
        }
        #lab
        {
            float: right;
            background-color: transparent;
            position: fixed;
        }
        .style5
        {
            width: 289px;
        }
        .style6
        {
            width: 289px;
        }
        .style7
        {
            width: 289px;
        }
        .btstyle
        {
            background-color: transparent;
            font-size: larger;
            font-variant: normal;
            font-family: Arial;
            font-style: normal;
            border-width: 0;
        }
        .btstyle1
        {
            background-color: transparent;
            font-size: larger;
            font-family: Arial Black;
            font-variant: small-caps;
            font-style: normal;
            border-width: 0;
        }
        .style8
        {
            width: 306px;
        }
        BODY
        {
            background-image: url('image/Student/TopNew.jpg');
            background-repeat: no-repeat;
        }
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 6px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptMrgr" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Student Leave Request </span>
        </div>
    </center>
    <center>
        <div class="maindivstyle" style="width: 970px;">
            <br />
            <center>
                <div style="width: 950px;">
                    <div style="height: 85px; width: 950px; background-color: #226399; margin-left: 5px;
                        border-radius: 5px;">
                        <div id="divRequestLink" runat="server" style="margin-left: 5px; height: 78px; width: 60px;
                            padding: 3px; padding-left: 20px; color: White; float: left;">
                            <asp:ImageButton ID="btnLeaveRequest" runat="server" ImageUrl="~/images/LeaveRequest.png"
                                Height="60px" Width="60px" OnClick="ButtonReq_Click" /><br />
                            <b>Request</b>
                        </div>
                        <div id="divApproveRejectLink" runat="server" style="height: 78px; width: 60px; padding: 3px;
                            padding-left: 20px; color: White; float: left;">
                            <asp:ImageButton ID="btnLeaveApproveReject" runat="server" ImageUrl="~/images/LeaveApproveReject.jpg"
                                Height="60px" Width="60px" OnClick="ButtonApprove_Click" /><br />
                            <b>Authorize</b>
                        </div>
                        <div id="divReportLink" runat="server" style="height: 78px; width: 60px; padding: 3px;
                            padding-left: 20px; color: White; float: left;">
                            <asp:ImageButton ID="btnLeaveReport" runat="server" ImageUrl="~/images/LeaveReport.png"
                                Height="60px" Width="60px" OnClick="ButtonReport_Click" /><br />
                            <b>Report</b>
                        </div>
                    </div>
                    <div>
                        <asp:Panel ID="divRequestTab" runat="server" CssClass="cpBody">
                            <center>
                                <asp:UpdatePanel ID="upReq" runat="server">
                                    <ContentTemplate>
                                        <div style="background-color: #226399; height: 30px; color: White; width: 950px;
                                            border-radius: 5px;">
                                            <center>
                                                <asp:Label ID="pHeaderpersonal" runat="server" Text="Leave Request" Height="30px"
                                                    Font-Bold="true" Font-Size="Medium" ForeColor="White" />
                                            </center>
                                        </div>
                                        <table id="rcptsngle" runat="server">
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight2"
                                                        Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexchange">
                                                    </asp:DropDownList>
                                                </td>
                                                <td rowspan="3">
                                                    <asp:Image ID="img_stud" runat="server" Style="height: 200px; width: 160px;" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_Smartno" runat="server" placeholder="Smartcard No" CssClass="textbox  txtheight2"
                                                        Visible="false" OnTextChanged="txt_Smartno_Changed" TextMode="Password" AutoPostBack="true"></asp:TextBox>
                                                    <asp:TextBox ID="txt_rollno" runat="server" placeholder="Roll No" CssClass="textbox  txtheight2"
                                                        OnTextChanged="txt_rollno_Changed" AutoPostBack="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txt_rollno"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:Button ID="btn_roll" runat="server" CssClass="textbox btn1 textbox1" Text="?"
                                                        OnClick="btn_roll_Click" />
                                                    <asp:TextBox ID="txtIntAppNo" runat="server" CssClass="textbox txtheight2" Width="60px"
                                                        Visible="false"></asp:TextBox>
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
                                                                <asp:TextBox ID="txtIntDegCode" runat="server" CssClass="textbox txtheight2" Width="60px"
                                                                    Visible="false"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_Sem" runat="server" placeholder="Semester" CssClass="textbox txtheight2"
                                                                    Width="60px" ReadOnly="true"></asp:TextBox>
                                                                <asp:TextBox ID="txt_Batc" runat="server" placeholder="Batch" CssClass="textbox txtheight2"
                                                                    Width="45px" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnSearch" ImageUrl="~/images/SearchImg.png" runat="server"
                                                                    CssClass="textbox btn1 textbox1" Width="30px" Height="30px" OnClick="btn_search_Click" />
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
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <div runat="server" id="divReq" visible="false">
                                            <table cellpadding="10">
                                                <tr>
                                                    <td>
                                                        <table class="maindivstyle">
                                                            <tr>
                                                                <td>
                                                                    <center>
                                                                        <asp:Label ID="lblHe" runat="server" Font-Bold="True" Font-Size="Medium" Text="Leave Request"
                                                                            ForeColor="Green"></asp:Label>
                                                                    </center>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <%--     <asp:Label ID="lblReNo" runat="server" Text="Requisition No" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:TextBox ID="txt_rqstn_leave" runat="server" CssClass="newtextbox textbox1 txtheight"> </asp:TextBox>
                                                        <asp:Label ID="lbl_rqstn_leave" Text="Req Date" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:TextBox ID="txt_time_rqstn_leave" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                         <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_time_rqstn_leave"
                                                            runat="server" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLvType" runat="server" Text="Leave Type" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium"></asp:Label>
                                                                    <asp:DropDownList ID="ddl_leave_type" AutoPostBack="true" CssClass="textbox textbox1 ddlheight4"
                                                                        Width="270px" runat="server" ToolTip="Select The Leave Type">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLeaveReason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Reason"></asp:Label>
                                                                    <asp:DropDownList ID="ddlLeaveReason" runat="server" CssClass="textbox  ddlheight"
                                                                        Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlLeaveReason_Indexchange">
                                                                        <asp:ListItem Value="-1">Select</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtReasonLeave" runat="server" Placeholder="Reason" CssClass="textbox textbox1"
                                                                        Width="340px" Visible="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLeaveFrom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Leave From"></asp:Label>
                                                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" Width="70px"
                                                                        OnTextChanged="checkDate" AutoPostBack="true"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                    </asp:CalendarExtender>
                                                                    <asp:Label ID="lblLeaveTo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="To"></asp:Label>
                                                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                                                        Width="70px" AutoPostBack="true"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                    </asp:CalendarExtender>
                                                                    <asp:Button ID="btnReqSave" runat="server" Text="Make Request" CssClass="textbox btn"
                                                                        Width="100px" OnClick="btnSaveRequest_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div style="height: 100px; overflow: auto;">
                                                                        <span id="spanHolidays" runat="server"></span>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table class="maindivstyle" style="width: 250px;">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <center>
                                                                        <%-- <asp:Label ID="lblHeadLeave" runat="server" Font-Bold="True" Font-Size="Medium" Text="Leave Details (In Days)"
                                                                            ForeColor="Green"></asp:Label>--%>
                                                                    </center>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-left: 50px;">
                                                                    <asp:Label ID="lblMaxLeave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Maximum Leave :"></asp:Label>
                                                                    <asp:Label ID="lblMaxLeaveAns" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text=""></asp:Label>
                                                                    <br />
                                                                    <asp:Label ID="lblTakenLeave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Leave Consumed :"></asp:Label>
                                                                    <asp:Label ID="lblTakenLeaveAns" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text=""></asp:Label>
                                                                    <br />
                                                                    <asp:Label ID="lblRemLeave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="Leave Remaining :"></asp:Label>
                                                                    <asp:Label ID="lblRemLeaveAns" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <center>
                                                                        <asp:Label ID="lblLeaveTaken" runat="server" Font-Bold="true" Font-Size="Medium"
                                                                            ForeColor="Green" Text="Leave History"></asp:Label>
                                                                        <div style="width: 400px; overflow: auto;">
                                                                            <asp:GridView ID="gridLeaveHistory" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                                                HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="true" Font-Size="Medium"
                                                                                OnRowDataBound="gridLeaveHistory_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="S.No">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                                            <%-- <asp:Label ID="lblMonthVal" Visible="false" runat="server" Text='<%#Eval("MonthVal") %>'></asp:Label>--%>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </center>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <td colspan="2">
                                                        <center>
                                                            <asp:Label ID="lblHeadGrid" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Green"
                                                                Text="Approval Staffs"></asp:Label>
                                                            <div style="width: 960px; height: 500px; overflow: auto;">
                                                                <asp:GridView ID="gridStaffDetails" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="true" Font-Size="Medium">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Panel ID="divReportTab" runat="server" CssClass="cpBody">
                            <br />
                            <center>
                               <%-- <asp:UpdatePanel ID="upRep" runat="server">
                                    <ContentTemplate>--%>
                                        <div style="background-color: #226399; height: 30px; color: White; width: 950px;
                                            border-radius: 5px;">
                                            <center>
                                                <asp:Label ID="Label1" runat="server" Text="Leave Report" Height="30px" Font-Bold="true"
                                                    Font-Size="Medium" ForeColor="White" />
                                            </center>
                                        </div>
                                        <div runat="server" id="divReport">
                                            <table class="maindivstyle">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblClgRep" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlClgRep" runat="server" CssClass="textbox  ddlheight2" Width="300px"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlClgRep_OnSelectedIndexchange">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label3" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_fromdateRep" runat="server" CssClass="textbox  txtheight" Width="70px"
                                                                                AutoPostBack="true" OnTextChanged="checkDateRep"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdateRep" runat="server"
                                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                            </asp:CalendarExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium" Text="To"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_todateRep" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDateRep"
                                                                                Width="70px" AutoPostBack="true"></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_todateRep" runat="server"
                                                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                            </asp:CalendarExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                                Font-Size="Medium" Text="Status"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlRepMode" runat="server" CssClass="textbox ddlheight" Width="120px"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="checkDateRep">
                                                                                <asp:ListItem Selected="True">All</asp:ListItem>
                                                                                <asp:ListItem>Requested</asp:ListItem>
                                                                                <asp:ListItem>Approved</asp:ListItem>
                                                                                <asp:ListItem>Rejected</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="btnGoRep" runat="server" CssClass="textbox btn" OnClick="checkDateRep"
                                                                                ImageUrl="~/images/SearchImg.png" Width="30px" Height="30px" />
                                                                        </td>
                                                                    </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 940px; overflow: auto;">
                                                            <center>
                                                                <asp:GridView ID="gridLeaveReport" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="false" Font-Size="Medium"
                                                                    OnRowDataBound="gridLeaveReport_RowDataBound" OnRowCommand="gridLeaveReport_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                                <asp:Label ID="lblReqPk" Visible="false" runat="server" Text='<%#Eval("LeaveRequestPk") %>'></asp:Label>
                                                                                <asp:Label ID="lblResonCode" Visible="false" runat="server" Text='<%#Eval("LeaveReason") %>'></asp:Label>
                                                                                <asp:Label ID="lblLeaveCode" Visible="false" runat="server" Text='<%#Eval("LeaveType") %>'></asp:Label>
                                                                                <asp:Label ID="lblLeaveReqStat" Visible="false" runat="server" Text='<%#Eval("RequestStatus") %>'></asp:Label>
                                                                                <asp:Label ID="lblAppNo" Visible="false" runat="server" Text='<%#Eval("AppNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <HeaderTemplate>
                                                                                <asp:CheckBox ID="cbSelHead" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelHead_CheckedChange" />
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="cbSel" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <%--<asp:TemplateField HeaderText="View">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="btnViewRep" runat="server" CssClass="textbox  btn"
                                                                                    Width="40px" OnClick="btnViewRep_OpenPopUp" ImageUrl="~/images/viewDet.png" Height="30px" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>--%>
                                                                        <asp:TemplateField HeaderText="View">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="btnViewRep" runat="server" CssClass="textbox  btn" BackColor="#7FBA00"
                                                                                    Text="View" Width="40px" CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Student Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_StudName" runat="server" Text='<%#Eval("StudName") %>' Width="150px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Branch">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_Branch" runat="server" Text='<%#Eval("Branch") %>' Width="150px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="AdmissionNo">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_AdmNo" runat="server" Text='<%#Eval("AdmNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="RegisterNo">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_RegNo" runat="server" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="RollNo">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_RollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="From Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_FromDate" runat="server" Text='<%#Eval("FromDate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_ToDate" runat="server" Text='<%#Eval("ToDate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Total Days">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_TotDays" runat="server" Text='<%#Eval("TotalLeave") %>' Width="80px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Half Day">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_isHalf" runat="server" Text='<%#Eval("IsHalfDay") %>' Width="80px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Half Session">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_HalfTime" runat="server" Text='<%#Eval("HalfTime") %>' Width="100px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_HalfDate" runat="server" Text='<%#Eval("HalfDayDate") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Leave Type">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_lType" runat="server" Text='<%#Eval("LeaveDisp") %>' Width="100px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reason">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_lReason" runat="server" Text='<%#Eval("Reason") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reject Reason">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRejreason" runat="server" Text='<%#Eval("rejectReson") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Status">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_lStatus" runat="server" Text='<%#Eval("RequestStatusName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </center>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btnDeleteRequest" runat="server" Text="Delete" OnClick="btnDeleteRequest_Click"
                                                                Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </center>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Panel ID="divApproveTab" runat="server" CssClass="cpBody">
                            <br />
                           <%-- <asp:UpdatePanel ID="upLeaveApp" runat="server">
                                <ContentTemplate>--%>
                                    <div style="background-color: #226399; height: 30px; color: White; width: 950px;
                                        border-radius: 5px;">
                                        <center>
                                            <asp:Label ID="Label2" runat="server" Text="Leave Approve" Height="30px" Font-Bold="true"
                                                Font-Size="Medium" ForeColor="White" />
                                        </center>
                                    </div>
                                    <div runat="server" id="divApprove">
                                        <table class="maindivstyle">
                                            <tr>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblClgApp" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlClgApp" runat="server" CssClass="textbox  ddlheight2" Width="300px"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlClgApp_OnSelectedIndexchange">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label7" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_fromdateApp" runat="server" CssClass="textbox  txtheight" Width="70px"
                                                                            AutoPostBack="true" OnTextChanged="checkDateApp"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdateApp" runat="server"
                                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="To"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_todateApp" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDateApp"
                                                                            Width="70px" AutoPostBack="true"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todateApp" runat="server"
                                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                        </asp:CalendarExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlStudStaff" runat="server" CssClass="textbox ddlheight" Width="120px"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="checkDateApp">
                                                                            <asp:ListItem Selected="True">Standard</asp:ListItem>
                                                                            <asp:ListItem>Staff</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="Stage"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlReqStage" runat="server" CssClass="textbox ddlheight" Width="120px"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="checkDateApp">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnGoApp" runat="server" CssClass="textbox btn" OnClick="checkDateApp"
                                                                            ImageUrl="~/images/SearchImg.png" Width="30px" Height="30px" />
                                                                    </td>
                                                                </tr>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 940px; overflow: auto;">
                                                        <center>
                                                            <asp:GridView ID="gridLeaveApprove" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                                HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="false" Font-Size="Medium"
                                                                OnRowDataBound="gridLeaveApprove_RowDataBound" OnRowCommand="gridLeaveApprove_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                            <asp:Label ID="lblReqPk" Visible="false" runat="server" Text='<%#Eval("LeaveRequestPk") %>'></asp:Label>
                                                                            <asp:Label ID="lblResonCode" Visible="false" runat="server" Text='<%#Eval("LeaveReason") %>'></asp:Label>
                                                                            <asp:Label ID="lblLeaveCode" Visible="false" runat="server" Text='<%#Eval("LeaveType") %>'></asp:Label>
                                                                            <asp:Label ID="lblLeaveReqStat" Visible="false" runat="server" Text='<%#Eval("RequestStatus") %>'></asp:Label>
                                                                            <asp:Label ID="lblAppNo" Visible="false" runat="server" Text='<%#Eval("AppNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="cbSelHead" runat="server" AutoPostBack="true" OnCheckedChanged="cbSelHeadAPp_CheckedChange" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="cbSel" runat="server" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="View">
                                                                        <ItemTemplate>
                                                                            <asp:Button ID="btnViewApp" runat="server" CssClass="textbox  btn" BackColor="#7FBA00"
                                                                                Text="View" Width="40px" CommandName="View" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Student Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_StudName" runat="server" Text='<%#Eval("StudName") %>' Width="150px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Branch">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Branch" runat="server" Text='<%#Eval("Branch") %>' Width="150px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AdmissionNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_AdmNo" runat="server" Text='<%#Eval("AdmNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="RegisterNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_RegNo" runat="server" Text='<%#Eval("RegNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="RollNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_RollNo" runat="server" Text='<%#Eval("RollNo") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="From Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_FromDate" runat="server" Text='<%#Eval("FromDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="To Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_ToDate" runat="server" Text='<%#Eval("ToDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Days">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_TotDays" runat="server" Text='<%#Eval("TotalLeave") %>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Half Day">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_isHalf" runat="server" Text='<%#Eval("IsHalfDay") %>' Width="80px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Half Session">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_HalfTime" runat="server" Text='<%#Eval("HalfTime") %>' Width="100px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_HalfDate" runat="server" Text='<%#Eval("HalfDayDate") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Leave Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_lType" runat="server" Text='<%#Eval("LeaveDisp") %>' Width="100px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Reason">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_lReason" runat="server" Text='<%#Eval("Reason") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_lStatus" runat="server" Text='<%#Eval("RequestStatusName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </center>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnApproveReq" runat="server" Text="Approve" OnClick="btnApproveReq_Click"
                                                            Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                        <asp:Button ID="btnRejectReq" runat="server" Text="Reject" OnClick="btnRejectReq_Click"
                                                            Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                               <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </asp:Panel>
                    </div>
                </div>
            </center>
            <br />
            <asp:Label ID="lblDegree" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblBranch" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblSemester" runat="server" Visible="false"></asp:Label>
        </div>
    </center>
    <%--  ******popup window******--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
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
                                    <asp:Label ID="lbl_sec2" runat="server" Text="Section"></asp:Label>
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
                            <asp:Label ID="lbl_errormsg" Visible="false" runat="server" Text="No Records Found"
                                ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ShowHeaderSelection="false"
                                BorderWidth="0px" Width="650px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                OnUpdateCommand="Fpspread1_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--  ******popup View window******--%>
    <center>
       <%-- <asp:UpdatePanel ID="upUpView" runat="server">
            <ContentTemplate>--%>
                <div id="divViewPopUp" runat="server" visible="false" class="popupstyle popupheight1 "
                    style="height: 150em;">
                    <br />
                    <br />
                    <asp:ImageButton ID="imgViewClose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-left: 460px;" OnClick="imgViewClose_Click" />
                    <div style="background-color: White; height: 640px; width: 950px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <span id="spanPopUpHeader" runat="server" class="fontstyleheader" style="color: Green;">
                                Leave Request Report</span>
                        </center>
                        <table class=" maindivstyle">
                            <tr>
                                <td style="width: 500px; padding-left: 20px;">
                                    <table>
                                        <tr>
                                            <td colspan="2" style="border: 1px solid green; border-radius: 5px;">
                                                <center>
                                                    <span id="span1" class="fontstyleheader" style="color: Green; font-size: medium;">Student
                                                        Details</span>
                                                </center>
                                                <span id="spanPopViewStud" runat="server" style="color: Black; font-size: medium;
                                                    font-weight: bold;"></span>
                                                <br />
                                            </td>
                                              <td style="width: 400px; overflow: auto;">
                                    <center>
                                        <span id="spanLeaveHistHeader" class="fontstyleheader" style="color: Green; font-size: medium;">
                                            Leave History</span>
                                        <div>
                                            <asp:GridView ID="gridPopLeavehistory" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="true" Font-Size="Medium"
                                                OnRowDataBound="gridLeaveHistory_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </center>
                                </td>

                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="float: left; border: 1px solid green; border-radius: 5px; padding: 3px;">
                                                    <center>
                                                        <span id="span3" class="fontstyleheader" style="color: Green; font-size: medium;">Request
                                                            Details</span>
                                                    </center>
                                                    <span id="spanPopViewReqDet" runat="server" style="color: Black; font-size: medium;
                                                        font-weight: bold;"></span>
                                                    <br />
                                                    <asp:TextBox ID="txtPopAppNo" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopReqPk" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopTotDays" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopReqStatus" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopFromDate" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopToDate" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopRollNo" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopLeaveCode" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopIsHalf" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopHalfDate" runat="server" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtPopHalfSession" runat="server" Visible="false"></asp:TextBox>
                                                    <center>
                                                        <asp:Button ID="btnDeleteReqPop" runat="server" Text="Delete" OnClick="btnDeleteReqPop_Click"
                                                            Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                        <asp:Button ID="btnApproveReqPop" runat="server" Text="Approve" OnClick="btnApproveReqPop_Click"
                                                            Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                        <asp:Button ID="btnRejectReqPop" runat="server" Text="Reject" OnClick="btnRejectReqPop_Click"
                                                            Visible="false" Width="80" Height="30px" CssClass=" textbox btn" />
                                                    </center>
                                                    <br />
                                                </div>
                                            </td>
                                            <td>
                                                <div style="float: left; margin-left: 15px; border: 1px solid green; border-radius: 5px;">
                                                    <center>
                                                        <span id="span2" class="fontstyleheader" style="color: Green; font-size: medium;">Leave
                                                            Details (In Days)</span>
                                                        <div>
                                                            <asp:GridView ID="GridView1" runat="server" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                AutoGenerateColumns="false" Font-Size="Medium" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Leave Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblODtype" Visible="true" runat="server" Text='<%#Eval("DispText") %>'></asp:Label>
                                                                            <asp:Label ID="lblEntCode" Visible="false" runat="server" Text='<%#Eval("EntryCode") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Max Value">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMaxval" Visible="true" runat="server" Text='<%#Eval("Maxval") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Approved">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblApp" Visible="true" runat="server" Text='<%#Eval("Approved") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remaining">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBal" Visible="true" runat="server" Text='<%#Eval("bal") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </center>
                                                    <%--<span id="spanPopViewLeave" runat="server" style="color: Black; font-size: medium;
                                                        font-weight: bold;"></span>--%>
                                                </div>
                                            </td>
                                            <td>
                                    <div id="div_GV1" runat="server" style="width: 290px; height: 140px; overflow: auto;">
                                        <center>
                                            <span id="span6" class="fontstyleheader" style="color: Green; font-size: medium;">Leave
                                                Details</span>
                                        </center>
                                        <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_gv1" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                            CssClass="textbox txtheight"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Morning" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_mrng" runat="server" Checked='<%# Eval("ischecked").ToString().Equals("1") %>'/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Evening" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_evng" runat="server" Checked='<%# Eval("ischecked1").ToString().Equals("1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                        </tr>
                                        <tr>
                                        <td>
                                                        <asp:LinkButton ID="linkdownload" Text="DownloadAttachment" Font-Name="Book Antiqua" Font-Size="11pt"
                                                            OnClick="lnkdownlaodattachement_Click" runat="server" Width="22px" /></td>
                                                  
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                              
                                 
                               
                            </tr>
                        </table>
                    </div>
                </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" visible="false" class="popupstyle popupheight1 " style="height: 100em;">
                    <div style="background-color: White; height: 300px; width: 950px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-left: 460px;" OnClick="ImageButton1_Click" />
                        <center>
                            <span id="span4" runat="server" class="fontstyleheader" style="color: Green;">Reason</span>
                        </center>
                        <table class=" maindivstyle">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtnote" runat="server" Width="514px" Height="160px" Style="margin-left: 0px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="Button1" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                        OnClick="Button1_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div id="div2" runat="server" visible="false" class="popupstyle popupheight1 " style="height: 100em;">
                    <div style="background-color: White; height: 300px; width: 950px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-left: 460px;" OnClick="ImageButton2_Click" />
                        <center>
                            <span id="span5" runat="server" class="fontstyleheader" style="color: Green;">Reason</span>
                        </center>
                        <table class=" maindivstyle">
                            <tr>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="514px" Height="160px" Style="margin-left: 0px"
                                        TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="Button2" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                                        OnClick="Button2_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>
