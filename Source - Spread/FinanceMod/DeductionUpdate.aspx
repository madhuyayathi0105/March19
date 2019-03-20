<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DeductionUpdate.aspx.cs" Inherits="DeductionUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="Script1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <span style="font-family: Book Antiqua; color: Green; font-size: 20px; text-align: center;
                        font-weight: bold;">Deduction Reason Update</span>
                </center>
            </div>
            <br />
            <div>
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 300px; height: 180px;">
                                            <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                            PopupControlID="pnl_studhed" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 300px; height: 180px;">
                                            <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                            PopupControlID="pnl_studled" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" Text="Semester" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem" runat="server" CssClass="textbox ddlstyle ddlheight3">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label3" Text="Deduction Reason" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldeduct" runat="server" CssClass="textbox ddlstyle ddlheight3">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <%-- <td>
                    <asp:RadioButtonList ID="rbmode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                        OnSelectedIndexChanged="rbmode_Selected">
                        <asp:ListItem Text="FFC" Value="0" Selected="True">  </asp:ListItem>
                        <asp:ListItem Text="SFC" Value="1">  </asp:ListItem>
                    </asp:RadioButtonList>
                </td>--%>
                            <td>
                                Enter The Reg No
                            </td>
                            <td>
                                <asp:TextBox ID="txtroll" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnupdate" runat="server" Text="update" OnClientClick="return confirm('Are sure want to update')"
                                    OnClick="btnupdate_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
