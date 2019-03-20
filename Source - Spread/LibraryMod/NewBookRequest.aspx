<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="NewBookRequest.aspx.cs"
    Inherits="LibraryMod_NewBookRequest" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" MasterPageFile="~/LibraryMod/LibraryMaster.master" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="head">
    <link href="Styles/css/Registration.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content2" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="scriptmanager" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <span class="fontstyleheader" style="color: Green;">New Book Request</span>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle" style="margin-bottom: 10px; margin-left: 0px; margin-top: 10px;
                        height: auto; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" CssClass="CommonHeaderFont" Text="College"
                                    Style="margin-left: -1px; width: 80px"></asp:Label>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 0px; width: 150px" OnSelectedIndexChanged="ddlclg_selectedIndex_changed">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" CssClass="CommonHeaderFont" Text="Library"
                                    Style="margin-left: 9px; width: 80px;"></asp:Label>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 0px; width: 150px" OnSelectedIndexChanged="ddllibrary_selectedindex_changed">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" CssClass="commonheaderFont" Text="Search By"
                                    Style="margin-left: 7px; width: 80px"></asp:Label>
                                <asp:DropDownList ID="ddlSearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="margin-left: 3px; width: 150px;" OnSelectedIndexChanged="ddlsearchby_selectedindex_changed"
                                    AutoPostBack="true">
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Title</asp:ListItem>
                                    <asp:ListItem>Author</asp:ListItem>
                                    <asp:ListItem>Publisher</asp:ListItem>
                                    <asp:ListItem>Req No</asp:ListItem>
                                    <asp:ListItem>Req Date</asp:ListItem>
                                    <asp:ListItem>Req StaffCode</asp:ListItem>
                                    <asp:ListItem>Req StaffName</asp:ListItem>
                                    <asp:ListItem>Department</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsearchby" runat="server" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchby"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:DropDownList ID="ddlsearch" runat="server" Style="margin-left: 0px; width: 150px;"
                                    Visible="false" OnSelectedIndexChanged="ddlsearch_selectedindex_changed">
                                </asp:DropDownList>
                                <asp:Label ID="lblfromdt" runat="server" Text="From:" CssClass="commonheader" Visible="false"
                                    Style="margin-left: 4px; width: 80px"></asp:Label>
                                <asp:TextBox ID="txt_fromdate1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                    onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbl_todate" runat="server" Text="To:" Visible="false" Style="margin-left: -8px;"
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txt_todate1" runat="server" Visible="false" CssClass="textbox txtheight2"
                                    onchange="return checkDate()"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnGo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_click" />
                                        <asp:ImageButton ID="btnAddnew" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnaddnew_click" />
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divaddnew" runat="server" visible="false" style="height: 70px; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <br />
                    <center>
                        <div id="divaddnew1" runat="server" class="table" style="background-color: White;
                            font-family: Book Antiqua; font-weight: bold; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-left: auto; margin-right: auto; width: 700px; height: auto; z-index: 1000;
                            border-radius: 5px;">
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Book Requisition</span>
                            </center>
                            <br />
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: 10px; margin-left: 10px;
                                position: relative; width: 680px; height: 382px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbllib" runat="server" CssClass="CommonHeaderFont" Text="Library"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:DropDownList ID="ddllib" runat="server" Style="margin-left: 15px;" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddllib_selectedIndex_changed">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrequisitionno" Visible="false" runat="server" Text="Requisition No"
                                            CssClass="commonheader" Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtrequisitionno" runat="server" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbltitle" runat="server" CssClass="CommonHeaderFont" Text="Title"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:TextBox ID="txttitle" runat="server" Style="margin-left: 23px;" CssClass="textbox txtheight2"
                                            OnTextChanged="txttit_ontextchanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblreqon" runat="server" Text="Requested On" CssClass="commonheader"
                                            Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtreqon" runat="server" CssClass="textbox txtheight2" Style="margin-left: 25px;"
                                            onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txtreqon" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblauthor" runat="server" CssClass="CommonHeaderFont" Text="Author"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:TextBox ID="txtauthor" runat="server" Style="margin-left: 6px; width: 200px;
                                            height: 20px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPrice" runat="server" CssClass="CommonHeaderFont" Text="Price"
                                            Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtPrice" runat="server" Style="margin-left: 95px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpublish" runat="server" CssClass="CommonHeaderFont" Text="Publisher"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:TextBox ID="txtpublish" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblnoofcopies" runat="server" CssClass="CommonHeaderFont" Text="No Of Copies"
                                            Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtnoofcopies" runat="server" CssClass="textbox txtheight2" Style="margin-left: 21px;"
                                            OnTextChanged="txtnoofcopies_changed" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblsupplier" runat="server" CssClass="CommonHeaderFont" Text="Supplier"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:TextBox ID="txtsupplier" runat="server" Style="margin-left: -4px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltotprice" runat="server" CssClass="CommonHeaderFont" Text="Total Price"
                                            Style="width: 39px;"></asp:Label>
                                        <asp:TextBox ID="txttotprice" runat="server" Style="margin-left: 54px;" CssClass="textbox txtheight2"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblreqstaff" runat="server" CssClass="CommonHeaderFont" Text="Requested By"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:Label ID="lblreqstaf" runat="server" CssClass="CommonHeaderFont" Text="(Staff Code)"
                                            Style="margin-left: -1px;"></asp:Label>
                                        <asp:TextBox ID="txtreqBystaff" runat="server" Style="margin-left: 0px; width: 79px;
                                            height: 20px;" OnTextChanged="txtreqbystaff_changed" AutoPostBack="true"></asp:TextBox>
                                        <asp:Button ID="btnreqstaff" runat="server" Style="margin-left: 0px; width: 30px;"
                                            Text="?" Font-Names="Book Antiqua" OnClick="btnreqstaff_click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblrecievedcopy" runat="server" CssClass="CommonHeaderFont" Text="Recieved Copies"
                                            Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtrecievedcopy" runat="server" Style="margin-left: 12px;" CssClass="textbox txtheight2"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpurpose" runat="server" CssClass="CommonHeaderFont" Text="Purpose"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:TextBox ID="txtpurpose" runat="server" Style="margin-left: 22px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblremcopy" runat="server" CssClass="CommonHeaderFont" Text="Remaining Copies"
                                            Style="width: 80px;"></asp:Label>
                                        <asp:TextBox ID="txtremcopy" runat="server" CssClass="textbox txtheight2" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldep" runat="server" CssClass="CommonHeaderFont" Text="Department"
                                            Style="margin-left: -1px; width: 80px"></asp:Label>
                                        <asp:DropDownList ID="ddldep" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddldep_selectedIndex_changed">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:UpdatePanel ID="UpSave" runat="server">
                                        <ContentTemplate>
                                            <td colspan="2" align="right">
                                                <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsave_click" />
                                                <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                    OnClick="btnexit_click" />
                                                <asp:ImageButton ID="btnupdate" runat="server" ImageUrl="~/LibImages/update.jpg"
                                                    OnClick="btnupdate_click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divstafflist" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="divstafflist1" runat="server" style="background-color: White; height: 600px;
                            width: 800px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Staff List</span>
                            </center>
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: 10px; margin-left: 4px;
                                position: relative; width: 520px; height: 40px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsearstaf" runat="server" CssClass="commonheaderFont" Text="Search By"
                                            Style="margin-left: 67px; width: 80px"></asp:Label>
                                        <asp:DropDownList ID="ddlsearstaff" runat="server" CssClass=" Dropdown commonHeaderFont"
                                            Style="margin-left: 3px; width: 150px;" OnSelectedIndexChanged="ddlsearstaff_selectedindex_changed"
                                            AutoPostBack="true">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Staff Name</asp:ListItem>
                                            <asp:ListItem>Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtsearstaff" runat="server" Style="margin-left: 0px; width: 150px;
                                            height: 15px" Visible="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btnseargo" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                                    OnClick="btnseargo_click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <div id="div2" runat="server" visible="false" style="height: 450px; width: 650px;
                                overflow: auto;">
                                <center>
                                    <asp:UpdatePanel ID="UpgrdStaff" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="SelectedGridCellgrdStaff" runat="server" Value="-1" />
                                            <asp:GridView ID="grdStaff" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStaff_onselectedindexchanged"
                                                Width="600px" OnRowCreated="grdStaff_OnRowCreated">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                            </asp:Label></center>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="grdStaff" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </center>
                            </div>
                            </br>
                            <table>
                                <tr>
                                    <asp:UpdatePanel ID="UpdatePanelbtn4" runat="server">
                                        <ContentTemplate>
                                            <td>
                                                <asp:ImageButton ID="btnex" runat="server" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_ex_Click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divtable" runat="server" visible="false">
                    <center>
                        <table>
                            <tr id="select_range" runat="server" visible="false" align="center">
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="Range
"></asp:Label>
                                    <asp:Label ID="Label14" runat="server" Text="From"></asp:Label>
                                    <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                                        MaxLength="4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_frange"
                                        FilterType="Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="Label15" runat="server" Text="To"></asp:Label>
                                    <asp:TextBox ID="txt_trange" CssClass="textbox
