<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HrLeaveReport.aspx.cs" Inherits="HrLeaveReport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 150px;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="top: 90px;
            left: -16px; position: absolute; width: 1040px; height: 21px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Staff Leave Report"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="lbldegree" runat="server" Text="Department" Width="100px" Font-Bold="true"
            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute;
            top: 125px; left: 18px;"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtdept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="top: 125px; left: 120px; float: left; position: absolute;
                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
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
        <asp:Label ID="lbldegisnation" runat="server" Style="position: absolute; top: 125px;
            left: 300px" Text="Designation" Width="100px" Font-Bold="true" ForeColor="Black"
            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtdesign" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="left: 403px; top: 125px; position: absolute; font-family: 'Book Antiqua';"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
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
        <asp:Label ID="lblcatege" runat="server" Style="position: absolute; top: 125px; left: 550px"
            Text="Category" Width="100px" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtcategory" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="left: 640px; top: 125px; position: absolute; font-family: 'Book Antiqua';"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
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
        <asp:Label ID="lbltype" runat="server" Style="position: absolute; top: 125px; left: 790px;"
            Text="Staff Type" Width="100px" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txttype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="float: left; left: 874px; top: 125px; position: absolute;
                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
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
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <asp:Label ID="Label1" runat="server" align="left" Style="position: absolute; left: 65px;
                    top: 165px;" Font-Bold="true" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"
                    Text="Type"></asp:Label>
                <asp:RadioButtonList ID="rdbtnlst" runat="server" RepeatDirection="Horizontal" Font-Bold="true"
                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" Style="position: absolute;
                    left: 108px; top: 165px" runat="server" OnSelectedIndexChanged="rdbtnlst_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Selected="True">Leave</asp:ListItem>
                    <asp:ListItem>Absent</asp:ListItem>
                </asp:RadioButtonList>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblleave" runat="server" Style="position: absolute; left: 301px; top: 165px;"
            Text="Leave Type" Width="100px" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtleave" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="float: left; left: 403px; top: 165px; position: absolute;
                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:Panel ID="pleave" runat="server" Height="300px" Width="300px" CssClass="multxtpanel">
                    <asp:CheckBox ID="chkleave" runat="server" Font-Bold="True" OnCheckedChanged="chkleave_ChekedChange"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                    <asp:CheckBoxList ID="chklsleave" runat="server" Font-Size="Medium" AutoPostBack="True"
                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsleave_SelectedIndexChanged">
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtleave"
                    PopupControlID="pleave" Position="Bottom">
                </asp:PopupControlExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblname" Text="Staff Name" Style="position: absolute; top: 165px;
            left: 550px" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                    Width="120px" Style="float: left; top: 165px; left: 640px; position: absolute;
                    font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                <asp:Panel ID="pstaff" runat="server" Height="300px" Width="400px" CssClass="multxtpanel">
                    <asp:CheckBox ID="chkstaff" runat="server" Font-Bold="True" OnCheckedChanged="chkstaff_ChekedChange"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                    <asp:CheckBoxList ID="chklsstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsstaff_SelectedIndexChanged">
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstaff"
                    PopupControlID="pstaff" Position="Bottom">
                </asp:PopupControlExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <asp:CheckBox ID="chkdatewise" runat="server" Text="Date Wise" AutoPostBack="true"
                    OnCheckedChanged="chkdate_CheckedChange" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Style="position: absolute; top: 165px; left: 780px" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblleavevalue" Text="Value" Style="position: absolute; top: 208px;
                    left: 62px;" runat="server" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                <asp:DropDownList ID="ddlleavevalue" runat="server" Style="float: left; position: absolute;
                    top: 208px; left: 117px;" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                    <asp:ListItem Value="0">Greater Then</asp:ListItem>
                    <asp:ListItem Value="1">Lesser Then</asp:ListItem>
                    <asp:ListItem Value="2">Equals</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtvalue" Style="position: absolute; top: 208px; left: 257px" runat="server"
                    Font-Bold="true" Height="19px" Width="80px" ForeColor="Black" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtvalue"
                    FilterType="Numbers">
                </asp:FilteredTextBoxExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblfrom" Style="position: absolute; top: 208px; left: 363px" runat="server"
            Text="From Date" Width="80px" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <asp:TextBox ID="txtfrom" runat="server" Style="position: absolute; top: 208px; left: 442px"
            Font-Bold="true" AutoPostBack="true" Width="80px" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
            Format="dd/MM/yyyy">
        </asp:CalendarExtender>
        <asp:Label ID="lblto" Style="position: absolute; top: 208px; left: 550px" runat="server"
            Text="To Date" Font-Bold="true" Width="80px" ForeColor="Black" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
        <asp:TextBox ID="txtto" runat="server" Font-Bold="true" AutoPostBack="true" Style="position: absolute;
            top: 208px; left: 640px" Width="80px" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"
            OnTextChanged="txtto_TextChanged"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
            Format="dd/MM/yyyy">
        </asp:CalendarExtender>
        <div>
            <asp:Button ID="btngo" runat="server" Text="Go" Font-Size="Medium" Font-Names="Book Antiqua"
                Font-Bold="true" Style="position: absolute; left: 789px; top: 208px" OnClick="btngo_Click"
                OnClientClick="return validation()" />
        </div>
        <br />
        <br />
        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="16px"
            Style="margin-left: 0px; top: 250px; left: -4px; position: absolute; width: 1028px;">
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td>
                    <FarPoint:FpSpread ID="FpstaffLeave" runat="server" Height="250px" Width="400px"
                        ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
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
                    <br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
                </td>
            </tr>
        </table>
    </body>
</asp:Content>
