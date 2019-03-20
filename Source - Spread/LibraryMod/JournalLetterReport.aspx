<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="JournalLetterReport.aspx.cs" Inherits="LibraryMod_JournalLetterReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Journal Report</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="font-family: Book Antiqua;
                        font-weight: bold; width: 1070px; height: auto">
                        <div>
                            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                                margin-bottom: 10px; width: 1060px; padding: -20px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSupplierType" runat="server" Text="Supplier Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSupplierType" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    Width="144px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="PanelSupplierType" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chkSupplierType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkSupplierType_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklSupplierType" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklSupplierType_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtSupplierType"
                                                    PopupControlID="PanelSupplierType" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    Width="140px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chkSupplierName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkSupplierName_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklSupplierName" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklSupplierName_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtSupplierName"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLanguage" runat="server" Text="Language"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtLanguage" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chkLanguage" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkLanguage_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklchkLanguage" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklchkLanguage_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtLanguage"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJournalName" runat="server" Text="Journal Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtJournalName" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                    Width="132px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" Width="280px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="chkJournalName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chkJournalName_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklJournalName" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="chklJournalName_SelectedIndexChanged" Font-Bold="True"
                                                        Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtJournalName"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblreporttype" runat="server" Text="Report Type">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="155px" AutoPostBack="True" OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Proformafromdate1" runat="server" Visible="false" Text="Proforma From"></asp:Label>
                                        <asp:Label ID="lbl_SubscriptionYear" runat="server" Visible="false" Text="Subscription Year"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Proformafromdate1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_Proformafromdate1"
                                            runat="server" Format="MMMM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:TextBox ID="txt_SubscriptionYear" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_SubscriptionYear"
                                            runat="server" Format="yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Proformatodate1" runat="server" Visible="false" Text="Proforma To"
                                            Style="margin-left: 4px;"></asp:Label>
                                        <asp:Label ID="lbl_DDAmount" runat="server" Visible="false" Text="DD Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Proformatodate1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_Proformatodate1"
                                            runat="server" Format="MMMM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:TextBox ID="txt_DDAmount" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_localsupplierpricelist" runat="server" Visible="false" CssClass="textbox txtheight2"
                                            Style="width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_localsupplierpricelist"
                                            runat="server" Format="MMMM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpGo" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                    OnClick="btn_go_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
                <br />
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdJournal" runat="server" ShowHeader="true" ShowFooter="false"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="false"
                        OnRowDataBound="grdJournal_OnRowDataBound">
                        <%-- AllowPaging="true" PageSize="10" OnPageIndexChanging="grdManualExit_OnPageIndexChanged"--%>
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox txtheight2" Width="180px"
                            onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                       
                        <asp:ImageButton ID="btnExcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                            OnClick="btnExcel1_Click" />
                        <asp:ImageButton ID="btnprintmaster1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                            OnClick="btnprintmaster1_Click" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExcel1" />
                    <asp:PostBackTrigger ControlID="btnprintmaster1" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    <%--progressBar for UpGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
