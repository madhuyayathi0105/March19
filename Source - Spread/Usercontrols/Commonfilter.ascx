<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Commonfilter.ascx.cs"
    Inherits="Usercontrols_Commonfilter" %>
<asp:UpdatePanel ID="updprint" runat="server">
    <ContentTemplate>
       
            <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                background-color: lightblue; border-width: 1px;">
                <tr>
                    <td>
                        <asp:Label ID="lblsearch" Text="Search By" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" ></asp:Label>
                    </td>
                    <td>                    
                        <asp:DropDownList ID="ddlheader" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlheader_SelectIndexChange">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlmatch" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua">
                            <asp:ListItem Text="Like"></asp:ListItem>
                            <asp:ListItem Text="Equal"></asp:ListItem>
                            <asp:ListItem Text="Not Equal"></asp:ListItem>
                            <asp:ListItem Text="Start With"></asp:ListItem>
                            <asp:ListItem Text="End With"></asp:ListItem>
                            <asp:ListItem Text="Grater Than"></asp:ListItem>
                            <asp:ListItem Text="Grater Than Equals"></asp:ListItem>
                            <asp:ListItem Text="Laser Than"></asp:ListItem>
                            <asp:ListItem Text="Laser Than Equals"></asp:ListItem>
                            <asp:ListItem Text="Between"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlvalue" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Width="100px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
    </ContentTemplate>
</asp:UpdatePanel>
