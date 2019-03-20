<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Result_Analysis_Report.aspx.cs" Inherits="Result_Analysis_Report" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <style type="text/css">
        .style1
        {
            width: 763px;
        }
        .style2
        {
            width: 154px;
        }
        .style3
        {
            width: 116px;
        }
        .style4
        {
            width: 328px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <html>
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
        <center>
            <asp:Label ID="lblhead" runat="server" Text="Degree wise Result Analysis" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
       <br />
       <center>
        <table style="width:700px; height:50px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Exam Month" Width="95px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True"
                        Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Exam Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True"
                        Width="61px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                        Width="61px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="90px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td class="style4">
                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="200px"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" Style="font-weight: 700" OnClick="btnGo_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" Width="36px" />
                </td>
            </tr>
        </table>
</center>
    </html>
   
    <br />
    <asp:Label ID="errorlabl" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <br />
   <center>
    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="900">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    </center>
</asp:Content>

