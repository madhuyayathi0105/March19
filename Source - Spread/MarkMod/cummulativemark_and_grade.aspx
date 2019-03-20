<%@ Page Title="Cummulative Mark And Grade" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="cummulativemark_and_grade.aspx.cs" Inherits="cummulativemark_and_grade" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
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
    </style>
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
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
        <div style="width: 1016px; height: 26px; padding-right: auto; background-color: Teal;
            text-align: right; position: relative">
            <center>
                <asp:Label ID="lbl" runat="server" Text="Cummulative Mark And Grade" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Azure"></asp:Label>
            </center>
        </div>
        <div style="width: 1016px; height: auto; -webkit-border-radius: 10px; -moz-border-radius: 10px;
            padding: 0px; padding-right: auto; background-color: #219DA5;">
            <center>
                <table style="height: auto; width: 600px; margin-bottom: 0px; line-height: 27px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblschool" runat="server" Width="46px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="School" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddschool" runat="server" Width="213px" Height="25px" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddschool_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblyear" runat="server" Width="70px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="Year" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                                OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Style="margin-left: -30px;" Font-Size="Medium" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblschooltype" runat="server" Width="125px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="School Type" ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddschooltype" runat="server" Width="80px" Height="25px" AutoPostBack="true"
                                OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-left: -30px;" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblstandard" runat="server" Width="37px" Height="20px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-right: 25px;" Font-Size="Medium" Text="Standard"
                                ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddstandard" runat="server" Width="110px" Height="25px" AutoPostBack="true"
                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged"
                                Style="" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblterm" runat="server" Font-Color="white" Width="100px" Height="20px"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="" Font-Size="Medium" Text="Term"
                                ForeColor="#ffffff"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dropterm" runat="server" Width="35px" Height="25px" Font-Bold="True"
                                Font-Names="Book Antiqua" Style="margin-left: -50px;" Font-Size="Medium" AutoPostBack="true"
                                OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Iblsec" runat="server" Style="width: 40px; color: white;" Font-Size="Medium"
                                            Font-Bold="true" Text="Sec"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dropsec" runat="server" Width="44px" Height="25px" Style=""
                                            Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="dropsec_OnSelectedIndexChanged"
                                            Font-Bold="true" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <div runat="server" id="divTest" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTest" runat="server" ForeColor="White" Text="Test" CssClass="commonHeaderFont"
                                                            AssociatedControlID="ddlTest"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div style="position: relative;">
                                                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtTest" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                                    <asp:Panel ID="pnlTest" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                                        Width="230px">
                                                                        <asp:CheckBox ID="chkTest" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                                            AutoPostBack="True" OnCheckedChanged="chkTest_CheckedChanged" />
                                                                        <asp:CheckBoxList ID="cblTest" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="cblTest_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="popExtTest" runat="server" TargetControlID="txtTest"
                                                                        PopupControlID="pnlTest" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                    <asp:DropDownList ID="ddlTest" runat="server" Visible="false" CssClass="commonHeaderFont"
                                                                        OnSelectedIndexChanged="ddlTest_SelectedIndexChanged" AutoPostBack="True" Width="80px">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReportType" runat="server" Text="Report Type" CssClass="commonHeaderFont"
                                            AssociatedControlID="ddlTest" ForeColor="White"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReportType" runat="server" Visible="true" CssClass="commonHeaderFont"
                                            OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="True"
                                            Width="80px">
                                            <asp:ListItem Text="Format 1" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Format 2" Value="1" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Style="background-color: silver; border: 2px solid white;
                                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                            height: 27px; width: 42px;" Text="Go" OnClick="btngo_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnMarkTypeSettings" CssClass="textbox textbox1 commonHeaderFont"
                                            runat="server" OnClick="btnMarkTypeSettings_Click" Text="Settings" Style="width: auto;
                                            height: auto;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </center>
    <asp:Label ID="lblgradeval" Text="" Visible="false" runat="server" CssClass="font14"></asp:Label>
    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
    <asp:Label ID="lblstuderrormsg" runat="server" Text="" Width="302px" Style="" Font-Bold="True"
        Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
    <center>
        <div id="divMainContents" runat="server" visible="false" style="width: auto; height: auto;">
           
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                OnButtonCommand="Fpspread1_Command" Style="margin-top: 10px; margin-bottom: 10px;
                position: relative;">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
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
                            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <%-- Alert Box --%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Subject Mark Type Setting --%>
    <center>
        <div id="divSubjectSetting" runat="server" visible="false" style="height: 150em;
            z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0; left: 0px;">
            <center>
                <div id="divSetting" runat="server" class="table" style="background-color: White;
                    height: auto; width: 68%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 15%; right: 15%; top: 8%; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%; padding: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rblSubjectOrSubjectType" AutoPostBack="true" CssClass="commonHeaderFont"
                                        runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblSubjectOrSubjectType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Subject" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Subject Type" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <FarPoint:FpSpread ID="FpSubjectList" AutoPostBack="False" runat="server" Visible="false"
                                        BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
                                        OnUpdateCommand="FpSubjectList_Command">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnSave" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnSave_Click" Text="Save" runat="server" />
                                        <asp:Button ID="btnExit" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnExit_Click" Text="Exit" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
