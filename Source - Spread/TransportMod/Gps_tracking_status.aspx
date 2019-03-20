<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Gps_tracking_status.aspx.cs" Inherits="Gps_tracking_status" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <head>
        <style type="text/css">
            .modalPopup
            {
                background: rgba(54, 25, 25, .2);
            }
            
            .head
            {
                background-color: Teal;
                font-family: Book Antiqua;
                font-size: medium;
                color: black;
                top: 80px;
                position: absolute;
                font-weight: bold;
                width: 950px;
                height: 25px;
                left: 15px;
            }
            
            .mainbatch
            {
                background-color: #3AAB97;
                width: 950px;
                position: absolute;
                height: 50px;
                top: 100px;
                left: 15px;
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
                color: black;
            }
            
            .font12
            {
                font-family: Book Antiqua;
                font-size: medium;
                color: White;
                font-weight: bold;
            }
        </style>
        <script type="text/javascript">

            function btnscript() {

                var vehi = document.getElementById('<%=txt_Vechicle.ClientID%>').value;
                var rout = document.getElementById('<%=txt_Route.ClientID%>').value;

                if (vehi == "--Select--") {
                    alert("Please Select Vehicle ID");
                    return false;
                }
                if (rout == "--Select--") {
                    alert("Please Select  Route");
                    return false;
                }
                if (vehi == "--Select--" && rout == "--Select--") {

                    alert("Please Select VehicleID & Route");
                    return false;
                }

                else {
                    return true;
                }
            }
            
        </script>
    </head>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <div class="head">
                <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Style="left: 355px; top: 0px;
                    position: absolute;" Font-Names="Book Antiqua" ForeColor="white" Font-Size="Large"
                    Text="GPS Tracking System"></asp:Label>
                <%-- <asp:LinkButton ID="LinkButtonb1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                    Style="left: 760px; top: 0px; position: absolute;" ForeColor="white" Font-Bold="true"
                    PostBackUrl="~/Default_login.aspx">Back</asp:LinkButton>
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                    Style="left: 800px; top: 0px; position: absolute;" ForeColor="white" Font-Bold="true"
                    PostBackUrl="~/Default_login.aspx">Home</asp:LinkButton>
                <asp:LinkButton ID="lb2" Font-Size="Small" Font-Names="Book Antiqua" Font-Bold="true"
                    Style="left: 850px; top: 0px; position: absolute;" runat="server" ForeColor="white"
                    OnClick="Logout_btn_Click">Logout</asp:LinkButton>--%>
            </div>
            <div class="mainbatch">
                <table style="margin-left: 10px; margin-top: -200px; position: absolute;">
                    <tr>
                        <td>
                            <asp:Label ID="lblVechicle" runat="server" CssClass="font12" Text="Vehicle ID" Style="margin-top: 210px;
                                margin-left: 20px; position: absolute;" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel_Vechicle" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Vechicle" runat="server" ReadOnly="true" Font-Bold="True" Width="125px"
                                        Style="margin-left: 115px; margin-top: 205px; position: absolute;" Font-Names="Book Antiqua"
                                        Font-Size="medium" CssClass="Dropdown_Txt_Box">---Select---</asp:TextBox>
                                    <asp:Panel ID="panel_Vechicle" runat="server" Width="180px" Height="250px" CssClass="MultipleSelectionDDL">
                                        <asp:CheckBox ID="cb_Vechicle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Vehicle_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_Vechicle" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_Vehicle_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender_Vehicle" runat="server" TargetControlID="txt_Vechicle"
                                        PopupControlID="panel_Vechicle" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="IblRoute" runat="server" CssClass="font12" Width="120px" Text="Route ID"
                                Style="margin-left: 285px; margin-top: 210px; position: absolute;"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel_Route" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Route" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="position: absolute; margin-left: 363px; margin-top: 202px;" runat="server"
                                        ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1_Route" runat="server" CssClass="MultipleSelectionDDL" Width="150px"
                                        Height="250px">
                                        <asp:CheckBox ID="cb_Route" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Text="Select All" OnCheckedChanged="cb_Route_Checkedchanged" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cb1_Route" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            runat="server" OnSelectedIndexChanged="cbl_Route_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_Route"
                                        PopupControlID="Panel1_Route" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                            <ContentTemplate>
                            <asp:Button ID="btnclick" runat="server" OnClick="btn_go" CssClass="font12" ForeColor="Black"
                                Text="Go" Style="margin-left: 515px; margin-top: 196px; position: absolute;"
                                OnClientClick="return btnscript()" />
                              </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label ID="IblError" Visible="false" runat="server" Style="margin-left: 10px;
                    margin-top: 0px; position: absolute;" ForeColor="Red" Font-Bold="true" Font-Names="Book Antique"
                    Font-Size="Medium"></asp:Label>
            </div>
            <div style="margin-left: -9px; margin-right: 200px; margin-top: 170px;">
                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                    Visible="false" BorderWidth="0.5" autopostback="true" Height="200" OnCellClick="FSpread1_CellClick"
                    OnPreRender="FSpread1_SelectedIndexChanged" Width="950px">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <div>
                <asp:Button ID="spreadexcel1" runat="server" Text="Export Excel" Width="120px" Height="35px"
                    Font-Bold="true" Visible="false" Font-Names="Book Antique" Style="position: absolute;
                    margin-left: 698px;" Font-Size="Medium" OnClick="spreadexcel1_click" />
                <asp:Button ID="spreadpdf1" runat="server" Text="Print" Width="120px" Font-Bold="true"
                    Visible="false" Font-Names="Book Antique" Font-Size="Medium" Style="position: absolute;
                    margin-left: 822px;" Height="35px" OnClick="spreadpdf1_click" />
            </div>
            <br />
            <br />
            <asp:ModalPopupExtender ID="modelpopsetting" PopupControlID="Panelshow" BackgroundCssClass="modalPopup"
                TargetControlID="FpSpread1" runat="server">
            </asp:ModalPopupExtender>
            <asp:Panel ID="Panelshow" runat="server" Visible="false" CssClass="modalPopup" Style="background-color: White;
                border-color: Black; border-style: solid; border-width: 3px; padding: 15px; margin-top: 50px;"
                Width="850px">
                <center>
                    <asp:Label ID="Labelheader" Text="Stage Details" runat="server" Font-Bold="true"
                        Font-Names="Book Antique" Font-Size="X-Large" ForeColor="Brown" BackColor="#d8d8d8"></asp:Label>
                </center>
                <br />
                <asp:Label ID="Error" runat="server" ForeColor="Red" Text="" Font-Bold="true" Font-Names="Book Antique"
                    Font-Size="Large"></asp:Label>
                <div>
                    <asp:Label ID="lblmale" runat="server" ForeColor="Brown" Font-Bold="true" Text="Male"
                        BackColor="#d8d8d8" Style="margin-left: 50px; text-decoration: blink;" Visible="false"
                        Font-Names="Book Antique" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="Label5" runat="server" Visible="false" Font-Bold="true" ForeColor="teal"></asp:Label>
                    <asp:Label ID="Label2" runat="server" ForeColor="teal" Font-Bold="true" Text="" Visible="false"
                        Font-Names="Book Antique" Font-Size="Medium"></asp:Label>
                    <br />
                    <asp:Label ID="lblfemale" runat="server" ForeColor="Brown" Font-Bold="true" Text=""
                        BackColor="#d8d8d8" Style="margin-left: 50px;" Font-Names="Book Antique" Visible="false"
                        Font-Size="Medium"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Visible="false" Font-Bold="true" ForeColor="teal"></asp:Label>
                    <asp:Label ID="Label3" runat="server" ForeColor="teal" Font-Bold="true" Text="" Visible="false"
                        Font-Names="Book Antique" Font-Size="Medium"></asp:Label>
                    <br />
                    <asp:Label ID="lblstaff" runat="server" ForeColor="Brown" Font-Bold="true" Style="margin-left: 50px;"
                        BackColor="#d8d8d8" Font-Names="Book Antique" Visible="false" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="Label6" runat="server" Visible="false" Font-Bold="true" ForeColor="teal"></asp:Label>
                    <asp:Label ID="Label4" runat="server" ForeColor="teal" Font-Bold="true" Text="" Visible="false"
                        Font-Names="Book Antique" Font-Size="Medium"></asp:Label>
                    <br />
                    <asp:Label ID="Totalstudent" runat="server" ForeColor="Brown" Font-Bold="true" Style="margin-left: 50px;"
                        BackColor="#d8d8d8" Font-Names="Book Antique" Visible="false" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="Label8" runat="server" Visible="false" Font-Bold="true" ForeColor="teal"></asp:Label>
                    <asp:Label ID="Label1" runat="server" ForeColor="teal" Font-Bold="true" Text="" Visible="false"
                        Font-Names="Book Antique" Font-Size="Medium"></asp:Label>
                </div>
                <asp:LinkButton ID="close" Text="X" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" ForeColor="Black" Style="right: 5px; position: absolute; top: 0px;"
                    runat="server" OnClick="closepanel" OnClientClick="closepanel1"></asp:LinkButton>
                <br />
                <br />
                <center>
                    <div style="height: 325px; width: 750px; overflow-x: hidden; overflow-y: auto;">
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                            Visible="false" BorderWidth="0.5" autopostback="true" Height="200" OnCellClick="FSpread1_CellClick"
                            OnPreRender="FSpread1_SelectedIndexChanged" Width="750px">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                &nbsp;
                <%--  <asp:Button ID="spreadexcel2" runat="server" Text="Export Excel" Width="120px" Font-Bold="true"
                    Visible="false" Font-Names="Book Antique" Font-Size="Medium" Style="position: absolute;
                    margin-left: 710px;" Height="35px" OnClick="spreadexcel2_click" />
                <asp:Button ID="spreadpdf2" runat="server" Text="Print" Width="120px" Height="35px"
                    Font-Bold="true" Visible="false" Font-Names="Book Antique" Style="position: absolute;
                    margin-left: 830px;" Font-Size="Medium" OnClick="pdf2_click" />--%>
            </asp:Panel>
            <br />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </body>

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
</asp:Content>
