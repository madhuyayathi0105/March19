<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="InvigilationReport.aspx.cs" Inherits="MarkMod_InvigilationReport" %>

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
                <span class="fontstyleheader" style="color: Green;">Invigilation Report</span>
            </div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_cycletest" Text="Test" runat="server" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_cycletest" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_cycletest" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_cycletest" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_cycletest" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_cycletest_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_cycletest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_cycletest_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_cycletest" runat="server" TargetControlID="txt_cycletest"
                                                                PopupControlID="panel_cycletest" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_hall" Text="Hall" runat="server" Font-Names="Book Antiqua"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UP_hall" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_hall" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel_hall" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                                                height: auto;">
                                                                <asp:CheckBox ID="cb_hall" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_hall_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="cbl_hall" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hall_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pce_hall" runat="server" TargetControlID="txt_hall"
                                                                PopupControlID="panel_hall" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTestSession" runat="server" Text="Sessions" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSession" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"
                                                                    AutoPostBack="True" Width="100px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStaff" runat="server" Text="StaffName" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlStaff" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlStaff_SelectedIndexChanged"
                                                                    AutoPostBack="True" Width="100px">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="height: 21px; width: 82px; right: 909px;">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Height="20px" Width="75px" Style="" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="height: 21px; width: 76px">
                                                    </asp:Label>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="txtToDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Width="80px" OnTextChanged="txtToDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                                                        Text="Go" OnClick="btnGo_Click" Style="width: auto; height: auto;" />
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
        <br />
        <br />
        <center>
            <div id="showreport2" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                OnSelectedIndexChanged="gridview1_onselectedindexchanged" OnPageIndexChanging="gridview1_onpageindexchanged"
                                Width="980px">
                                <Columns>
                                   
                                  
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="print2" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:Button ID="btnExcel2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click2" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed2" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click2" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
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
        </center>
    </div>
</asp:Content>
