<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateFeecategoryandFinyear.aspx.cs"
    Inherits="UpdateFeecategoryandFinyear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="ddlCollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollegeChange_NEW">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlFinyear" runat="server">
        </asp:DropDownList>
        <asp:Button ID="btnFeeCode" runat="server" Text="UpdateFeeCode" OnClick="btnFeeCodeClike" />
        <asp:Label ID="ErrorLbl" runat="server" ForeColor="Red" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
