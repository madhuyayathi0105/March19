<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="UtilizationReport.aspx.cs" Inherits="LibraryMod_UtilizationReport" %>

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
                <span class="fontstyleheader" style="color: Green;">Utilization Report</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <center>
                    <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <center>
                                            <div>
                                                <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                                                    margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCollege" runat="server" Text="College">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblreporttype" runat="server" Text="Report Type">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlreporttype_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_department" Text="Department" runat="server" Font-Names="Book Antiqua"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_department" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_department" runat="server" CssClass="multxtpanel" Style="width: 170px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_department" runat="server" Width="200px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_department_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_department" runat="server" TargetControlID="txt_department"
                                                                PopupControlID="panel_department" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:Label ID="lblcount" runat="server" Text="Count">
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddlcount" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlcount_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtcount" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Height="24px" Width="50px" Visible="true"></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <fieldset style="width: 280px; height: 20px;">
                                                                <asp:CheckBox ID="cbdate" runat="server" AutoPostBack="true" OnCheckedChanged="cbdate_OnCheckedChanged" />
                                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From: "></asp:Label>
                                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                                    onchange="return checkDate()" Enabled="false"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                                <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"
                                                                    Enabled="false"></asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <fieldset style="width: 240px; height: 15px;">
                                                                <asp:RadioButtonList ID="rblmembertype" runat="server" RepeatDirection="Horizontal"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="rblmembertype_Selected" Enabled="false">
                                                                </asp:RadioButtonList>
                                                            </fieldset>
                                                        </td>
                                                        <td colspan="2">
                                                            <fieldset style="width: 220px; height: 15px;">
                                                                <asp:RadioButtonList ID="rbltype" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="rbltype_Selected">
                                                                </asp:RadioButtonList>
                                                            </fieldset>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_RollNo" Text="RollNo:" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_Title" Text="Title:" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lbl_AccessNo" Text="AccessNo:" runat="server" Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Height="24px" Width="200px" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txttitle" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Height="24px" Width="200px" Visible="false"></asp:TextBox>
                                                            <asp:TextBox ID="txtaccessno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Height="24px" Width="200px" Visible="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click"
                                                                        Style="margin-left: -101px;" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btn_go" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdatePanel ID="Upgrid" runat="server">
                <ContentTemplate>
                    <div id="showreport1" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="grdManualExit" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        Font-Names="Book Antiqua" toGenerateColumns="false" OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged"
                                        Width="980px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td>
                                <center>
                                    <div id="print" runat="server" visible="false">
                                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                        <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>--%>
                                        <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                            Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                        <NEW:NEWPrintMater runat="server" ID="Printcontrolhed" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="UpdatePanel16" runat="server">
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
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                </center>
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
    <%--progressBar for go and add--%>
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
    <%--progressBar for grid--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="Upgrid">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
