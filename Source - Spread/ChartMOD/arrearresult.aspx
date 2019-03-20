<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="arrearresult.aspx.cs" Inherits="arrearresult" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 37px;
        }
    </style>
    <p style="font-family: Book Antiqua; font-size: medium;">
    </p>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Arrear Chart Report</span>
    </center>
    <br />
    <center>
        <asp:Panel ID="panel1" runat="server" Width="907px" Style="border-style: solid; border-width: thin;
            border-color: Black; background: #0CA6CA;" Height="84px" CssClass="maintablestyle ">
            <table >
                <tr style="border-bottom-style: solid; border-top-style: solid; background-color: #0CA6CA;
                    border-left-style: solid; border-width: 1px;">
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="249px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Height="22px" Width="90px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldept" runat="server" Height="22px" Width="136px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px">
                            <asp:RadioButtonList ID="radiobutton1" runat="server" RepeatDirection="Horizontal"
                                Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="radiobutton1_selectedindexchanged"
                                AutoPostBack="true">
                                <asp:ListItem Selected>CAM Wise</asp:ListItem>
                                <asp:ListItem>University Wise</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlsem" runat="server" Height="22px" Width="71px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server" Height="22px" Width="71px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbltest" runat="server" Text="Test" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList2" runat="server" Height="22px" Width="136px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="btngo_Click" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"
            Font-Size="Medium" Font-Names="Book Antiqua" Visible="false"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Chart ID="Chart1" runat="server" Width="900px" OnClick="Chart1_Click">
                        <Titles>
                            <asp:Title ShadowOffset="3" Name="Time chart" />
                        </Titles>
                        <Series>
                            <asp:Series Name="Series1" PostBackValue="#VALX" YValuesPerPoint="2">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                        </Legends>
                    </asp:Chart>
                </td>
            </tr>
        </table>
        <br />
        <table align="center">
            <tr>
                <td>
                    <center>
                        <FarPoint:FpSpread ID="fpstudentdetails" runat="server" ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonType="PushButton"
                                ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
