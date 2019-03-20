<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="Book_Availability.aspx.cs" Inherits="LibraryMod_Book_Availability" %>

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
                <span class="fontstyleheader" style="color: Green;">Book Availability</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 900px; height: auto">
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div style="width: 1000px; height: auto">
                                                <table class="maintablestyle" style="height: auto; margin-left: -105px; margin-top: 10px;
                                                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblclg" runat="server" Text="College">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="230px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbllibrary" runat="server" Text="Library:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="177px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_issuetype" runat="server" Text="Issue Type:">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_issuetype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                                Width="200px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddl_issuetype_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_accno" runat="server" Text="Access No:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_accno" runat="server" Height="20px" CssClass="textbox txtheight2"
                                                                Width="185px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getsearchaccno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_accno"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                                <%--    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground"--%>
                                                            </asp:AutoCompleteExtender>
                                                            <asp:Button ID="btn_accno" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_accno_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Title" runat="server" Text="Title:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Title" runat="server" Height="20px" CssClass="textbox txtheight2"
                                                                Width="169px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getsearchtitle" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_Title"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Author:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtauthor" runat="server" Height="20px" CssClass="textbox txtheight2"
                                                                Width="169px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getsearchauthor" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtauthor"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpGo" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
    </div>
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdBookAvail" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                    ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true"
                    PageSize="500" OnSelectedIndexChanged="grdBookAvail_onselectedindexchanged" OnPageIndexChanging="grdBookAvail_onpageindexchanged"
                    Width="980px">
                    <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                </asp:GridView>
                <br />
                <br />
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        Font-Size="Medium" CssClass="textbox txtheight2"></asp:TextBox>
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                        OnClick="btnExcel_Click" />
                    <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                        OnClick="btnprintmaster_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="popupselectBook" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em; font-family: Book Antiqua;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <br />
                    <div runat="server" style="background-color: White; height: 600px; width: 800px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                        margin-left: 150px">
                        <br />
                        <center>
                            <asp:Label ID="Label2" runat="server" Text="Select Access No" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lb_search" runat="server" Text="Search" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dd_search" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" Font-Size="Medium"
                                        AutoPostBack="true" OnSelectedIndexChanged="dd_search_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search_book" runat="server" Style="width: 154px; margin-left: 3px"
                                        CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search_book"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpGoBook" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_go_book" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btn_go_book_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div id="BookAcc" runat="server" style="height: 400px; width: 750px; overflow: auto;">
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="grdBookAccess" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                Font-Names="book antiqua" togeneratecolumns="false" AllowPaging="true" PageSize="100"
                                OnSelectedIndexChanged="grdBookAccess_onselectedindexchanged" OnRowDataBound="grdBookAccess_RowDataBound"
                                OnRowCreated="grdBookAccess_OnRowCreated" OnPageIndexChanging="grdBookAccess_onpageindexchanged"
                                Width="700px">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>">
                                            </asp:Label></center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Access No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_accessno" runat="server" Text='<%#Eval("Access No") %>' Width="80px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_title" runat="server" Text='<%#Eval("Title") %>' Width="150px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Journal Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_journal" runat="server" Text='<%#Eval("Journal Code") %>' Width="150px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Non Book No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <center>
                                                <asp:Label ID="lbl_nonbook" runat="server" Text='<%#Eval("Non Book No") %>' Width="150px"></asp:Label>
                                            </center>
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btn_Acc_exit1" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                            Visible="false" OnClick="btn_Acc_exit1_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
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
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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
    <%--progressBar for GO--%>
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
    <%--progressBar for UpGoBook--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpGoBook">
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
</asp:Content>
