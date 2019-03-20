<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DegreePriority.aspx.cs" Inherits="DegreePriority" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Department Priority</span></div>
            </center>
        </div>
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                Width="250px">
                            </asp:DropDownList>
                            <%--OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"--%>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
                <center>
                    <%-- <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">--%>
                    <table>
                        <tr>
                            <td>
                                <br />
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Style=" height:550px;overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                    background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                    OnCellClick="Cell_Click" OnPreRender="Fpspread1_render" OnButtonCommand="FpSpread1_ButtonCommand">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" >
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <br />
                                <br />
                                <div id="divpriority" runat="server" visible="false">
                                    <asp:Button ID="btnSetPriority" runat="server" Text="Set Priority" CssClass=" textbox textbox1 btn2"
                                        Width="100px" OnClick="btnSetPriority_Click" />
                                    <asp:Button ID="btnResetPriority" runat="server" Text="Reset" CssClass=" textbox textbox1 btn2"
                                        OnClick="btnResetPriority_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="print" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                        CssClass="textbox textbox1" Width="60px" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <%--  </div>--%>
                </center>
            </center>
        </div>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 55em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 30%;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
</asp:Content>
