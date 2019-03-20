<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="LibraryInformation.aspx.cs"
    Inherits="LibraryMod_LibraryInformation" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptmanager" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">Library Holdings Information </span>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; margin-bottom: 10px; margin-top: 10px;
                        height: auto; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College" CssClass="commonheaderFont"
                                    Style="margin-left: 0px; width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 20px; width: 171px;" OnSelectedIndexChanged="ddlclg_Selectedindexchanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllib" runat="server" Text="Library" CssClass="commonheader" Style="margin-left: 5px;
                                    width: 80px"></asp:Label>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 6px; width: 150px" OnSelectedIndexChanged="ddllibrary_selectedindexchanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonheader"
                                    Style="margin-left: 7px; width: 80px"></asp:Label>
                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 1px; width: 150px" OnSelectedIndexChanged="ddldept_selectedindexchanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldate" runat="server" Text="Date" CssClass="commonheader" Style="margin-left: 1px;
                                    width: 80px"></asp:Label>
                                <asp:CheckBox ID="cbdate1" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate1_OnCheckedChanged" />
                                <asp:Label ID="lblfromdt" runat="server" Text="From:" CssClass="commonheader" Style="margin-left: 10px;
                                    width: 80px"></asp:Label>
                                <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                    onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                    Format="yyyy/MM/dd" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 3px;" Width="77px"></asp:Label>
                                <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="margin-left: -17px;"
                                    CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                    Format="yyyy/MM/dd" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btnGo_Click"
                                            Style="margin-left: 7px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divtable" runat="server" visible="false">
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    ShowHeader="false" Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true"
                                    PageSize="50" OnSelectedIndexChanged="gridview1_onselectedindexchanged" OnPageIndexChanging="gridview1_onpageindexchanged"
                                    Width="980px">
                                    <HeaderStyle BackColor="#0ca6ca" ForeColor="white" />
                                </asp:GridView>
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
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                        </asp:FilteredTextBoxExtender>--%>
                                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                            AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                                        <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                                            CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                                            Font-Bold="true" />
                                        <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                                    </center>
                                </div>
                </center>
                </td> </tr> </table> </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Excel" />
            <asp:PostBackTrigger ControlID="btn_printmaster" />
        </Triggers>
    </asp:UpdatePanel>
    <%--progressBar for upGo--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upGo">
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
