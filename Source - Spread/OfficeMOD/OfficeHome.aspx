<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="OfficeHome.aspx.cs" Inherits="Office" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
    <div>
        <center>
            <asp:Label ID="lbltitle" runat="server" Text="Office" CssClass="lbl"></asp:Label></center>
    </div>
    <div>
        <center>
            <table>
                <tr>
                    <td>
                    </td>
                    <td>
                        <div>
                            <asp:GridView ID="officegrid" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                OnRowDataBound="officegrid_OnRowDataBound" OnDataBound="officegrid_OnDataBound"
                                CssClass="grid-view" BackColor="WhiteSmoke" Width="800px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblsno" runat="server" CssClass="grid_view_lnk_button" Text='<%#Container.DataItemIndex+1 %>'></asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HeaderName">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblheadername" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName") %>' /></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Report ID">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lblreportid" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportId") %>' /></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Menu">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbreportname" runat="server" CssClass="grid_view_lnk_button"
                                                Text='<%#Eval("ReportName") %>' PostBackUrl='<%#Eval("PageName") %>' Font-Underline="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Help" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbHelp" runat="server" CssClass="grid_view_lnk_button" Text="Help"
                                                PostBackUrl='<%#Eval("HelpURL") %>' Font-Underline="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
