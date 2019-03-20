<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimeTableStaffCodeUpdate.aspx.cs" Inherits="TimeTableStaffCodeUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; Width=113px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Timetable Staffcode Update </span>
        </center>
        <div class="maindivstyle maindivstylesize">
     <br />
     <center>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <tr>
                 <td>
                   <asp:Label ID="Lbloldstaffcode" runat="server" Text="Old staff code"></asp:Label>
                    &nbsp;
                    <asp:TextBox ID="txtoldstaffcode" runat="server"></asp:TextBox>
                 </td>
            </tr>
           <tr>
              <td>
                 <asp:Label ID="Lblnewstaffcode" runat="server" Text="New staff code"></asp:Label>
                <asp:TextBox ID="txtnewstaffcode" runat="server"></asp:TextBox>
              </td>
        </tr>
        <tr>
           <td>
                <asp:Button ID="btn_update" runat="server" Text="Update StaffCode" 
                      OnClick="btn_update_click" style="margin-left: 168px" Width="113px" />
        </tr>
        </table>
       </center>
        <center>

                 <asp:Label ID="lbl_error" Font-Bold="true" style="color: red;" runat="server" Font-Size="Medium"></asp:Label>
          </center>
       </div>
    </div>
    </form>
</body>
</html>
