<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateNewRollNo.aspx.cs" Inherits="UpdateNewRollNo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
    <div>
     <div>
     <table>
        <tr>
                                <td>                                
                                    <asp:Label ID="lblfile_upload" runat="server" Text="Browse File"></asp:Label>
                                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="textbox" ForeColor="Green" />
                                </td>
                                <td>
                                <asp:Button ID="btn_update" runat="server" Text="Update"
                                OnClick="btn_update_click" />
                                </td>
                                </tr>
                                </table>
        <br />
        <asp:Label ID="lbl_error" Font-Bold="true" runat="server" Font-Size="Medium"></asp:Label>
    </div>
    <br />
    <center>
    <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
    </center>
    </div>
    </center>
    </form>
</body>
</html>
