<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateFeeallotanddailytrans.aspx.cs" Inherits="UpdateFeeallotanddailytrans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_click" />
     <asp:Button ID="update" runat="server" Text="Update negative" OnClick="update_click" />
      <asp:Button ID="jamalUpdate" runat="server" Text="jamalUpdate transaction" OnClick="update1_click" />
       <asp:Label ID="lbllastrcpt" runat="server" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
