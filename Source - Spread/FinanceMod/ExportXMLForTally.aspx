<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExportXMLForTally.aspx.cs" Inherits="ExportXMLForTally" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Export To XML</title>
    <link rel="SHORTCUT ICON" href="college/Left_Logo.jpeg">
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="smXmlExport" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Export To XML</span></div>
            </center>
            <center>
                <div class="maindivstyle" style="height: 250px; width: 700px; overflow: auto;">
                    <br />
                    <table>
                        <tr>
                            <td style="width: 350px;">
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight5"
                                    OnSelectedIndexChanged="ddl_college_Changed">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 300px;">
                                <b>Account</b>
                                <asp:DropDownList ID="ddl_AccountDetail" runat="server" CssClass="textbox  ddlheight4">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <b>From Date</b>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                    Width="65px" AutoPostBack="true"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <b>To Date</b>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                    Width="65px" AutoPostBack="true"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <br />
                                <b>Format</b>
                                <asp:DropDownList ID="ddlExpFormat" runat="server" CssClass="textbox  ddlheight4"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlExpFormat_IndexChange">
                                    <asp:ListItem Selected="True">Format I</asp:ListItem>
                                    <asp:ListItem>Format II</asp:ListItem>
                                    <asp:ListItem>Format III</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trfnl" runat="server" visible="false">
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                                <%--</td>
                            <td>--%>
                                <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox textbox1 ddlheight2"
                                    Style="width: 130px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btnRcptExport" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Export Counter Receipt" OnClick="btnRcptExport_Click" Width="200px" Font-Bold="true" />
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnRcptUndo" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Undo Exported Counter Receipt" OnClick="btnRcptUndo_Click" Width="210px"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btnChlnExport" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Export Challan Receipt" OnClick="btnChlnExport_Click" Width="200px" Font-Bold="true" />
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnChlnUndo" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Undo Exported Challan Receipt" OnClick="btnChlnUndo_Click" Width="210px"
                                    Font-Bold="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="contentDiv" runat="server" visible="false">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <center>
                <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
    </body>
    </html>
</asp:Content>
