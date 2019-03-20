<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Driver_Allotment.aspx.cs" Inherits="Driver_Allotment" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <div style="margin-top: -90px;">
        <div>
            <asp:Panel ID="Panel2" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="width: 995px;
                height: 21px; top: 50px; left: -56px; margin-top: 148px; margin-left: 15px;">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Driver Allotment" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Style="color: White; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold; position: absolute; left: 419px;"></asp:Label>
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <%--  &nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False" style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold; position: absolute; left: 818px;">Back</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False" style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold; position: absolute; left: 856px;">Home</asp:LinkButton>
                &nbsp;
                <asp:LinkButton ID="lb2" runat="server"  Font-Bold="True" OnClick="lb2_Click"
                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="White" CausesValidation="False" style="color: White; font-family: Book Antiqua; font-size: small; font-weight: bold; position: absolute; left: 900px;">Logout</asp:LinkButton>
                
                <br />
                <br />
                <br />--%>
            </asp:Panel>
        </div>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>    --%>
        <asp:Accordion ID="Accordion1" CssClass="style252" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            runat="server" Width="1100px" BorderColor="White" Style="border-color: White;
            height: auto; overflow: auto; width: 1100px; margin-left: 15px;">
            <Panes>
                <asp:AccordionPane ID="AccordionPane1" runat="server">
                    <Header>
                        View
                    </Header>
                    <Content>
                        <asp:Panel ID="Panel5" runat="server" Style="border-style: solid; border-width: thin;
                            border-color: Black; background: White;">
                            <br />
                            <table class="tabl" style="width: 486px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblvehicleid" runat="server" Font-Bold="true" CssClass="font" Text="Vehicle ID"></asp:Label>
                                    </td>
                                    <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                        border-width: 1px; border-right-style: solid;">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_vech" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                    Width="165px" ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                                    <asp:CheckBox ID="vehiclecheck" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="vehiclecheck_CheckedChanged" />
                                                    <asp:CheckBoxList ID="vehiclechecklist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="vehiclechecklist_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_vech"
                                                    PopupControlID="Panel4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrouteid" runat="server" Font-Bold="true" CssClass="font" Text="Route ID"></asp:Label>
                                    </td>

                                    <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                                    border-width: 1px; border-right-style: solid;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_route" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                Width="165px" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px" Style="position: absolute;">
                                                <asp:CheckBox ID="checkro" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="checkro_CheckedChanged" />
                                                <asp:CheckBoxList ID="checkrolist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="checkrolist_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_route"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                   <%-- <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_route" runat="server" Font-Bold="true" Style="background-color: LightYellow;"
                                                    CssClass="font" Width="122px">--Select--</asp:TextBox>
                                                <asp:Panel ID="routeid" runat="server" CssClass="MultipleSelectionDDL" Style="font-family: 'Book Antiqua';
                                                    position: absolute;" Font-Bold="True" Font-Names="Book Antiqua" Height="197px"
                                                    Width="124px">
                                                    <asp:CheckBox ID="checkro" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="checkro_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="checkrolist" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="checkrolist_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_route"
                                                    PopupControlID="routeid" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                    <td>

                                    
                                        <asp:Button ID="btnMainGo" runat="server" Text="Go" Font-Bold="True" OnClick="btnMainGo_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                           
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Height="200" Width="400" OnCellClick="FpSpread1_CellClick"
                            OnPreRender="FpSpread1_SelectedIndexChanged">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <asp:Label ID="lbl_err" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                    </Content>
                </asp:AccordionPane>
                <asp:AccordionPane ID="AccordionPane2" runat="server">
                    <Header>
                        <asp:Label ID="lbl_add" runat="server"></asp:Label>
                    </Header>
                    <Content>
                        <asp:Panel ID="Panel1" runat="server" Style="border-style: solid; border-color: Gray;
                            border-width: 2px; height: 820px; width: 970px;">
                            <div>
                                <div style="height: 22px; width: 600px; top: 10px; left: 10px;">
                                    <asp:Label ID="lbl_Validation" runat="server" Font-Bold="true" ForeColor="Red" Text=""></asp:Label>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_vehid" runat="server" Text="Vehicle ID" Style="font-family: Book Antiqua;
                                                font-size: medium; left: 50px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_vehid" runat="server" AutoPostBack="true" Style="left: 143px;
                                                width: 120px; height: 22px; top: 0px;" OnSelectedIndexChanged="ddl_vehid_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_routeid" runat="server" Text="Route ID" Style="font-family: Book Antiqua;
                                                font-size: medium; left: 60px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_routeid" runat="server" AutoPostBack="true" Style="left: 143px;
                                                height: 22px; width: 120px; top: 0px;" OnSelectedIndexChanged="ddl_routeid_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lbl_name" runat="server" Text="Drivers Name" Style="font-family: Book Antiqua;
                                        font-size: medium;  left: 24px;"></asp:Label>--%>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txt_dvr_name" runat="server" OnTextChanged="txt_dvr_name_TextChanged"
                                        Style="left: 143px;"></asp:TextBox>
                                    <asp:Button ID="btn_driv_name" runat="server" Text="?" OnClick="btn_driv_name_Click"
                                        Style=" left: 265px;" /><--%>
                                        </td>
                                    </tr>
                                </table>
                                <div style="text-decoration: underline; text-align: center;">
                                    <asp:Label ID="Label5" runat="server" Text="Driver Details" Style="font-family: Book Antiqua;
                                        font-size: medium; left: 24px;"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lbl_name" runat="server" Text="Drivers Name" Style="font-family: Book Antiqua;
                                        font-size: medium; left: 24px;"></asp:Label>
                                    <asp:TextBox ID="txt_dvr_name" runat="server" OnTextChanged="txt_dvr_name_TextChanged"
                                        Style="left: 143px;"></asp:TextBox>
                                    <asp:Button ID="btn_driv_name" runat="server" Text="?" OnClick="btn_driv_name_Click"
                                        Style="left: 265px;" />
                                </div>
                                <asp:Panel ID="Panel3" ToolTip="Driver Photo" BorderStyle="Solid" runat="server"
                                    Style="border-style: solid; float: left; width: 100px; height: 110px; position: absolute;
                                    left: 406px; top: 155px;">
                                    <asp:ImageButton ToolTip="Driver Photo" ID="Driver_Img" runat="server" Style="width: 100px;
                                        height: 110px; position: absolute;" />
                                </asp:Panel>
                                <div style="height: 20px;">
                                </div>
                                <div style="top: 100px; left: 10px; width: 905px;">
                                    <FarPoint:FpSpread ID="Fp_Driver" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Height="300" Width="950" OnButtonCommand="FpDriver_ButtonCommand"
                                        OnCellClick="Fp_Driver_CellClick" OnPreRender="Fp_Driver_SelectedIndexChanged">
                                        <CommandBar BackColor="Control" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <div style="height: 30px;">
                                </div>
                                <div style="text-decoration: underline; text-align: center;">
                                    <asp:Label ID="Label6" runat="server" Text="Helper & Checker Details" Style="font-family: Book Antiqua;
                                        font-size: medium; left: 24px;"></asp:Label>
                                </div>
                                <div style="height: 22px; width: 295px; left: 111px; top: 417px;">
                                    <asp:Label ID="lbl_helper" runat="server" Text="Staff Name" Style="font-family: Book Antiqua;
                                        font-size: medium; left: 24px;"></asp:Label>
                                    <asp:TextBox ID="txt_helper" runat="server" OnTextChanged="txt_helper_TextChanged"
                                        Style="left: 143px; height: 15px;"></asp:TextBox>
                                    <asp:Button ID="btn_helper" runat="server" Text="?" OnClick="btn_helper_Click" Style="left: 265px;" />
                                </div>
                                <div style="height: 20px;">
                                </div>
                                <div style="border-collapse: collapse; left: 10px; top: 455px;">
                                    <FarPoint:FpSpread ID="Fp_Helper" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Height="200" Width="950" VerticalScrollBarPolicy="AsNeeded">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <div style="height: 30px;">
                                </div>
                                <div style="text-align: center;">
                                    <asp:Button ID="Btn_cancel" runat="server" Text="New" Width="100px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Btn_cancel_Click" />
                                    <asp:Button ID="Btn_save" runat="server" Text="Save" Width="100px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Btn_save_Click" />
                                    <asp:Button ID="btn_delete" runat="server" Text="Delete" Font-Bold="True" OnClick="btn_delete_click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </div>
                            </div>
                        </asp:Panel>
                    </Content>
                </asp:AccordionPane>
            </Panes>
        </asp:Accordion>
        <div>
            <asp:Panel ID="panel8" runat="server" BorderColor="Black" BackColor="AliceBlue" BorderWidth="2px"
                Style="background-color: AliceBlue; border-color: Black; border-width: 2px; border-style: solid;
                left: 150px; position: absolute; width: 520px; top: 430px; height: 440px;">
                <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text=" Staff List" Style="width: 150px; position: absolute;
                        left: 200px; top: 4px;"></asp:Label>
                    <%-- <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Staff List
                    </caption>--%>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcollege" runat="server" Text="College" Style="width: 150px; position: absolute;
                                    left: -41px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack ="true" Style="width: 150px;
                                    position: absolute; left: 70px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDepartment" runat="server" Text="Department" Style="width: 150px;
                                    position: absolute; left: 237px; top: 30px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged" AutoPostBack ="true"
                                    Style="width: 150px; position: absolute; left: 360px; top: 30px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Staff Type" Style="width: 150px; position: absolute;
                                    left: -41px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_stftype" runat="server" Width="150px" OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged"
                                    AutoPostBack="true" Style="width: 150px; position: absolute; left: 70px; top: 65px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Designation" Style="width: 150px; position: absolute;
                                    left: 237px; top: 65px;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_design" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddl_design_SelectedIndexChanged" Style="width: 150px;
                                    position: absolute; left: 360px; top: 65px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 510px; position: absolute; top: 95px;">
                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                            Width="510" OnPreRender="fsstaff_SelectedIndexChanged" VerticalScrollBarPolicy="AsNeeded"
                            BorderWidth="0.5" Visible="False" OnUpdateCommand="fsstaff_UpdateCommand">
                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <fieldset style="width: 160px; position: absolute; left: 328px; height: 15px; top: 405px;">
                        <asp:Button runat="server" ID="btnstaffadd" OnClick="btnstaffadd_Click" Width="75px" />
                        <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px" />
                    </fieldset>
            </asp:Panel>
            <asp:ModalPopupExtender ID="mpedirect" Drag="True" CancelControlID="btnexitpop" TargetControlID="hfdirect"
                PopupControlID="panel8" runat="server" BackgroundCssClass="ModalPopupBG" DynamicServicePath=""
                Enabled="True">
            </asp:ModalPopupExtender>
            <asp:HiddenField ID="hfdirect" runat="server" />
        </div>
        <div>
            <asp:Panel ID="PanelUpload" runat="server" BorderColor="Black" BackColor="AliceBlue"
                Visible="false" BorderWidth="2px" Style="background-color: AliceBlue; border-color: Black;
                border-width: 2px; border-style: solid; position: absolute; height: 160px; width: 325px;
                left: 275px; top: 430px;">
                <div style="height: 25px; width: 200px; position: absolute; left: 93px;">
                    <asp:Label ID="lbl_upload_head" Font-Bold="true" Font-Size="Medium" runat="server"
                        Text="Driver Licence Upload"></asp:Label>
                </div>
                <div style="height: 25px; position: absolute; top: 50px; width: 280px; left: 17px;">
                    <asp:Label ID="lbl_upload" runat="server" Font-Size="Medium" Text="Select Image"></asp:Label>
                    <asp:FileUpload ID="licence_upload" runat="server" />
                </div>
                <div style="height: 25px; width: 50px; position: absolute; top: 100px; left: 95px;">
                    <asp:Button ID="btn_upload" runat="server" Text="Ok" OnClick="btn_upload_Click" />
                </div>
                <div style="height: 25px; width: 50px; top: 100px; position: absolute; left: 162px;">
                    <asp:Button ID="btn_close" runat="server" Text="Close" OnClick="btn_close_Click" />
                </div>
                <div style="height: 25px; top: 130px; position: absolute; width: 286px; left: 22px;">
                    <asp:Label ID="lblup_error" Font-Bold="true" Visible="false" ForeColor="Red" runat="server"></asp:Label>
                </div>
            </asp:Panel>
        </div>
        <div>
            <asp:Panel ID="Panel_View" runat="server" BorderColor="Black" BackColor="AliceBlue"
                Visible="false" BorderWidth="2px" Style="background-color: AliceBlue; border-color: Black;
                border-width: 2px; border-style: solid; position: absolute; width: 325px; left: 275px;
                top: 250px; height: 200px;">
                <div style="height: 25px; width: 200px; position: absolute; left: 93px;">
                    <asp:Label ID="lbl_view_head" runat="server" Font-Bold="true" Font-Size="Medium"
                        Text="Licence Image"></asp:Label>
                </div>
                <div style="position: absolute; width: 220px; height: 135px; left: 60px; top: 30px;">
                    <asp:ImageButton ID="Img_Licence" runat="server" Height="125px" Width="200px" />
                </div>
                <div style="height: 25px; width: 50px; position: absolute; left: 140px; top: 170px;">
                    <asp:Button ID="btnclose_view" runat="server" Text="Close" OnClick="btnclose_view_Click" />
                </div>
            </asp:Panel>
        </div>

         <center>
                        <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 338px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                            width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                                     </center>
                                                </td>

                                               
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </div>
            </center>   


        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:Label ID="lblerror" runat="server" Text="Label"></asp:Label>
        <style type="text/css">
            ody, input
            {
                font-family: Tahoma;
                font-size: 11px;
            }
            
            .modalBackground
            {
                background-color: Gray;
                filter: alpha(opacity=80);
                opacity: 0.8;
            }
            
            .modalPopup
            {
                background-color: #ffffdd;
                border-width: 1px;
                -moz-border-radius: 5px;
                border-style: solid;
                border-color: Gray;
                min-width: 250px;
                max-width: 500px;
                min-height: 100px;
                max-height: 150px;
                top: 100px;
                left: 150px;
            }
            
            .topHandle
            {
                background-color: #97bae6;
            }
            .floatr
            {
                border-style: solid;
                border-color: Black;
                border-width: thin;
                float: right;
            }
            
            
            .tabl
            {
                empty-cells: show;
                border-style: solid;
                border-color: Gray;
                border-width: thin;
                caption-side: top;
                font-family: MS Sans Serif;
                font-size: Small;
                font-style: normal;
                font-weight: normal;
            }
            .tablfont
            {
                empty-cells: show;
                border-style: solid;
                border-color: Gray;
                border-width: thin;
                caption-side: top;
                font-family: MS Sans Serif;
                font-size: Small;
                font-style: normal;
                font-weight: bold;
            }
            .tabl3
            {
                empty-cells: hide;
                border-style: solid;
                border-color: Black;
                border-width: thin;
                caption-side: top;
                font-family: MS Sans Serif;
                font-size: Small;
                font-style: normal;
                font-weight: bold;
                text-align: left;
            }
            .tabl5
            {
                border-style: solid;
                border-color: Black;
                border-width: thin;
                text-align: left;
            }
            .tabl1
            {
                empty-cells: show;
                caption-side: top;
                font-family: MS Sans Serif;
                font-size: Small;
                font-style: normal;
                font-weight: bold;
            }
            .linkbtn
            {
                border-color: White;
                border-style: none;
                background-color: transparent;
                cursor: pointer;
            }
            .HeaderSelectedCSS
            {
                color: white;
                background-color: #719DDB;
                font-weight: bold;
                font-size: medium; /* font-style:italic;  */
            }
            .HellowWorldPopup
            {
                min-width: 600px;
                min-height: 400px;
                background: white;
            }
            .ModalPopupBG
            {
                background-color: #666699;
                filter: alpha(opacity=50);
                opacity: 0.7;
            }
            
            .style2
            {
                width: 118px;
            }
            .style4
            {
                width: 43px;
            }
            .stylefp
            {
                cursor: pointer;
            }
            .style5
            {
                width: 185px;
            }
            .style55
            {
                width: 25px;
            }
            .style27
            {
                width: 25px;
            }
            .style25
            {
                width: 200px;
            }
            .style251
            {
                width: 125px;
            }
            .style6
            {
                width: 528px;
            }
            .style12
            {
                width: 200px;
            }
            .style22
            {
                width: 122px;
            }
            .style24
            {
                width: 30px;
            }
            
            .font
            {
                font-size: Small;
                font-family: MS Sans Serif;
            }
            .HeaderCSS
            {
                color: white;
                background-color: #719DDB;
                font-size: medium; /* border:solid 1px salmon; */
                font-weight: bold;
            }
            .cpBody
            {
                background-color: #DCE4F9; /*font: normal 11px auto Verdana, Arial;
            border: 1px gray;               
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
            width:720;*/
            }
            .accordion
            {
                width: 400px;
            }
            
            .accordionHeader
            {
                border: 1px solid #2F4F4F;
                color: white;
                background-color: #2E4d7B;
                font-family: Arial, Sans-Serif;
                font-size: 12px;
                font-weight: bold;
                padding: 5px;
                margin-top: 5px;
                cursor: pointer;
            }
            
            .accordionHeaderSelected
            {
                border: 1px solid #2F4F4F;
                color: white;
                background-color: #5078B3;
                font-family: Arial, Sans-Serif;
                font-size: 12px;
                font-weight: bold;
                padding: 5px;
                margin-top: 5px;
                cursor: pointer;
            }
            
            .accordionContent
            {
                background-color: White;
                border: 1px dashed #2F4F4F;
                border-top: none;
                padding: 5px;
                padding-top: 10px;
            }
        </style>
        <%-- </ContentTemplate>
        </asp:TabPanel>
        </asp:TabContainer>
        </asp:Panel>
        </Content>
        </asp:AccordionPane>
        </Panes>
            </asp:Accordion>--%>
    </div>
        </ContentTemplate>
     </asp:UpdatePanel>

      
</asp:Content>
