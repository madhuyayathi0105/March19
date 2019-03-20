<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Equalpaperselection.aspx.cs" Inherits="Equalpaperselection" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <center>
        <div class="maindivstyle">
            <span style="color: Green; font-size: large;" class=" fontstyleheader">Equal Paper Selection</span>
            <table class="maintablestyle">
            <tr> <td>
                    <asp:CheckBox id="cbGeneral" runat="server" Text="General" OnCheckedChanged="cbGeneral_OnCheckedChanged" AutoPostBack="true" />
                    </td></tr>
                <tr>
                    <td >
                        <asp:Label ID="lbltype" runat="server" Font-Bold="true" Text="Mode" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                        <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddldegree" runat="server" CssClass="textbox ddlheight" Width="90px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="textbox ddlheight" Width="90px"
                            OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Month"></asp:Label>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="textbox ddlheight" Width="90px"
                            OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                   
                
                </tr>
            </table>
            <br />
            <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red"></asp:Label>
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="fpshowsubject" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="460" Width="450" HorizontalScrollBarPolicy="AsNeeded"
                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" VerticalScrollBarPolicy="Never"
                            ShowHeaderSelection="false" OnButtonCommand="fpshowsubject_OnButtonCommand">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                    <td>
                        <asp:Button ID="btnpassadd" runat="server" Text="Add" OnClick="btnpassadd_Click"
                            CssClass="textbox btn" Width="60px" />
                    </td>
                    <td>
                        <FarPoint:FpSpread ID="fpselected" runat="server" BorderColor="Black" BorderStyle="Solid"
                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" BorderWidth="1px"
                            Height="460" Width="450" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="Never"
                            ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" Visible="false">
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
                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="textbox btn"
                            Width="60px" />
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="Fpsaved" runat="server" BorderColor="Black" BorderStyle="Solid"
                            Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" BorderWidth="1px"
                            Height="460" Width="700" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="Never"
                            ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnremove" runat="server" Text="Remove" OnClick="btnremove_Click"
                            CssClass="textbox btn" Width="60px" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
</asp:Content>
