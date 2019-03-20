<%@ Page Title="Re-Valuation Request" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Revaluation_Request.aspx.cs" Inherits="Revaluation_Request" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/Commonfilter.ascx" TagName="Search" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Re-Valuation Request</span>
        </center>
        <br />
        <center>
            <fieldset id="maindiv" runat="server" style="width: 960px; margin-left: 0px; height: 1300px;
                border-color: silver; border-radius: 10px;">
                <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                    box-shadow: 0 0 8px #999999; height: 35px; margin-left: 0px; margin-top: 8px;
                    padding: 1em; margin-left: 0px; width: 930px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_mm" runat="server" Text="Month" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_mm" runat="server" Font-Bold="true" CssClass="textbox txtheight5"
                                            Style="height: 30px;" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_yy" runat="server" Text="Year" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_yy" runat="server" Font-Bold="true" CssClass="textbox txtheight5"
                                            Style="height: 30px;" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <fieldset style="width: 190px; height: 20px; background-color: #ffccff; margin-left: 0px;
                                    border-radius: 10px; border-color: #6699ee;">
                                    <asp:RadioButton ID="rdb_tot" runat="server" Text="Re-Val" AutoPostBack="true" OnCheckedChanged="rdb_tot_CheckedChanged"
                                        GroupName="a" Checked="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:RadioButton ID="rdb_take" runat="server" Text="Re-Take" AutoPostBack="true"
                                        GroupName="a" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="rdb_take_CheckedChanged" />
                                </fieldset>
                            </td>
                            <td>
                                <asp:Label ID="lbl_searchby" runat="server" Text="Search By" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_searchby" runat="server" Font-Bold="true" OnSelectedIndexChanged="ddl_searchby_OnSelectedIndexChanged"
                                            CssClass="textbox txtheight5" Style="height: 30px;" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="125px" AutoPostBack="True">
                                            <asp:ListItem Selected="True" Value="1">Reg No</asp:ListItem>
                                            <asp:ListItem Value="2">Roll No</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="ddl_searchby" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchbyreg" runat="server" Visible="false" CssClass="textbox txtheight2"
                                    Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium;
                                    margin-left: 0px;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetRegNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbyreg"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_searchbyroll" runat="server" Visible="false" CssClass="textbox txtheight2"
                                    Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium;
                                    margin-left: 0px;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetRollNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbyroll"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" runat="server" OnClick="btn_go_OnClick" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium; background-color: #6699ee; border-radius: 6px;" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <asp:Label ID="lblErr" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                <br />
                <center>
                    <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" OnButtonCommand="Fpspread3_Command"
                        overflow="true" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="750px"
                        Height="300px" class="spreadborder" ShowHeaderSelection="false" Style="border-radius: 10px;
                        margin-left: 1px;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <center>
                    <asp:Button ID="btn_save" Text="Save" OnClick="btn_save_OnClick" runat="server" Visible="false"
                        Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                        border-radius: 6px;" />
                </center>
            </fieldset>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                    <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                            OnClick="btn_errorclose_Click" Text="Ok" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </body>
    </html>
</asp:Content>