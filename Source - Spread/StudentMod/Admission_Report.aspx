<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Admission_Report.aspx.cs" Inherits="Admission_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        *
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader">Admission Report</span>
        <div style="width: 950px; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
            <%--  <div style="background-color: #007abc; height: 30px; width: 950px;">margin-top: 190px;
            <center>
                <span style="color: White; line-height: 2;">Admission Report</span></center>
            <div style="left: 802px; position: relative; top: -28px;">
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    ForeColor="White" Font-Size="Small" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Back</asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Style="margin-left: 10px;" ForeColor="White" Font-Size="Small" PostBackUrl="~/Default_login.aspx"
                    CausesValidation="False">Home</asp:LinkButton>
                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="lb2_Click" Font-Bold="True"
                    Style="margin-left: 10px;" ForeColor="White" Font-Names="Book Antiqua" Font-Size="Small"
                    CausesValidation="False">Logout</asp:LinkButton>
            </div>
        </div>--%>
            <div style="background-color: #10BADC; height: 75px; width: 950px;">
                <table style="position: relative; top: 10px; left: 10px;">
                    <tr>
                        <td>
                             <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="265px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltype" runat="server" OnSelectedIndexChanged="ddltype_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="110px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Batch
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="65px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Edu Level
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                        Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--<asp:DropDownList ID="ddledu" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="70px">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                        </asp:DropDownList>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Report
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlreport" runat="server" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="130px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <asp:Label ID="Errorlable" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        <br />
        <center>
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
            <br />
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="true"></asp:Label>
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
    </center>
</asp:Content>
