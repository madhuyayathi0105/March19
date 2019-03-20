<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TranserUpdate.aspx.cs" Inherits="TranserUpdate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>   
    <div>
        <fieldset style="width: 150px;">
            <legend>Transfer Student Update</legend>
            <table>
                <tr>
                    <td>
                        Update
                    </td>
                    <td>
                        <asp:Button ID="btnTrans" runat="server" Text="Update" OnClick="btnTrans_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>       
    </div>
</asp:Content>
