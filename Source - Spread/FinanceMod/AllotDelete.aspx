<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AllotDelete.aspx.cs" Inherits="AllotDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    Batch
                </td>
                <td>
                    <asp:DropDownList ID="ddlyear" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnDel" runat="server" Text="Update" OnClick="btnDel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
