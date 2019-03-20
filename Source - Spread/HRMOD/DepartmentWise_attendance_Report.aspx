<%@ Page Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="DepartmentWise_attendance_Report.aspx.cs" Inherits="HRMOD_DepartmentWise_attendance_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Department Wise Staff Attendance
                    Report</span>
            </div>
        </center>
        <center>
            <table class="maintablestyle" style="height: 40px; width: 275px;">
                <tr>
                    <td>
                        <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Text="Date"></asp:Label>
                    </td>
                    <td class="style212" colspan="3">
                        <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="20px"
                            Width="75px" Font-Bold="True" Font-Names="Book Antiqua" OnTextChanged="Validate_Date"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="regdate1" runat="server" ControlToValidate="Txtentryfrom"
                            ErrorMessage="Please enter the Date" ForeColor="#FF3300" Style="top: 43px; position: absolute;
                            height: 26px; width: 131px; left: 278px;"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updatedepartment" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddldept" runat="server" OnSelectedIndexChanged="ddldepartment_OnSelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="285px" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbformate1" runat="server" Text="Formate1" AutoPostBack="true"
                            Checked="true" Width="100px" OnCheckedChanged="rdbformate1_changed" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbformate2" runat="server" Text="Formate2" AutoPostBack="true"
                            Checked="false" Width="100px" OnCheckedChanged="rdbformate2_changed" />
                    </td>
                    <td>
                        <asp:Label ID="lblsection" runat="server" Text="Session" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsession" runat="server" Width="50px">
                            <asp:ListItem Value="M">M</asp:ListItem>
                            <asp:ListItem Value="E">E</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            OnClick="btngo_Click" Text="GO" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <center>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                ForeColor="Red" Visible="False"></asp:Label>
        </center>
        <br />
        <div>
            <center>
                <FarPoint:FpSpread ID="Fpspread1" runat="server" ShowHeaderSelection="false" Visible="false"
                    BorderWidth="5px" BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0"
                    Style="margin-left: -5px" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                    OnButtonCommand="Fpspread1_ButtonCommand">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
        </div>
        <center>
            <asp:UpdatePanel ID="upgo" runat="server">
                <ContentTemplate>
                    <%--<asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />--%>
                    <asp:GridView ID="grddepartmentwiseLeave" Width="1000px" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua"
                        toGenerateColumns="false" ShowHeader="false" OnRowDataBound="grddepartmentwiseLeave_RowDataBound">
                        <%--  OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                    OnSelectedIndexChanged="SelectedIndexChanged"--%>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        </br>
        <center>
            <asp:UpdatePanel ID="Upleavedetails" runat="server">
                <ContentTemplate>
                    <%--<asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />--%>
                    <asp:GridView ID="grdleavedetails" Width="1000px" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua"
                        toGenerateColumns="false" ShowHeader="false" OnRowDataBound="grdleavedetails_RowDataBound">
                        <%--  OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                    OnSelectedIndexChanged="SelectedIndexChanged"--%>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <div id="rptprint" runat="server" visible="false">
                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                    Visible="false"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                    Width="180px" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                    Text="Export To Excel" Width="127px" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    CssClass="textbox btn1" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
        </center>
    </html>
</asp:Content>
