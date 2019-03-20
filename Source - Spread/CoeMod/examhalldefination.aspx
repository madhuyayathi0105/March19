<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="examhalldefination.aspx.cs" Inherits="examhalldefination"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label7" CssClass="fontstyleheader" runat="server" Text="Exam Hall Defination"
            Font-Bold="True" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px; color: Green;"></asp:Label>
        <table style="width: 800px; height: 70px; margin: 0px; margin-top: 15px; margin-bottom: 15px;
            background-color: #0CA6CA; border-radius: 5px; padding: 8px;">
            <tr>
                <td>
                    <asp:Label ID="lbltype" runat="server" Font-Bold="true" Text="Mode" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbuilding" runat="server" Text="Building Name" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbuilding" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlbuilding_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblfloor" runat="server" Text="Floor Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlflooring" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlflooring_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblroomname" runat="server" Text="Hall No" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlroom" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlroom_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblcol" runat="server" Text="Columns" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtroomcolumn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="84px" MaxLength="2"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtroomcolumn"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="lblroomrow" runat="server" Text="Rows" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtroomrow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="84px" MaxLength="2"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtroomrow"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="lblHallType" runat="server" Text="Hall Definition" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlHallType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlHallType_SelectedIndexChanged">
                        <asp:ListItem Text="General Definition" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Alternate Definition" Value="1" Selected="False"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnroomgo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnroomgo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"
            Font-Names="Book Antiqua" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;"></asp:Label>
        <asp:Label ID="lblroomerror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" ForeColor="Red" Style="margin: 0px; margin-top: 15px;
            margin-bottom: 15px;"></asp:Label>
        <div id="columnsetting" runat="server" style="margin: 0px; margin-top: 15px; margin-bottom: 15px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblsel_col" runat="server" Text="Select Column :" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtselcol" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="84px" MaxLength="2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtselcol"
                            FilterType="Numbers" />
                    </td>
                    <td>
                        <asp:Label ID="lblvalue" runat="server" Text="Enter Value" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtvalue" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="84px" MaxLength="2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtvalue"
                            FilterType="Numbers" />
                    </td>
                    <td>
                        <asp:Button ID="btnset" runat="server" Text="Set" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnset_Click" />
                    </td>
                    <td>
                        <asp:Label ID="lblcolerror" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <table style="margin: 0px; margin-top: 15px; margin-bottom: 15px;">
            <tr>
                <td>
                    <center>
                        <asp:Label ID="lbldefault" runat="server" Text="Default View" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Blue"></asp:Label></center>
                </td>
                <td>
                </td>
                <td>
                    <center>
                        <asp:Label ID="lblarrange" runat="server" Text="Arrange View" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Blue"></asp:Label></center>
                </td>
            </tr>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="fproom" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="260" Width="350" HorizontalScrollBarPolicy="AsNeeded"
                        VerticalScrollBarPolicy="Never">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
                <td>
                    <asp:Button ID="btnpassval" runat="server" Text=">>" OnClick="btnpassval_Click" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 26px" />
                </td>
                <td>
                    <FarPoint:FpSpread ID="fproomarra" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="260" Width="440" HorizontalScrollBarPolicy="AsNeeded"
                        VerticalScrollBarPolicy="Never">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td align="right">
                    <asp:Label ID="lbltotaldef" runat="server" Text="Actual Seats :" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Blue"></asp:Label>
                    <asp:Label ID="lblvaldef" runat="server" Text="0" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Blue"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbltotalarge" runat="server" Text="Alloted Seats :" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Blue"></asp:Label>
                    <asp:Label ID="lblvalallot" runat="server" Text="0" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Blue"></asp:Label>
                    <asp:Button ID="btnsaveseats" runat="server" Text="Save" OnClick="btnsaveseats_Click"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 26px" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <div style="margin: 0px; margin-top: 15px; margin-bottom: 15px;">
            <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="350" Width="940" HorizontalScrollBarPolicy="Never"
                Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" VerticalScrollBarPolicy="Never">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" Visible="false">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView GridLineColor="Black" SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
    </center>
        <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
