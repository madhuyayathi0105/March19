<%@ Page Title="" Language="C#" MasterPageFile="~/ChartMOD/ChartSubSiteMaster.master" AutoEventWireup="true" CodeFile="deg_dept_chart.aspx.cs" Inherits="deg_dept_chart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  <br /><center>
        <asp:Label ID="lblhead" runat="server" Text="Degree And Department Wise CAM And University Chart"
            CssClass="fontstyleheader" ForeColor="Green"></asp:Label></center>
  <br />
  <center>
     <table style="width:900px; height:70px; " class="maintablestyle ">
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="249px" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="labbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Height="22px" Width="78px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
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
                    <asp:Label ID="lbldept" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                <div style="position:relative">
                    <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_Dept" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                            <asp:Panel ID="panel_Department" runat="server" Height="100px" width="180px" CssClass="MultipleSelectionDDL"    BackColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Auto">
                                <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdept_checkedchanged" />
                                <asp:CheckBoxList ID="ddldept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldept_selectedchanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Dept"
                                PopupControlID="panel_Department" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pndes1" runat="server" BorderColor="Black" BorderWidth="1px">
                        <asp:RadioButtonList ID="radiobutton1" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="radiobutton1_selectedindexchanged"
                            AutoPostBack="true">
                            <asp:ListItem Selected Value="1">CAM Wise</asp:ListItem>
                           <%-- <asp:ListItem Value="2">University Wise</asp:ListItem>--%>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
       </table></center>
   <br />
    <asp:Panel ID="panelerror" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblerror" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panelchart" runat="server">
    </asp:Panel>
</asp:Content>

