<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newreport.aspx.cs" Inherits="newreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>--%>
    <center>
        <span class="fontstyleheader">Admission Report</span>
        <div style="width: 950px; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
            <%--<div style="background-color: #007abc; height: 30px; width: 950px;">margin-top: 190px; 
           
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
                    <tr style="display: none;">
                        <td colspan="2">
                            <asp:RadioButton ID="rdbdepartemntwise" runat="server" Text="Departement Allocation"
                                GroupName="same" />
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="rdbManagementwise" runat="server" Text="Management Allocation"
                                GroupName="same" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
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
            <asp:Label ID="hiddenvalueapplied" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="hiddenvalueadmitted" runat="server" Visible="false"></asp:Label>
        </center>
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
        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            <asp:PostBackTrigger ControlID="ddlreport" />
        </Triggers>
    </asp:UpdatePanel>--%>
    </center>
</asp:Content>
