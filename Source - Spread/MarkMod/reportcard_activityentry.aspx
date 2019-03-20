<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="reportcard_activityentry.aspx.cs" Inherits="reportcard_activityentry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblexcelerror').innerHTML = "";
        }   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="25px"
            Style="width: 954px">
            <center>
                <asp:Label ID="Label1" runat="server" Text="Report Card - Activity Entry" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="White"></asp:Label>
            </center>
        </asp:Panel>
        <div style="height: 53px; background-color: LightBlue; border-color: Black; border-style: solid;
            border-width: 1px; width: 954px;">
            <table style="">
                <tr>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 21px; width: 100px;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Height="20px" Width="59px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Font-Bold="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="100px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="74px" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            Width="190px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                            margin-top: -15px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSemYr" runat="server" Text="Sem" Font-Bold="True" Visible="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 33px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Visible="true"
                            OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 21px; width: 44px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px;
                            width: 47px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlactivity" runat="server" Visible="false" Style="width: 146px;
                            background-color: #E1E3E4" AutoPostBack="true" OnSelectedIndexChanged="ddlactivity_SelectedIndexChanged"
                            CssClass="fontcomman">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_att" Text="Attendance / Remarks" AutoPostBack="true" runat="server"
                            OnCheckedChanged="chk_att_CheckedChanged" Width="180px" />
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Names="Book Antiqua"
                            ForeColor="Black" Font-Size="Medium" Style="height: 30px; width: 40px" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="Panel3" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Height="22px"
            Style="width: 954px; margin-bottom: 10px; position: relative;">
        </asp:Panel>
    </center>
    <asp:Label ID="lblErrorMsg" runat="server" Text="" Style="font-family: Book Antiqua;
        color: Red; font-size: medium; font-weight: bold; height: 20px; width: 263px;
        margin-top: 5px; margin-bottom: 10px; position: relative;"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="false" Height="450" ShowHeaderSelection="false" Width="400"
            OnButtonCommand="FpSpread1_OnButtonCommand" Style="margin-top: 5px; margin-bottom: 10px;
            position: relative;">
            <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="false" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <asp:Button ID="btnok" runat="server" Text="OK" Visible="false" Font-Names="Book Antiqua"
            Font-Size="Medium" Width="60px" OnClick="btnok_Click1" />
        <FarPoint:FpSpread ID="fpspread" runat="server" ShowHeaderSelection="false" BorderColor="Black"
            BorderStyle="Solid" CssClass="pos" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded"
            HorizontalScrollBarPolicy="AsNeeded" Style="margin-top: 10px; margin-bottom: 10px;
            position: relative;">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Text="Kindly Enter report name"></asp:Label>
        <asp:Button ID="btnfpspread1save" runat="server" Text="Save" CssClass="fontcomman"
            Width="60px" OnClick="btnfpspread1save_Click1" />
        <asp:Button ID="btnfpspread1delete" runat="server" Text="Delete All" CssClass="fontcomman"
            Width="90px" OnClick="btnfpspread1delete_Click1" />
        <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
            Width="150px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
        <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
            CssClass="stylefp" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <style>
        .fontcomman
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .pos
        {
            position: relative;
        }
    </style>
    <center>
        <table>
            <tr>
                <td style="width: auto;">
                    <asp:FileUpload runat="server" ID="fpmarkexcel" Visible="true" Font-Names="Book Antiqua"
                        Font-Bold="True" Font-Size="Medium" />
                </td>
                <td>
                    <asp:Button ID="btn_import" Text="Import" runat="server" Visible="true" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btn_importex" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
