<%@ Page Title="" Language="C#" MasterPageFile="~/RequestMOD/RequestSubSiteMaster.master" AutoEventWireup="true" CodeFile="EventReport.aspx.cs" Inherits="EventReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 1100px;
            width: 1000px;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .fontstyleheaderrr
        {
            font-family: Book Antiqua;
            font-size: larger;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: #008000;">Event Report</span>
                    </div>
                </center>
            </div>
            <div class="maindivstyle maindivstylesize">
                <table class="maindivstyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_branch" Text="Department" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
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
                            <asp:Label ID="lb_eventtype" runat="server" Text="Event Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_evetype" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_evetype" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_evetype_checkedchange" />
                                        <asp:CheckBoxList ID="cb1_evetype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cb1_evetype_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_evetype"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_eventname" runat="server" Text="Event Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_eventname" runat="server" CssClass="textbox textbox1 txtheight3"
                                AutoPostBack="true" OnTextChanged="txt_eventname_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetEvent" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_eventname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_staffname" runat="server" CssClass="textbox txtheight3 textbox1"
                                AutoPostBack="true" OnTextChanged="txt_staffname_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_studname" runat="server" Text="Student Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_studname" runat="server" CssClass="textbox txtheight3 textbox1"
                                AutoPostBack="true" OnTextChanged="txt_studname_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getstudname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:CheckBox ID="cb_date" runat="server" AutoPostBack="true" OnCheckedChanged="cb_date_CheckedChanged" />
                            <asp:Label ID="lbl_fromdate" Text="From Date" runat="server"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_fromdate" Enabled="false" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                            <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server"
                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                            <asp:TextBox ID="txt_todate" Enabled="false" runat="server" CssClass="textbox txtheight"></asp:TextBox>
                            <asp:CalendarExtender ID="Cal_date1" TargetControlID="txt_todate" runat="server"
                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                OnClick="btn_go_OnClick" />
                        </td>
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
                                    <asp:ListItem Selected="True" Value="RequestDate">Request Date</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="ReqEventName">Event Name</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="eventdate">Event Date</asp:ListItem>
                                    <asp:ListItem Value="StartTime">Start Time</asp:ListItem>
                                    <asp:ListItem Value="EndTime">End Time</asp:ListItem>
                                    <asp:ListItem Value="StartPeriod">Start Period</asp:ListItem>
                                    <asp:ListItem Value="EndPeriod">End Period</asp:ListItem>
                                    <asp:ListItem Value="NoOfAction">No Of Action</asp:ListItem>
                                    <asp:ListItem Value="LocationType">Location Type</asp:ListItem>
                                    <asp:ListItem Value="OutdoorLoc">Location</asp:ListItem>
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
                <%--  //2nd--%>
                <div>
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="850px" Style="margin-top: -0.1%;">
                            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
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
                                <asp:TextBox ID="tborder" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                    AutoPostBack="true" runat="server" Enabled="false">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                    Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                    RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="RequestDate">Request Date</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="ReqEventName">Event Name</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="eventdate">Event Date</asp:ListItem>
                                    <asp:ListItem Value="StartTime">Start Time</asp:ListItem>
                                    <asp:ListItem Value="EndTime">End Time</asp:ListItem>
                                    <asp:ListItem Value="StartPeriod">Start Period</asp:ListItem>
                                    <asp:ListItem Value="EndPeriod">End Period</asp:ListItem>
                                  <%--  <asp:ListItem Value="NoOfAction">No Of Action</asp:ListItem>--%>
                                    <asp:ListItem Value="LocationType">Location Type</asp:ListItem>
                                    <asp:ListItem Value="ActionName">Action Name</asp:ListItem>
                                    <asp:ListItem Value="ACtionDesc">Description</asp:ListItem>
                                    <asp:ListItem Value="MemType">Mem Type</asp:ListItem>
                                    <asp:ListItem Value="ActionType">Action Type</asp:ListItem>
                                    <asp:ListItem Value="memberaction">Member Action</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                    CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                    TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <br />
                <asp:Label ID="lbl_err_item" runat="server" ForeColor="Red"></asp:Label>
                <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnCellClick="FpSpread1_CellClick"
                    OnPreRender="FpSpread1_SelectedIndexChanged">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="popview" runat="server" class="popupstyle popupheight1" visible="false">
                    <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 471px;"
                        OnClick="btn_popclose_Click" />
                    <br />
                    <div style="background-color: White; height: 654px; width: 960px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <span class="fontstyleheader" style="color: #008000;">Event Report Details</span>
                        <br />
                        <br />
                        <div style="width: 900px; height: 300px; overflow: auto;">
                            <asp:GridView ID="gridadd" runat="server" Visible="true" AutoGenerateColumns="false"
                                GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="txteventdate" ReadOnly="true" Width="100px" runat="server" Text='<%#Eval("Dummy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="txtactname" Width="150px" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_descri" Width="150px" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Time" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_start" ReadOnly="true" Width="100px" runat="server" Text='<%#Eval("Dummy3") %>'
                                                placeholder="Ex: 12:00:AM"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Time" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_end" ReadOnly="true" Width="100px" runat="server" Text='<%#Eval("Dummy4") %>'
                                                placeholder="Ex: 12:00:PM"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_loc" Width="200px" ReadOnly="true" Text='<%#Eval("Dummay5") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mem Type" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_memtype" Width="100px" ReadOnly="true" Text='<%#Eval("Dummay6") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action Type" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_actiontype" Width="150px" ReadOnly="true" Text='<%#Eval("Dummay7") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Members Name" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_memname" Width="200px" ReadOnly="true" Text='<%#Eval("Dummay8") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Members Action" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txtmemact" Width="150px" ReadOnly="true" Text='<%#Eval("Dummay9") %>'
                                                runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div style="width: 900px; height: 200px; overflow: auto;">
                            <asp:GridView ID="GridView1" runat="server" Visible="true" AutoGenerateColumns="false"
                                GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mem Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="txteventdate" ReadOnly="true" Width="150px" runat="server" Text='<%#Eval("Dummy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Organizer Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="txtactname" Width="350px" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="txt_descri" Width="350px" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <br />
                <asp:Label ID="lbl_norec" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                            AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
        </center>
    </div>
</asp:Content>

