<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="OnlineFeesTransactionUpdate.aspx.cs" Inherits="OnlineFeesTransactionUpdate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green;">IOB MISSING PAYMENT UPDATION</span>
        </div>
    </center>
    <center>
        <div>
            <br />
            <br />
            <br />
            <br />
            <fieldset style="width: 517px; height: 73px;">
                <center>
                    <asp:Label ID="bankname" Text="Bank" runat="server"></asp:Label>
                    <asp:DropDownList ID="ddl_bankname" runat="server" CssClass="textbox  ddlheight2"
                        AutoPostBack="true">
                        <asp:ListItem Value="0">IOB</asp:ListItem>
                        <asp:ListItem Value="1">SBI</asp:ListItem>
                        <asp:ListItem Value="2">ATOM</asp:ListItem>
                        <asp:ListItem Value="3">KOTAk Mahendra</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Date" runat="server" Text="Date"></asp:Label>
                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight" Height="15px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                        CssClass="cal_Theme1 ajax__calendar_active">
                    </asp:CalendarExtender>
                    <asp:Button ID="btn_save" BackColor="#8199FD" Text="UPDATE FEES" runat="server" CssClass="textbox btn2 textbox1"
                        OnClick="btn_update_Click" Font-Names=" Book Antiqua" Font-Size="Medium" Style="width: 143px;
                        height: 33px;" />
                </center>
            </fieldset>
        </div>
    </center>
</asp:Content>
