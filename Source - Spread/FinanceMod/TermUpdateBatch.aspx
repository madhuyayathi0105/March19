<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TermUpdateBatch.aspx.cs" Inherits="TermUpdateBatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <span>
        <center>
            Term and Financial Year Update
        </center>
    </span>
    <table>
        <tr>
            <td>
                College
            </td>
            <td>
                <asp:DropDownList ID="ddlclg" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                Year
            </td>
            <td>
                <asp:DropDownList ID="ddlbatch" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnupdate" runat="server" Text="Update" OnClick="btnupdate_Click" />
            </td>
             <td>
                <asp:Button ID="btnmode" runat="server" Text="Mode Update" OnClick="btnmode_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
