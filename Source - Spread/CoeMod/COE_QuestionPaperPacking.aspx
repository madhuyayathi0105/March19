<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COE_QuestionPaperPacking.aspx.cs" Inherits="CoeMod_COE_QuestionPaperPacking" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function validation() {

            var ddlYear = document.getElementById("<%=ddlYear.ClientID %>");
            var ddlMonth = document.getElementById("<%=ddlMonth.ClientID %>");
            var ddlDate = document.getElementById("<%=ddlDate.ClientID %>");
            var ddlSession = document.getElementById("<%=ddlSession.ClientID %>");
            var ddlhall = document.getElementById("<%=ddlhall.ClientID %>");
            var btnView1 = document.getElementById("<%=btnView1.ClientID %>");
            if ((ddlYear.value == 0)) {
                alert("Please Select The Year");
                return false;
            }
            if ((ddlMonth.value == 0)) {
                alert("Please Select The Month");
                return false;
            }
            if ((ddlDate.value == 0)) {
                alert("Please Select The Date");
                return false;
            }

            if ((ddlSession.value == 0)) {
                alert("Please Select The Session");
                return false;
            }
            if ((ddlhall.value == 0)) {
                alert("Please Select The Hall");
                return false;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrptmngr" runat="server">
    </asp:ScriptManager>
    <center>
        <span style="color: Green; font-size: x-large;">Question Paper Packing With Seat Number</span>
    </center>
    <br />
    <table style="text-align: left; background-color: #0CA6CA;">
        <tr>
            <td>
                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" Font-Bold="True"
                    ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox1_click"
                    Font-Size="Medium" Text="Format1" Width="120px" />
            </td>
            <td>
                <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Font-Bold="True"
                    ForeColor="Black" Font-Names="Book Antiqua" OnCheckedChanged="CheckBox2_click"
                    Font-Size="Medium" Text="Format2" Width="100px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Month And Year" Width="125px"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <%--<asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" 
               Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>--%>
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="60px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddltype" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbldate" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <%-- <asp:TextBox ID="txtdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="90px" ></asp:TextBox>
                                    <asp:CalendarExtender ID="calfromdate" TargetControlID="txtdate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>--%>
            </td>
            <td>
                <asp:DropDownList ID="ddlDate" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldate_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblsession" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Session"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlSession" runat="server" CssClass="font" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsession_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblhall" runat="server" Text="Hall" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlhall" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlhall_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnView" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnView_Click" />
            </td>
            <td>
                <asp:Button ID="btnView1" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClientClick="return validation()" OnClick="btnView1_Click" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblerror" runat="server" CssClass="font" ForeColor="Red" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td>
                <FarPoint:FpSpread ID="fpspread" runat="server" OnUpdateCommand="fpspread_OnUpdateCommand"
                    BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="550" Width="940"
                    HorizontalScrollBarPolicy="Never" Style="font-family: Book Antiqua; font-size: medium;
                    font-weight: bold;" VerticalScrollBarPolicy="Never">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark" Visible="false">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </td>
            <td>
                <center>
                    <FarPoint:FpSpread ID="fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="600" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False">
                </asp:Label>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblrptname" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" Visible="false"></asp:Label>
    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
        onkeypress="display()"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
    </asp:FilteredTextBoxExtender>
    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
        Font-Size="Medium" Text="Export To Excel" Width="127px" Visible="false" OnClick="btnExcel_Click" />
    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
        Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Click" />
    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    <table style="margin-left: 0px; left: 450px; position: absolute;">
        <tr>
            <td align="center">
                <asp:Button ID="btngenerate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="false" OnClick="btngenerate_click" Text="Print" />
            </td>
        </tr>
    </table>
    </contenttemplate>
    <triggers>
            <asp:PostBackTrigger ControlID="btngenerate" />
        </triggers>
</asp:Content>
