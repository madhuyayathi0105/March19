<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="BookPhoto.aspx.cs"
    Inherits="LibraryMod_BookPhoto" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptmanager" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green">Adding Book Photo</span>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="margin-left: 0px; margin-bottom: 10px; margin-top: 10px;
                        padding: 6px; font-family: Book Antiqua; font-weight: bold; width: 50%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlclg_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddllibrary_OnSelectedIndexedChange" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbooktype" runat="server" Text="Book Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbooktype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlbooktype_OnSelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem>Book</asp:ListItem>
                                    <asp:ListItem>Periodicals</asp:ListItem>
                                    <asp:ListItem>Project Book</asp:ListItem>
                                    <asp:ListItem>Non-Book Material</asp:ListItem>
                                    <asp:ListItem>Question Bank</asp:ListItem>
                                    <asp:ListItem>Back Volume</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlstatus_selectedindexchange">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Selected</asp:ListItem>
                                    <asp:ListItem>Not Selected</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsearch" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearchby_selectedindexchange" AutoPostBack="true">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Access No</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Author</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlsearchby1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearchby1_selectedindexchange" AutoPostBack="true"
                                    Visible="false">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Access No</asp:ListItem>
                                    <asp:ListItem>Journal Name</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlsearchby2" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearchby2_selectedindexchange" AutoPostBack="true">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Access No</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Type Of Project</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlsearchby4" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearchby4_selectedindexchange" AutoPostBack="true"
                                    Visible="false">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Access No</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Paper Name</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlsearchby5" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearchby5_selectedindexchange" AutoPostBack="true"
                                    Visible="false">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Access No</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Periodical Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtserach" runat="server" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtserach"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                    <%--    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground"--%>
                                </asp:AutoCompleteExtender>
                                <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlsearch_selectedindexchange" Visible="false">
                                </asp:DropDownList>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="Browsefile_div" runat="server" style="height: 100%; z-index: 1000; width: 100%;
                    height: 100em; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 193px; margin-left: -88px;"
                        OnClick="imagebtnpopclose_Click" />
                    <center>
                        <div id="div1" runat="server" class="table" style="background-color: White; height: 187px;
                            width: 330px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px; margin-left: -480px;">
                            <center>
                                <br />
                                <center>
                                    <asp:Label ID="lbl_header" runat="server" ForeColor="Green" Visible="true" Font-Size="Larger"></asp:Label>
                                </center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblfile_upload" runat="server" Text="Browse Image" Style="margin-left: 41px;"></asp:Label>
                                                    <asp:Image ID="imgstudp" runat="server" Style="width: 105px; height: 105px; position: absolute;
                                                        left: 448px; top: 272px;" />
                                                    <asp:FileUpload ID="fulstudp" runat="server" Style="position: absolute; top: 385px;
                                                        width: 183px; margin-left: -83px;" />
                                                    <asp:Button ID="Button1" runat="server" Text="Save" OnClick="BtnsaveStud_Click" Style="width: 62px;
                                                        position: absolute; left: 595px; top: 383px;" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="Button1" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div id="divtable" runat="server" visible="false">
                <center>
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdBookPhoto" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                        Font-Names="book antiqua" togeneratecolumns="false" AllowPaging="true" PageSize="50"
                        OnPageIndexChanging="grdBookPhoto_onpageindexchanged" OnRowCreated="grdBookPhoto_OnRowCreated"
                        Width="900px">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true" HWidth="50px">
                                    </asp:Label></center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Access No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_accno" runat="server" Text='<%#Eval("Access No") %>' Style="text-align: center"
                                            Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_title" runat="server" Text='<%#Eval("Title") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Author" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_author" runat="server" Text='<%#Eval("Author") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Journal Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_journal" runat="server" Text='<%#Eval("Journal Name") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type Of Project" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_type" runat="server" Text='<%#Eval("Type Of Project") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Paper Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_paper" runat="server" Text='<%#Eval("Paper Name") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Periodical Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_perodocal" runat="server" Text='<%#Eval("Periodical Name") %>'
                                            Style="text-align: left" Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Photo Path" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_PhotoPath" runat="server" Text='<%#Eval("Photo Path") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select Photo" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Button ID="btn_selectphoto" Text="Select Photo" OnClick="btn_edit_click" Style="text-align: center"
                                        runat="server" Width="105px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                    </asp:GridView>
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                 
                </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdBookPhoto" />
        </Triggers>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
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
</asp:Content>
