<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="staffattendreport.aspx.cs" Inherits="staffattendreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
    <body oncontextmenu="return false">
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Staff Attendance Strength Report</span>
            </div>
        </center>
        <div>
            <br />
            <center>
                <table class="maintablestyle" style="height: 40px; width: 275px;">
                    <tr>
                        <td>
                            <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Date"></asp:Label>
                        </td>
                        <td class="style212" colspan="3">
                            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                            </asp:ToolkitScriptManager>
                            <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="20px"
                                Width="75px" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txtentryfrom"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="regdate1" runat="server" ControlToValidate="Txtentryfrom"
                                ErrorMessage="Please enter the Date" ForeColor="#FF3300" Style="top: 43px; position: absolute;
                                height: 26px; width: 131px; left: 278px;"></asp:RequiredFieldValidator>
                        </td>
                        <td class="style301">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="To:" Visible="False"></asp:Label>
                        </td>
                        <td class="style457" colspan="3">
                            <asp:TextBox ID="Txtentryto" runat="server" Height="20px" Width="75px" Font-Bold="True"
                                Font-Names="Book Antiqua" Visible="False"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                                Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="reqdateto" runat="server" ControlToValidate="Txtentryto"
                                ErrorMessage="Please enter the  to Date" ForeColor="Red" Style="top: 68px; left: 504px;
                                position: absolute; height: 16px; width: 161px"></asp:RequiredFieldValidator>
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                ForeColor="Red" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbldepttype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="130px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
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
                    ForeColor="Red" Text="There is No Record Found" Visible="False"></asp:Label>
            </center>
            <br />
        </div>
        <center>
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="fpattendance" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="478px" Width="683px" OnCellClick="fpattendance_Click"
                            OnPreRender="fpattendance_Render">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonType="PushButton" ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtxl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
        </center>
        <center>
            <div id="popper1" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                    width: 30px; position: absolute; margin-top: 9px; margin-left: 420px;" OnClick="imagebtnpopclose2_Click" />
                <center>
                    <br />
                    <div style="background-color: White; height: 550px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <span style="color: Blue; font-size: large;">Date:</span> <span id="DateSpan" runat="server">
                                        </span>
                                    </td>
                                    <td>
                                        <span style="color: Blue; font-size: large;">Department:</span> <span id="DepartmentSpan"
                                            runat="server"></span>
                                    </td>
                                    <td>
                                        <span style="color: Blue; font-size: large;">Attendance: </span><span id="AttendanceSpan"
                                            runat="server"></span>
                                    </td>
                                    <td>
                                        <span style="color: Blue; font-size: large;">Session:</span> <span id="SessionSpan"
                                            runat="server"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <center>
                            <FarPoint:FpSpread ID="Fpspreadpay1" runat="server" Visible="false" BorderColor="Gray"
                                BorderStyle="Solid" BorderWidth="1px" Width="570px" Height="270px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightGreen">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </html>
</asp:Content>
