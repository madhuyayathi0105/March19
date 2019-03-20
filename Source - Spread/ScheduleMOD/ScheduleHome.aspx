<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ScheduleHome.aspx.cs" Inherits="Schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <div>
        <center>
            <asp:Label ID="Label1" CssClass="lbl" runat="server" Text="Schedule"></asp:Label>
        </center>
    </div>
    <br />
    <div>
        <center>
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="schedulegrid" runat="server" GridLines="Both" AutoGenerateColumns="false"
                            OnRowDataBound="schedulegrid_OnRowDataBound" OnDataBound="schedulegrid_OnDataBound"
                            CssClass="grid-view" BackColor="WhiteSmoke" Width="800px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HeaderName">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblheadername" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName") %>' />
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Report ID">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblreportid" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportId") %>' />
                                        </center>
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
                                        <%--<asp:LinkButton ID="lbHelp" runat="server" Text="Help" PostBackUrl='<%#Eval("HelpURL") %>' Font-Underline="false"></asp:LinkButton>--%>
                                        <a id="lbHelp" runat="server" target="_blank" href='<%#Eval("HelpURL") %>'>Help</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </center>
    </div>
</asp:Content>
