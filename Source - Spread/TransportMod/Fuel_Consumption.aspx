<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Fuel_Consumption.aspx.cs" Inherits="Fual_Consumption" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <%--<asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="1000" Enabled="True">
            </asp:Timer>--%>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div style="top: 70px; position: absolute;">
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 1020px; height: 21px; margin-bottom: 0px; top: 8px; left: 10px;">
                <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Fuel Consumption Report" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" Style="color: White;
                    font-family: Book Antiqua; font-size: medium; font-weight: bold; position: absolute;
                    left: 415px;"></asp:Label>
                <%-- &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;
                <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/reports.aspx" CausesValidation="False"
                    Style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold;
                    position: absolute; left: 840px;">Back</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False"
                    Style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold;
                    position: absolute; left: 875px;">Home</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb2" runat="server" OnClick="lb2_Click" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" CausesValidation="False" Style="color: White;
                    font-family: Book Antiqua; font-size: small; font-weight: bold; position: absolute;
                    left: 916px;">Logout</asp:LinkButton>
                <br />
                <br />
                <br />--%>
            </asp:Panel>
        </div>
    </div>
            </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
    <br />
    <table style="border-style: solid; border-color: gray; border-width: 1px; margin-left: 10px;
        margin-top: 42px;">
        <tr>
            <td style="width=200px;">
                <asp:Label ID="lblvehicleid" runat="server" Text="Vehicle ID" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td style="width=200px;">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:TextBox ID="txt_vech" runat="server" CssClass="textbox  textbox1 txtheight3"
                            Width="165px" ReadOnly="true">-- Select--</asp:TextBox>
                        <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                            <asp:CheckBox ID="vehiclecheck" runat="server" Text="Select All" AutoPostBack="true"
                                OnCheckedChanged="vehiclecheck_CheckedChanged" />
                            <asp:CheckBoxList ID="vehiclechecklist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="vehiclechecklist_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_vech"
                            PopupControlID="Panel6" Position="Bottom">
                        </asp:PopupControlExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
            <td>
                <asp:CheckBox ID="Chkdate" runat="server" />
            </td>
            <td>
                <asp:Label ID="lblfromdate" runat="server" Text="From Date" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="24px" Width="92px" OnTextChanged="txtfromdate_TextChanged"
                    AutoPostBack="True"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" TargetControlID="txtfromdate"
                    runat="server">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="24px" Width="92px" OnTextChanged="txttodate_TextChanged"
                    AutoPostBack="True"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="txttodate"
                    runat="server">
                </asp:CalendarExtender>
            </td>
            <%--<td>
                <asp:Label ID="lblExamMonth" runat="server" Text="From Month" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                </asp:DropDownList>
                <br />
            </td>
            <td>
                <asp:Label ID="lblExamYear" runat="server" Text="From Year" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                </asp:DropDownList>
                <br />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="To Month" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                </asp:DropDownList>
                <br />
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="To Year" Font-Bold="True" Font-Size="Medium"
                    Font-Names="Book Antiqua"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                </asp:DropDownList>
                <br />
            </td>--%>
            <td style="width: 30px;">
            </td>
            <td>
            <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" />
                     </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>
    </table>
    <br />
        </ContentTemplate>
    </asp:UpdatePanel>

     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <table>
        <tr>
            <td style="margin-left: 100px">
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
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Size="Medium"></asp:Label>
            </td>
        </tr>
    </table>

        </ContentTemplate>
    </asp:UpdatePanel>
    <%--    </ContentTemplate>
    </asp:UpdatePanel>--%>

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
    <br />
    <table>
        <tr>
            <td>
            <asp:UpdatePanel ID="btnprintupdatepanel" runat="server">
                <ContentTemplate>
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:CheckBox ID="cbhourwise" runat="server" Text="Hourwise" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="False" />
            </td>
        </tr>
    </table>

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
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="btnprintupdatepanel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
