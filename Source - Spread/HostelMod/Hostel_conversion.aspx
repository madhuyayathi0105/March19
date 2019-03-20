<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Hostel_conversion.aspx.cs"
    Inherits="Hostel_conversion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="uppbtn">
                <ProgressTemplate>
                    <center>
                        <div style="height: 100%; z-index: 100000; width: 100%; background-color: rgba(54, 25, 25, .2);
                            position: absolute; top: 0; left: 0px;">
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <div class="CenterPB" style="height: 40px; width: 40px; top: 200px;">
                                <image src="images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </center>
        <div class="div" style="position: fixed; width: 100%; z-index: 1000; height: 100px;
            background-color: lightblue; border-style: 1px;">
            <asp:Image ID="image1" runat="server" ImageUrl="~/Handler6.ashx" Style="height: 100px;
                width: 100%; height: 100px; position: fixed;" />
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <center>
            <div>
                <span style="color: Green;" class="fontstyleheader">Conversion</span>
                <div style="margin-left: 780px;">
                    <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        CssClass="lnkstyle" PostBackUrl="~/Hostel.aspx" CausesValidation="False">Back</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        CssClass="lnkstyle" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lb2" runat="server" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                        OnClick="lb2_Click" CssClass="lnkstyle" CausesValidation="False">Logout</asp:LinkButton>
                </div>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="uppbtn" runat="server">
                <ContentTemplate>
                    <div class="maindivstyle" style="height: 550px; width: 1000px;">
                    <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_conversion" runat="server" CssClass="textbox btn2" Text="Conversion"
                                        OnClick="btn_conversion_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lbl_convert" Visible="false" runat="server" ForeColor="Green" > </asp:Label>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    </form>
</body>
</html>
