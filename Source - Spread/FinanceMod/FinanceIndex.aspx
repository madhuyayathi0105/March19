<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="FinanceIndex.aspx.cs" Inherits="FinanceIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
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
    <body>
        <div>
            <center>
                <asp:Label ID="Label1" CssClass="lbl" runat="server" Text="Finance"></asp:Label>
            </center>
        </div>
        <div>
            <center>
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="GdFin" runat="server" AutoGenerateColumns="False" GridLines="Both"
                                CssClass="grid-view" BackColor="WhiteSmoke" OnRowDataBound="GdFin_OnRowDataBound"
                                OnDataBound="GdFin_OnDataBound" Width="800px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblSno" runat="server" CssClass="grid_view_lnk_button" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Header Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHdrName" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblReportId" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportId") %>'></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Menu">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPagelink" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportName") %>'
                                                PostBackUrl='<%#Eval("PageName") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Help" Visible="false">
                                        <ItemTemplate>
                                            <center>
                                                <%--<asp:LinkButton ID="lbHelplink" runat="server" CssClass="grid_view_lnk_button" Text="Help"
                                                    PostBackUrl='<%#Eval("HelpURL") %>'></asp:LinkButton>--%>
                                                <a id="lbHelplink" runat="server" target="_blank" href='<%#Eval("HelpURL") %>'>Help</a>
                                            </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
