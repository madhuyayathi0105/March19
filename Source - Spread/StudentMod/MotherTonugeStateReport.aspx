<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MotherTonugeStateReport.aspx.cs" Inherits="MotherTonugeStateReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="../Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .textbox
        {
            border: 1px solid #c4c4c4;
            height: 30px;
            width: 50px;
            font-size: 13px;
            text-transform: capitalize;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .textbox1:hover
        {
            outline: none;
            border: 1px solid #7bc1f7;
            box-shadow: 0px 0px 8px #7bc1f7;
            -moz-box-shadow: 0px 0px 8px #7bc1f7;
            -webkit-box-shadow: 0px 0px 8px #7bc1f7;
        }
    </style>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green;">Mother Tongue & State Wise Report</span></center>
        <br />
        </div>
        <center>
            <div>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">College</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">Batch</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">Degree </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">Department </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldepartment" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldepartment_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">Sem</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                                Width="50px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-family: Book Antiqua; font-size: medium;">Report Type</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltype" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                Font-Size="medium" Width="150px" OnSelectedIndexChanged="ddltype_Change">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox textbox1" OnClick="btngo_click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:Label ID="errorlable" runat="server" Visible="false" ForeColor="Red"></asp:Label>
            <br />
            <div>
                <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <div id="rptprint" runat="server">
                <center>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </div>
        </center>
        </div>
    </body>
    </html>
</asp:Content>
