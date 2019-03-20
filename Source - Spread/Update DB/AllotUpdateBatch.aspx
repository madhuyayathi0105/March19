<%@ Page Title="" Language="C#" MasterPageFile="~/Update DB/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AllotUpdateBatch.aspx.cs" Inherits="FinanceMod_AllotUpdateBatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span style="font-size: 18px; font-weight: bold; color: Green;">Update Balance Amount
                Based On Allot Amt & Paid Amt</span>
        </center>
    </div>
    <br />
    <div>
        <table>
            <tr>
                <td>
                    College
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                        OnSelectedIndexChanged="ddlcollegename_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    Batch
                </td>
                <td>
                    <asp:DropDownList ID="ddlyear" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblheader" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
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
                    <asp:Label ID="lblledger" runat="server" Text="Ledger"></asp:Label>
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
                    Admission No
                </td>
                <td>
                    <asp:TextBox ID="txtroll" runat="server" Style="height: 20px; width: 100px;"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtroll"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ,"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td>
                    <asp:Button ID="btnDel" runat="server" Text="Update" OnClick="btnDel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
