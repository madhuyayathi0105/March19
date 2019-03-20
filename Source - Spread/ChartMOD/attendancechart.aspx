<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master" AutoEventWireup="true" CodeFile="attendancechart.aspx.cs" Inherits="attendancechart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .limitheight
        {
            height: 75px;
        }
        .style4
        {
            width: 1026px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager><br />
  <center>
        <asp:Label ID="lbl_Header" runat="server" CssClass="fontstyleheader" ForeColor="Green" Text="Attendance Chart"></asp:Label>
  </center><br />
  <center>
        <table style="width:900px; height:40px; " class="maintablestyle ">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Height="25px" Width="70px" Font-Names="Book Antiqua"
                        Font-Bold="True" Font-Size="Medium" CssClass="limitheight" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="labedu" runat="server" Text="Edu-level" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddledu" runat="server" Height="25px" Width="70px" Font-Names="Book Antiqua"
                        Font-Bold="True" Font-Size="Medium" CssClass="limitheight">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldate" runat="server" Text="From Date" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdate" runat="server" CssClass="font" Width="80px" Height="20px"
                        AutoPostBack="true" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                        OnTextChanged="txtdate_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txttxtdate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                        ValidChars="/" runat="server" TargetControlID="txtdate">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdate" Format="dd/MM/yyyy"
                        runat="server" Enabled="True">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="font" Width="80px" Height="20px"
                        AutoPostBack="true" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                        OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                        ValidChars="/" runat="server" TargetControlID="txtdate">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="TextBox1" Format="dd/MM/yyyy"
                        runat="server" Enabled="True">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:RadioButtonList ID="radiolist1" runat="server" RepeatDirection="Horizontal"
                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Width="200px">
                        <asp:ListItem Value="1" Selected="True">Day Wise</asp:ListItem>
                        <asp:ListItem Value="2">Hour Wise</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
  </center>
    <table>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
            </td>
        </tr>
        <tr>
            <td class="style4">
                <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                    Font-Size="Medium" Font-Names="Book Antiqua" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4">
                <asp:Label ID="lblerrmsg1" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                    Font-Size="Medium" Font-Names="Book Antiqua" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" class="style4">
                <asp:Chart ID="Chart1" runat="server" Width="964px">
                    <Titles>
                        <asp:Title ShadowOffset="3" Name="Time chart" />
                    </Titles>
                    <Series>
                        <asp:Series Name="Series1" ChartType="Radar">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                            <AxisY>
                                <MajorGrid LineColor="#6699FF" />
                            </AxisY>
                            <AxisX>
                                <MajorGrid LineColor="#6699FF" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend BorderColor="Violet">
                        </asp:Legend>
                    </Legends>
                </asp:Chart>
            </td>
        </tr>
    </table>
    <asp:Panel ID="panelerrormesg" runat="server">
    </asp:Panel>
    <asp:Panel ID="panelchart" runat="server">
    </asp:Panel>  
</asp:Content>

