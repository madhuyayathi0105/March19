<%@ Page Language="C#" AutoEventWireup="true" CodeFile="update.aspx.cs" Inherits="HostelMod_update" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btn_update" runat="server" Text="Update Student" OnClick="btn_update_click" />
    </div>
    <br />
    <asp:Label ID="lbl_error" Font-Bold="true" runat="server" Font-Size="Medium"></asp:Label>
    </form>
</body>
</html>
