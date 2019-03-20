<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="InvoiceReport.aspx.cs"
    Inherits="LibraryMod_InvoiceReport" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Invoice Report</span>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-bottom: 10px;
                        margin-top: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" CssClass="commonHeaderFont" Text="College"
                                    Style="width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlclg_selectedIndexchanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" CssClass="commonheaderfont" Text="Library"
                                    Style="width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddllib_selectedIndexchanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" CssClass="commonheaderfont" Text="Department"
                                    Style="width: 80px;">
                                </asp:Label>
                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2px">
                                <asp:Label ID="lblfrom" runat="server" CssClass="commonHeaderFont" Text="From:"></asp:Label>
                                <asp:TextBox ID="txtfrom" runat="server" CssClass="textbox txtheight2" Style="margin-left: 15px;
                                    width: 124px;" onchange="return checkdate()"></asp:TextBox>
                                <asp:CalendarExtender ID="calender1" TargetControlID="txtfrom" runat="server" Format="MM/dd/yyyy"
                                    CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:Label ID="lblto" runat="server" CssClass="commonHeaderFont" Text="To:"></asp:Label>
                                <asp:TextBox ID="txtto" runat="server" CssClass="textbox txtheight2" Style="margin-left: 29px"
                                    onchange="return checkdate()"></asp:TextBox>
                                <asp:CalendarExtender ID="calender2" TargetControlID="txtto" runat="server" Format="MM/dd/yyyy"
                                    CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td colspan="2px">
                                <asp:Label ID="Label1" runat="server" Text="Invoice No" CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:TextBox ID="txtsearch1" runat="server" CssClass="textbox txtheight2" Style="margin-left: 9px;"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanelSpread" runat="server">
            <ContentTemplate>
                <div id="divtable" runat="server" visible="false">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <center>
                                            <asp:GridView ID="grid_Details" runat="server" ShowFooter="false"  ShowHeader="true"
                                                Font-Names="Book Antiqua"  AllowPaging="true" PageSize="500"
                                                OnSelectedIndexChanged="grdManualExit_OnSelectedIndexChanged" OnPageIndexChanging="grdManualExit_OnPageIndexChanged"
                                                Width="980px">
                                             
                                                <HeaderStyle BackColor="#0CA6CA" ForeColor="White" />
                                            </asp:GridView>
                                        </center>
                                      
                                        </br>
                                        <center>
                                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                            </asp:Label></center>
                                        <div id="div_report" runat="server" visible="false">
                                            <center>
                                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                                    CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                                    OnClick="btnExcel_Click" />
                                                <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                                    OnClick="btn_printmaster_Click" />
                                              <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                                            </center>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
              <Triggers>
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                                <ContentTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
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
    <%--progressBar for Go--%>
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
