<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="individualbookcirculation.aspx.cs" Inherits="LibraryMod_individualbookcirculation" %>

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
                <span class="fontstyleheader" style="color: Green;">Individual Student Book Circulation
                    Report</span></div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbdate" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate_OnCheckedChanged" />
                            <asp:Label ID="lbl_fromdate" runat="server" Text="From: "></asp:Label>
                            <asp:TextBox ID="txt_fromdate" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                onchange="return checkDate()"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                            <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                            <asp:TextBox ID="txt_todate" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                onchange="return checkDate()"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddldegree" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="commonHeaderFont"
                                Style="margin-left: 17px">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlbranch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" Style="margin-left: -300px;">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Name" CssClass="commonHeaderFont">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsearchname" runat="server" CssClass="textbox txtheight2" AutoPostBack="True"
                                OnTextChanged="txtsearch1_TextChanged"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getrno1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchname"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td colspan="3px">
                            <asp:Label ID="lblsearch" runat="server" Text="By Search" CssClass="commonHeaderFont">
                            </asp:Label>
                            <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:TextBox ID="txtsearchroll" runat="server" Visible="true" CssClass="textbox txtheight2"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchroll"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpGo" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="btn_go" ImageUrl="~/LibImages/Go.jpg" runat="server" Style="margin-left: -134px"
                                        OnClick="btn_go_Click" />
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
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="gridview1" runat="server" ShowFooter="false" ShowHeader="true"
                    AutoGenerateColumns="true" Font-Names="book antiqua" togeneratecolumns="true"
                    OnRowCreated="gridview1_OnRowCreated" OnSelectedIndexChanged="gridview1_onselectedindexchanged"
                    Width="980px">
                    <%--OnPageIndexChanging="gridview1_onpageindexchanged"--%>
                    <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gridview1" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <center>
                    <div id="popwindowdetails" runat="server" visible="false" class="popupstyle popupheight1">
                        <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                            OnClick="imagebtnpop2close_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 750px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <span style="color: Green;" class="fontstyleheader">View Details</span></div>
                            </center>
                            <center>
                                <table id="colour" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <fieldset id="Fieldset8" runat="server" style="width: 33px; font-family: Book Antiqua;
                                                height: 7px; background-color: LightYellow;">
                                                <asp:Label ID="Label6" runat="server" Text="Returned"></asp:Label>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="Fieldset6" runat="server" enabled="false" style="width: 33px; font-family: Book Antiqua;
                                                height: 7px; background-color: LightGreen;">
                                                <asp:Label ID="Label3" runat="server" Text="Issued"></asp:Label>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="Fieldset7" runat="server" enabled="false" style="width: 33px; font-family: Book Antiqua;
                                                height: 7px; background-color: Red;">
                                                <asp:Label ID="Label7" runat="server" Text="Lost"></asp:Label>
                                            </fieldset>
                                        </td>
                                          <td>
                                            <fieldset id="Fieldset1" runat="server" enabled="false" style="width: 86px; font-family: Book Antiqua;
                                                height: 7px; background-color:Orange;">
                                                <asp:Label ID="Label2" runat="server" Text="Late Return"></asp:Label>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <div id="grid2div" runat="server" style="height: 600px; overflow: auto; width: 850px;">
                                            <asp:GridView ID="gridview2" runat="server" ShowFooter="false" ShowHeader="true"
                                                Style="height: 600px; width: auto; font-family: Book Antiqua;" Font-Names="book antiqua"
                                                Visible="false" OnRowDataBound="gridview2_OnRowDataBound">
                                                <HeaderStyle BackColor="#0ca6ca" ForeColor="Black" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <div id="print2" runat="server" visible="false">
                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel2" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click2" />
                    <asp:ImageButton ID="btnprintmasterhed2" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click2" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel2" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed2" />
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
                                        <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                            <ContentTemplate>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                    </center>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
