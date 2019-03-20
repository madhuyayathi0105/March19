<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="staffexperiencereport.aspx.cs" Inherits="staffexperiencereport" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="pnl4" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="20px"
        Style="margin-left: 0px; top: 95px; left: -23px; width: 1008px; position: absolute;">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label31" runat="server" Text="Staff Experience Report" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </asp:Panel>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblcollege" runat="server" Text="College Name" CssClass="font" Width="150px"
                        Style="top: 135px; left: 10px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                        CssClass="font" Height="20px" Width="200px" AutoPostBack="True" Style="top: 135px;
                        left: 120px; position: absolute;">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="font" Style="top: 135px;
                        left: 345px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdept" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="110px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="height: 20px; width: 120px; position: absolute; left: 443px; top: 135px;">---Select---</asp:TextBox>
                            <asp:Panel ID="pdept" runat="server" Height="240px" CssClass="multxtpanel">
                                <asp:CheckBox ID="chkdept" runat="server" CssClass="font" OnCheckedChanged="chkdept_CheckedChanged"
                                    Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklsdept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklsdept_SelectedIndexChanged"
                                    CssClass="font" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdept"
                                PopupControlID="pdept" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbldesign" runat="server" Text="Designation" ForeColor="Black" CssClass="font"
                        Style="height: 20px; width: 110px; position: absolute; left: 590px; top: 135px;"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdesign" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="150px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="height: 20px; width: 110px; position: absolute; left: 690px; top: 135px;">---Select---</asp:TextBox>
                            <asp:Panel ID="pdesign" runat="server" Height="240px" CssClass="multxtpanel">
                                <asp:CheckBox ID="chkdesign" runat="server" CssClass="font" OnCheckedChanged="chkdesign_CheckedChanged"
                                    Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklsdesign" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklsdesign_SelectedIndexChanged"
                                    CssClass="font" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtdesign"
                                PopupControlID="pdesign" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblcategory" runat="server" Text="Category" ForeColor="Black" Style="height: 20px;
                        width: 110px; position: absolute; left: 10px; top: 175px;" CssClass="font"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtcategory" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="150px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="height: 20px; width: 110px; position: absolute; left: 90px; top: 175px;">---Select---</asp:TextBox>
                            <asp:Panel ID="pcategory" runat="server" Height="240px" CssClass="multxtpanel">
                                <asp:CheckBox ID="chkcategory" runat="server" CssClass="font" OnCheckedChanged="chkcategory_CheckedChanged"
                                    Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklscetegory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklscetegory_SelectedIndexChanged"
                                    CssClass="font" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtcategory"
                                PopupControlID="pcategory" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Staff Type" ForeColor="Black" Style="height: 20px;
                        width: 110px; position: absolute; left: 220px; top: 175px;" CssClass="font"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txttype" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="150px" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="height: 20px; width: 110px; position: absolute; left: 305px; top: 175px;">---Select---</asp:TextBox>
                            <asp:Panel ID="ptype" runat="server" Height="240px" CssClass="multxtpanel">
                                <asp:CheckBox ID="chktype" runat="server" CssClass="font" OnCheckedChanged="chktype_CheckedChanged"
                                    Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklstype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstype_SelectedIndexChanged"
                                    CssClass="font" Height="58px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txttype"
                                PopupControlID="ptype" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblorder" runat="server" Text="Order By" ForeColor="Black" Style="height: 20px;
                        width: 110px; position: absolute; left: 440px; top: 175px;" CssClass="font"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlorder" runat="server" CssClass="font" Style="height: 25px;
                        width: 150px; position: absolute; left: 515px; top: 174px;">
                        <asp:ListItem Text="Priority"></asp:ListItem>
                        <asp:ListItem Text="Dept & Staff Code"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="Chkfilter" runat="server" CssClass="font" Height="50px" AutoPostBack="true"
                        OnCheckedChanged="Chkfilter_CheckedChanged" Style="position: absolute; left: 10px;
                        top: 215px;" />
                </td>
                <td>
                    <asp:Label ID="lblType3" runat="server" Text="Type" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 215px; left: 30px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" CssClass="font" Height="25px" Width="135px"
                        Style="height: 25px; left: 89px; position: absolute; top: 214px; width: 115px;">
                        <asp:ListItem>Other Experience</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblselect" runat="server" Text="Select" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 215px; left: 222px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlselect" runat="server" CssClass="font" Height="25px" Width="115px"
                        Style="top: 214px; left: 305px; position: absolute;">
                        <asp:ListItem>Equal</asp:ListItem>
                        <asp:ListItem>Below</asp:ListItem>
                        <asp:ListItem>Above</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Years" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 215px; left: 444px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtexperince" runat="server" Height="20px" Width="40px" Font-Bold="true"
                        Font-Names="Book Antiqua" MaxLength="2" Font-Size="Medium" Style="top: 214px;
                        left: 495px; position: absolute;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="filex" runat="server" FilterType="Numbers" TargetControlID="txtexperince">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td>
                    <asp:CheckBox ID="chkdate" runat="server" AutoPostBack="true" OnCheckedChanged="chkdate_CheckedChanged"
                        Style="top: 215px; left: 570px; position: absolute;" />
                </td>
                <td>
                    <asp:Label ID="lbldate" runat="server" Text="From Date" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 215px; left: 598px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtfromdate" runat="server" Height="20px" Width="80px" Font-Bold="true"
                        Font-Names="Book Antiqua" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged"
                        Font-Size="Medium" Style="top: 213px; left: 680px; position: absolute;"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender4" Format="dd/MM/yyyy" TargetControlID="txtfromdate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Style="top: 215px; left: 780px; position: absolute;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txttodate" runat="server" Height="20px" Width="80px" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"
                        Style="top: 213px; left: 845px; position: absolute;"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txttodate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Button ID="btnMainGo" runat="server" Text="Go" CssClass="font" Style="top: 215px;
                        left: 937px; position: absolute;" OnClick="btnMainGo_Click" />
                </td>
            </tr>
        </table>
    <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Height="20px"
        Style="margin-left: 0px; top: 260px; left: -23px; width: 1008px; position: absolute;" />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="errmsg" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
    <br />
    <FarPoint:FpSpread ID="Fpexperience" runat="server" Height="250px" Width="400px"
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
    <br />
    <table>
        <tr>
            <td>
                <asp:Label ID="lblnorec" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" CssClass="font" Height="20px" onkeypress="display()"
                    Width="180px">
                </asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
