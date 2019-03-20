<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="RoomAvailability.aspx.cs" Inherits="RoomAvailability" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" >
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Room Availability</span></div>
        </center>
        <br />
        <div class="maindivstyle" style="height: auto;">
            <center>
                <br />
              
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_hostel" runat="server" CssClass="textbox ddlheight2" OnSelectedIndexChanged="ddl_hostel_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_building" Text="Building Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_building" runat="server" CssClass="textbox ddlheight2"
                                OnSelectedIndexChanged="ddl_building_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_floor" Text="Floor" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uppanel_floor" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_floor" runat="server" ReadOnly="true" CssClass="textbox txtheight2 ">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_floor" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_floor" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_floor_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floor_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_floor"
                                        PopupControlID="panel_floor" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_room" Text="Room Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Uppanel_room" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_room" runat="server" ReadOnly="true" CssClass="textbox txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_room" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_room" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_room_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_room"
                                        PopupControlID="panel_room" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_vaccant" Text="Vaccant Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_vaccant" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_vaccant_SelectedIndexChanged">
                                <asp:ListItem>Filled</asp:ListItem>
                                <asp:ListItem>Un Filled</asp:ListItem>
                                <asp:ListItem>Partialy Filled</asp:ListItem>
                                <asp:ListItem>All</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="4">
                            <div>
                                <div style="float: left;">
                                    <asp:Label ID="lbl_include" Text="Include:" runat="server"></asp:Label>
                                    <asp:CheckBox ID="cb_includeall" runat="server" Text="All" OnCheckedChanged="cb_includeall_CheckedChanged"
                                        AutoPostBack="true" />
                                    </asp:CheckBox>
                                </div>
                                <div style="float: left;">
                                    <asp:CheckBoxList ID="cbl_roomcheck" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="cbl_roomcheck_SelectedIndexChanged"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                        <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                        <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <center>
                    <div>
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </center>
                <%-- <div id="divSpread" runat="server"  style="width: 930px; height: 330px;
                    overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                    <br />--%>
                <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Width="800px" Height="340px" class="spreadborder">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <%-- </div>--%>
                <br />
                <table runat="server" id="tblStatus" style="border-bottom-style: solid; border-top-style: solid;
                    border-left-style: solid; border-right-style: solid; width: 500px; background-color: lightblue;
                    border-width: 0px;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_totalroom" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="lbl_totalvaccants" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_fill" runat="server" Width="20px" BackColor="GreenYellow" />
                            <asp:Label ID="fill" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="109px"></asp:Label>
                            <asp:Button ID="btn_partialfill" runat="server" Width="20px" BackColor="Coral" />
                            <asp:Label ID="partialfill" runat="server" Text="Partialy Filled" Font-Bold="True"
                                Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                            <asp:Button ID="btn_unfill" runat="server" Width="20px" BackColor="MistyRose" />
                            <asp:Label ID="unfill" runat="server" Text="UnFilled" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="145px"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn2" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                        CssClass="textbox btn2" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </center>
            <center>
                <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="textbox btn2" Visible="false" />
                <%--<asp:Button ID="btnexit" Text="Exit" runat="server" CssClass="textbox btn2" />--%>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
