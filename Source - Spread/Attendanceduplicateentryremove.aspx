<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendanceduplicateentryremove.aspx.cs"
    Inherits="Attendanceduplicateentryremove" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:Button ID="btn_update" runat="server" Text="Missing Attendance update Step 1"
            OnClick="btn_update_click" />
        <asp:Button ID="btn_update2" runat="server" Text="Missing Attendance update Step 2"
            OnClick="btn_update2_click" />
        <asp:Button ID="Button1" runat="server" Visible="false" Text="Holiday Delete" OnClick="Button1_click" />
        <asp:Button ID="Button2" runat="server" Text="Delete CAM " OnClick="Button2_click" />
        <br />
        <asp:Button ID="Button4" runat="server" Text="Remove SubjectChooser" OnClick="Button4_click" />
    </div>
    <br />
    <br />
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lbldate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtDate" CssClass="txt" runat="server" Height="20px" Width="79px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox><%--OnTextChanged="txtFromDate_TextChanged"--%>
                    <asp:FilteredTextBoxExtender ID="txtDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                        ValidChars="/" runat="server" TargetControlID="txtDate">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="caldate" TargetControlID="txtDate" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblTodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txttoDate" CssClass="txt" runat="server" Height="20px" Width="79px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox><%--OnTextChanged="txtFromDate_TextChanged"--%>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                        ValidChars="/" runat="server" TargetControlID="txttoDate">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttoDate" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Button ID="Button3" runat="server" Text="Update Fee of Roll Attendance" OnClick="Button3_click" />
                </td>
                <td>
                    <asp:Button ID="Btnodmark" runat="server" Text="Student OD Attendance Mark" OnClick="Btnodmark_click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button7" runat="server" Text="RemoveCo Marks" OnClick="btnRemoveCo_click" />
                </td>
                </td></tr>
        </table>
    </div>
    <div>
        <asp:Label ID="lbl_error" Font-Bold="true" runat="server" Font-Size="Medium"></asp:Label></div>
    <br />
    <div>
        <asp:Button ID="Button5" runat="server" Text="Remove Duplicate Mark Entry" OnClick="Button5_click" /></div>
    <div>
        <asp:Button ID="Button6" runat="server" Text="Update OD Entry" OnClick="Button6_click" /></div>
    </form>
</body>
</html>
