<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ConsolidateMarksheet.aspx.cs" Inherits="ConsolidateMarksheet" %>

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
    </style>
    <script language="javascript">
        function check() {
            var checkvalidation3 = document.getElementById('<%=ddlMonth.ClientID%>').value;
            var checkval = document.getElementById('<%=ddlYear.ClientID%>').value;
            if (checkvalidation3 == 0) {

                alert(" Please Select Month");
                return false;
            }

            if (checkval == 0) {

                alert(" Please Select Year");
                return false;
            }
            return true;
        }
    </script>
   <br />
        <center>
            <asp:Label ID="Label1" runat="server" Text="Consolidate Mark Sheet" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
        </center>
        
    <br />
    <center>
        <table style="width:900px; height:70px; background-color:#0CA6CA;">
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
                                    Font-Size="Medium"></asp:Label></td>
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
                                Font-Size="Medium" Width="75px"                              ></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdoi" Format="dd/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                         <td>
 <asp:Label ID="Label3" runat="server" Text="Format" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:Label>
    
    </td>
    <td>
    <asp:DropDownList ID="ddlformate" runat="server"  Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlformate_SelectedIndexChanged">
                                <asp:ListItem>Format 1</asp:ListItem>
                                <asp:ListItem>Format 2</asp:ListItem>
                                 <asp:ListItem>Format 3</asp:ListItem>
                                  <asp:ListItem>Format 4</asp:ListItem>
                                </asp:DropDownList>
    
    </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="Button1_Click" OnClientClick="check()" />
                            </td>
                            <td>
                                <asp:Image ID="image3" Style="margin-left: 131px;" Visible="false" runat="server"
                                    Width="100px" />
                            </td>
                        </tr>
                    </table>
      </center>
   
    <br />
    <asp:Label ID="lblno" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
    <asp:Label ID="errmsg" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Visible="False"></asp:Label>
    <br />
    <br />
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
        Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
    <center>
        <asp:Label ID="Label2" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="False"></asp:Label>
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
        <br />
        <asp:Button ID="printbtn" runat="server" Text="Print" Style="margin-left: -615px;"
            Font-Bold="True" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium"
            OnClick="printbtn_Click" />
    </center>
</asp:Content>

