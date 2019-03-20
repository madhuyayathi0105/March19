<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Vehicleusage_rpt.aspx.cs" Inherits="Vehicleusage_rpt" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
        }
        
        .gvRow td
        {
            background-color: #F0FFFF;
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
        }
        
        .gvAltRow td
        {
            font-family: Book Antiqua;
            font-size: medium;
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <center>
        <div style="width: 1016px; height: 26px; margin-left: 10px; margin: 60px auto 159px 10px;
            padding-left: auto; padding-right: auto; background-color: Teal; text-align: right;">
            <center>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Label ID="lbl" runat="server" Text="Trip Sheet" Font-Bold="true" Font-Names="Bood Antiqua"
                    Font-Size="Large" ForeColor="Azure"></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <%-- &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:LinkButton ID="back" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 70px; position: absolute;" PostBackUrl="~/reports.aspx" ForeColor="White">Back</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="home" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 99px; position: absolute;" PostBackUrl="~/Default_login.aspx" ForeColor="White">Home</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="log" runat="server" OnClick="go_Click" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 136px; position: absolute;" ForeColor="White">Logout</asp:LinkButton>--%>
            </center>
        </div>
    </center>
    <div style="width: 996px; height: 39px; background-color: -webkit-border-radius: 10px;
        -moz-border-radius: 10px; padding: 10px; padding-left: auto; padding-right: auto;
        margin: -159px  auto -159px 10px; background-color: #219DA5;">
        <center>
            <table style="margin-left: -101px; margin-top: -220px; position: absolute; height: 50px;
                width: 600px; margin-bottom: 0px; line-height: 27px;">
                <tr>
                    <td>
                        <asp:Label ID="lblveh" runat="server" Style="position: absolute; left: 105px; top: 225px;
                            color: white;" Font-Size="Medium" Font-Bold="true" Text="Vehicle ID"></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtveh" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                    ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                    Style="position: absolute; left: 182px; top: 225px;" Width="105px"></asp:TextBox>
                                <asp:Panel ID="pnlsec" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                    <asp:CheckBox ID="cbveh" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbveh_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cblveh" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblveh_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <br />
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtveh"
                                    PopupControlID="pnlsec" Position="Bottom">
                                </asp:PopupControlExtender>
                                <Triggers>
                                <asp:PostBackTrigger ControlID="cbveh" />
                                <asp:PostBackTrigger ControlID="cblveh" />
                            </Triggers>
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblroute" runat="server" Style="position: absolute; left: 299px; top: 225px;
                            color: white;" Font-Size="Medium" Font-Bold="true" Text="Route ID"></asp:Label>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtroute" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                    ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="--Select--"
                                    Style="position: absolute; left: 365px; top: 225px;" Width="105px"></asp:TextBox>
                                <asp:Panel ID="plnrt" runat="server" CssClass="multxtpanel" Style="width: 123px;"
                                            Height="250px">
                                    <asp:CheckBox ID="cbrt" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbrt_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cblrt" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblrt_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <br />
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtroute"
                                    PopupControlID="plnrt" Position="Bottom">
                                </asp:PopupControlExtender>
                                 <Triggers>
                                <asp:PostBackTrigger ControlID="cbrt" />
                                <asp:PostBackTrigger ControlID="cblrt" />
                            </Triggers>
                            </ContentTemplate>
                           
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblfrmdate" runat="server" Style="position: absolute; left: 478px;
                            top: 225px; color: white;" Font-Size="Medium" Font-Bold="true" Text="From Date"></asp:Label>
                        <%--  <asp:TextBox ID="tbstart_date" runat="server" Height="20px" Style="height: 20px; left: 557px; 
                                    position: absolute; top: 225px; width: 92px;" Font-Bold="true" AutoPostBack="true" OnTextChanged="tbstart_date_OnTextChanged" ></asp:TextBox>--%>
                        <asp:TextBox ID="tbstart_date" runat="server" Height="20px" Style="left: 557px; position: absolute;
                            top: 225px; width: 92px;" Font-Bold="true" AutoPostBack="true" OnTextChanged="tbstart_date_OnTextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbstart_date" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Font-Names="Book Antiqua" Style="position: absolute;
                            left: 660px; width: 101px; top: 225px; color: white;" Font-Size="Medium" Font-Bold="true"
                            Text="To Date"></asp:Label>
                        <asp:TextBox ID="tbend_date" runat="server" Height="20px" Style="height: 20px; left: 725px;
                            position: absolute; top: 225px; width: 92px;" Font-Bold="true" AutoPostBack="true"
                            OnTextChanged="tbend_date_OnTextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="tbend_date" Format="dd/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btngo" runat="server" Style="background-color: silver; border: 2px solid white;
                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                            height: 27px; margin-left: 349px; margin-top: 199px; position: absolute; width: 42px;"
                            Text="Go" OnClick="btngo_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div>
            </div>
        </center>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <asp:Label ID="lblerroe" Text="" Visible="false" runat="server" ForeColor="Red" CssClass="comm"></asp:Label>
    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="margin-left: 5px;"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
        ForeColor="#FF3300"></asp:Label>
    <asp:Label ID="lblstuderrormsg" runat="server" Text="" Width="302px" Style="margin-left: 5px;"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
        ForeColor="#FF3300"></asp:Label>
    <br />
    <br />
    <center>
        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
            OnButtonCommand="Fpspread1_Command">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
    </center>
    <table id="final" runat="server">
        <tr>
            <td>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:UpdatePanel ID="btnprintupdatepanel" runat="server">
                        <ContentTemplate>
                <center>
                    <asp:Label ID="lblrptname" runat="server" CssClass="comm" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </center>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <%--  </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
        </Triggers>
    </asp:UpdatePanel>--%>
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

    <style>
        .comm
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
