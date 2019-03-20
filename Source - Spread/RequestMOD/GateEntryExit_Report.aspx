<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="GateEntryExit_Report.aspx.cs" Inherits="GateEntryExit_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="initial-scale=1.0;width=device-width" />
    <%--  <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000">Gate Entry/Exit Report</span></div>
                    <br />
                </center>
            </div>
            <div class="maindivstyle">
                <br />
                <div>
                    <center>
                        <table class="maintablestyle">
                            <tr style="padding-left: 20px;">
                                <td>
                                    College
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_col" runat="server" Height="15px" CssClass="textbox  txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_col" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_col_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_col" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_col_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_col"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Hostel
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlhostel" runat="server" CssClass="textbox1 ddlheight4" Height="25px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                                    <asp:CheckBox ID="chk_batch" runat="server" AutoPostBack="true" OnCheckedChanged="chk_batch_OnCheckedChanged" />
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" Enabled="false" Height="15px" CssClass="textbox  txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <span style="float: left;">Degree</span>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" Style="float: right;" Enabled="false"
                                                CssClass="textbox  txtheight1" Height="15px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_degree_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Branch
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" Enabled="false" Height="15px" CssClass="textbox  txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_branch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Status
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_status" runat="server" Height="15px" CssClass="textbox txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_status" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_status_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_status" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_status_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_status"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Student Type
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_studtype" runat="server" Height="15px" CssClass="textbox txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_studtype" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_studtype_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_studtype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_studtype_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_studtype"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <span style="float: left;">Status</span>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_appstatus" runat="server" Style="float: right;" Height="15px"
                                                CssClass="textbox txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_appstatus" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_appstatus_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_appstatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_appstatus_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_appstatus"
                                                PopupControlID="Panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Entered By
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_enter" runat="server" Height="15px" CssClass="textbox txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="height: 100px;">
                                                <asp:CheckBox ID="cb_enter" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_enter_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_enter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_enter_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_enter"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <fieldset id="chk_field" runat="server" style="border-color: Black; height: 10px;">
                                        <div id="flddiv" runat="server" style="margin-left: 30px;">
                                            <asp:CheckBox ID="chk_entry" runat="server" Text="Entry" AutoPostBack="true" OnCheckedChanged="chk_entry_OnCheckedChanged" />
                                            <asp:CheckBox ID="chk_exit" runat="server" Text="Exit" AutoPostBack="true" OnCheckedChanged="chk_exit_OnCheckedChanged" />
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkdtfrm" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkdtfrm_OnCheckedChanged" />
                                    From
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfrmdt" runat="server" CssClass="textbox textbox1 txtheight1"
                                        Height="20px" Enabled="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="Cal_date" TargetControlID="txtfrmdt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="2">
                                    To
                                    <asp:TextBox ID="txttodt" runat="server" CssClass="textbox textbox1 txtheight1" Height="20px"
                                        Enabled="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodt" CssClass="cal_Theme1 ajax__calendar_active"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbtimefrm" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="cbtimefrm_OnCheckedChanged" />
                                    From
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlhourreq" Width="50px" runat="server" CssClass="ddlheight textbox1"
                                        Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlminreq" Width="50px" runat="server" CssClass="ddlheight
        textbox1" Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlsessionreq" runat="server" Width="50px" CssClass="ddlheight textbox1"
                                        Enabled="false">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    To
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlendhourreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                        Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlendminreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                        Enabled="false">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlenssessionreq" runat="server" Width="50px" CssClass="ddlheight2 textbox1"
                                        Enabled="false">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Search By
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_searchby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_searchby_OnSelectedIndexChanged"
                                        CssClass="textbox1 ddlheight3">
                                        <asp:ListItem Selected="True" Text="Roll No" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Reg No" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox textbox1" Height="20px"
                                        AutoPostBack="true" Visible="false" Placeholder="Roll No"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_roll" runat="server" DelimiterCharacters="" Enabled="True"
                                        ServiceMethod="GetRoll" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txt_reg" runat="server" CssClass="textbox textbox1" Height="20px"
                                        AutoPostBack="true" Visible="false" Placeholder="Reg No"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_reno" runat="server" DelimiterCharacters="" Enabled="true"
                                        ServiceMethod="GetReg" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_reg" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                   <asp:CheckBox ID="Chktutor" runat="server" AutoPostBack="true" OnCheckedChanged="Chktutor_OnCheckedChanged" />
                                    Tutor
                                </td>
                                <td>
                                 <asp:TextBox ID="txttutor" runat="server" CssClass="textbox textbox1" Height="20px"
                                        AutoPostBack="true" enabled="false" Placeholder="Tutor"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters="" Enabled="true"
                                        ServiceMethod="Gettutor" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txttutor" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" CssClass="textbox1 textbox btn1" Text="Go"
                                        OnClick="btngo_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                        </div>
                        <br />
                        <center>
                            <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                                Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                                <asp:ListItem Enabled="false" Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                                 <asp:ListItem Enabled="false" Selected="True" Value="Id">Student Id</asp:ListItem>
                                                <asp:ListItem Enabled="false" Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                <%-- <asp:ListItem Enabled="false" Selected="True" Value="staff_name">Approved Staff</asp:ListItem>--%>
                                                <asp:ListItem Enabled="false" Selected="True" Value="Purpose">Purpose</asp:ListItem>
                                                <asp:ListItem Value="GatePassDate">Approved ExitDate</asp:ListItem>
                                                <asp:ListItem Value="GateReqExitTime">Approved ExitTime</asp:ListItem>
                                                <asp:ListItem Value="ExpectedDate">Approved EntryDate</asp:ListItem>
                                                <asp:ListItem Value="ExpectedTime">Approved EntryTime</asp:ListItem>
                                                <asp:ListItem Value="GatepassExitdate">Exit Date</asp:ListItem>
                                                <asp:ListItem Value="GatepassExittime">Exit Time</asp:ListItem>
                                                <asp:ListItem Value="GatepassEntrydate">Entry Date</asp:ListItem>
                                                <asp:ListItem Value="GatepassEntrytime">Entry Time</asp:ListItem>
                                                <asp:ListItem Value="islate">Entered Time</asp:ListItem>
                                                <asp:ListItem Value="gatetype">Status</asp:ListItem>
                                                <asp:ListItem Value="ReqAppStatus">Approve Status</asp:ListItem>
                                                 <asp:ListItem Value="stu_Relationship">Guardian </asp:ListItem>


                                              <%--  <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                            ExpandedImage="~/images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <center>
                            <asp:Label Style="color: Red;" ID="lblerr" Visible="false" Text="Record Not Found"
                                runat="server"></asp:Label>
                        </center>
                    </center>
                </div>
                <br />
                <div id="div1" runat="server" visible="false" style="margin-left: 0px">
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" CssClass="spreadborder"
                        BorderStyle="Solid" BorderWidth="1px" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" CssClass="textbox textbox1" onkeypress="display()"
                        Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Height="30px" Font-Size="Medium" Text="Export To Excel"
                        CssClass="textbox textbox1" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="30px" Font-Bold="true" CssClass="textbox textbox1" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
        </center>
    </div>
</asp:Content>
