<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="hallticket.aspx.cs" Inherits="Hallticket" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <style type="text/css">
        .stylefp
        {
            cursor: pointer;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label1" runat="server" Text="Hall Ticket" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green"></asp:Label></center>
    <br />
    <center>
        <table style="width: 1000px; height: 70px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        Width="71px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btngo_Click" />
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="CheckRegular" runat="server" Text="Regular" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" Width="80px" />
                    <%--</td>
                <td>--%>
                    <asp:CheckBox ID="CheckArrear" runat="server" Text="Arrear" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" Width="80px" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:RadioButton ID="RadioButton1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                        Text="Format 1" AutoPostBack="True" Width="90px" />
                    <%--</td>
                <td>--%>
                    <asp:RadioButton ID="RadioButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton2_CheckedChanged"
                        Text="Format 2" AutoPostBack="True" Width="90px" />
                    <%--</td>
                <td>--%>
                    <asp:RadioButton ID="RadioButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                        Text="Format 3" AutoPostBack="True" Width="90px" />
                    <%--</td>
                <td>--%>
                    <asp:RadioButton ID="RadioButton4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                        Text="Format 4" AutoPostBack="True" Width="90px" />
                    <%--</td>
                <td>--%>
                    <asp:RadioButton ID="RadioButton5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                        Text="Format 5" AutoPostBack="True" Width="90px" />
                    <%--</td>
                <td>--%>
                    <asp:RadioButton ID="rbFormat6" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                        Text="Format 6" AutoPostBack="True" Width="90px" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <table style="width: 1000px; height: 70px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:CheckBox ID="chkpassout" runat="server" Font-Bold="True" Text="Include Passed Out"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Visible="true"
                        OnCheckedChanged="chkpassout_CheckedChanged" Width="180px" />
                </td>
                <td>
                    <asp:CheckBox ID="Checkeligible" runat="server" Font-Bold="True" Text="Eligible Attendance"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="180px" />
                </td>
                <td>
                    <asp:CheckBox ID="chkboxvdate" runat="server" Font-Bold="True" Text="Include Session And Date For Practical"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="210px" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox1" runat="server" Font-Bold="True" Text="Include Session And Date For Theory"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="210px" />
                </td>
                <td>
                    <asp:Label ID="selectMonth" runat="server" Text="Select Month" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="selectyear" runat="server" Text="Select Year" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chksupplym" runat="server" Font-Bold="True" Text="Supplementary Report"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Width="190px" />
                </td>
                <td>
                    <asp:CheckBox ID="chk_sesdat" runat="server" Font-Bold="True" Text="Include Session And Date Header"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" Checked="true"
                        Width="199px" Visible="false" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Label ID="lblno" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="#FF3300" Visible="False"></asp:Label>
        <asp:Label ID="errmsg" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Small" ForeColor="Red" Visible="False"></asp:Label>
    </center>
    <center>
        <FarPoint:FpSpread ID="HAllSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
            Visible="false" BorderWidth="1px" Height="330" Width="580" OnCellClick="HAllSpread_CellClick"
            OnPreRender="HAllSpread_SelectedIndexChanged" CssClass="stylefp" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
        <asp:Button ID="printbtn" runat="server" Text="Print Hall Ticket" Font-Bold="True"
            Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="printbtn_Click" />
        <br />
        <br />
        <div class="maindivstyle" id="Rangechk" align="center" runat="server" style="border-radius: 7px;
            width: 520px; height: 35px;">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Text="Range :"></asp:Label>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                            MaxLength="4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_frange"
                            FilterType="Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txt_trange" CssClass="textbox textbox1 txtheight" runat="server"
                            MaxLength="4"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_trange"
                            FilterType="Numbers" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="Btn_range" runat="server" Text="Select" OnClick="Btn_range_Click"
                            CssClass="textbox1 textbox btn2" Font-Bold="true" Font-Names="Book Antiqua" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="False"></asp:Label></center>
    <center>
        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Height="350" Width="780" Visible="false" OnUpdateCommand="FpSpread2_UpdateCommand"
            HorizontalScrollBarPolicy="Never" ShowHeaderSelection="false">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ShowPDFButton="false">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="Backbtn" runat="server" Text="Back" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" OnClick="Backbtn_Click" Visible="false" />
    <asp:Image ID="Image1" runat="server" Width="1px" Style="top: 0px; left: 897px; position: absolute;
        height: 1px" />
    <asp:Image ID="Image2" runat="server" Width="1px" Style="top: 0px; left: 897px; position: absolute;
        height: 1px" />
    <br />
    <asp:HiddenField ID="printhllticket" runat="server" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="printhllticket"
        CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel4" runat="server" Width="840px" Height="600px" ScrollBars="Auto"
        BorderColor="Black" BorderStyle="Double" Style="display: none; height: 400; width: 700;">
        <div class="HellowWorldPopup">
            <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book 

Antiqua; font-size: xx-large; font-weight: bold">
            </div>
            <div class="PopupBody">
            </div>
            <div class="Controls">
                <center>
                    <FarPoint:FpSpread ID="printspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="800" Width="820" Visible="false" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="Never" ShowHeaderSelection="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button1" runat="server" Text="Close" />
                <br />
    </asp:Panel>
</asp:Content>
