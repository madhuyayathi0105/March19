<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="PaymentCal.aspx.cs" Inherits="PaymentCal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <title>Payment Calculation</title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function display() {

            document.getElementById('MainContent_lblvalidation1').innerHTML = "";

        }
    </script>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green">PF & ESI Report</span>
        </div>
    </center>
    <div>
        <center>
            <table style="background-color: #0CA6CA; border-color: Black; border-top-style: solid;
                border-left-style: solid; border-bottom-style: solid; border-right-style: solid;
                border-width: 1px; top: 135px; position: absolute; left: 33px;">
                <tr>
                    <td>
                        <asp:Label ID="lblcollege" runat="server" Text="College Name" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="135px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                    AutoPostBack="True" Style="top: 4px; position: absolute; left: 110px;">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Department" runat="server" Text="Department" Font-Bold="True" ForeColor="Black"
                            Style="margin-left: 133px;" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Department" runat="server" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="top: 5px;
                                    left: 338px; position: absolute;" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panel_Department" runat="server" Height="300px" CssClass="multxtpanel"
                                    Style="margin-bottom: 10px;">
                                    <asp:CheckBox ID="chk_Department" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_Department_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklst_Department" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklst_Department_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Department"
                                    PopupControlID="panel_Department" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Designation" Font-Bold="True" ForeColor="Black"
                            Style="margin-left: 137px;" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Designation" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_designation" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="top: 5px;
                                    position: absolute; left: 575px;" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panel_Designation" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chk_Designation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chk_Designation_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklst_Designation" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklst_Designation_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_designation"
                                    PopupControlID="panel_Designation" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblstafftype" runat="server" Text="Staff Type" Font-Bold="True" ForeColor="Black"
                            Style="margin-left: 139px;" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_stafftype" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panelstafftype" runat="server" Height="100px" Width="200px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbstafftype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="cbstafftype_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cblstafftype" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="cblstafftype_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stafftype"
                                    PopupControlID="panelstafftype" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table style="background-color: #0CA6CA; top: 168px; position: absolute; left: 33px;
                border-color: Black; border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                border-right-style: solid; border-width: 1px; margin-left: 1px;">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Staff Category" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Category" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Category" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panel_Category" runat="server" Height="100px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chk_Category" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="chk_Category_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklst_Category" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklst_Category_SelectedIndexChanged" Font-Bold="True"
                                        Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                    PopupControlID="panel_Category" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="True" Font-Bold="true"
                            OnSelectedIndexChanged="ddlyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" Height="24px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" Width="110px" Height="24px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdbtnlst" RepeatDirection="Horizontal" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="rdbtnlst_change">
                            <asp:ListItem Selected="True">PF</asp:ListItem>
                            <asp:ListItem>ESI</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkincamnt" runat="server" Checked="false" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" Text="Incl Zero Amnt" />
                    </td>
                    <td>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="ltd_set" runat="server" Text="LTD Settings" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="ltd_set_click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAllow" runat="server" Text="Allowance" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAllowance" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="133px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="panel1" runat="server" Height="100px" Width="133px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cballowance" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="cballowance_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="cblallowane" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="cblallowane_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtAllowance"
                                    PopupControlID="panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                   
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lbtn_percentageSetting" runat="server" Text="Percentage Settings" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Percentage_set_click"></asp:LinkButton>
                   
                   
                    </td>
                    <td>
                        &nbsp;&nbsp;
                        <asp:Button ID="BtnSearch" runat="server" Text="GO" Font-Bold="True" ForeColor="Black"
                            OnClick="BtnSearch_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <br />
        </center>
    </div>
    <center>
        <asp:Label ID="msg" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-top: 10%"></asp:Label>
        <asp:Label ID="msg1" runat="server" ForeColor="Red" Visible="False" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-top: 10%"></asp:Label>
    </center>
    <br />
    <%-- //delsi--%>
    <div> <br />
        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
            Width="889px">
            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
    </div>
    <br />
    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="890px">
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                </td>
                <td>
                    <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                        Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">Staff Name</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">Department Name</asp:ListItem>
                        <asp:ListItem Value="4" Selected="True">ESI No</asp:ListItem>
                         <asp:ListItem Value="5">Basic Pay</asp:ListItem>
                          <asp:ListItem Value="6">SA</asp:ListItem>
                        <asp:ListItem Value="7">Total Wages</asp:ListItem>
                        <asp:ListItem Value="8">LLP Days</asp:ListItem>
                        <asp:ListItem Value="9">ESI Employee Share</asp:ListItem>
                        <asp:ListItem Value="10">ESI Employeer Share</asp:ListItem>
                        <asp:ListItem Value="11">Total</asp:ListItem>
                        <asp:ListItem Value="12">UAN Number</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
        ExpandedImage="~/images/down.jpeg">
    </asp:CollapsiblePanelExtender>
    <div>
        <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
            Width="889px">
            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Label ID="Labelfilter1" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
            <asp:Image ID="Imagefilter1" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
    </div>
    <br />
    <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="890px">
        <table>
            <tr>
                <td>
                    <asp:CheckBox ID="cb_column1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged1" />
                </td>
                <td>
                    <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                        Visible="false" Width="111px" OnClick="lb_Click1">Remove  All</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" Width="850px"
                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">Staff Name</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">EPF Number</asp:ListItem>
                        <asp:ListItem Value="4">Basic Pay</asp:ListItem>
                        <asp:ListItem Value="5">Grade Pay</asp:ListItem>
                        <asp:ListItem Value="6">DA</asp:ListItem>
                        <asp:ListItem Value="7">Total Wages</asp:ListItem>
                        <asp:ListItem Value="8">LLP Days</asp:ListItem>
                        <asp:ListItem Value="9">MPF Amount</asp:ListItem>
                        <asp:ListItem Value="10">EPF Employee Share</asp:ListItem>
                        <asp:ListItem Value="11" Selected="True">EPF Employeer Share</asp:ListItem>
                        <asp:ListItem Value="12">FPF</asp:ListItem>
                        <asp:ListItem Value="13">LTD Wages</asp:ListItem>
                        <asp:ListItem Value="14">UAN Number </asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:CollapsiblePanelExtender ID="cpecolumnorder1" runat="server" TargetControlID="pcolumnorder1"
        CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
        TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="~/images/right.jpeg"
        ExpandedImage="~/images/down.jpeg">
    </asp:CollapsiblePanelExtender>
    <%--end column order--%>
    <FarPoint:FpSpread ID="FpPayment" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" VerticalScrollBarPolicy="Never" ActiveSheetViewIndex="0">
        <Sheets>
            <FarPoint:SheetView SheetName="Payment Calculation" AutoPostBack="true">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <asp:Label ID="lblvalidation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Style="top: 296px; position: absolute;" Font-Size="Medium" ForeColor="Red" Text=""
        Visible="false"></asp:Label>
    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
    <br />
    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" Text="Report Name" Visible="false"></asp:Label>
    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="false"
        onkeypress="display()"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?,./">
    </asp:FilteredTextBoxExtender>
    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" Visible="false" OnClick="btnxl_Click" />
    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
        Font-Size="Medium" Font-Bold="true" Visible="false" OnClick="btnprintmaster_Clcik" />
    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 200px;
                    width: 400px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalerterr" runat="server" Text="LTD Wages" Style="color: Black;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;<span>=</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtltdwages" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterltd" runat="server" FilterType="Numbers" TargetControlID="txtltdwages">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblage" runat="server" Text="Age" Style="color: Black;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><=</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtage" runat="server" MaxLength="2" CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterage" runat="server" FilterType="Numbers" TargetControlID="txtage">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnsave" CssClass=" textbox1 btn2" OnClick="btnsave_Click" Text="Save"
                            runat="server" />
                        <asp:Button ID="btnexit" CssClass=" textbox1 btn2" OnClick="btnexit_Click" Text="Exit"
                            runat="server" />
                    </center>
                </div>
            </center>
        </div>
    </center>

  <%--delsi2602--%>
  
   <center>
        <div id="divpercentage" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="percentagediv" runat="server" class="table" style="background-color: White; height: 200px;
                    width: 400px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_empshare" runat="server" Text="EPF Employee Share %" Style="color: Black;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>=</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_empshare" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom" ValidChars=" ." TargetControlID="txt_empshare">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_employershare" runat="server" Text="EPF Employeer Share %" Style="color: Black;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>=</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_employershare" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers,Custom" ValidChars=" ." TargetControlID="txt_employershare">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>

                             <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_fpf" runat="server" Text="FPF Percentage %" Style="color: Black;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>=</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fpf" runat="server"  CssClass="textbox txtheight1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers, Custom,Numbers" ValidChars=" ." TargetControlID="txt_fpf">
                                    </asp:FilteredTextBoxExtender>

                                   </td>
                      
                              
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btn_savePercentage" CssClass=" textbox1 btn2" OnClick="btnsavePercentage_Click" Text="Save"
                            runat="server" />
                        <asp:Button ID="btn_exit" CssClass=" textbox1 btn2" OnClick="btn_exit_Click" Text="Exit"
                            runat="server" />
                    </center>
                </div>
            </center>
        </div>
    </center>


    <center>
        <div id="img_div1" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="alertdiv" runat="server" class="table" style="background-color: White; height: 100px;
                    width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: auto; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblsavealert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnerrclose" CssClass="textbox textbox1 btn2" Width="50px" OnClick="btnerrclose_Click"
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
</asp:Content>
