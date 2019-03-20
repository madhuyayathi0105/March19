<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BarCode.aspx.cs" Inherits="BarCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnButton" runat="server" Text="Submit" OnClick="btnButton_Click" />
    </div>
    <img src='../BarCode/barcodeimg21032017180355IC000001.Jpeg?' style='height: 80px;
        width: 80px;' />
    <div id="IDdiv" runat="server" visible="false">
        <div runat="server" id="printDivFnl" style="height: 510px; overflow: auto;">
            <asp:PlaceHolder ID="phTimeTable" runat="server"></asp:PlaceHolder>
        </div>
    </div>
    </form>
</body>
</html>
