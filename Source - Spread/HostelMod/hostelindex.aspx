<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="hostelindex.aspx.cs" Inherits="hostelindex" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <style type="text/css">
            /* body
        {
            background-image: url('images/money.jpg');
            background-repeat: no-repeat;
            background-attachment: fixed;
        }
        img
        {
            opacity: 0.5;
            filter: alpha(opacity=50); 
        }*/
            .grid-view
            {
                padding: 0;
                margin: 0;
                border: 1px solid #333;
                font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
                font-size: 0.9em;
            }
            
            .grid-view tr.header
            {
                color: white;
                background-color: #0CA6CA;
                height: 30px;
                vertical-align: middle;
                text-align: center;
                font-weight: bold;
                font-size: 20px;
            }
            
            .grid-view tr.normal
            {
                color: black;
                background-color: #FDC64E;
                height: 25px;
                vertical-align: middle;
                text-align: center;
            }
            
            .grid-view tr.alternate
            {
                color: black;
                background-color: #D59200;
                height: 25px;
                vertical-align: middle;
                text-align: center;
            }
            
            .grid-view tr.normal:hover, .grid-view tr.alternate:hover
            {
                background-color: white;
                color: black;
                font-weight: bold;
            }
            
            .grid_view_lnk_button
            {
                color: Black;
                text-decoration: none;
                font-size: large;
            }
            .lbl
            {
                font-family: Book Antiqua;
                font-size: 30px;
                font-weight: bold;
                color: Green;
                text-align: center;
                font-style: italic;
            }
            .hdtxt
            {
                font-family: Book Antiqua;
                font-size: large;
                font-weight: bold;
            }
            .FixedHeader
            {
                position: absolute;
                font-weight: bold;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <div>
            <center>
                <asp:Label ID="lbl_header" runat="server" Text="Hostel" CssClass="lbl"></asp:Label>
            </center>
            <center>
                <div>
                    <asp:GridView ID="importgrid" runat="server" AutoGenerateColumns="false" CssClass="grid-view"
                        BackColor="White" OnDataBound="importgrid_span" OnRowDataBound="importgrid_OnRowDataBound"
                        Width="800px">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_sno" runat="server" CssClass="grid_view_lnk_button" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Header Name">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblModul_name" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName") %>'></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Report ID">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_rid" runat="server" CssClass="grid_view_lnk_button" Text='<%# Eval("ReportId") %>'></asp:Label>
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Menu">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbl_menu" runat="server" CssClass="grid_view_lnk_button" Font-Underline="false"
                                        Text='<%#Eval("ReportName")%>' PostBackUrl='<%#Eval("PageName") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Help" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_help" runat="server" CssClass="grid_view_lnk_button" Text='<%# Eval("HelpURL") %>'
                                        ForeColor="Blue" Font-Underline="True" Font-Size="Small"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </center>
            <center>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