textbox1 txtheight" runat="server" MaxLength="4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_trange"
                                        FilterType="Numbers" ValidChars="/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="Btn_range" runat="server" Text="Select" CssClass="textbox1 textbox btn2"
                                        OnClick="Btn_range_Click" Style="margin-left: -1186px;" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btn_delete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                        OnClick="btn_delete_Click" Style="margin-left: -1010px;" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                                <asp:GridView ID="grdBookReq" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                    Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdBookReq_onselectedindexchanged"
                                                    Width="600px" OnRowCreated="grdBookReq_OnRowCreated">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="<%#Container.DataItemIndex+1
%>" Visible="true"> </asp:Label></center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkenbl" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="grdBookReq" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <center>
                                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False"> </asp:Label></center>
                                        <div id="div_report" runat="server" visible="false">
                                            <center>
                                                <asp:Label ID="lbl_reportname" runat="server" Text="Report
Name" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                                    CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                                    OnClick="btnExcel_Click" />
                                                <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                                    OnClick="btn_printmaster_Click" />
                                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                            </center>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <div id="divaddpopup" runat="server" visible="false" style="height: 100em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                        left: 0%; right: 0%;">
                        <center>
                            <div id="divaddpopup1" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 436px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%; font-family: Book Antiqua; font-weight: bold">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbladdalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="Book
