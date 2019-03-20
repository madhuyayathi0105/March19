<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="BookStatistic.aspx.cs" Inherits="LibraryMod_BookStatistic" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .commonHeaderFont
        {
            margin-left: 0px;
        }
    </style>
    <style type="text/css">
        .div
        {
            left: 0%;
            top: 0%;
        }
        .watermark
        {
            color: #999999;
        }
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .popsty3
        {
            height: 600px;
            width: 700px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Department Book Statistics</span></div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <table class="maintablestyle" style="height: auto; font-family:Book Antiqua; font-weight:bold; margin-left: 0px; margin-top: 10px;
                    margin-bottom: 10px; padding: 6px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="188px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_department" Text="Department" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_department" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_department" runat="server" Style="height: 20px; width: 120px;"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_department" runat="server" CssClass="multxtpanel" Style="width: 170px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_department" runat="server" Width="200px" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_department_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_department" runat="server" TargetControlID="txt_department"
                                        PopupControlID="panel_department" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblbooks" runat="server" Text="Book Type" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbooks" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlbooks_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbltype" runat="server" Text="Type" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddltype" runat="server" Enabled="false" CssClass="textbox ddlstyle ddlheight3"
                                        Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblcategory" runat="server" Text="Category" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlcategory" runat="server" Enabled="false" CssClass="textbox ddlstyle ddlheight3"
                                        Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblwise" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rblwise_Selected">
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Label ID="lblsearch1" runat="server" Text="Search"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearch1" runat="server" CssClass="textbox txtheight2" Width="179px"
                                AutoPostBack="True" OnTextChanged="txtsearch1_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td colspan="2">
                            <fieldset style="height: 21px; width: 280px;">
                                <asp:CheckBox ID="cbRange" runat="server" Text="Range" />
                                <span>From</span>
                                <asp:TextBox ID="txtFromRange" runat="server" Enabled="true" Style="height: 20px;
                                    width: 60px;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtFromRange"
                                    FilterType="Numbers" ValidChars="." InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <span>To</span>
                                <asp:TextBox ID="txtToRange" runat="server" Enabled="true" Style="height: 20px; width: 60px;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtToRange"
                                    FilterType="Numbers" ValidChars="." InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td>
                                        InvoiceDate
                                    </td>
                                    <td>
                                        <fieldset style="width: 280px; height: 20px;">
                                            <asp:CheckBox ID="cbdate" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate_OnCheckedChanged" />
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From: "></asp:Label>
                                            <asp:TextBox ID="txt_fromdate" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </fieldset>
                                    </td>
                                    <td colspan="1">
                                        <fieldset style="width: 355px; height: 20px;">
                                            <asp:CheckBox ID="cbdate1" runat="server" AutoPostBack="true" OnCheckedChanged="cbdate1_OnCheckedChanged" />
                                            <asp:Label ID="lbl_access" runat="server" Text="AccessNo" CssClass="commonHeaderFont">
                                            </asp:Label>
                                            <asp:Label ID="lbl_fromdate1" runat="server" Text="From:"></asp:Label>
                                            <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:Label ID="lbl_todate1" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td>
                                        AccessionDate
                                    </td>
                                    <td>
                                        <fieldset style="width: 280px; height: 20px;">
                                            <asp:CheckBox ID="cbdate2" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate2_OnCheckedChanged" />
                                            <asp:Label ID="lbl_fromdate2" runat="server" Text="From:" CssClass="commonHeaderFont">
                                            </asp:Label>
                                            <asp:TextBox ID="txt_fromdate2" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate2" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                            <asp:Label ID="lbl_todate2" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                            <asp:TextBox ID="txt_todate2" runat="server" Enabled="false" Style="height: 20px;
                                                width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_todate2" runat="server"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                            OnSelectedIndexChanged="rblType_Selected">
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="1">
                                        <asp:Label ID="lblsearch2" runat="server" Text="Search"></asp:Label>
                                        <asp:TextBox ID="txtsearch2" runat="server" CssClass="textbox txtheight2" Width="185px"
                                            AutoPostBack="True" OnTextChanged="txtsearch2_TextChanged"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getrno1" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch2"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td colspan="5">
                                        <asp:ImageButton ID="btn_go" ImageUrl="~/LibImages/Go.jpg" runat="server" OnClick="btn_go_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <br />
     
        <br />
        <center>
            <div id="showreport2" runat="server" visible="false">
                <table>
                    <br />
                    <tr>
                      
                        <asp:GridView ID="GridView1" runat="server" ShowFooter="false" ShowHeader="true" AutoGenerateColumns="true"
                            Font-Names="Book Antiqua" toGenerateColumns="true" AllowPaging="true" PageSize="500"
                            OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnPageIndexChanging="GridView1_OnPageIndexChanged"
                            Width="980px">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </tr>
                    <tr>
                        <center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                               <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                    Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                  <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
