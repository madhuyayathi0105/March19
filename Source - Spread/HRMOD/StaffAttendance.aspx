<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="StaffAttendance.aspx.cs" Inherits="StaffAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <title>Staff Attendance</title>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green">Staff Attendance</span>
        </div>
    </center>
    <br />
    <center>
        <table class="maintablestyle">
            <tr>
                <td class="style9">
                    <asp:Label ID="Label6" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td class="style10">
                    <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="19px" Width="78px" AutoPostBack="True"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td class="style11">
                    <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td class="style12">
                    <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="19px" Width="78px" AutoPostBack="True"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="Lbldept" runat="server" Visible="false" Text="Department" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td class="style464" id="tdDepartment" runat="server" visible="false">
                    <div id="castediv" runat="server" class="linkbtn">
                        <asp:TextBox ID="txtDept" runat="server" Height="16px" ReadOnly="true" Width="135px"
                            Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">---Select---</asp:TextBox>
                        <br />
                    </div>
                    <asp:Panel ID="pnlDept" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="300px" ScrollBars="Vertical" Width="350px">
                        <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chkselect_CheckedChanged"
                            Text="Select All" />
                        <asp:CheckBoxList ID="cbldepttype" runat="server" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="cbldepttype_OnSelectedIndexChanged"
                            Width="235px" Height="102px" Font-Bold="True" Font-Names="Book Antiqua">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="ddlDept" runat="server" DropDownControlID="pnlDept" DynamicServicePath=""
                        Enabled="true" TargetControlID="txtDept">
                    </asp:DropDownExtender>
                </td>
                <td>
                    <asp:Label ID="LblStcodeName" runat="server" Visible="false" Text="Staff Code/Name"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSt_codeandName" Visible="false" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Width="200px" Font-Size="Medium" Height="23px">
                        <%--OnSelectedIndexChanged="ddlSt_codeandName_SelectedIndexChanged"--%>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="BtnSearch" runat="server" Text="GO" Font-Bold="True" ForeColor="Black"
                        OnClick="BtnSearch_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Label ID="msg" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-top: 10%"></asp:Label>
        <asp:Label ID="msg1" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-top: 10%"></asp:Label>
    </center>
    <br />
    <asp:Panel ID="panel_Total" runat="server" Visible="false">
        <center>
            <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                border-right-style: solid; width: 500px; background-color: #0CA6CA; border-width: 0px;">
                <tr>
                    <td>
                        <asp:Label ID="lblta" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Width="197px"
                            Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lbltp" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Width="282px"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbltper" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Width="282px" Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lbltl" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Width="282px"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblto" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Width="282px"
                            Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <br />
    <br />
    <center>
        <FarPoint:FpSpread ID="FpStaffAttendance" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" VerticalScrollBarPolicy="Never" ActiveSheetViewIndex="0" ShowHeaderSelection="false">
            <Sheets>
                <FarPoint:SheetView SheetName="Staff Attendance">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
        <br />
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </center>
</asp:Content>
