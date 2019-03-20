<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamMarkSheetForTheoryAndPractical.aspx.cs" Inherits="ExamMarkSheetForTheoryAndPractical" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
    </style><br />
     <center>
            <asp:Label ID="Label1" runat="server" Text="Mark Sheet" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Large" ForeColor="Green"></asp:Label>
        </center>
       
   
    <br /><center>
    <table style="height: 50px; width:700px; background-color:#0CA6CA; ;">
        <tr>
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
                        </td>
                        <td>
                           <asp:Label ID="lbldoi" runat="server" Text="Date Of Issue" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdoi" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdoi" Format="dd/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="Button1_Click" OnClientClick="check()" />
      </tr>
    </table>
    </center>
    <br />
    <asp:Label ID="lblno" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
    <asp:Label ID="errmsg" runat="server" Text="No Records Found" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
    <br />
    
    <center>
        <FarPoint:FpSpread ID="HAllSpread" runat="server" BorderColor="Black" BorderStyle="Solid"
            Visible="false" BorderWidth="1px" Height="330" Width="580" OnCellClick="HAllSpread_CellClick"
            OnPreRender="HAllSpread_SelectedIndexChanged" CssClass="stylefp">
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
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            BorderWidth="1px" Height="350" Width="680" Visible="false" OnUpdateCommand="FpSpread2_UpdateCommand"
            HorizontalScrollBarPolicy="Never">
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
</asp:Content>

