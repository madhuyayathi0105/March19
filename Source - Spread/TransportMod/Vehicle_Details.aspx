<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Vehicle_Details.aspx.cs" Inherits="Vehicle_Details" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <div style="top: 60px; position: absolute;">
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="position: absolute;
                width: 1020px; height: 21px; margin-bottom: 0px; top: 8px; left: 10px;">
                <%-- style="top: 71px; left: 0px; position: absolute; width: 960px"--%>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Detailed Vehicle Report" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" Style="color: White;
                    font-family: Book Antiqua; font-size: medium; font-weight: bold; position: absolute;
                    left: 415px;"></asp:Label>
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%-- &nbsp;&nbsp;
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
                <%--<asp:Label ID="lbltitle" runat="server" Font-Names="Book Antiqua" 
                    Font-Size="Large" ForeColor="White"></asp:Label>--%>
            </asp:Panel>
        </div>
    </div>
    <br />
    <table style="margin-left: 10px; margin-top: 12px;">
        <tr>
            <td style="width=200px;">
                <asp:Label ID="lblvehicleid" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                    Text="Vehicle ID"></asp:Label>
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
                <%--Commented by Prabhakaran jan 03 2018 --%>
                <%--    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
    <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/image/Top Band-2.jpg" Width="1000px"
        Height="16px" Style="height: 16px; width: 1020px; left: 0px; margin-top: 15px;
        margin-left: 10px;">
        <br />
        <br />
        <div style="margin-left: 10px;">
            <asp:UpdatePanel ID="updspread" runat="server">
                <ContentTemplate>
                    <FarPoint:FpSpread ID="Fp_Vehicle" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="200" Width="1000">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <div>
                        <asp:Label ID="lbl_errmsg" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="height: 20px">
        </div>
        <div style="text-align: center;">
         <asp:UpdatePanel ID="btnprintupdatepanel" runat="server">
            <ContentTemplate>
            <asp:Button ID="Button1" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </asp:Panel>

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
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
