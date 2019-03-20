<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="timetablechangerreport.aspx.cs" Inherits="Default5" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 327px;
        }
        .style16
        {
            width: 34px;
        }
        .style21
        {
            width: 101px;
        }
        .style23
        {
            width: 86px;
        }
        .style24
        {
            width: 65px;
        }
        .style25
        {
            width: 20px;
        }
        .style26
        {
            width: 48px;
        }
        .style27
        {
            width: 69px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errlbl').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Time Table Changer Report" CssClass="fontstyleheader"
            ForeColor="Green"></asp:Label></center>
    <br />
    <div>
        <table style="width: 921px; height: 89px;" class="maintablestyle ">
            <tr>
                <td class="style1">
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td class="style1">
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style1">
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style1">
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td class="style1">
                    <asp:Label ID="lblbranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style2">
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                    PopupControlID="pbranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td class="style23">
                    <asp:Label ID="fromdatelbl" runat="server" Text="From Date" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="fromdatetxt" AutoPostBack="true" runat="server" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua" Height="20px" Width="75px" OnTextChanged="fromdatetxt_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="fromdateext" runat="server" TargetControlID="fromdatetxt"
                        Format="d/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="fromdatetxtfilter" runat="server" TargetControlID="fromdatetxt"
                        FilterType="Custom,Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                </td>
                <tr>
                    <td class="style27">
                        <asp:Label ID="todatelbl" runat="server" Text="To Date" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="todatetxt" AutoPostBack="true" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Height="20px" Width="75px" OnTextChanged="todatetxt_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="todateext" runat="server" TargetControlID="todatetxt" Format="d/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="todatefilter" runat="server" TargetControlID="todatetxt"
                            FilterType="Custom,Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td class="style21">
                        <asp:Label ID="fromhrlbl" runat="server" Text="Period From" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="fromhrddl" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Height="25px" Width="41px" AutoPostBack="True" OnSelectedIndexChanged="fromhrddl_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style25">
                        <asp:Label runat="server" Text="To" ID="tohrlbl" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="tohrddl" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Height="23px" Width="40px" Style="margin-left: 0px"
                            AutoPostBack="True" OnSelectedIndexChanged="tohrddl_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style16">
                        <asp:Label ID="yrlbl" runat="server" Text="Year" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style26">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                    Width="70px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">--Select--</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td class="style24">
                        <asp:Button ID="gobtn" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="gobtn_Click" Width="40px" />
                    </td>
                </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <center>
            <FarPoint:FpSpread ID="alter_spread" runat="server" BorderWidth="2px" Height="50px"
                BorderColor="Black">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="true" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton" ShowPDFButton="true">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
    </div>
    </html>
</asp:Content>
