<%@ Page Title="" Language="C#" MasterPageFile="~/IpatchMod/ipatch.master" AutoEventWireup="true"
    CodeFile="i_patch_master.aspx.cs" Inherits="i_patch_master" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
     
        </script>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <div>
                    <asp:Label ID="lblcanteenmennuitem" runat="server" Style="color: Green;" Text="I Patch Master"
                        CssClass="fontstyleheader"></asp:Label>
                </div>
                <center>
                    <div class="maindivstyle" style="height: 470px; width: 1000px;">
                        <br />
                        <br />
                        <asp:Label ID="errmsg" runat="server" ForeColor="Red"></asp:Label>
                        <asp:GridView ID="patchgrid" runat="server" AutoGenerateColumns="false" Width="950px"
                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" AllowPaging="false"
                            BackColor="white" CssClass="spreadborder" OnRowDataBound="patchgrid_RowDataBound"
                            OnRowCommand="patchgrid_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Module Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblModul_name" runat="server" Text='<%#Eval("Module Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patch Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_patchname" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Descp" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modified Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_modifiy_date" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Text='<%# Eval("Modified Date") %>' ForeColor="Blue" Font-Size="Small"></asp:Label><%--Download--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="85px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Updated Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_updated" runat="server" ForeColor="Blue" Font-Underline="false"
                                            Font-Bold="true" Font-Size="Small" Text='<%# Eval("Client Updated Date") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="85px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update All">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbl_updat" runat="server" Style="height: 38px; width: 100px;
                                            text-decoration: none; background-color: #FFE4B5;" Text="Update" ForeColor="green"
                                            Font-Size="Medium" Font-Bold="true" OnClick="gridOnclick"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="85px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lbler" runat="server" Visible="false"></asp:Label>
                    </div>
                    <center>
                    </center>
                    <center>
                        <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <br />
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_alerterror" Visible="true" runat="server" Text="" Style="color: Red;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
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
                <center>
                </center>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
