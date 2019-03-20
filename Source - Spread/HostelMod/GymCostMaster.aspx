<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="GymCostMaster.aspx.cs" Inherits="HostelMod_GymCostMaster" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Gym Cost Master</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <table>
                    <tr>
                        <td>
                            <center>
                                <div>
                                    <table class="maintablestyle">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_pattern" Text="Pattern" runat="server" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_pattern" runat="server" CssClass="textbox1  ddlheight2"
                                                    Width="100px" OnSelectedIndexChanged="ddl_pattern_SelectedIndexChanged" AutoPostBack="True">
                                                    <%--<asp:ListItem Value="0">Year</asp:ListItem>--%>
                                                    <asp:ListItem Value="1">Month</asp:ListItem>
                                                   <%-- <asp:ListItem Value="2">Term</asp:ListItem>
                                                    <asp:ListItem Value="3">Semester</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="Btn_go" runat="server" Visible="true" Text="Go" OnClick="btnGo_Click"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                                    font-size: medium; font-weight: bold;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <center>
            <div id="showreport2" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="spreadDet2" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder" >
  <%--                              OnCellClick="spreadDet2_CellClick"
                                OnPreRender="spreadDet2_SelectedIndexChanged"--%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <br />
                    <br />
                </table>
                <br />
                <div>
                    <center>
                        <asp:Button ID="btn_save" Text="Save" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            CssClass="textbox btn2" OnClick="btnsave_Click" />
                    </center>
                </div>
                <tr>
                    <td>
                        <center>
                            <div id="print2" runat="server" visible="false">
                                <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnExcel2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click2" Text="Export To Excel" Width="127px"
                                    Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click2" Height="32px"
                                    Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrolhed2" Visible="false" />
                            </div>
                        </center>
                    </td>
                </tr>
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
</asp:Content>
