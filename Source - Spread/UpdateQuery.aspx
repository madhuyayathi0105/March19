<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateQuery.aspx.cs" Inherits="UpdateQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="BtnUpdate" runat="server" Text="Update Attendance" OnClick="BtnUpdate_Click" />
        <asp:Label ID="lbl" runat="server" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
