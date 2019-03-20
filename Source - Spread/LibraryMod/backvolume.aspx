<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="backvolume.aspx.cs" Inherits="LibraryMod_backvolume"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function valid1() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txtjournal.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtjournal.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txtaccno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txtaccno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txttile.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txttile.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txtfromyear.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txtfromyear.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txttoyear.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txttoyear.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlmonthpublication.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlmonthpublication.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlto.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlto.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtjournal.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtjournal.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlperidical.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlperidical.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
   
    </script>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Back Volume</span></div>
                    </center>
                </div>
                <div>
                    <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                        margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="updatepanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblclg" runat="server" Text="College">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_selectedindexchange">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lbllibrary" runat="server" Text="Library">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblsearchby" runat="server" Text="SerachBy"></asp:Label>
                                        <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlsearch_selectedindexchange">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Acc No</asp:ListItem>
                                            <asp:ListItem>Title</asp:ListItem>
                                            <asp:ListItem>Datewise</asp:ListItem>
                                            <asp:ListItem>Departmentwise</asp:ListItem>
                                            <asp:ListItem>Year</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                            <asp:ListItem>Content</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtusernam" runat="server" Visible="false" Style="width: 115px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getsearchby" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtusernam"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Label ID="lblfrom" runat="server" Text="From:" Visible="false" Style="margin-left: 4px;"></asp:Label>
                                        <asp:TextBox ID="txt_fromdate1" runat="server" Visible="false" Style="height: 15px;
                                            width: 74px;" onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                            Format="dd/MMM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lbl_todate" Visible="false" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                        <asp:TextBox ID="txt_todate1" runat="server" Visible="false" Style="height: 15px;
                                            width: 74px;" onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                            Format="dd/MMM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddldept_OnselectedindexchangeD">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlstatus1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlstatus1_selectedindexchange">
                                            <asp:ListItem>Issued</asp:ListItem>
                                            <asp:ListItem>Available</asp:ListItem>
                                            <asp:ListItem>Lost</asp:ListItem>
                                            <asp:ListItem>Binding</asp:ListItem>
                                            <asp:ListItem>Condemn</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upadtepanelbutton1" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                        <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnadd_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="updatepanel5" runat="server">
        <ContentTemplate>
            <div id="div4" runat="server" visible="false">
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblnoofbooks" runat="server" Text="Total No Of Records:"></asp:Label>
                                <asp:Label ID="txtnoofbooks" runat="server" Enabled="false" Font-Names="Book Antiqua"
                                    AutoPostBack="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updatepanel6" runat="server">
        <ContentTemplate>
            <div id="divtable" runat="server" visible="false">
                <center>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <asp:GridView ID="grdBackVol" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        ShowHeader="true" Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true"
                                        PageSize="500" OnPageIndexChanging="grdBackVol_OnPageIndexChanged" OnRowCreated="grdBackVol_OnRowCreated"
                                        OnSelectedIndexChanged="grdBackVol_SelectedIndexChanged">
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    </asp:GridView>
                                    <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                    </asp:Label>
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
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grdBackVol" />
            <asp:PostBackTrigger ControlID="btn_Excel" />
            <asp:PostBackTrigger ControlID="btn_printmaster" />
        </Triggers>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="updatepanel7" runat="server">
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
                                        <asp:UpdatePanel ID="updatebuttonok" runat="server">
                                            <ContentTemplate>
                                                <td align="center">
                                                    <center>
                                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
                                                    </center>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="updatepanelbtnyes" runat="server">
                                            <ContentTemplate>
                                                <td align="center">
                                                    <center>
                                                        <asp:ImageButton ID="btnyes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnyes_Click" />
                                                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/LibImages/no (2).jpg" OnClick="btnNo_Click" />
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
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <center>
        <asp:UpdatePanel ID="updatepanel9" runat="server">
            <ContentTemplate>
                <div id="divPopAlertbackvolume" runat="server" visible="false" style="height: 550em;
                    z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0%; font-family: Book Antiqua; font-weight: bold">
                    <center>
                        <div id="divPopAlertback" runat="server" class="table" style="background-color: White;
                            height: 512px; width: 61%; border: 5px solid #0CA6CA; margin-right: auto; margin-left: auto;
                            border-top: 25px solid #0CA6CA; left: 23%; right: 39%; top: 10%; padding: 5px;
                            z-index: 1000; position: fixed; border-radius: 10px; font-family: Book Antiqua;
                            font-weight: bold">
                            <center>
                                <asp:Label ID="lblnonbook" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Back Volume Entry" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                            </center>
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lbljournal" runat="server" Text="Journal:" Style="margin-left: -2px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtjournal" runat="server" AutoPostBack="true" Width="100px" CssClass="textbox txtheight2"
                                                Enabled="false"  Style="margin-left: -29px"></asp:TextBox>
                                            <asp:CheckBox ID="chkjour" runat="server" Text="New Journal" AutoPostBack="true"
                                                OnCheckedChanged="chkjour_OncheckedChanged" Style="margin-left: -3px" />
                                            <asp:TextBox ID="txtnewjour" runat="server" Style="margin-left: 1px" Visible="false"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblperidical" runat="server" Text="Periodical Name:" Style="margin-left: 3px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlperidical" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" OnSelectedIndexChanged="ddlperidical_SelectedIndexChanged"
                                                Style="margin-left: -28px">
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="lnissues" Text="Issues" Font-Name="Book Antiqua" Font-Size="12pt"
                                                OnClick="lnkIssues_Click" runat="server" Width="20px" Style="margin-left: 2px"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblissuedate" runat="server" Text="Date Of Access" Style="margin-left: 4px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_accessdate2" runat="server" Style="height: 16px; width: 100px;
                                                margin-left: -27px" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txt_accessdate2"
                                                Format="dd/MMM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblacccno" runat="server" Text="Access No:" Style="margin-left: -1px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtaccno" runat="server" AutoPostBack="true" OnTextChanged="Txtaccno_OnTextChanged" Width="100px" CssClass="textbox txtheight2"
                                                Style="margin-left: -26px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lbltitle" runat="server" Text="Title:" Style="margin-left: -1px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txttile" runat="server" Width="180px" CssClass="textbox txtheight2"
                                                Style="margin-left: -25px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="Label1" runat="server" Text="Year From:" Style="margin-left: -1px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtfromyear" runat="server" Width="100px" MaxLength="4" CssClass="textbox txtheight2"
                                                Style="margin-left: -25px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="Txtfromyear"
                                                FilterType="Numbers" ValidChars="/">
                                            </asp:FilteredTextBoxExtender>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lbltoyear" runat="server" Text="To:" CssClass="commonHeaderFont" Font-Names=" Book antiqua"
                                                Style="margin-left: 20px">
                                            </asp:Label>
                                            <asp:TextBox ID="Txttoyear" runat="server" Width="77px" MaxLength="4" CssClass="textbox txtheight2"
                                                Style="margin-left: 59px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtfromyear"
                                                FilterType="Numbers" ValidChars="/">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="Label2" runat="server" Text="Month Of Publication:" Style="margin-left: -1px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlmonthpublication" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="110px" AutoPostBack="True" OnSelectedIndexChanged="ddlmonthpublication_SelectedIndexChanged"
                                                Style="margin-left: -24px">
                                                <asp:ListItem Value="1">January</asp:ListItem>
                                                <asp:ListItem Value="2">February</asp:ListItem>
                                                <asp:ListItem Value="3">March</asp:ListItem>
                                                <asp:ListItem Value="4">April</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">June</asp:ListItem>
                                                <asp:ListItem Value="7">July</asp:ListItem>
                                                <asp:ListItem Value="8">August</asp:ListItem>
                                                <asp:ListItem Value="9">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblto" runat="server" Text="To:" CssClass="commonHeaderFont" Font-Names=" Book antiqua"
                                                Style="margin-left: 20px">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlto" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="85px" AutoPostBack="True" OnSelectedIndexChanged="ddlto_SelectedIndexChanged"
                                                Style="margin-left: 59px">
                                                <asp:ListItem Value="1">January</asp:ListItem>
                                                <asp:ListItem Value="2">February</asp:ListItem>
                                                <asp:ListItem Value="3">March</asp:ListItem>
                                                <asp:ListItem Value="4">April</asp:ListItem>
                                                <asp:ListItem Value="5">May</asp:ListItem>
                                                <asp:ListItem Value="6">June</asp:ListItem>
                                                <asp:ListItem Value="7">July</asp:ListItem>
                                                <asp:ListItem Value="8">August</asp:ListItem>
                                                <asp:ListItem Value="9">September</asp:ListItem>
                                                <asp:ListItem Value="10">October</asp:ListItem>
                                                <asp:ListItem Value="11">November</asp:ListItem>
                                                <asp:ListItem Value="12">December</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblmissing" runat="server" Text="Missing Month & Year:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua" Style="margin-left: 2px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlmonth" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtmonth" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true" Style="margin-left: -22px;
                                                        height: 23px"></asp:TextBox>
                                                    <asp:TextBox ID="Txtyear" runat="server" Width="50px" Height="23px" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:Panel ID="pnlmonth" runat="server" CssClass="multxtpanel" Height="200px" Width="124px">
                                                        <asp:CheckBox ID="chkmonth" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkmonth_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblmonth" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkmonth_SelectedIndexChanged">
                                                            <asp:ListItem>January</asp:ListItem>
                                                            <asp:ListItem>February</asp:ListItem>
                                                            <asp:ListItem>March</asp:ListItem>
                                                            <asp:ListItem>April</asp:ListItem>
                                                            <asp:ListItem>May</asp:ListItem>
                                                            <asp:ListItem>June</asp:ListItem>
                                                            <asp:ListItem>July</asp:ListItem>
                                                            <asp:ListItem>August</asp:ListItem>
                                                            <asp:ListItem>September</asp:ListItem>
                                                            <asp:ListItem>October</asp:ListItem>
                                                            <asp:ListItem>November</asp:ListItem>
                                                            <asp:ListItem>December</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popupExtmonth" runat="server" TargetControlID="txtmonth"
                                                        PopupControlID="pnlmonth" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnAddyear" runat="server" Text="+" Font-Bold="true" Style="margin-left: -113px;
                                                width: 33px; height: 24px" OnClick="btnAddyear_OnClick" />
                                            <asp:UpdatePanel ID="UpView" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btnView" runat="server" ImageUrl="~/LibImages/view.jpg" OnClick="btnview_OnClick" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblvolno" runat="server" Text="Volume No:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua" Style="margin-left: 3px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtvolumeno" runat="server" Width="100px" CssClass="textbox txtheight2"
                                                Style="margin-left: -21px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblissue" runat="server" Text="Issue No:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua" Style="margin-left: 4px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtissueno" runat="server" Width="100px" CssClass="textbox txtheight2"
                                                Style="margin-left: -22px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblstatus" runat="server" Text="Status:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua" Style="margin-left: 5px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged" Style="margin-left: -22px">
                                                <asp:ListItem>Issued</asp:ListItem>
                                                <asp:ListItem Selected="True">Available</asp:ListItem>
                                                <asp:ListItem>Lost</asp:ListItem>
                                                <asp:ListItem>Binding</asp:ListItem>
                                                <asp:ListItem>Condemn</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblremark" runat="server" Text="Remark:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua" Style="margin-left: 5px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtremark" runat="server" Width="206px" CssClass="textbox txtheight2"
                                                Style="margin-left: -21px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsave_Click"
                                                        OnClientClick="return valid1()" />
                                                    <asp:ImageButton ID="btnupdate" runat="server" ImageUrl="~/LibImages/update (2).jpg"
                                                        OnClick="btnupdate_Click" Visible="false" />
                                                    <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                                        OnClick="btndelete_Click" Visible="false" />
                                                    <asp:ImageButton ID="Btnclose" runat="server" ImageUrl="~/LibImages/close.jpg" OnClick="btnclose_Click" />
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
        <asp:UpdatePanel ID="updatepanel8" runat="server">
            <ContentTemplate>
                <div id="divissues" runat="server" visible="false" style="height: 70px; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divissue" runat="server" class="table" style="background-color: White; border: 5px solid #0CA6CA;
                            border-top: 25px solid #0CA6CA; margin-left: auto; margin-right: auto; width: 900px;
                            height: auto; z-index: 1000; border-radius: 5px; margin-top: 65px">
                            <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                            <div id="div1issue" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:GridView ID="GrdIssues" Width="850px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                    Font-Names="Book Antiqua" toGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                </asp:GridView>
                                            </center>
                                            <br />
                                            <asp:Button ID="hide" runat="server" Text="Hide" Style="margin-left: 800px;" OnClick="hide_click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </br>
                            <table>
                                <tr>
                                    <asp:UpdatePanel ID="updatepanelexit" runat="server">
                                        <ContentTemplate>
                                            <td>
                                                <asp:ImageButton ID="btnex" runat="server" Visible="false" ImageUrl="~/LibImages/save (2).jpg"
                                                    OnClick="btn_ex_Click" />
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
        <asp:UpdatePanel ID="updatepanel11" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" visible="false" style="height: 70px; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="div2" runat="server" class="table" style="background-color: White; border: 5px solid #0CA6CA;
                            border-top: 25px solid #0CA6CA; margin-left: auto; margin-right: auto; width: 400px;
                            height: auto; z-index: 1000; border-radius: 5px; margin-top: 109px">
                            <asp:Label ID="Label4" runat="server" Visible="false"></asp:Label>
                            <div id="div3" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:GridView ID="GrdView" Width="350px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                    Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                </asp:GridView>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            </br>
                            <table>
                                <tr>
                                    <asp:UpdatePanel ID="updatepanelbutton2" runat="server">
                                        <ContentTemplate>
                                            <td>
                                                <asp:ImageButton ID="btnext" runat="server" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_ex_Click" />
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
    <%--progressBar for GO--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upadtepanelbutton1">
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
    <%--progressBar for save--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel10">
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
    <%--progressBar for View--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpView">
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
</asp:Content>