Antiqua"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanelbtn6" runat="server">
                                                    <ContentTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="btnaddpopok" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnPopAlertaddok_Click" />
                                                            <asp:ImageButton ID="btnaddpopexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                                OnClick="btnPopAlertAddexit_Click" />
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
        <center>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="div1" runat="server" visible="false" style="height: 70px; z-index: 100;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                        left: 0px;">
                        <center>
                            <div id="div3" runat="server" class="table" style="background-color: White; border: 5px solid #0CA6CA;
                                border-top: 25px solid #0CA6CA; margin-left: auto; margin-right: auto; width: 550px;
                                height: auto; z-index: 1000; border-radius: 5px;">
                                <center>
                                    <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                        position: relative; font-weight: bold;">Select Books</span>
                                </center>
                                <div id="div4" runat="server" visible="false">
                                    <table style="margin: 10px; margin-bottom: 10px; margin-top: 13px; margin-left: 4px;
                                        position: relative; width: 510px; height: 316px; font-family: Book Antiqua; font-weight: bold">
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:GridView ID="gridview3" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                        Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                                        OnSelectedIndexChanged="gridview3_onselectedindexchanged" Width="980px" OnRowCreated="gridview3_OnRowCreated">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1
%>" Visible="true"> </asp:Label></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                                    </asp:GridView>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                </br>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanelbtn7" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btnok1" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_ok1_Click" />
                                                    <asp:ImageButton ID="btnexit1" runat="server" ImageUrl="~/LibImages/save
(2).jpg" OnClick="btn_exit_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <div>
            <center>
                <asp:UpdatePanel ID="updatepanel17" runat="server">
                    <ContentTemplate>
                        <div id="Diveleterecord" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <center>
                                        <table style="height: 100px; width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_Diveleterecord" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:UpdatePanel ID="Updetele_yes" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btn_detele_yes__record" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_detele_yes__record_Click" Text="yes" runat="server" />
                                                                <asp:Button ID="btn_detele_no__record" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_detele_no__recordClick" Text="no" runat="server" />
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
        </div>
        <center>
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                        left: 0%; right: 0%;">
                        <center>
                            <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%; font-family: Book Antiqua; font-weight: bold">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:UpdatePanel ID="UpdatePanelbtn5" runat="server">
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
        <%--progressBar for Save--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpSave">
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
        <%--progressBar for SearchGo--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn3">
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
        <%--progressBar for Ok--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn4">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
                PopupControlID="UpdateProgress4">
            </asp:ModalPopupExtender>
        </center>
</asp:Content>
