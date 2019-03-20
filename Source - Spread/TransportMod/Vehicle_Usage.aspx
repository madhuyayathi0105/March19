<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Vehicle_Usage.aspx.cs" Inherits="maintenancepage" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .blinkytext
        {
            text-decoration: blink;
        }
        .textbox
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ftalukp() {
            document.getElementById('<%=typepadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=typeremove.ClientID%>').style.display = 'block';

        }
        function subu() {
            document.getElementById('<%=Btnadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsub.ClientID%>').style.display = 'block';
        }
        function frelig() {
            document.getElementById('<%=btnnewcriteria.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnreovecritreia.ClientID%>').style.display = 'block';
        } 
        

    </script>
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
        .cap
        {
            text-transform: capitalize;
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
            cursor: pointer;
        }
        
        .accordionContent
        {
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
            height: 700px;
        }
    </style>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>


        
    <div style="top: 60px; position: absolute; margin-left: 35px;">
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 995px; height: 21px; margin-bottom: 0px; top: 8px; left: -30px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Vehicle Usage" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Style="color: White; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold; position: absolute; left: 415px;"></asp:Label>
                <%-- &nbsp;&nbsp; &nbsp;
                <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False"
                    Style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold;
                    position: absolute; left: 855px;">Home</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" CausesValidation="False" Style="color: White;
                    font-family: Book Antiqua; font-size: small; font-weight: bold; position: absolute;
                    left: 896px;">Logout</asp:LinkButton>
                <br />
                <br />
                <br />--%>
            </asp:Panel>
        </div>
    </div>
    <br />
    <asp:Accordion ID="Accordion1" CssClass="style252" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        runat="server" Width="985px" BorderColor="White" Style="margin-right: 0px; margin-top: 48px;
        margin-left: 20px;">
        <Panes>
            <asp:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    View
                </Header>
                <Content>
                    <asp:Panel ID="Panel5" runat="server" Style="border-style: solid; border-width: thin;
                        border-color: Black; background: White;">
                        <br />
                        <table style="border-style: solid; border-color: gray; border-width: 1px;">
                            <tr>
                                <td style="width=200px;">
                                    <asp:Label ID="Label3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlselectcollege" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="120px" OnSelectedIndexChanged="ddlselectcollege_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td style="width=200px;">
                                    <asp:Label ID="Label4" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Type"></asp:Label>
                                </td>
                                <td style="width=200px;">
                                    <asp:DropDownList ID="ddl_report" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_report_SelectedIndexChanged">
                                        <asp:ListItem>General</asp:ListItem>
                                        <asp:ListItem>Fuel Consumption</asp:ListItem>
                                        <asp:ListItem>Fuel Report</asp:ListItem>
                                        <asp:ListItem>Maintenance</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width=200px;">
                                    <asp:Label ID="lblvehicleid" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Vehicle ID"></asp:Label>
                                </td>
                                <td style="width=200px;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_vech" runat="server" CssClass="font"
                                                Width="165px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                                <asp:CheckBox ID="vehiclecheck" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="vehiclecheck_CheckedChanged" />
                                                <asp:CheckBoxList ID="vehiclechecklist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="vehiclechecklist_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_vech"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_vech" runat="server" CssClass="font" Width="122px">--Select--</asp:TextBox>
                                            <asp:Panel ID="vehiclpan" runat="server" CssClass="MultipleSelectionDDL" Style="font-family: 'Book Antiqua';
                                                position: absolute;" Font-Bold="True" Font-Names="Book Antiqua" Height="172px"
                                                Width="124px">
                                                <asp:CheckBox ID="vehiclecheck" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="vehiclecheck_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="vehiclechecklist" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="vehiclechecklist_SelectedIndexChanged" Font-Bold="True"
                                                    Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_vech"
                                                PopupControlID="vehiclpan" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width=200px;">
                                    <asp:Label ID="lblrouteid" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="From Date"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="txtfrm_date" runat="server" Height="20px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtfrm_date" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtfrm_date"
                                        FilterType="Custom,Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="Label6" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="To Date"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="txtend_date" runat="server" Height="20px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtend_date" Format="dd/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtend_date"
                                        FilterType="Custom,Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>

                                
                                    <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />

                                    
                                </td>
                            </tr>
                        </table>
                        <FarPoint:FpSpread ID="Fpmaintance" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="800" OnCellClick="Fpmaintance_CellClick"
                            OnPreRender="Fpmaintance_SelectedIndexChanged" OnButtonCommand="Fpmaintance_ButtonCommand">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="Fp_Fuel" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="800">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="FpfuelReport" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="900" OnButtonCommand="FpfuelReport_ButtonClickHandler">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="Fpfueldetails" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="900" OnButtonCommand="FpfuelReport_ButtonClickHandler">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <FarPoint:FpSpread ID="Fpmaintenance" runat="server" Width="850px" Visible="false"
                            OnButtonCommand="Fpmaintenance_ButtonCommand" OnCellClick="Fpmaintenance_CellClick">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <%-- <FarPoint:FpSpread ID="FpSpread1" runat="server" OnCellClick="Fpmaintenance_Cellclick" Width="850px" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"  
                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="false">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>--%>
                        <br />
                        <asp:Button ID="btnback" runat="server" Text="Back" OnClick="btnback_Click" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" />
                        <div style="width=200px; height=22px;">
                            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Size="Medium"></asp:Label>
                        </div>
                    </asp:Panel>
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </Content>
            </asp:AccordionPane>
            <asp:AccordionPane ID="AccordionPane2" runat="server" Style="height: 500px; overflow: visible;
                position: absolute;">
                <Header>
                    <asp:Label ID="lbl_add" runat="server" Text="Add"></asp:Label>
                </Header>
                <Content>
                    <asp:Panel ID="Panel1" runat="server" Style="left: -147px; border-color: Gray; border-style: solid;
                        width: 977px; height: 600px; margin-bottom: 0px; margin-right: 212px; margin-left: -6px;
                        margin-top: -10px;">
                        <asp:Label ID="lblerr" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="" Style="height: 22px; position: absolute;
                            width: 100px; top: 513px; left: 100px;"></asp:Label>
                        <table width="900">
                            <tr>
                                <td align="right" style="width: 400px;">
                                    <asp:RadioButton ID="rdbfuel" runat="server" Text="Fuel" Font-Names="Book Antiqua"
                                        Font-Size="Medium" GroupName="vehicle" AutoPostBack="true" OnCheckedChanged="rdbfuel_CheckedChanged" />
                                </td>
                                <td align="left" style="width: 400px;">
                                    <asp:RadioButton ID="Rdbmailtaince" runat="server" Text="Maintenance" Font-Names="Book Antiqua"
                                        Font-Size="Medium" GroupName="vehicle" OnCheckedChanged="Rdbmailtaince_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="fuelpanel" runat="server">
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblmessagefule" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_type" runat="server" Text="Vehicle Type" Style="text-align: right;"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_vehtype" Width="122px" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddl_Vehid_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_vehid" runat="server" Text="Vehicle ID" Style="font-family: Book Antiqua;
                                            font-size: medium;">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlvecid" runat="server" Height="22px" Width="122px" Style="font-family: Book Antiqua;
                                            font-size: medium;" AutoPostBack="True" OnSelectedIndexChanged="ddlvecid_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrotid" runat="server" Text="Route ID" Style="font-family: Book Antiqua;
                                            font-size: medium;">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_routeid" runat="server" Height="22px" Width="122px" AutoPostBack="True"
                                            Style="font-family: Book Antiqua; font-size: medium;" OnSelectedIndexChanged="ddl_routeid_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldriver" runat="server" Text="Driver Name" Style="font-family: Book Antiqua;
                                            font-size: medium;">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldriver" runat="server" Height="22px" Width="122px" AutoPostBack="True"
                                            Style="font-family: Book Antiqua; font-size: medium;" OnSelectedIndexChanged="ddldriver_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstrplace" runat="server" Text="Start Place" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstartplace" runat="server" Height="22px" Width="122px" AutoPostBack="True"
                                            Style="font-family: Book Antiqua; font-size: medium;" OnSelectedIndexChanged="ddlstartplace_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Start Date" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" Height="17px" Style="font-family: Book Antiqua;
                                            font-size: medium;" Width="120px" AutoPostBack="true" OnTextChanged="txt_date_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" Format="dd/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstarttime" runat="server" Text="Start Time" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlhour" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>00</asp:ListItem>
                                            <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlmin" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>00</asp:ListItem>
                                            <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                            <asp:ListItem>24</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>26</asp:ListItem>
                                            <asp:ListItem>27</asp:ListItem>
                                            <asp:ListItem>28</asp:ListItem>
                                            <asp:ListItem>29</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>31</asp:ListItem>
                                            <asp:ListItem>32</asp:ListItem>
                                            <asp:ListItem>33</asp:ListItem>
                                            <asp:ListItem>34</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>36</asp:ListItem>
                                            <asp:ListItem>37</asp:ListItem>
                                            <asp:ListItem>38</asp:ListItem>
                                            <asp:ListItem>39</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>41</asp:ListItem>
                                            <asp:ListItem>42</asp:ListItem>
                                            <asp:ListItem>43</asp:ListItem>
                                            <asp:ListItem>44</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>46</asp:ListItem>
                                            <asp:ListItem>47</asp:ListItem>
                                            <asp:ListItem>48</asp:ListItem>
                                            <asp:ListItem>49</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>51</asp:ListItem>
                                            <asp:ListItem>52</asp:ListItem>
                                            <asp:ListItem>53</asp:ListItem>
                                            <asp:ListItem>54</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                            <asp:ListItem>56</asp:ListItem>
                                            <asp:ListItem>57</asp:ListItem>
                                            <asp:ListItem>58</asp:ListItem>
                                            <asp:ListItem>59</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlsession" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <%--<asp:Label ID="lblcriteria" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 600px;
                                            top: 11px;" Text="Criteria"></asp:Label>--%>
                                        <asp:Label ID="lblcriteria" runat="server">Remark</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnreovecritreia" runat="server" OnClick="btnreovecritreia_Click"
                                            Font-Names="Book Antiqua" Font-Size="Small" Height="22px" Style="height: 23px;
                                            display: none; left: 850px; position: absolute; top: 352px; width: 27px;" Text="-" />
                                        <asp:DropDownList ID="ddl_itemtype" runat="server" Height="22px" Width="100px" Font-Bold="true"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnnewcriteria" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="22px" Style="height: 23px; display: none; left: 990px; position: absolute;
                                            top: 352px; width: 27px;" OnClick="btnnewcriteria_Click" Text="+" />
                                    </td>
                                    <%-- added by raghul--%>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldescplace" runat="server" Text="Designation Place" Style="font-family: Book Antiqua;
                                            font-size: medium;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldescplace" runat="server" Height="22px" Width="122px" AutoPostBack="True"
                                            Style="font-family: Book Antiqua; font-size: medium;" OnSelectedIndexChanged="ddldescplace_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblarrivaldate" runat="server" Text="Arrival Date" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtarivaldate" runat="server" Height="17px" Style="font-family: Book Antiqua;
                                            font-size: medium;" Width="122px" AutoPostBack="true" OnTextChanged="txtarivaldate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txtarivaldate" Format="dd/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblarivaltime" runat="server" Text="Arrival Time" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlendhour" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>00</asp:ListItem>
                                            <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlendmin" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>00</asp:ListItem>
                                            <asp:ListItem>01</asp:ListItem>
                                            <asp:ListItem>02</asp:ListItem>
                                            <asp:ListItem>03</asp:ListItem>
                                            <asp:ListItem>04</asp:ListItem>
                                            <asp:ListItem>05</asp:ListItem>
                                            <asp:ListItem>06</asp:ListItem>
                                            <asp:ListItem>07</asp:ListItem>
                                            <asp:ListItem>08</asp:ListItem>
                                            <asp:ListItem>09</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>11</asp:ListItem>
                                            <asp:ListItem>12</asp:ListItem>
                                            <asp:ListItem>13</asp:ListItem>
                                            <asp:ListItem>14</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>16</asp:ListItem>
                                            <asp:ListItem>17</asp:ListItem>
                                            <asp:ListItem>18</asp:ListItem>
                                            <asp:ListItem>19</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>21</asp:ListItem>
                                            <asp:ListItem>22</asp:ListItem>
                                            <asp:ListItem>23</asp:ListItem>
                                            <asp:ListItem>24</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>26</asp:ListItem>
                                            <asp:ListItem>27</asp:ListItem>
                                            <asp:ListItem>28</asp:ListItem>
                                            <asp:ListItem>29</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>31</asp:ListItem>
                                            <asp:ListItem>32</asp:ListItem>
                                            <asp:ListItem>33</asp:ListItem>
                                            <asp:ListItem>34</asp:ListItem>
                                            <asp:ListItem>35</asp:ListItem>
                                            <asp:ListItem>36</asp:ListItem>
                                            <asp:ListItem>37</asp:ListItem>
                                            <asp:ListItem>38</asp:ListItem>
                                            <asp:ListItem>39</asp:ListItem>
                                            <asp:ListItem>40</asp:ListItem>
                                            <asp:ListItem>41</asp:ListItem>
                                            <asp:ListItem>42</asp:ListItem>
                                            <asp:ListItem>43</asp:ListItem>
                                            <asp:ListItem>44</asp:ListItem>
                                            <asp:ListItem>45</asp:ListItem>
                                            <asp:ListItem>46</asp:ListItem>
                                            <asp:ListItem>47</asp:ListItem>
                                            <asp:ListItem>48</asp:ListItem>
                                            <asp:ListItem>49</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>51</asp:ListItem>
                                            <asp:ListItem>52</asp:ListItem>
                                            <asp:ListItem>53</asp:ListItem>
                                            <asp:ListItem>54</asp:ListItem>
                                            <asp:ListItem>55</asp:ListItem>
                                            <asp:ListItem>56</asp:ListItem>
                                            <asp:ListItem>57</asp:ListItem>
                                            <asp:ListItem>58</asp:ListItem>
                                            <asp:ListItem>59</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlenssession" runat="server" OnSelectedIndexChanged="ddlhour_SelectedIndexChanged"
                                            AutoPostBack="true" Font-Size="Medium" Font-Names="Book Antiqua">
                                            <asp:ListItem>AM</asp:ListItem>
                                            <asp:ListItem>PM</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="lblrm" runat="server" Text="Remarks" Style="font-family: Book Antiqua;
                                            font-size: medium;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        <asp:TextBox ID="txtrm" runat="server" Height="20px" Style="font-family: Book Antiqua;
                                            border: 1px solid black; font-size: medium;" Width="420px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Fuel(Lt)" Style="font-family: Book Antiqua;
                                            font-size: medium;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_fuel" runat="server" Height="17px" Width="122px" Style="font-family: Book Antiqua;
                                            font-size: medium;" AutoPostBack="true" CssClass="textbox" OnTextChanged="txt_fuel_TextChanged"></asp:TextBox><span
                                                style="color: Red;">*</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txt_fuel"
                                            FilterType="Numbers,Custom"  ValidChars="." runat="server">   <%-- rajasekar --%>
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblfuelltrs" runat="server" Text="Fuel/Lt (Rs)" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfuelltrs" runat="server" Height="17px" Style="font-family: Book Antiqua;
                                            font-size: medium;" Width="120px" CssClass="textbox" OnTextChanged="txtfuelltrs_TextChanged"
                                            AutoPostBack="true"></asp:TextBox><span style="color: Red;">*</span>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" TargetControlID="txtfuelltrs"
                                            FilterType="Numbers,Custom" ValidChars="." runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Lblfuelamount" runat="server" Text="Fuel Amount" Style="font-family: Book Antiqua;
                                            font-size: medium; text-align: right;"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txtfuelamount" runat="server" Height="17px" Width="120px" Style="font-family: Book Antiqua;
                                            font-size: medium;" CssClass="textbox" Enabled="true"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" TargetControlID="Txtfuelamount"
                                            FilterType="Numbers,Custom" ValidChars="." runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="900">
                                <tr>
                                    <td align="right" style="width: 100px;">
                                        <asp:Label ID="lbl_amt" runat="server" Text="Opening KM" Style="font-family: Book Antiqua;
                                            font-size: medium;"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 100px;">
                                        <asp:TextBox ID="txt_openkm" runat="server" Height="17px" Enabled="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txt_openkm"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td align="right" style="width: 100px;">
                                        <asp:Label ID="Label5" runat="server" Text="Closing KM" Style="font-family: Book Antiqua;
                                            font-size: medium;"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 100px;">
                                        <asp:TextBox ID="txt_closekm" runat="server" AutoPostBack="true" OnTextChanged="txt_closekm_TextChanged"
                                            Height="17px"></asp:TextBox>
                                        <asp:Label ID="Label7" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" TargetControlID="txt_closekm"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td align="right" style="width: 100px;">
                                        <asp:Label ID="lbl_expensekm" runat="server" Text="Travelling KM" Style="font-family: Book Antiqua;
                                            font-size: medium;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 100px;">
                                        <asp:TextBox ID="txt_expensekm" runat="server" Enabled="false" Height="17px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" TargetControlID="txt_expensekm"
                                            FilterType="Numbers" runat="server">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="900">
                                <tr>
                                    <td align="right" style="width: 405px;">
                                        <asp:Label ID="lbl_typeexpenses" runat="server" Text="Purpose" Style="font-family: Book Antiqua;
                                            font-size: medium;"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 20px;">
                                        <asp:Button ID="typepadd" runat="server" Text="+" Style="font-family: Book Antiqua;
                                            font-size: small; height: 22px; display: none;" Font-Names="Book Antiqua" Visible="true"
                                            Font-Size="Small" Height="22px" OnClick="typeadd_Click" />
                                    </td>
                                    <td align="left" style="width: 120px;">
                                        <asp:DropDownList ID="ddlexpensestype" runat="server" Height="22px" Width="122px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlexpensestype_SelectedIndexChanged"
                                            Style="height: 22px; width: 122px;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="typeremove" runat="server" Text="-" Style="font-family: Book Antiqua;
                                            font-size: small; height: 22px; display: none;" Font-Names="Book Antiqua" Visible="true"
                                            Font-Size="Small" Height="22px" OnClick="typeremove_Click" />
                                    </td>
                                    <td align="right" style="width: 70px;">
                                        <asp:Label ID="lblerrorexpensetype" runat="server" ForeColor="Red" Text="" Font-Names="Book Antiqua"
                                            Font-Size="5pt" Visible="false"></asp:Label>
                                    </td>
                                    <%--<td align="right" style="width: 70px;">
                                            <asp:Label ID="Lblamount" runat="server" Text="Amount" Style="font-family: Book Antiqua;
                                                font-size: medium;"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 100px;">
                                            <asp:TextBox ID="TextBox1" runat="server" Height="17px"></asp:TextBox>
                                        </td>--%>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <br />
                            <br />
                            <table width="900">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_cancel" runat="server" Text="New" OnClick="btn_cancel_click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: medium; height: 25px; width: 60px;" />
                                        <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_click" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="font-family: MS Sans Serif; font-size: medium; height: 25px;
                                            width: 60px;" />
                                        <asp:Button ID="Btnupdate" runat="server" Text="update" OnClick="Btnupdate_click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: medium; height: 25px; width: 60px;" />
                                        <asp:Button ID="btn_delete1" runat="server" Text="Delete" Enabled="false" OnClick="btn_delete_click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: medium; height: 25px; width: 60px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <asp:Label ID="lbl_Validation" runat="server" Font-Bold="true" ForeColor="Red" Text=""
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Panel ID="maintainpanel" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="Rdbservice" runat="server" Text="Service" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Font-Bold="true" GroupName="service" AutoPostBack="true" OnCheckedChanged="Rdbservice_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="Rdbitem" runat="server" Text="Item" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Font-Bold="true" GroupName="service" AutoPostBack="true" OnCheckedChanged="Rdbitem_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldate" runat="server" Text="Date" Style="font-family: Book Antiqua;
                                                        font-size: medium; text-align: right;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtdate" runat="server" Height="17px" OnTextChanged="Txtdate_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="Txtdate" Format="dd/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="Txtdate"
                                                        FilterType="Custom,Numbers" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblvehicletype" runat="server" Text="Vehicle Type" Style="text-align: right;"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlvechicletype" Width="120" runat="server" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlvechicletype_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblvehicleid1" runat="server" Text="Vehicle ID" Style="font-family: Book Antiqua;
                                                        font-size: medium;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="Ddlvehicleid" runat="server" Height="22px" Width="122px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="Ddlvehicleid_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lblregno" runat="server" Text="Register No" Style="font-family: Book Antiqua;
                                                        font-size: medium;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlregno" runat="server" Height="22px" Width="122px" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lblopeingkm" runat="server" Text="Opening KM" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtopeingkm" runat="server" Height="17px" Enabled="false"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" TargetControlID="Txtopeingkm"
                                                        FilterType="Numbers" runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblclosingkm" runat="server" Text="Closing KM" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtclosingkm" runat="server" AutoPostBack="true" OnTextChanged="txtclosingkm_TextChanged"
                                                        Height="17px"></asp:TextBox>
                                                    <asp:Label ID="Label9" runat="server" Text="*" Style="color: Red;"></asp:Label>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" TargetControlID="txtclosingkm"
                                                        FilterType="Numbers" runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltravellingkm" runat="server" Text="Travelling KM" Style="font-family: Book Antiqua;
                                                        font-size: medium;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttravellingkm" runat="server" Enabled="false" Height="17px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" TargetControlID="txttravellingkm"
                                                        FilterType="Numbers" runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblpurpose" runat="server" Text="Purpose" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Btnadd" runat="server" Text="+" Font-Names="Book Antiqua" Font-Size="Small"
                                                        Height="22px" Style="height: 23px; display: none; left: 95px; position: absolute;
                                                        top: 545px; width: 27px;" OnClick="Btnadd_Click" />
                                                    <asp:DropDownList ID="ddlpurpose" runat="server" Height="22px" Width="124px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btnsub" runat="server" Text="-" Style="left: 250px; display: none;
                                                        position: absolute; top: 545px; font-family: Book Antiqua; font-size: small;
                                                        height: 22px;" Font-Names="Book Antiqua" Font-Size="Small" Height="22px" OnClick="btnsub_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblcompanyname" runat="server" Text="Company Name" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtcompanyname" runat="server" Height="17px" Width="117px" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btncmpy" runat="server" Text="?" Style="left: 585px; position: absolute;
                                                        top: 270px; font-family: Book Antiqua; font-size: small; height: 22px;" Font-Names="Book Antiqua"
                                                        Font-Size="medium" Font-Bold="true" Height="22px" OnClick="btncmpy_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbillno" runat="server" Text="Bill No" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtbillno" runat="server" Height="17px" Width="117px" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbilldate" runat="server" Text="Bill Date" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtbilldate" runat="server" Height="17px" Width="117px" OnTextChanged="Txtbilldate_TextChanged"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="Txtbilldate" Format="dd/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="Txtbilldate"
                                                        FilterType="Custom,Numbers" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblremarks" runat="server" Text="Remarks" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtremarks" runat="server" Height="17px" Width="117px" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbldescription" runat="server" Text="Description" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtdescription" runat="server" TextMode="MultiLine" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:TextBox>
                                                    <asp:Label ID="lbldescribtionvalid" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblamount" runat="server" Text="Amount" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtamount" runat="server" Height="17px" CssClass="textbox" OnTextChanged="txtamount_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" TargetControlID="txtamount"
                                                        FilterType="Numbers" runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Label ID="lblamountvalid" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblvat" runat="server" Text="Tax (%)" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtvat" runat="server" Height="17px" CssClass="textbox" OnTextChanged="Txtvat_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" TargetControlID="Txtvat"
                                                        FilterType="Custom" ValidChars="123456789." runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Lblfinalvat" runat="server" Text="Total Cost" Style="font-family: Book Antiqua;
                                                        font-size: medium;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtfinalvat" runat="server" Height="17px" CssClass="textbox" Enabled="false"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" TargetControlID="Txtfinalvat"
                                                        FilterType="Numbers,Custom" ValidChars="." runat="server">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnaddrow" runat="server" Text="Add Row" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnaddrow_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnremoverow" runat="server" Text="Remove" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnremoverow_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <FarPoint:FpSpread ID="Fpvehicle" runat="server" Width="650px" OnCellClick="fpcell"
                                            ClientAutoCalculation="true" OnUpdateCommand="Fpvehiclecmd" OnPreRender="fpvehirender">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="false">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="Update1" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbltotalamount" runat="server" Text="Total Amount" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txttotalamount" AutoPostBack="true" runat="server" Height="23px"
                                                    Width="117px" Style="font-family: Book Antiqua; font-size: medium; text-align: right;"
                                                    OnTextChanged="Txttotalamount_TextChanged"></asp:TextBox>
                                                <asp:Button ID="btntotalamount" OnClick="btntolclick" runat="server" Text="" Height="17px"
                                                    Width="121px" Style="height: 17px; left: 559px; opacity: 0; position: absolute;
                                                    top: 727px; width: 103px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbltax" runat="server" Text="Tax (%)" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txttax" runat="server" Height="17px" Width="117px" Style="font-family: Book Antiqua;
                                                    font-size: medium; text-align: right;" OnTextChanged="Txttax_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblfinalcost" runat="server" Text="Total Cost" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Txtfinalcost" runat="server" Height="17px" Width="117px" Style="font-family: Book Antiqua;
                                                    font-size: medium; text-align: right;" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table width="900">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblerr1" runat="server" Text="" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="Btnnew" runat="server" Text="New" OnClick="btn_cancel_click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: small; height: 25px; width: 60px; top: 770px; position: absolute;
                                            left: 708px;" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Btnsave" runat="server" Text="Save" Font-Bold="True" OnClick="Btnsave_click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: small; height: 25px; width: 60px; top: 770px; position: absolute;
                                            left: 774px;" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Btndelete" runat="server" Text="Delete" Enabled="false" Font-Bold="True"
                                            OnClick="btn_delete_click" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: MS Sans Serif;
                                            font-size: small; height: 25px; width: 60px; top: 770px; position: absolute;
                                            left: 840px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Lblwarning" runat="server" ForeColor="Red" Text="" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--<div style="position: absolute; height: 80px; width: 235px; top: 467px; left: 451px;">--%>
                        <asp:Panel ID="Paneladd" runat="server" Visible="false" Style="width: 200px; height: 80px;
                            position: absolute; margin-left: 392px; margin-top: -130px;" BorderStyle="Solid"
                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="MS Sans Serif" Font-Size="Small">
                            <div>
                                <caption runat="server" id="newcaption" style="height: 10px; top: 10px; text-align: center;
                                    font-variant: Small-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txt_addexpense" Height="14px" runat="server" Style="height: 14px;
                                    position: absolute; top: 24px; left: 38px;"></asp:TextBox>
                                <br />
                                <asp:Button ID="addnew" Width="50px" runat="server" Text="Add" OnClick="addnew_Click"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif;
                                    font-size: small; height: 25px; width: 50px; position: absolute; top: 50px; left: 35px;" />
                                &nbsp;
                                <asp:Button ID="exitnew" Width="50px" runat="server" Text="Exit" OnClick="exitnew_Click"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif;
                                    font-size: small; height: 25px; width: 50px; top: 50px; position: absolute; left: 108px;" />
                                <%-- </div>--%>
                        </asp:Panel>
                        </div>
                        <asp:Panel ID="paneladdremove" runat="server" Visible="false" Style="width: 200px;
                            height: 80px; left: 79px; position: absolute; top: 528px" BorderStyle="Solid"
                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="MS Sans Serif" Font-Size="Small">
                            <div>
                                <caption runat="server" id="cappurpose" style="height: 10px; top: 10px; text-align: center;
                                    font-variant: Small-caps">
                                </caption>
                                <br />
                                <asp:TextBox ID="txtpurpose" Height="14px" runat="server" Style="height: 14px; position: absolute;
                                    top: 20px; left: 38px;"></asp:TextBox>
                                <br />
                                <br />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:Button ID="btnaddpurpose" Width="50px" runat="server" Text="Add" OnClick="btnaddpurpose_Click"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif;
                                    font-size: small; height: 25px; width: 50px;" />
                                <asp:Button ID="btnminuspurpose" Width="50px" runat="server" Text="Exit" OnClick="btnminuspurpose_Click"
                                    Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif;
                                    font-size: small; height: 25px; width: 50px;" />
                            </div>
                        </asp:Panel>
                        </div>
                    </asp:Panel>
                </Content>
            </asp:AccordionPane>
        </Panes>
    </asp:Accordion>
    <%-- </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btncmpy" /></Triggers>
    </asp:UpdatePanel>--%>
    <asp:Panel ID="Panel_Intimation" runat="server" BorderColor="Black" BackColor="AliceBlue"
        BorderWidth="2px">
        <div style="text-align: right;">
            <asp:Button ID="Btn_Close" runat="server" Text="Close" Width="100px" OnClick="Btn_Close_Click" />
        </div>
        <div>
            <FarPoint:FpSpread ID="Fp_Intimation_Driver" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Height="200" Width="800" OnUpdateCommand="Fp_Intimation_Driver_UpdateCommand">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div style="height: 20px;">
        </div>
        <div>
            <FarPoint:FpSpread ID="Fp_Intimation_Vehicle" runat="server" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Height="200" Width="800">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hf_remove" runat="server" />
    <asp:ModalPopupExtender ID="Popup_Intimation" Drag="True" CancelControlID="Btn_Close"
        TargetControlID="hfdirect" PopupControlID="Panel_Intimation" runat="server" BackgroundCssClass="ModalPopupBG"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:ModalPopupExtender ID="mpemsgboxsave" runat="server" TargetControlID="hf_remove"
        PopupControlID="pnlmsgboxsave">
    </asp:ModalPopupExtender>
    <asp:HiddenField ID="hfdirect" runat="server" />
    <asp:Panel ID="pnlmsgboxsave" runat="server" CssClass="modalPopup" Style="display: none;
        height: 500; width: 500;" DefaultButton="btnOk">
        <table width="500">
            <tr class="topHandle">
                <td colspan="2" align="left" runat="server" id="tdCaption">
                    <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 60px" valign="middle" align="center">
                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                </td>
                <td valign="middle" align="left">
                    <asp:UpdatePanel ID="udp15" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" Text="Do you want Remove" runat="server" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnOk" runat="server" Text="Yes" OnClick="btnOk_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                    <asp:Button ID="btnCancel" runat="server" Text="No" OnClick="btnCancel_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Style="width: 200px; height: 80px; left: 424px;
        position: absolute; top: 435px" BorderStyle="Solid" BorderWidth="1px" Visible="false"
        BackColor="#CCCCCC" Font-Names="MS Sans Serif" Font-Size="Small">
        <%--<caption runat="server" id="Caption1" style="height: 10px; top: 10px; text-align: center;
            font-variant: Small-caps">
        </caption>--%>
        <br />
        <asp:TextBox ID="TextBox1" Height="14px" runat="server" Style="height: 14px; position: absolute;
            top: 20px; left: 38px;"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" Width="50px" runat="server" Text="Add" Font-Names="MS Sans Serif"
            Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif; font-size: small;
            height: 25px; width: 50px; position: absolute; top: 50px; left: 35px;" OnClick="Buttonadd1_Click" />
        &nbsp;
        <asp:Button ID="Button2" Width="50px" runat="server" Text="Exit" OnClick="Button2_Click"
            Font-Names="MS Sans Serif" Font-Size="Small" Height="25px" Style="font-family: MS Sans Serif;
            font-size: small; height: 25px; width: 50px; top: 50px; position: absolute; left: 108px;" />
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfdelete" />
    <asp:ModalPopupExtender ID="mpemsgboxdelete" Drag="True" PopupDragHandleControlID="PopupHeaderrstud2"
        TargetControlID="hfdelete" PopupControlID="Panelfee" runat="server" BackgroundCssClass="ModalPopupBG"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="panel6" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
        BorderWidth="2px" Style="left: 140px; top: 177px; position: absolute;" Height="439px"
        Width="593px">
        <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: Book Antiqua;
            font-size: medium; font-weight: bold">
            <br />
            <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                left: 200px">
                <asp:Label ID="Label17" runat="server" Text="Select Items" Font-Bold="true" Font-Size="Large"
                    Font-Names="Book Antiqua"></asp:Label>
            </caption>
            <table width="500">
                <tr>
                    <td align="center">
                        <FarPoint:FpSpread ID="fsitem" runat="server" ActiveSheetViewIndex="0" Height="300"
                            Width="500" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="true">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#666699">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
            </table>
            <div>
                <p>
                    <asp:Label ID="msg2" Text="No of Items:" Style="top: 385px; left: 10px; position: absolute;"
                        runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="mmmg" Text="" Style="top: 383px; left: 10px; position: absolute;"
                        runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                </p>
                <fieldset style="width: 160px; height: 23px; position: absolute; top: 347px; left: 306px;
                    font-family: Book Antiqua;">
                    <asp:Button runat="server" ID="btnstaffadd" Text="Ok" OnClick="btnstaffadd_Click"
                        Width="75px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    <asp:Button runat="server" ID="btnexitpop" Text="Exit" Width="75px" OnClick="btnexitpop_Click"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                </fieldset>
    </asp:Panel>
    <center>
        <div id="popupnewitem" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="popupnewitemNEW" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 50%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:TextBox ID="txtADDnewItem" runat="server" Width="100px"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnnewItemSave" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        OnClick="btnnewItemSave_Click" Text="Save" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="btnnewItemExit" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        OnClick="btnnewItemExit_Click" Text="Close" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="plusdiv" runat="server" visible="false" style="height: 56em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                    height: auto; width: 367px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_header1" runat="server" Style="color: Green; font: bold;" Text="REMARKS"></asp:Label>
                            </td>
                            <br />
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txt_addgroup" runat="server" Height="25px" Style="text-transform: capitalize;"
                                    Width="232px"></asp:TextBox>
                                <%--onfocus=" return display(this)"--%>
                            </td>
                        </tr>
                        <br />
                        <tr>
                            <td align="center">
                                <br />
                                <asp:Button ID="btn_addgroup1" runat="server" Visible="true" CssClass="textbox btn1"
                                    Text="Add" OnClientClick="return checkadd()" OnClick="btn_addgroup_Click" />
                                <asp:Button ID="btn_exitgroup1" runat="server" Visible="true" CssClass="textbox btn1"
                                    Text="Exit" OnClick="btn_exitaddgroup_Click" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <%--   <center>
        <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
           
            <center>
                <div id="Div2" runat="server" visible="true" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium" Text="Remark"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_addgroup_Click" /><%--OnClick="btn_addgroup_Click"
                                        <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click"  /><%--OnClick="btn_exitaddgroup_Click"
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>--%>
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                                        <asp:Button ID="btnerrclose"  Style="height: 28px; width: 65px;"
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



     

      
</asp:Content>
