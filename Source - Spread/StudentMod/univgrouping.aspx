<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="univgrouping.aspx.cs" Inherits="univgrouping"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
        }
        .ww
        {
            color: White;
        }
        .gvRow td
        {
            background-color: #F0FFFF;
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
        }
        
        .gvAltRow td
        {
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
        .fontcomman
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
    <center>
        <span class="fontstyleheader" style="color: Green;">University Grouping</span>
        <br />
        <br />
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight5" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1 ddlheight3" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbledulevel" runat="server" Text="Edu Level" ForeColor="Black" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddledulevel" runat="server" CssClass="textbox1 ddlheight3"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                        OnSelectedIndexChanged="ddledulevel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Visible="false" Text="Degree" Font-Bold="True"
                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdegree" runat="server" Visible="false" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" Visible="false" CssClass="multxtpanel" Height="300px"
                                Style="font-family: 'Book Antiqua';">
                                <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="font-family: 'Book Antiqua'; font-size: medium; font-weight: bold;">Group
                        Name</span>
                </td>
                <td colspan="4">
                    <asp:Button ID="titleplus" runat="server" Text="+" Width="30px" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="titleplus_OnClick" />
                    <asp:DropDownList ID="ddltitlename" runat="server" AutoPostBack="true" CssClass="textbox1 ddlheight3"
                        Font-Names="Book Antiqua" OnSelectedIndexChanged="ddltitlename_OnSelectedIndexChanged"
                        Font-Bold="true" Font-Size="Medium">
                    </asp:DropDownList>
                    <asp:Button ID="titleminus" runat="server" Text="-" Width="30px" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="titleminus_OnClick" />
                    <asp:Button ID="Button1" runat="server" CssClass="textbox1 btn1" Text="Go" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
        <div id="imgdiv1" runat="server" visible="false" style="height: 82em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: fixed; top: 0;
            left: 0px;">
            <center>
                <table>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Panel ID="pnltitle" runat="server" Visible="false" Style="background-color: #add8e6;
                                border-color: Black; z-index: 99; border-style: solid; border-width: 0.5px; height: 150px;
                                width: 537px;">
                                <center>
                                    <asp:Label ID="lbltitname" runat="server" Text="Enter Title Name" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label></center>
                                <br />
                                <br />
                                <asp:TextBox ID="txttitle" runat="server" Width="505px" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                                <asp:Button ID="btnadd2" runat="server" Text="Add" Style="height: 28px; width: 80px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px"
                                    OnClick="btnadd2_OnClick" />
                                <asp:Button ID="btnexit2" runat="server" Text="Exit" Style="height: 28px; width: 80px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnexit2_OnClick" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <br />
        <asp:Label ID="lblerrormsg" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
        <asp:Label ID="lblerrmsg2" runat="server" Text="" Font-Bold="true" ForeColor="Red"
            Font-Size="Medium"></asp:Label>
        <br />
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div id="movdiv" runat="server">
                        <table>
                            <tr>
                                <td colspan="3">
                                    <center>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <asp:Label ID="lblgrperr" runat="server" Text="" Font-Bold="true" ForeColor="Red"
                                            Font-Size="Medium"></asp:Label>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="Never"
                                        OnButtonCommand="Fpspread1_Command" ShowHeaderSelection="false" CssClass="spreadborder">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                                <td>
                                    <asp:Button ID="Btnmoveright" runat="server" Text=">>" Width="40px" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Btnmoveright_OnClick" />
                                    <br />
                                    <asp:Button ID="Btnmoveleft" runat="server" Text="<<" Width="40px" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Btnmoveleft_OnClick" />
                                </td>
                                <td>
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="Never"
                                        OnButtonCommand="Fpspread2_Command" ShowHeaderSelection="false" CssClass="spreadborder">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <asp:Button ID="btnsave" runat="server" Text="Save" Width="120px" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnsave_OnClick" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                    </center>
                    <table id="final" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
</asp:Content>
