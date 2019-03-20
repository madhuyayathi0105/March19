<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffStrenthReport.aspx.cs" Inherits="StaffStrenthReport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 115px;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Staff Gender Wise Report</span>
            </div>
        </center>
        <br />
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Department" Width="100px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdept" runat="server" Height="400px" Width="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" OnCheckedChanged="chkdept_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsdept" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdept_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdept"
                                    PopupControlID="pdept" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lbldegisnation" runat="server" Text="Designation" Width="100px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdesign" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdesign" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkdesign" runat="server" Font-Bold="True" OnCheckedChanged="chkdesign_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsdesign" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdesign_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdesign"
                                    PopupControlID="pdesign" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblcatege" runat="server" Text="Category" Width="100px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtcategory" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pcategory" runat="server" Height="107px" ScrollBars="Vertical" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkcategory" runat="server" Font-Bold="True" OnCheckedChanged="chkcategory_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklscategory" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklscategory_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtcategory"
                                    PopupControlID="pcategory" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="lbltype" runat="server" Text="Staff Type" Width="100px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style1">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txttype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="ptype" runat="server" Height="107px" ScrollBars="Vertical" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chktype" runat="server" Font-Bold="True" OnCheckedChanged="chktype_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstype_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txttype"
                                    PopupControlID="ptype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="true" OnClick="btngo_Click" />
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rdobtn_CategoryWise" runat="server" RepeatDirection="Horizontal"
                            Font-Size="Medium" Font-Names="Book Antiqua" Font-Bold="true" AutoPostBack="true"
                            OnSelectedIndexChanged="rdobtn_CategoryWise_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Gender Wise</asp:ListItem>
                            <asp:ListItem>Category Wise</asp:ListItem>
                            <asp:ListItem>Staff Wise</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <center>
            <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
        </center>
        <br />
        <center>
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Height="250px" ActiveSheetViewIndex="0"
                            currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                            EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" />
                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" />
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                    GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                    SelectionForeColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                            <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False">
                            </TitleInfo>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="true"></asp:Label>
                        <asp:TextBox ID="txtrptname" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="true"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtrptname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Size="Medium" Font-Names="Book Antiqua"
                            Font-Bold="true" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
        </center>
    </body>
</asp:Content>
