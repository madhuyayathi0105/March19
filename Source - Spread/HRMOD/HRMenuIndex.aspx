<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HRMenuIndex.aspx.cs" Inherits="HRMenuIndex" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <div>
                        <center>
                            <asp:Label ID="Label1" CssClass="lbl" runat="server" Text="HR"></asp:Label>
                        </center>
                    </div>
                    <asp:GridView ID="grdhrmenu" runat="server" AutoGenerateColumns="false" OnDataBound="grdhrmenu_databound"
                        OnRowDataBound="grdhrmenu_OnRowDataBound" GridLines="Both" CssClass="grid-view"  BackColor="WhiteSmoke" Width="800px">
                        <%----%>
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblSno" runat="server" CssClass="grid_view_lnk_button" Text='<%#Container.DisplayIndex+1 %>'>
                                        </asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Header Name" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblHdrName" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName")%>'>
                                        </asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lblReportId" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportId")%>'>
                                        </asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Menu" HeaderStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbPagelink" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportName") %>' PostBackUrl='<%#Eval("PageName") %>'
                                        Style="text-decoration: none;"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Help" HeaderStyle-HorizontalAlign="Center" Visible="false" >
                                <ItemTemplate>
                                    <center>
                                        <asp:LinkButton ID="lbHelplink" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HelpURL")%>' />
                                    </center>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    <br />
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
