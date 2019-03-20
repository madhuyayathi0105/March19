<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentBankStatemnetImport.aspx.cs" Inherits="StudentBankStatemnetImport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>       
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Student Bank Statemnet Import</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: auto; height: auto;">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblmemtype" Text="Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlmemtype" runat="server" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label1" Text="Mode"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmode" runat="server" Width="146px" ReadOnly="true">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight">
                                <asp:CheckBox ID="cbmode" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Text="Select All" OnCheckedChanged="cbmode_changed" AutoPostBack="True" />
                                <asp:CheckBoxList ID="cblmode" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                    runat="server" OnSelectedIndexChanged="cblmode_selected" AutoPostBack="True">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtmode"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </td>
                        <td>
                            <asp:Label ID="lblhed" Text="Header" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlheader" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblledg" Text="Ledger" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlledger" runat="server" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblbk" Text="Bank" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbank" runat="server" CssClass="textbox ddlstyle ddlheight3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbr" runat="server" Text="Browse File"></asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" Height="25px" CssClass="textbox"
                                ForeColor="White" />
                        </td>
                        <td>
                            <asp:Button ID="btnimport" runat="server" CssClass="textbox btn2" Text="Import" OnClick="btnimport_Click" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkhelp" runat="server" Text="Help" OnClick="lnkhelp_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
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
        </center>
    </body>
    </html>
</asp:Content>
