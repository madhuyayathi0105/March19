<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HallTicketFormateaspx.aspx.cs" Inherits="HallTicketFormateaspx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerror').innerHTML = "";
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label4" runat="server" Text="Hall Ticket" CssClass="fontstyleheader"
            Font-Bold="True" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;"></asp:Label>
        <table style="width: 700px; height: 50px; background-color: #0CA6CA; margin: 0px;
            margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td>
                    <asp:Label ID="lblexamyear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblexammonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="100px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="true" Width="90px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Lblbatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="120px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="LblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="Buttongo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="Buttongo_Click" Text="Go" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerror" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        Font-Bold="true" ForeColor="Red" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;"></asp:Label>
    <center>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td>
                    <asp:CheckBox ID="chkheadimage" runat="server" Text="Header Image" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
                <td>
                    <asp:RadioButton ID="rbformate1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 1" AutoPostBack="true" OnCheckedChanged="Radiochanged"
                        GroupName="Hall Ticket" />
                </td>
                <td>
                    <asp:RadioButton ID="rbformate2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 2" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                <td>
                    <asp:RadioButton ID="rbformate3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 3" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                <td>
                    <asp:RadioButton ID="rbformate4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 4" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                 <td>
                    <asp:RadioButton ID="rbformate5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 5" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                <td>
                    <asp:RadioButton ID="rbformat6" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="FORMAT 6" GroupName="Hall Ticket" AutoPostBack="true"
                        OnCheckedChanged="Radiochanged" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckArrear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Include Arrear" />
                </td>
                <td>
                    <asp:CheckBox ID="chkboxvdate" runat="server" Font-Bold="True" Text="Include Session And Date For Practical"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox1" runat="server" Font-Bold="True" Text="Include Session And Date For Theory"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" />
                </td>
                <td>
                    <asp:CheckBox ID="cbpractical" runat="server" Font-Bold="True" Text="Include Practical"
                        Font-Names="Book Antiqua" Font-Size="Medium"  />
                </td>
            </tr>
            <tr>
            <td >
                    <asp:CheckBox ID="cbsignature" runat="server" Font-Bold="True" Text="Include HOD Signature"
                        Font-Names="Book Antiqua" Font-Size="Medium"  />
                </td>
                <td colspan="8" align="left">
                    <asp:Button ID="btnprint" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnprint_Click" Text="Print" />
                </td>
            </tr>
        </table>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td align="center">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="350" Width="680" Visible="false" OnUpdateCommand="FpSpread1_UpdateCommand"
                        HorizontalScrollBarPolicy="Never">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ShowPDFButton="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
