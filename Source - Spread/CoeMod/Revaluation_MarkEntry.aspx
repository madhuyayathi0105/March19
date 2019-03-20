<%@ Page Title="Re-Valuation Mark Entry" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Revaluation_MarkEntry.aspx.cs" Inherits="Revaluation_MarkEntry"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/Commonfilter.ascx" TagName="Search" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 15px;
            margin-top: 15px;">Re-Valuation Mark Entry</span>
    </center>
    <center>
        <fieldset id="maindiv" runat="server" style="width: 960px; margin-left: 0px; height: 1300px;
            border-color: silver; border-radius: 10px; margin: 0px; margin-bottom: 15px;
            margin-top: 15px;">
            <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                box-shadow: 0 0 8px #999999; height: 200px; margin-left: 0px; margin-top: 8px;
                padding: 1em; margin-left: 0px; width: 930px;">
                <table style="margin-left: -475px;">
                    <tr>
                        <td>
                            <fieldset style="width: 190px; height: 20px; background-color: #ffccff; margin-left: 0px;
                                border-radius: 10px; border-color: #6699ee;">
                                <asp:Label ID="lbl_mm" runat="server" Text="Month" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddl_mm" runat="server" Font-Bold="true" CssClass="textbox txtheight5"
                                    Style="height: 30px;" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px">
                                </asp:DropDownList>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset style="width: 190px; height: 20px; background-color: #ffccff; margin-left: 0px;
                                border-radius: 10px; border-color: #6699ee;">
                                <asp:Label ID="lbl_yy" runat="server" Text="Year" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddl_yy" runat="server" Font-Bold="true" CssClass="textbox txtheight5"
                                    Style="height: 30px;" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px">
                                </asp:DropDownList>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
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
                            <fieldset style="width: 260px; height: 20px; background-color: #ffccff; margin-left: 0px;
                                border-radius: 10px; border-color: #6699ee;">
                                <asp:Label ID="lbl_regno" runat="server" Text="Reg No" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txt_searchbyreg" runat="server" AutoPostBack="true" OnTextChanged="txt_searchbyreg_OnTextChanged"
                                    CssClass="textbox txtheight2" Style="font-weight: bold; width: 150px; font-family: book antiqua;
                                    font-size: medium; margin-left: 0px;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetRegNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbyreg"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset style="width: 190px; height: 20px; background-color: #ffccff; margin-left: 0px;
                                border-radius: 10px; border-color: #6699ee;">
                                <asp:CheckBox ID="cbRegulation" runat="server" Text="2015 - Regulation" Checked="false"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </fieldset>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <fieldset style="width: 190px; height: 50px; background-color: #ffccff; margin-left: 0px;
                                border-radius: 10px; border-color: #6699ee;">
                                <asp:CheckBox ID="chknearestval" runat="server" Text="Get Nearest Value From Revaluation 1,2 and 3" Checked="false"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table id="result" runat="server" visible="false" style="margin-left: 470px; border-radius: 10px;
                    height: 200px; margin-top: -182px; background-color: #ffccff; border-color: #6699ee;
                    width: 460px;">
                    <tr>
                        <td>
                            <asp:Label ID="Name" runat="server" Text="Name :" Visible="false" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_name" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="149px"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Rno" runat="server" Text="Reg No :" Visible="false" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_rno" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Label ID="lbl_reg" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Batch" runat="server" Text="Batch :" Visible="false" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                                <asp:Label ID="lblCollegeC" runat="server" Visible="false" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="degree" runat="server" Text="Degree :" Visible="false" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Dept" runat="server" Text="Department :" Visible="false" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_dept" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Section" runat="server" Visible="false" Text="Section :" Style="color: indigo;"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sec" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                                 <asp:Label ID="lbl_sem" runat="server" Visible="false" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblDegCode" runat="server" Text="" Visible="false" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" Visible="false" OnClick="btn_go_OnClick" runat="server"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                border-radius: 6px;" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <br />
            <FarPoint:FpSpread ID="FpBefore" runat="server" Visible="false" overflow="true" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Width="900px" Height="300px" class="spreadborder"
                ShowHeaderSelection="false" Style="border-radius: 10px; margin-left: 1px; margin: 0px;
                margin-bottom: 20px; margin-top: 25px;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <asp:Button ID="btnSave" Text="Save" Visible="false" OnClick="btn_save_OnClick" runat="server"
                Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                border-radius: 6px; margin: 0px; margin-bottom: 15px; margin-top: 15px;" />
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
</asp:Content>
