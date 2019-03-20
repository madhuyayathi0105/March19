<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Manualmarksheet.aspx.cs" Inherits="Manualmarksheet"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .color
        {
            color: White;
        }
    </style>
    <br />
    <center>
        <asp:Label ID="Label1" runat="server" Text="Manual Mark Sheet" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <table style="width: 800px; height: 70px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lbledulevel" runat="server" Text="Edu Level" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddledulevel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddledulevel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltype" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcoltypeadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblmonth" runat="server" Text="Month" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
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
                    <asp:Label ID="lbldop" runat="server" Text="Date Of Publication" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdop" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="75px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdop" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                    <asp:Label ID="lbldoi" runat="server" Text="Date Of Issue" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:Label>
                    <asp:TextBox ID="txtdoi" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="75px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdoi" Format="dd/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Width="51px" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="chk_subjectwise" runat="server" Text="Subject Wise" AutoPostBack="true"
                        OnCheckedChanged="chk_subjectwise_CheckedChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                        Font-Bold="True" Width="125px" />
                </td>
                <td>
                    <asp:Label ID="lblsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Subject"></asp:Label>
                    <asp:TextBox ID="txtsubject" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                        ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="-Select-"
                        Style="left: 955px; position: absolute; top: 191px;" Width="124px"></asp:TextBox>
                    <asp:Panel ID="pnlsec" runat="server" CssClass="MultipleSelectionDDL" Height="95"
                        Width="124px">
                        <asp:CheckBox ID="cbsubj" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbsubj_OnCheckedChanged" />
                        <asp:CheckBoxList ID="ddlsubject" runat="server" Font-Size="Small" AutoPostBack="True"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubject_OnSelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <br />
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubject"
                        PopupControlID="pnlsec" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="11">
                    <asp:Label ID="lblsubjecttype" runat="server" Text="Subject Type" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:Label>
                    <asp:DropDownList ID="dropsubjecttype" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropsubjecttype_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblterm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Sem"></asp:Label>
                    <asp:DropDownList ID="dropterm" runat="server" Width="55px" Height="25px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="Button1_Click" OnClientClick="check()" />
                    <asp:CheckBox ID="chkBasedOnExamApplication" runat="server" Text="Based On Exam Application" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Label ID="lblno" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
        <asp:Label ID="errmsg" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
    </center>
    <br />
    <center>
        <FarPoint:FpSpread ID="HAllSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
            Visible="false" BorderWidth="1px" Height="330" Width="580" OnButtonCommand="HAllSpread_Command"
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
    <br />
    <asp:Label ID="IblError" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
        Style="margin-left: 139px;" Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
    <center>
        <div>
            <table id="headoffp2" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlformate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="false" OnSelectedIndexChanged="ddlformate_SelectedIndexChanged">
                            <asp:ListItem>Format 1</asp:ListItem>
                            <asp:ListItem>Format 2</asp:ListItem>
                            <asp:ListItem>Format 3</asp:ListItem>
                            <asp:ListItem>Format 4</asp:ListItem>
                            <asp:ListItem>Format 5</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RadioButton ID="RadioButton1" runat="server" Visible="false" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton1_CheckedChanged"
                            Text="Format 1" AutoPostBack="True" Width="100px" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RadioButton2" runat="server" Font-Bold="True" Visible="false"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton2_CheckedChanged"
                            Text="Format 2" Width="100px" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RadioButton3" runat="server" Font-Bold="True" Visible="false"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" OnCheckedChanged="RadioButton3_CheckedChanged"
                            Text="Format 3" Width="100px" />
                    </td>
                    <td>
                        <asp:RadioButton ID="RadioButton4" runat="server" Font-Bold="True" Visible="false"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" Text="Format 4"
                            Width="100px" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
            Style="font-size: medium; font-weight: bold; font-family: Book Antiqua;" BorderWidth="1px"
            Height="350" Width="680" Visible="false" OnButtonCommand="FpSpread2_UpdateCommand"
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
    <asp:Button ID="printbtn" runat="server" Text="Print" Style="margin-left: 140px;"
        Font-Bold="True" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
        OnClick="printbtn_Click" />
    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
</asp:Content>
