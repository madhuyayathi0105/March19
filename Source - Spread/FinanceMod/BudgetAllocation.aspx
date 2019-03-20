<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="BudgetAllocation.aspx.cs" Inherits="BudgetAllocation" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Budget Allocation</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 900px;">
                <%--maincontent--%>
                <center>
                    <div>
                        <table width="950px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_dept" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        Text="Department" OnCheckedChanged="rb_dept_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                </td>
                                <td colspan="2">
                                    <fieldset style="border-radius: 15px;">
                                        <asp:CheckBox ID="cb_dept" runat="server" AutoPostBack="true" Text="Department" Checked="true"
                                            OnCheckedChanged="cb_dept_CheckedChanged" />
                                        <asp:CheckBox ID="cb_depitem" runat="server" AutoPostBack="true" Text="Department With Item"
                                            OnCheckedChanged="cb_depitem_CheckedChanged" /></fieldset>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb_header" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                        Text="Header With Ledger" OnCheckedChanged="rb_header_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_frmdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_frmdate" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_date_TextChanged"
                                                Width="70px" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_frmdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_todate" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_todate_TextChanged"
                                                Width="70px" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <%-- **********--%>
                        <div id="div_dept" runat="server">
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcol" runat="server" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcolload" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlcolload_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_deprt" runat="server" Style="top: 10px; left: 6px;" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UPdp_deprt" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_deprt" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_deprt" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_deprt" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_deprt_Change"
                                                        AutoPostBack="true" />
                                                    <asp:CheckBoxList ID="cbl_deprt" runat="server" OnSelectedIndexChanged="cbl_deprt_OnSelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popdesgtype" runat="server" TargetControlID="txt_deprt"
                                                    PopupControlID="panel_deprt" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Style="top: 10px; left: 6px;" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="multxtpanel1">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemhdr" runat="server" Style="top: 10px; left: 6px;" Text="Item Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_itemhdr" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_itenheader1" runat="server" Width="100px" Text=" Select All"
                                                        AutoPostBack="true" OnCheckedChanged="cb_itenheader_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_itemnheader1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemnheader1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemhdr"
                                                    PopupControlID="panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_item" runat="server" Style="top: 10px; left: 6px;" Text="Item"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_item" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel2" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_item1" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_item1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_itemname1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_item1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_item"
                                                    PopupControlID="panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itmsearch" runat="server" Style="top: 10px; left: 6px;" Text="Search By"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_itmsearch" runat="server" CssClass="textbox  ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_itmsearch_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Item Header</asp:ListItem>
                                            <asp:ListItem Value="1">Item Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txt_search" runat="server" Visible="false" Width="230px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="multxtpanel1">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchhd1" runat="server" Visible="false" Width="230px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchhd1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="multxtpanel1">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                        <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                            OnClick="btn_addnew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_header" runat="server" visible="false">
                            <table class="maintablestyle" width="950px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcolhead" runat="server" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlheadcol" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlheadcol_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_header" runat="server" Style="top: 10px; left: 6px;" Text="Header">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_header" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_header" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_header" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chk_header1" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="chk_header_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_header"
                                                    PopupControlID="panel_header" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_ledger" runat="server" Style="top: 10px; left: 6px;" Text="Ledger"> 
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_ledger" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_ledger" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_ledger" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_ledgr" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_ledgr_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_ledger1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_ledger_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_ledger"
                                                    PopupControlID="panel_ledger" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_search" runat="server" Style="top: 10px; left: 6px;" Text="Search By"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddl_hdr_ldg" runat="server" CssClass="textbox  ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_hdr_ldg_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Header</asp:ListItem>
                                            <asp:ListItem Value="1">Ledger</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_search1" runat="server" Visible="false" Width="190px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getheader" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="multxtpanel1">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_ldg_search" runat="server" Visible="false" Width="190px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getledger" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ldg_search"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="multxtpanel1">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btn_go1" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go1_Click" />
                                        <asp:Button ID="btn_addnew1" runat="server" CssClass="textbox btn2" Text="Add New"
                                            OnClick="btn_addnew1_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
                <%--    column order--%>
                <asp:Label ID="lblerrmsg" runat="server" ForeColor="Red"></asp:Label>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="850px" Style="margin-top: -0.1%;">
                            <asp:Label ID="lbl_par" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
                <center>
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
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
                                        <asp:ListItem Selected="True" Value="Header_Id">Header Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Fee_Code">Ledger Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BudgetQty">Budget Quantity </asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BudgetAmt">Budget Amount</asp:ListItem>
                                        <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                                        <asp:ListItem Value="Allow_Aditional">Allow Additional</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                    CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                    TextLabelID="lbl_par" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--   ******************** column order 2**********************--%>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="850px" Style="margin-top: -0.1%;">
                            <asp:Label ID="lbl_pa" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
                <center>
                    <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column1_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove2_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborder1" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder1_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="Dept_Code">Department</asp:ListItem>
                                        <%-- <asp:ListItem Selected="True" Value="Fee_Code">Ledger Name</asp:ListItem>--%>
                                        <asp:ListItem Selected="True" Value="BudgetQty">Budget Quantity </asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BudgetAmt">Budget Amount</asp:ListItem>
                                        <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                                        <asp:ListItem Value="Allow_Aditional">Allow Additional</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                    CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                    TextLabelID="lbl_pa" CollapsedSize="0" ImageControlID="Image3" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--  ************************COLUMN ORDER 3*********************--%>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter2" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="850px" Style="margin-top: -0.1%;">
                            <asp:Label ID="lbl_p" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
                <center>
                    <asp:Panel ID="pcolumnorder2" runat="server" CssClass="maintablestyle" Width="850px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_column2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column2_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremove3_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborder2" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder2_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="Dept_Code">Department</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Item_Code">Item Code</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Item_Code">Item Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BudgetQty">Budget Quantity </asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BudgetAmt">Budget Amount</asp:ListItem>
                                        <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                                        <asp:ListItem Value="Allow_Aditional">Allow Additional</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pcolumnorder2"
                    CollapseControlID="pheaderfilter2" ExpandControlID="pheaderfilter2" Collapsed="true"
                    TextLabelID="lbl_p" CollapsedSize="0" ImageControlID="Image4" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <%--
   ************************** spread ***********************--%>
                <br />
                <br />
                <div id="div1" runat="server" visible="false" style="width: 650px; height: 350px;
                    overflow: auto; border: 1px solid Gray; background-color: White;">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="1px"
                        ActiveSheetViewIndex="0" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <div id="div2" runat="server" visible="false" style="width: 650px; height: 350px;
                    overflow: auto; border: 1px solid Gray; background-color: White;">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="1px"
                        ActiveSheetViewIndex="0" OnCellClick="FpSpread2_CellClick" OnPreRender="FpSpread2_SelectedIndexChanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <%-- *************--%>
                <center>
                    <div id="popup_addnew" runat="server" visible="false" style="height: 70em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: -55px;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 58px; margin-left: 434px;"
                            OnClick="imagebtnpopclose3_Click" />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px; height: 783px; width: 900px;">
                            <br />
                            <center>
                                <span style="color: Green; font-size: large;">Budget Allocation</span>
                            </center>
                            <br />
                            <table style="margin-right: 45px;" class="table">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpopcol" runat="server" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpopcol" runat="server" CssClass="textbox textbox1 ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlpopcol_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_popfdate" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_popfdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_popfdate_TextChanged"
                                                    Width="70px" AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_popfdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_poptdate" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_poptdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_poptdate_TextChanged"
                                                    Width="70px" AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_poptdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div style="width: 700px;">
                                <fieldset style="border-radius: 15px;">
                                    <asp:RadioButton ID="rb_dep" runat="server" RepeatDirection="Horizontal" GroupName="same1"
                                        Text="Department" Checked="true" OnCheckedChanged="rb_dep_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:RadioButton ID="rb_depitem" runat="server" RepeatDirection="Horizontal" GroupName="same1"
                                        Text="Department With Item" OnCheckedChanged="rb_depitem_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:RadioButton ID="rb_hdr" runat="server" RepeatDirection="Horizontal" GroupName="same1"
                                        Text="Header With Ledger" OnCheckedChanged="rb_hdr_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                    &nbsp; &nbsp; &nbsp; &nbsp;
                                    <asp:Button ID="btn_pop1go" Text="Go" runat="server" CssClass="textbox1 textbox btn1"
                                        OnClick="btn_pop1go_Click" />
                                </fieldset>
                            </div>
                            <br />
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <div id="div_depwithitem" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <div id="div_rdo1" runat="server" visible="true">
                                                            <asp:Label ID="Label7" runat="server" Text="Department"></asp:Label>
                                                            <fieldset>
                                                                <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Style="height: 109px; width: 300px;">
                                                                    <asp:CheckBoxList ID="cbldepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbldepartment_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                            </fieldset>
                                                            <br />
                                                            <asp:CheckBox ID="cbdepartemt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                OnCheckedChanged="cbdepartment_Change" Font-Size="Medium" Text="Select All" AutoPostBack="true" /></div>
                                                    </td>
                                                    <td colspan="2">
                                                        <div id="div_rdo2" runat="server" visible="false">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label3" runat="server" Text="Header"></asp:Label>
                                                                        <span style="color: Red;">*</span>
                                                                        <fieldset>
                                                                            <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Style="height: 107px; width: 173px;">
                                                                                <asp:CheckBoxList ID="cblheader" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblheader_Change">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                        </fieldset>
                                                                        <br />
                                                                        <asp:CheckBox ID="cbhdr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbhdr_CheckedChanged" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label6" runat="server" Text="Item"></asp:Label>
                                                                        <span style="color: Red;">*</span>
                                                                        <fieldset>
                                                                            <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 107px; width: 250px;">
                                                                                <asp:CheckBoxList ID="cblitem" runat="server">
                                                                                </asp:CheckBoxList>
                                                                            </asp:Panel>
                                                                        </fieldset>
                                                                        <br />
                                                                        <asp:CheckBox ID="chckconitm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            OnCheckedChanged="chckconitm_change" Font-Size="Medium" Text="Select All" AutoPostBack="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_hdrwithledgr" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="Financial Header"></asp:Label>
                                                        <fieldset>
                                                            <asp:Panel ID="Panel6" runat="server" ScrollBars="Auto" Style="height: 109px; width: 300px;">
                                                                <asp:CheckBoxList ID="cbl_pop1hrdr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop1hrdr_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </fieldset>
                                                        <br />
                                                        <asp:CheckBox ID="cb_pop1header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_pop1header_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                                        <span style="color: Red;">*</span>
                                                        <fieldset>
                                                            <asp:Panel ID="Panel7" runat="server" ScrollBars="Auto" Style="height: 107px; width: 300px;">
                                                                <asp:CheckBoxList ID="cbl_ldgr" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_ldgr_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                        </fieldset>
                                                        <br />
                                                        <asp:CheckBox ID="cb_ldgr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_ldgr_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="lbl_purpose" runat="server" Text="Purpose"></asp:Label>
                                        <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Height="33px"
                                            Width="35px" />
                                        <asp:DropDownList ID="ddl_purpose" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label>
                            <br />
                            <div style="width: 800px; height: 250px; overflow: auto;">
                                <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                    OnRowDataBound="uggirdrowcommand2">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_dept" runat="server" Text='<%#Eval("department") %>' Width="450px"
                                                    Height="20px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_amt" Width="80px" Text='<%#Eval("BudAmt") %>' runat="server"
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filterext1" runat="server" TargetControlID="txt_amt"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Qty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_bqty" Width="80px" Text='<%#Eval("BudQty") %>' runat="server"
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filterext2" runat="server" TargetControlID="txt_bqty"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Purpose" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:DropDownList ID="ddl_pur2" runat="server" CssClass="ddlheight5 textbox">
                                                    </asp:DropDownList>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allow Additional" HeaderStyle-BackColor="#0CA6CA"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="cb_allo" runat="server" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                    OnRowDataBound="uggirdrowcommand1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="300px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_dept" runat="server" Text='<%#Eval("department") %>' Width="250px"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_hrcode" Text='<%#Eval("Header") %>' Visible="false" Width="100px"
                                                    runat="server" Height="17px">
                                                </asp:Label>
                                                <asp:Label ID="txt_header" Text='<%#Eval("Header") %>' Width="100px" runat="server"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_icode" Text='<%#Eval("ItemCode") %>' Width="100px" runat="server"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_iname" Text='<%#Eval("Itemname") %>' Width="330px" runat="server"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_qty" Width="60px" runat="server" CssClass="  textbox " Height="17px">
                                                    </asp:TextBox></center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Measurable" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_measure" Width="70px" runat="server" CssClass="  textbox " Height="17px">
                                                    </asp:TextBox></center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_amt" Width="80px" runat="server" Text='<%#Eval("BudAmt") %>'
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filterext3" runat="server" TargetControlID="txt_amt"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Qty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_bqty" Width="80px" Text='<%#Eval("BudQty") %>' runat="server"
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filterext4" runat="server" TargetControlID="txt_bqty"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Purpose" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:DropDownList ID="ddl_pur1" runat="server" CssClass="ddlheight5 textbox">
                                                    </asp:DropDownList>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allow Additional" HeaderStyle-BackColor="#0CA6CA"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="cb_allo" runat="server" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                    OnRowDataBound="uggirdrowcommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_hdr_code" Text='<%#Eval("Header_Code") %>' Visible="false" Width="200px"
                                                    runat="server" Height="17px">
                                                </asp:Label>
                                                <asp:Label ID="txt_header" Text='<%#Eval("Header") %>' Width="200px" runat="server"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="txt_ldgcode" Text='<%#Eval("Ledger_code") %>' Visible="false" Width="200px"
                                                    runat="server" Height="17px">
                                                </asp:Label>
                                                <asp:Label ID="txt_ledger" Text='<%#Eval("Ledger") %>' Width="200px" runat="server"
                                                    Height="17px">
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_amt" Width="80px" runat="server" Text='<%#Eval("BudAmt") %>'
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox><asp:FilteredTextBoxExtender ID="filterext5" runat="server" TargetControlID="txt_amt"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Budget Qty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:TextBox ID="txt_bqty" Width="80px" runat="server" Text='<%#Eval("BudQty") %>'
                                                        CssClass="  textbox " Height="17px">
                                                    </asp:TextBox><asp:FilteredTextBoxExtender ID="filterext6" runat="server" TargetControlID="txt_bqty"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Purpose" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:DropDownList ID="ddl_pur" runat="server" CssClass="ddlheight5 textbox">
                                                    </asp:DropDownList>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Allow Additional" HeaderStyle-BackColor="#0CA6CA"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:CheckBox ID="cb_allo" runat="server" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <center>
                                <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2" />
                                <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                                <div>
                                    <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click"
                                        CssClass="textbox btn2" />
                                    <asp:Button ID="btn_del" runat="server" Text="Delete" CssClass="textbox btn2" OnClick="btn_del_Click" />
                                    <asp:Button ID="btn_exit1" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit1_Click" />
                                </div>
                            </center>
                        </div>
                    </div>
                </center>
                <%-- *********end of popup**********--%>
                <center>
                    <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                        <center>
                            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table style="line-height: 30px">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_description11" runat="server" Text="Description" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_description11" runat="server" Width="200px" CssClass="textbox textbox1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="line-height: 35px">
                                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btndescpopadd_Click" />
                                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btndescpopexit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </center>
                <%--************--%>
                <center>
                    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_erroralert" runat="server" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <%-- ************--%>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
