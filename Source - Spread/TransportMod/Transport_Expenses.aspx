<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Transport_Expenses.aspx.cs" Inherits="Transport_Expenses" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <head>
        <script type="text/javascript">

            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }</script>
        <title></title>
        <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />


        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <body>
            <br />
            <asp:Panel ID="header_Panel" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                Style="left: -16px; position: absolute; width: 1088px; height: 21px">
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Vehicle Expenses Abstract "></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <%-- <asp:LinkButton ID="back_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/reports.aspx" CausesValidation="False">Back</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="home_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="logout_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                OnClick="logout_btn_Click" Font-Size="Small" ForeColor="White" CausesValidation="False">Logout</asp:LinkButton>--%>
        </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="left: -16px;
                top: 200px; position: absolute; width: 1088px; height: 21px">
            </asp:Panel>
            <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                left: -16px; border-right-style: solid; background-color: lightblue; border-width: 1px;">
                <tr>
                    <td>
                        <asp:Label ID="lblselectcollege" runat="server" Text=" College" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtcollege" class="Dropdown_Txt_Box" runat="server" Width="115px"
                            Text="" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua">
                        </asp:TextBox>
                        <%--<asp:DropDownList ID="ddlselectcollege" runat="server" Width="120px" Font-Bold="True"
                        Font-Size="Medium" Font-Names="Book Antiqua">
                    </asp:DropDownList>--%>
                        <asp:Panel ID="pclg" runat="server" CssClass="MultipleSelectionDDL" Height="147px">
                            <asp:CheckBox ID="chekclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chekclg_CheckedChanged" />
                            <asp:CheckBoxList ID="cheklist_clg" runat="server" Font-Size="Medium" Font-Bold="True"
                                Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="cheklist_clg_SelectedIndexChanged">
                            </asp:CheckBoxList>
                        </asp:Panel>
                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtcollege"
                            PopupControlID="pclg" Position="Bottom">
                        </asp:PopupControlExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblvehicletype" runat="server" Text="Vehicle Type" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlvehicletype" runat="server" Width="120px" Font-Bold="True"
                            OnSelectedIndexChanged="ddlvehicletype_SelectedIndexChanged" Font-Size="Medium"
                            AutoPostBack="true" Font-Names="Book Antiqua">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblvehicleid" runat="server" Text="Vehicle ID" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlvehicleid" runat="server" Width="120px" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblfromdate" runat="server" Text="From Date" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Width="113px">
                        </asp:TextBox>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:CalendarExtender ID="calenderextenderfromdate" runat="server" TargetControlID="txtfromdate"
                            Format="dd/MM/yyyy" Enabled="true">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Width="113px" OnTextChanged="txttodate_TextChanged" AutoPostBack="true">
                        </asp:TextBox>
                        <asp:CalendarExtender ID="calenderextendertodate" runat="server" Enabled="true" TargetControlID="txttodate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                    <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                        <ContentTemplate>
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" onclick="btngo_Click" />

                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblinvalid" runat="server" Text="" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="Fpexpenses" runat="server" Width="650px" Visible="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="false">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()">
                        </asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:UpdatePanel ID="btnexcelupdatepanel" runat="server">
                            <ContentTemplate>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="btnprintupdatepanel" runat="server">
                            <ContentTemplate>
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
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

    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="btnexcelupdatepanel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
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


    </head>
    </html>
</asp:Content>
