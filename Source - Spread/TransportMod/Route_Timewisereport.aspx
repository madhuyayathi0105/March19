<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Route_Timewisereport.aspx.cs" Inherits="Route_Timewisereport" %>


<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <head>
        <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
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
                top: 70px;
                position: absolute;
                font-weight: bold;
                width: 950px;
                height: 25px;
                left: 15px;
            }
            .fontmedium
            {
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
            }
            
            .mainbatch
            {
                background-color: #3AAB97;
                width: 950px;
                position: absolute;
                height: 50px;
                top: 90px;
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
                    alert("Please Select  Route ID");
                    return false;
                }

                else {
                    return true;
                }
            }

            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }
            function display1() {

                document.getElementById('MainContent_lbl_errmsg').innerHTML = "";
            }
            
        </script>
    </head>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="head">
            <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Style="left: 355px; top: 0px;
                position: absolute;" Font-Names="Book Antiqua" ForeColor="white" Font-Size="Large"
                Text="Route And Timewise Report"></asp:Label>
            <%--<asp:LinkButton ID="LinkButtonb1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                Style="left: 760px; top: 0px; position: absolute;" ForeColor="white" Font-Bold="true"
                PostBackUrl="~/reports.aspx">Back</asp:LinkButton>
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
                                    Font-Size="medium" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                                <asp:Panel ID="panel_Vechicle" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="250px">
                                    <asp:CheckBox ID="chk_Vechicle" runat="server" Text="Select All" AutoPostBack="True"
                                        CssClass="fontmedium" OnCheckedChanged="chk_Vechicle_checkedchanged" />
                                    <asp:CheckBoxList ID="chklst_Vechicle" runat="server" AutoPostBack="True" CssClass="fontmedium"
                                        OnSelectedIndexChanged="chklstvehicle_selected">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender_Vehicle" runat="server" TargetControlID="txt_Vechicle"
                                    PopupControlID="panel_Vechicle" Position="Bottom">
                                </asp:PopupControlExtender>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="chklst_Vechicle" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_Vechicle" />
                            </Triggers>
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="IblRoute" runat="server" CssClass="font12" Width="120px" Text="Route ID"
                            Style="margin-left: 285px; margin-top: 205px; position: absolute;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_Route" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Route" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="position: absolute; margin-left: 363px; margin-top: 202px;"
                                    runat="server" ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1_Route" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="250px">
                                    <asp:CheckBox ID="chk_Route" runat="server" CssClass="fontmedium" Text="Select All"
                                        OnCheckedChanged="chk_Route_Checkedchanged" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklst_Route" CssClass="fontmedium" runat="server" OnSelectedIndexChanged="chklst_Route_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_Route"
                                    PopupControlID="Panel1_Route" Position="Bottom">
                                </asp:PopupControlExtender>

                                <Triggers>
                                <asp:PostBackTrigger ControlID="chklst_Route" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_Route" />
                            </Triggers>
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
                margin-top: 0px; position: absolute;" ForeColor="Red" CssClass="fontmedium"></asp:Label>
        </div>
        <div style="margin-left: -9px; margin-right: 200px; margin-top: 150px;">
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
            <asp:Label ID="lbl_errmsg" runat="server" Visible="false" CssClass="fontmedium" ForeColor="Red"></asp:Label>
            <br>
            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="position: absolute;
                margin-left: 0px;" CssClass="fontmedium" Height="35px" Visible="false"></asp:Label>
            <asp:TextBox ID="txt_name" runat="server" Style="position: absolute; margin-left: 102px;"
                Height="25px" CssClass="fontmedium" Visible="false" onkeypress="display1()"></asp:TextBox>
            <asp:Button ID="spreadexcel1" runat="server" Text="Export To Excel" Width="127px"
                Height="35px" Visible="false" CssClass="fontmedium" Style="position: absolute;
                margin-left: 273px;" OnClick="btn_spreadexcel1" />
            <asp:Button ID="spreadpdf1" runat="server" Text="Print" CssClass="fontmedium" Visible="false"
                Style="position: absolute; margin-left: 406px;" Height="35px" OnClick="btn_spreadpdf1" />
        </div>
        <br />
        <br />
        <br />
        <asp:ModalPopupExtender ID="modelpopsetting" PopupControlID="Panelshow" BackgroundCssClass="modalPopup"
            TargetControlID="FpSpread1" runat="server">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panelshow" runat="server" Visible="false" CssClass="modalPopup" Style="background-color: White;
            border-color: Black; border-style: solid; border-width: 3px; padding: 15px; margin-top: 20px;"
            Width="850px">
            <br>
            <asp:Label ID="Labelheader" Text="ROUTE - " runat="server" Font-Bold="true" Font-Names="Book Antique"
                Font-Size="Large" ForeColor="Brown"></asp:Label>
            <asp:Label ID="lbl_routeid" runat="server" Visible="false" CssClass="fontmedium"
                ForeColor="Purple"></asp:Label>
            <asp:Label ID="lbl_routename" runat="server" ForeColor="Purple" CssClass="fontmedium"
                Text="" Visible="false"></asp:Label>
            <asp:LinkButton ID="close" Text="X" Font-Names="Book Antiqua" Font-Size="X-Large"
                Font-Bold="true" ForeColor="Red" Style="right: 5px; position: absolute; top: 0px;"
                runat="server" OnClick="closepanel" OnClientClick="closepanel1"></asp:LinkButton>
            <br />
            <br />
            <center>
                <div style="height: 350px; width: 515px; overflow-x: hidden; overflow-y: auto;">
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                        Visible="false" BorderWidth="0.5" autopostback="true" Height="300" OnCellClick="FSpread1_CellClick"
                        OnPreRender="FSpread1_SelectedIndexChanged">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
            </center>
            <br />
            <asp:Label ID="lblerr" runat="server" Text="" Visible="false" CssClass="fontmedium"
                Style="position: absolute; margin-left: 102px;" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lbl_rpt" runat="server" Text="Report Name" CssClass="fontmedium" Style="position: absolute;
                margin-left: 102px;" Height="35px"></asp:Label>
            <asp:TextBox ID="txt_rpt" runat="server" Style="position: absolute; margin-left: 212px;"
                onkeypress="display()" Height="25px"></asp:TextBox>
           
            <asp:Button ID="spreadexcel2" runat="server" Text="Export To Excel" Width="127px"
                Font-Bold="true" Visible="false" Font-Names="Book Antique" Font-Size="Medium"
                Style="position: absolute; margin-left: 359px;" Height="35px" OnClick="btn_spreadexcel2" />
                 

            
            <asp:Button ID="spreadpdf2" runat="server" Text="Print" Height="35px" Font-Bold="true" 
                Visible="false" Font-Names="Book Antique" Style="position: absolute; margin-left: 486px;"
                Font-Size="Medium" OnClick="btn_pdf2" />
                
        
        <br />
        
           
        </asp:Panel>
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </body>
            </ContentTemplate>
            <Triggers>
                                <asp:PostBackTrigger ControlID="spreadexcel1" />
                                <asp:PostBackTrigger ControlID="spreadpdf1" />
                                <asp:PostBackTrigger ControlID="spreadexcel2" />
                                <asp:PostBackTrigger ControlID="spreadpdf2" />
                                <asp:PostBackTrigger ControlID="FpSpread1" />
                            </Triggers>
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
