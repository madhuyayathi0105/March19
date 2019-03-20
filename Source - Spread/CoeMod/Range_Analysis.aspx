<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Range_Analysis.aspx.cs" Inherits="Range_Analysis" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            width: 763px;
        }
        .style40
        {
            margin-left: 13px;
        }
        .style41
        {
            width: 189px;
        }
        .style42
        {
            width: 174px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerror').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 <br /><center>
        <asp:Label ID="lblhead" runat="server" Text="Students Mark Range Status" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
</center>
    <br />
    <center>
        <table style="width:700px; height:70px; background-color:#0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Exam Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="True"
                        Width="61px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Exam Month" Width="95px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="True"
                        Width="80px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                        Width="61px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="90px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" Width="40px" Style="margin-left: 1px" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <br />
    <asp:Panel ID="Panel1" runat="server">
        <table>
            <tr>
                <td>
                    <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages"
                        OnCheckedChanged="RadioHeader_CheckedChanged" CssClass="style37" />
                </td>
                <td class="style42">
                    <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page"
                        OnCheckedChanged="Radiowithoutheader_CheckedChanged" CssClass="style38" />
                </td>
                <td>
                    <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CssClass="style39">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                        CssClass="style40" Width="50px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
<center>
    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Height="200" Width="900">
        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
            ButtonShadowColor="ControlDark" ButtonType="PushButton">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread></center>
    <br /><center>
    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" Text="Report Name"></asp:Label>
    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
        InvalidChars="/\">
    </asp:FilteredTextBoxExtender>
    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" OnClick="btnxl_Click" />
    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" /></center>
</asp:Content>

