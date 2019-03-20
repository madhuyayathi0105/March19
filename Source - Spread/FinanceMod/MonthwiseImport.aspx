<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="MonthwiseImport.aspx.cs" Inherits="MonthwiseImport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">MonthWise FeeAllot Import</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 950px; height: 600px;">
                <br />
                <center>
                    <div id="tblhdr" runat="server">
                        <table class="maintablestyle" id="maintbl" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_filename" runat="server" Style="top: 10px; left: 6px;" Text="File Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtfilename" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:FileUpload ID="FileUpload1" Height="25px" runat="server" CssClass="textbox" ForeColor="White" />
                                </td>
                                <td>
                                    Header
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlHeader" runat="server" CssClass="textbox1 ddlheight3" OnSelectedIndexChanged="ddlHeader_Change"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ledger
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLedger" runat="server" CssClass="textbox1 ddlheight4" Width="169px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnimport" runat="server" CssClass="textbox textbox1 btn2" Text="Import"
                                        OnClick="btnimport_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                    <fieldset style="border-radius: 10px; width: 500px;">
                        <legend style="font-size: larger; font-weight: bold">Header Settings</legend>
                        <table class="table">
                            <tr>
                                <td>
                                    <asp:ListBox ID="lb_selecthdr" runat="server" SelectionMode="Multiple" Height="300px"
                                        Width="224px"></asp:ListBox>
                                </td>
                                <td>
                                    <table class="table1">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                    OnClick="btnMvOneRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                    OnClick="btnMvTwoRt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                    OnClick="btnMvOneLt_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                    OnClick="btnMvTwoLt_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ListBox ID="lb_hdr" runat="server" SelectionMode="Multiple" Height="300px" Width="200px">
                                    </asp:ListBox>
                                    <asp:ListBox ID="lb_hdrDataType" runat="server" Visible="false"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <br />
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                                width: 430px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
        </center>
    </body>
    </html>
</asp:Content>
