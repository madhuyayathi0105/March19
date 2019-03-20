<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 185px;
        }
    </style>
    <link rel="Stylesheet" href="Styles/css/Commoncss.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <center>
            <div>
                <center>
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Large" ForeColor="Green" Text="Change Password"></asp:Label>
                </center>
            </div>
            <center>
                <br />
                <fieldset style="width: 500px; height: 220px; border: 1px solid black; background-color: #51A2D7;">
                    <table style="text-align: left">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblusername" runat="server" Text="User Name" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antique"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbluser" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antique"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lbloldpassword" runat="server" Text="Old Password" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antique"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtoldpassword" runat="server" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antique" TextMode="Password" CssClass="textbox" Height="26px"
                                    Width="242px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblnewpassword" runat="server" Text="New Password" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antique"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtnewpassword" runat="server" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antique" TextMode="Password" CssClass="textbox" MaxLength="20"
                                    Height="26px" Width="242px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblconform" runat="server" Text="Confirm Password" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antique"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtconform" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    TextMode="Password" CssClass="textbox" MaxLength="20" Height="26px" Width="242px"></asp:TextBox>
                                <asp:CompareValidator ID="cv1" runat="server" ControlToCompare="txtnewpassword" ControlToValidate="txtconform"
                                    ErrorMessage="Password Must Match" ForeColor="Red" Font-Names="Book Antique"></asp:CompareValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblerr" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antique"
                        ForeColor="REd"></asp:Label>
                    <br />
                    <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox btn" Font-Bold="true"
                        Font-Size="Medium" Width="80px" Font-Names="Book Antique" OnClick="btnsave_Click"
                        Style="z-index: 1;" />
                </fieldset>
            </center>
        </center>
    </body>
</asp:Content>
