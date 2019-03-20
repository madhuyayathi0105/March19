<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default_LoginPage.aspx.cs" Inherits="Default_LoginPage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div>
            <br />
            <br />
            <br />
            <asp:GridView ID="College_Grid" runat="server" AutoGenerateColumns="false" Width="650px"
                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" AllowPaging="false"
                BackColor="white" CssClass="spreadborder" OnRowCommand="College_Grid_RowCommand"
                OnRowDataBound="College_Grid_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Institution Name">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Institution" runat="server" Text='<%#Eval("Institution Name") %>'></asp:Label>
                            <asp:Label ID="lbl_institutioncode" Visible="false" runat="server" Text='<%#Eval("Institution Code") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="500px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </center>
</asp:Content>
