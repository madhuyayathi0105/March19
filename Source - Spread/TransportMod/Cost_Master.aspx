<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Cost_Master.aspx.cs" Inherits="Cost_Master_1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
        
        .topHandle
        {
            background-color: #97bae6;
        }
        .floatr
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            float: right;
        }
        
        
        .tabl
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: normal;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .tabl3
        {
            empty-cells: hide;
            border-style: solid;
            border-color: Black;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
            text-align: left;
        }
        .tabl5
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            text-align: left;
        }
        .tabl1
        {
            empty-cells: show;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .linkbtn
        {
            border-color: White;
            border-style: none;
            background-color: transparent;
            cursor: pointer;
        }
        .HeaderSelectedCSS
        {
            color: white;
            background-color: #719DDB;
            font-weight: bold;
            font-size: medium; /* font-style:italic;  */
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .style2
        {
            width: 118px;
        }
        .style4
        {
            width: 43px;
        }
        .stylefp
        {
            cursor: pointer;
        }
        .style5
        {
            width: 185px;
        }
        .style55
        {
            width: 25px;
        }
        .style27
        {
            width: 25px;
        }
        .style25
        {
            width: 200px;
        }
        .style251
        {
            width: 125px;
        }
        .style6
        {
            width: 528px;
        }
        .style12
        {
            width: 200px;
        }
        .style22
        {
            width: 122px;
        }
        .style24
        {
            width: 30px;
        }
        
        .font
        {
            font-size: Small;
            font-family: MS Sans Serif;
        }
        .HeaderCSS
        {
            color: white;
            background-color: #719DDB;
            font-size: medium; /* border:solid 1px salmon; */
            font-weight: bold;
        }
        .cpBody
        {
            background-color: #DCE4F9; /*font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width:720;*/
        }
        .accordion
        {
            width: 400px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>

     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Cost Master</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtclg" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Style="height: 20px; width: 230px; font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 290px;"
                                            Height="250px">
                                            <asp:CheckBox ID="cbclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="cbclg_CheckedChanged" Text="SelectAll" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblclg" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cblclg_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtclg"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pattern" runat="server" Text="Pattern" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pattern" runat="server" Enabled="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_pattern_SelectedIndexChanged" Style="width: 90px;">
                                    <asp:ListItem>Monthly</asp:ListItem>
                                    <asp:ListItem>Semester</asp:ListItem>
                                    <asp:ListItem>Yearly</asp:ListItem>
                                    <asp:ListItem>Term</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblrouteid" runat="server" Text="Route" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtrouteid" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                            ReadOnly="true" Style="height: 20px; width: 100px; font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="prouteid" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                            <asp:CheckBox ID="chkrouteid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnCheckedChanged="chkrouteid_CheckedChanged" Text="SelectAll"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklstrouteid" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklstrouteid_SelectedIndexChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtrouteid"
                                            PopupControlID="prouteid" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblstage" runat="server" Text="Stage" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstage" runat="server" Style="height: 20px; width: 160px;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_stage" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: 400px;">
                                            <asp:CheckBox ID="cbstage" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbstage_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" />
                                            <asp:CheckBoxList ID="cblstage" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="cblstage_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstage"
                                            PopupControlID="panel_stage" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>

                            <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                                <asp:Button ID="Btn_go" runat="server" Visible="true" Text="Go" OnClick="Btn_go_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold;" />

                                    </ContentTemplate>
                            </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_add" runat="server" Visible="true" Text="Add" OnClick="btn_add_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                    font-size: medium; font-weight: bold;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <br />
            <center>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_errmsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divspread" runat="server" visible="false" style="width: 730px;">
                                <FarPoint:FpSpread ID="Fp_Route" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Height="420" Width="770px" OnButtonCommand="Fp_Route_OnButtonCommand">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <div id="btndetails" runat="server" visible="false" style="width: 280px;">
                                <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:Button ID="btn_delete" runat="server" Text="Delete" Font-Bold="True" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                                    OnClick="btn_delete_click" Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <center>
            <div id="divadd" runat="server" class="popupstyle" visible="false" style="height: 39em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 15px; left: 0;">
                <asp:ImageButton ID="imgclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 434px;"
                    OnClick="imgclose_Click" />
                <center>
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Add Details</span></div>
                            <br />
                        </center>
                        <center>
                            <table id="tbldet" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblclgadd" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtclgadd" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Style="height: 20px; width: 230px; font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="width: 290px;"
                                                    Height="250px">
                                                    <asp:CheckBox ID="cbclgadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="cbclgadd_CheckedChanged" Text="SelectAll"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cblclgadd" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblclgadd_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtclgadd"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpatnadd" runat="server" Text="Pattern" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpatternadd" runat="server" Enabled="true" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlpatternadd_SelectedIndexChanged" Style="width: 90px;">
                                            <asp:ListItem>Monthly</asp:ListItem>
                                            <asp:ListItem>Semester</asp:ListItem>
                                            <asp:ListItem>Yearly</asp:ListItem>
                                            <asp:ListItem>Term</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrouteadd" runat="server" Text="Route" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtrouteadd" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true" Style="height: 20px; width: 100px; font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                                    Height="250px">
                                                    <asp:CheckBox ID="cbrouteadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="cbrouteadd_CheckedChanged" Text="SelectAll"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cblrouteadd" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblrouteadd_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtrouteadd"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstageadd" runat="server" Text="Stage" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtstageadd" runat="server" Style="height: 20px; width: 160px;"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="Dropdown_Txt_Box"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel4" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                    height: 400px;">
                                                    <asp:CheckBox ID="cbstageadd" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbstageadd_OnCheckedChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                    <asp:CheckBoxList ID="cblstageadd" runat="server" AutoPostBack="true" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="cblstageadd_OnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstageadd"
                                                    PopupControlID="panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_goadd" runat="server" Visible="true" Text="Go" OnClick="btn_goadd_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: Book Antiqua;
                                            font-size: medium; font-weight: bold;" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td colspan="9">
                                        <br />
                                        <div id="div1" runat="server" visible="true" style="width: 820px; height: 430px;
                                            overflow: auto; background-color: White; border-radius: 10px;">
                                            <FarPoint:FpSpread ID="fpfeesadd" runat="server" Visible="true" BorderStyle="Solid"
                                                BorderWidth="0px" Width="820px" Height="430px" Style="overflow: auto; border: 0px solid #999999;
                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                class="spreadborder" OnButtonCommand="fpfeesadd_OnButtonCommand" >
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9" align="center">
                                        <asp:Button ID="btnsaveadd" runat="server" Text="Save" Font-Bold="True" OnClick="btnsaveadd_click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Button ID="btnclearadd" runat="server" Text="Clear" OnClick="btnclearadd_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
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
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <div id="divpnlupdate" runat="server" class="popupstyle" visible="false" style="height: 60em;
            z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: -237px; left: 0;">
            <asp:Panel ID="pnlupdate" runat="server" Visible="false" Style="top: 376px; border-color: Black;
                background-color: lightyellow; border-style: solid; border-width: 0.5px; left: 446px;
                position: absolute; width: 375px; height: 475px;">
                <br />
                <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                    left: 200px">
                    <asp:Label ID="lblmonthwise" runat="server" Visible="true" Text="Monthwise Allotment"
                        Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 6px;"></asp:Label>
                    <asp:Label ID="lblTotalAmount" runat="server" Visible="true" Text="Monthwise Allotment"
                        Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua" Style="margin-left: 20px;"></asp:Label>
                </caption>
                <asp:Panel ID="Panel20" runat="server" Style="top: 48px; border-color: Black; background-color: lightyellow;
                    border-style: solid; border-width: 0.5px; left: 4px; position: absolute; width: 330px;
                    height: 334px;">
                    <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="0.5" autopostback="true" ClientAutoCalculation="true" ShowHeaderSelection="false"
                        OnUpdateCommand="FpSpread3_Command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </asp:Panel>
                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text="" Font-Bold="true"
                    Font-Size="Large" Visible="false" Font-Names="Book Antiqua" Style="top: 433px;
                    left: 10px; position: absolute;"></asp:Label>
                <asp:Button ID="btnok" runat="server" Text="Ok" OnClick="btnok_Click" Style="top: 411px;
                    left: 58px; position: absolute; height: 27px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" />
                <asp:Button ID="btnexi" runat="server" Text="Exit" OnClick="btnexi_Click" Style="top: 411px;
                    left: 155px; position: absolute; height: 27px; width: 88px" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" />
            </asp:Panel>
        </div>


        </ContentTemplate>
     </asp:UpdatePanel>

      <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>

    </body>
</asp:Content>
