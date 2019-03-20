<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StaffVendorOtherFeeEdit.aspx.cs" Inherits="FinanceMod_StaffVendorOtherFeeEdit" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">Staff/Vendor/Others Fee Edit</span>
        </div>
    </center>
    <center>
        <div class="maindivstyle" style="width: 497px; overflow: auto;">
            <center>
                <div style="padding-left: 10px; padding-top: 5px; clear: both;">
                    <div>
                        <table class="maintablestyle" style="float: left; height: 25px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true">
                                        <%--OnSelectedIndexChanged="ddl_college_OnSelectedIndexchange"--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_rollnoNew" runat="server" Height="10px" Width="300px"
                                        RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbl_rollnoNew_OnSelectedIndexChanged">
                                        <%----%>
                                        <asp:ListItem Selected="True">Staff</asp:ListItem>
                                        <asp:ListItem>Vendor</asp:ListItem>
                                        <asp:ListItem>Others</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </div>
        <br />
        <center>
            <div class="maindivstyle" style="width: 700px;">
                <table class="maintablestyle" id="rcptSngleStaff" runat="server" style="width: 497px;
                    overflow: auto;">
                    <tr id="staff" runat="server">
                        <td>
                            <asp:TextBox ID="txtroll_staff" runat="server" placeholder="Staff Id" CssClass="textbox  txtheight2"
                                AutoPostBack="true" OnTextChanged="txtroll_staff_Changed"></asp:TextBox><%----%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtroll_staff"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_staff"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_staffLook" runat="server" CssClass="textbox btn1 textbox1" Text="?" OnClick="btn_staffLook_Click" /><%----%>
                            <asp:Button ID="Button4" runat="server" Text="Clear" Style="color: Red; font-weight: bold;"
                                OnClick="btnClear_Click" /><%----%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_staff" runat="server" placeholder="Name" CssClass="textbox txtheight2"
                                Width="162px" AutoPostBack="true"></asp:TextBox><%--OnTextChanged="txtname_staff_Changed"--%>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_staff"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDept_staff" runat="server" placeholder="Department" CssClass="textbox txtheight2"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="editstaff" Text="Edit" runat="server" OnClick="btnedit_Click" CssClass="textbox btn1 textbox1" /><%--OnClick="btnGO_staff_Click"--%>
                        </td>
                    </tr>
                    <tr id="vendor" runat="server" visible="false">
                        <td>
                            <asp:TextBox ID="txtroll_vendor" runat="server" placeholder="Vendor" CssClass="textbox  txtheight2"
                                AutoPostBack="true" OnTextChanged="txtroll_vendor_Changed"></asp:TextBox>
                            <%----%>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_vendor"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_vendor" runat="server" placeholder="Contact Person" CssClass="textbox txtheight2"
                                AutoPostBack="true" OnTextChanged="txtname_vendor_Changed"></asp:TextBox><%----%>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtname_vendor"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDept_vendor" runat="server" placeholder="Designation" CssClass="textbox txtheight2"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_vendorLook" runat="server" CssClass="textbox btn1 textbox1" Text="?"  OnClick="btn_vendorLook_Click"/>
                            <%----%>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="Clear" Style="color: Red; font-weight: bold;"
                                OnClick="btnClear_Click" /><%----%>
                        </td>
                        <td>
                            <asp:Button ID="Edit" Text="Edit" runat="server" OnClick="btnedit_Click" CssClass="textbox btn1 textbox1" /><%--OnClick="btnGO_staff_Click"--%>
                        </td>
                    </tr>
                    <tr id="other" runat="server" visible="false">
                        <td>
                            <asp:TextBox ID="txtroll_other" runat="server" placeholder="Name" CssClass="textbox  txtheight2"
                                OnTextChanged="txtroll_other_Changed" AutoPostBack="true"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetOthername" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtroll_other"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtname_other" runat="server" placeholder="Company Name" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_otherMobile" runat="server" placeholder="Mobile" CssClass="textbox txtheight2"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="Clear" Style="color: Red; font-weight: bold;"
                                OnClick="btnClear_Click" /><%----%>
                        </td>
                        <td>
                            <asp:Button ID="editother" Text="Edit" runat="server" OnClick="btnedit_Click" CssClass="textbox btn1 textbox1" /><%--OnClick="btnGO_staff_Click"--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txt_HeaderPop" runat="server" ReadOnly="true" Height="20px" class="textbox  txtheight2">Header</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_HeaderPop" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_HeaderPop_ChekedChange" /><%----%>
                                <asp:CheckBoxList ID="cbl_HeaderPop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_HeaderPop_SelectedIndexChanged">
                                    <%--"--%>
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_HeaderPop"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Ledgerpop" runat="server" ReadOnly="true" Height="20px" class="textbox  txtheight2">Ledger</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_ledgerpop" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_ledgerpop_ChekedChange" /><%----%>
                                <asp:CheckBoxList ID="cbl_ledgerpop" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_ledgerpop_SelectedIndexChanged">
                                </asp:CheckBoxList>
                                <%----%>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Ledgerpop"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <span class="challanLabel">
                                            <p>
                                                Search By</p>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_ledgeSearch" runat="server" AutoPostBack="true" Width="101px"
                                            OnSelectedIndexChanged="ddl_ledgeSearch_Change">
                                            <%----%>
                                            <asp:ListItem>Header</asp:ListItem>
                                            <asp:ListItem>Ledger</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="tdsemadd" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpdatePanels6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsemadd" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        placeholder="Semester/Year" onfocus="myFunction(this)" Width="75px"></asp:TextBox>
                                    <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel" Height="150px" Width="150px">
                                        <asp:CheckBox ID="cbsemadd" runat="server" Width="100px" Text="Select All" AutoPostBack="True" />
                                        <%--OnCheckedChanged="cbsemadd_CheckedChanged"--%>
                                        <%--AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged"--%>
                                        <asp:CheckBoxList ID="cblsemadd" runat="server" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                        <%--OnSelectedIndexChanged="cblsemadd_SelectedIndexChanged"--%>
                                        <%--AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged"--%>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtsemadd"
                                        PopupControlID="panel8" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="ledgeUp" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_ledgeSearch" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                        Height="20px" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_ledgeSearch"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" &%-+/().,*@">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetLegerName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_ledgeSearch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_ledgesearch" Text="Go" runat="server" OnClick="btn_ledgesearch_Click"
                                CssClass="textbox btn1 textbox1" /><%--OnClick="btnGO_staff_Click"--%>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <br />
            <div id="div2" runat="server" style="width: 670px; height: 270px; overflow: auto;">
                <asp:GridView ID="grid_HeaderLedger" runat="server" AutoGenerateColumns="false" GridLines="Both"
                    Width="650px" OnDataBound="grid_HeaderLedger_OnDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lbl_serial1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Label ID="lbl_headeridpop" runat="server" Visible="false" Text='<%#Eval("HeaderPK") %>'></asp:Label>
                                <asp:Label ID="lbl_headpop" runat="server" Text='<%#Eval("HeaderName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ledgeridpop" runat="server" Visible="false" Text='<%#Eval("LedgerPK") %>'></asp:Label>
                                <asp:Label ID="lbl_legerpop" runat="server" Text='<%#Eval("LedgerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <center>
                                    <asp:TextBox ID="txt_NewLedger" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Total") %>'
                                        Style="text-align: right;"></asp:TextBox></center>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="padding-top: 5px; text-align: center;">
                    <asp:Button ID="btn_ledgersave" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Save" CssClass="textbox btn2 textbox1" OnClick="btnpopLedgersave_Click"
                        Visible="false" /><%--OnClick="btnpopLedgersave_Click"--%>
                </div>
        </center>
        </div>
    </center>
    </center>
    <%-- Pop Alert--%>
    <center>
        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 100000;
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
                                        <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%--Staff Lookup --%>
    <center>
        <div id="div_staffLook" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                OnClick="btn_exitstaff_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 400px; width: 650px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Select The Staff</span></div>
                </center>
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <span class="challanLabel">
                                <p>
                                    Search By</p>
                            </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsearch1" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlsearch1_OnSelectedIndexChanged">
                                <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearch1" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txtsearch1c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetStaffno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1c"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_go2Staff" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                OnClick="btn_go2Staff_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsgstaff" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" ShowHeaderSelection="false"
                    BorderWidth="0px" Style="width: 620px; height: 230px; auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    OnUpdateCommand="Fpspread2staff_Command">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Blue" SelectionPolicy="Single">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btn_staffOK" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                            OnClick="btn_staffOK_Click" />
                        <asp:Button ID="btn_exitstaff" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                            OnClick="btn_exitstaff_Click" />
                    </div>
                </center>
            </div>
        </div>
    </center>
    <%--Vendor Lookup --%>
    <center>
        <div id="div_vendorLook" runat="server" visible="false" class="popupstyle popupheight1 ">
            <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 310px;"
                OnClick="btn_exitvendor_Click" />
            <br />
            <br />
            <div style="background-color: White; height: 400px; width: 650px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <br />
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Select The Vendor</span></div>
                </center>
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Search By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsearch2" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlsearch2_OnSelectedIndexChanged">
                                <asp:ListItem Text="Search By Name" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Search By Code" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span class="challanLabel">
                                <p>
                                </p>
                            </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearch2" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorName1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                            <asp:TextBox ID="txtsearch2c" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="GetVendorno1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2c"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_goVendor" runat="server" CssClass="textbox btn1 textbox1" Text="Go"
                                OnClick="btn_goVendor_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <div>
                    <asp:Label ID="lbl_errormsgvendor" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
                <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" ShowHeaderSelection="false"
                    BorderWidth="0px" Style="width: 620px; height: 230px; auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    OnUpdateCommand="Fpspread3vendor_Command">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <div>
                        <asp:Button ID="btnvendor_ok" runat="server" CssClass="textbox btn2 textbox1" Text="Ok"
                            OnClick="btnvendor_ok_Click" />
                        <asp:Button ID="btnExit_vendor" runat="server" CssClass="textbox btn2 textbox1" Text="Exit"
                            OnClick="btn_exitvendor_Click" />
                    </div>
                </center>
            </div>
        </div>
    </center>
</asp:Content>
