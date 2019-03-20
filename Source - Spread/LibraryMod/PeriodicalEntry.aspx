<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="PeriodicalEntry.aspx.cs" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="LibraryMod_PeriodicalEntry" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
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
            id = document.getElementById("<%=ddllibname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddllibname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtaccess.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtaccess.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtjour.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtjour.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txttit.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txttit.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlsubsyr.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlsubsyr.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlissueyr.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlissueyr.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtmonth.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtmonth.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=TextBox1.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=TextBox1.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtissueno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtissueno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlattach.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlattach.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlstatus.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlstatus.ClientID %>");
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

        function SelLedgers() {
            var chkSelAll = document.getElementById("<%=chkGridSelectAll.ClientID %>");
            var tbl = document.getElementById("<%=grdPerEntry.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_grdPerEntry_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Periodical Entry</span>
            </div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                            margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold">
                            <tr>
                                <td colspan="0">
                                    <asp:Label ID="lblCollege" runat="server" Text="College:" Width="80px">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="0">
                                    <asp:Label ID="lbllibrary" runat="server" Text="Library:" Width="80px">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" Style="margin-left: -27px" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <span style="margin-left: 8px">Subs Year:</span>
                                    <asp:CheckBox ID="cbyear" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbyear_OnCheckedChanged"
                                        Style="margin-left: 5px;" />
                                    <asp:DropDownList ID="ddlyear" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="132px" Style="margin-left: -5px;" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsup" runat="server" Text="Suppliers:">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsup" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" OnSelectedIndexChanged="ddlsup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblissued" runat="server" Text="IssuedBy:" Width="80px">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlissued" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" OnSelectedIndexChanged="ddlissued_SelectedIndexChanged">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem Value="1">Daily</asp:ListItem>
                                        <asp:ListItem Value="2">Weekly</asp:ListItem>
                                        <asp:ListItem Value="3">Monthly</asp:ListItem>
                                        <asp:ListItem Value="4">Yearly</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbltype" runat="server" Text="Type:" Width="80px">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" Style="margin-left: -26px" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Journals</asp:ListItem>
                                        <asp:ListItem>News Paper</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsearch" runat="server" Text="Search by:" Width="102px" Style="margin-left: 10px">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsearch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="145px" AutoPostBack="True" OnSelectedIndexChanged="ddlsearch_SelectedIndexChanged"
                                        Style="margin-left: -22px">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Journal Code</asp:ListItem>
                                        <asp:ListItem>Journal Title</asp:ListItem>
                                        <asp:ListItem>Status</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsearch1" Visible="false" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlsearch1_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtsearch" runat="server" Visible="false" Style="width: 130px;"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:Label ID="lbllanguage" runat="server" Text="Language" Visible="false">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddllang" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                        Width="130px" AutoPostBack="True" OnSelectedIndexChanged="ddllang_SelectedIndexChanged">
                                        <asp:ListItem>English</asp:ListItem>
                                        <asp:ListItem>Tamil</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="Date From: "></asp:Label>
                                    <asp:CheckBox ID="cbdate1" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate1_OnCheckedChanged" />
                                    <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="width: 113px;"
                                        onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate1" runat="server"
                                        Format="yyyy-MM-dd" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 3px;" Width="77px"></asp:Label>
                                    <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="width: 112px;
                                        margin-left: -25px" onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate1" runat="server"
                                        Format="yyyy-MM-dd" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpGo" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-left: 104px;"
                                                OnClick="btn_go_Click" />
                                            <asp:ImageButton ID="Button1" runat="server" ImageUrl="~/LibImages/Add new.jpg" Style="margin-left: 7px;"
                                                OnClick="btn_add_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <span style="padding-right: 100px; margin-left: -260px; margin-top: 3px;">
                            <asp:CheckBox ID="chkGridSelectAll" runat="server" Text="SelectAll" Font-Names="book antiqua"
                                Visible="false" onchange="return SelLedgers();" Style="margin-left: -440px;" />
                        </span>
                        <div id="divtable" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpgrdPerEntry" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <asp:GridView ID="grdPerEntry" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        ShowHeader="true" Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true"
                                        PageSize="500" OnSelectedIndexChanged="grdPerEntry_onselectedindexchanged" OnPageIndexChanging="grdPerEntry_onpageindexchanged"
                                        Width="1067px" OnRowCreated="grdPerEntry_OnRowCreated" OnRowDataBound="grdPerEntry_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lbl_norec" runat="server" Text="Select" />
                                                    </asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="selectchk" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grdPerEntry" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
                        </div>
                        <asp:UpdatePanel ID="UpPrint" runat="server">
                            <ContentTemplate>
                                <asp:ImageButton ID="btnprint" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                    OnClick="btn_print_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </center>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div id="divsaveDetails" runat="server" visible="false" style="height: 70em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <br />
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divsaventry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 670px; height: auto; z-index: 1000; border-radius: 5px;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Periodical Entry </span>
                            </center>
                            <br />
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: 10px; margin-left: 5px;
                                position: relative; width: 650px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbllibname" runat="server" Text="Library Name:" Width="120px">

                                        </asp:Label>
                                        <asp:DropDownList ID="ddllibname" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddllibname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chkautocode" runat="server" Enabled="true" AutoPostBack="true"
                                            OnCheckedChanged="cbautocode_OnCheckedChanged" />
                                        <asp:Label ID="lblautocode" runat="server" Text="Auto Access No" Width="123px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblaccescode" runat="server" Text="Access Code:" Width="120px">
                                        </asp:Label>
                                        <asp:TextBox ID="txtaccess" runat="server" Visible="true" Style="width: 144px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbljourcode" runat="server" Text="Journal Code:" Width="100px">
                                        </asp:Label>
                                        <asp:TextBox ID="txtjour" runat="server" Visible="true" Style="width: 125px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:Label ID="Labellibcode" runat="server" Visible="false">
                                        </asp:Label>
                                        <asp:Button ID="btnjour" Text="?" CssClass=" textbox btn1" runat="server" OnClick="btn_jour_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbltit" runat="server" Text="Title:" Width="121px">
                                        </asp:Label>
                                        <asp:TextBox ID="txttit" runat="server" Visible="true" Style="width: 387px;" CssClass="textbox txtheight2"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblsuby" runat="server" Text="Subs Year:" Width="122px">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlsubsyr" runat="server" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddlsubsyear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblissuyr" runat="server" Text="Issue Year:" Width="100px">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlissueyr" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="133px" OnSelectedIndexChanged="ddlissueyear_SelectedIndexChanged" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Date From:" Width="119px"></asp:Label>
                                        <asp:CheckBox ID="chkdat" runat="server" Enabled="true" AutoPostBack="true" OnCheckedChanged="cbdate_OnCheckedChanged" />
                                        <asp:TextBox ID="TextBox3" runat="server" Enabled="false" Style="width: 115px;" onchange="return checkDate()"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TextBox3" runat="server"
                                            Format="dd-MMM-yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="Label2" runat="server" Text="To:" Style="width: 102px; margin-left: 23px;"></asp:Label>
                                        <asp:TextBox ID="TextBox4" runat="server" Enabled="false" Style="width: 126px; margin-left: 77px;"
                                            onchange="return checkDate()" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="TextBox4" runat="server"
                                            Format="dd-MMM-yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblmonth" runat="server" Text="Month:" Width="122px"></asp:Label>
                                        <asp:DropDownList ID="txtmonth" runat="server" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="txtmonth_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtmonth" runat="server" Enabled="true" Style="width: 100px;" CssClass="textbox txtheight2"></asp:TextBox>--%>
                                        <asp:Button ID="btnmonth" Text="?" CssClass=" textbox btn1" runat="server" OnClick="btn_month_Click" />
                                        <asp:Label ID="lblprice" runat="server" Text="Price:" Width="101px"></asp:Label>
                                        <asp:TextBox ID="txtprice" runat="server" Enabled="true" Style="height: 16px; width: 125px;
                                            margin-left: 10px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--<asp:TextBox ID="txtmon" runat="server" Enabled="true" Style="width: 138px; margin-left: 138px;"
                                    CssClass="textbox txtheight2"></asp:TextBox>--%>
                                        <asp:Label ID="lblPagesfrom" runat="server" Text="Pages From:" Width="104px"></asp:Label>
                                        <asp:TextBox ID="txtpagfrom" runat="server" Enabled="true" Style="height: 26px; width: 44px;
                                            margin-left: 29px;"></asp:TextBox>
                                        <asp:Label ID="lblpgto" runat="server" Text="Pages To :" Style="width: 79px; margin-left: 104px;"></asp:Label>
                                        <asp:TextBox ID="txtpgto" runat="server" Enabled="true" Style="width: 44px; margin-left: 36px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblRecieveddate" runat="server" Text="Recieved Date:" Width="121px"></asp:Label>
                                        <asp:TextBox ID="TextBox1" runat="server" Style="width: 145px;" onchange="return checkDate()"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="TextBox1" runat="server"
                                            Format="dd-MMM-yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblperiod" runat="server" Text="Periodicity:" Width="103px" Height="17px"></asp:Label>
                                        <asp:TextBox ID="txtperiod" runat="server" Enabled="false" Style="width: 131px; margin-left: 7px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblvol" runat="server" Text="Volume No:" Width="122px"></asp:Label>
                                        <asp:TextBox ID="txtvol" runat="server" Enabled="true" Style="width: 142px; margin-left: 9px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblissueno" runat="server" Text="Issue No:" Width="104px"></asp:Label>
                                        <asp:TextBox ID="txtissueno" runat="server" Enabled="true" Style="width: 44px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:Label ID="lblissn" runat="server" Text="ISSN No:" Width="82px"></asp:Label>
                                        <asp:TextBox ID="txtISSN" runat="server" Enabled="true" Style="width: 44px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblattach" runat="server" Text="Attachment:" Width="122px"></asp:Label>
                                        <asp:DropDownList ID="ddlattach" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddlattach_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblstatus" runat="server" Text="Status:" Width="104px"></asp:Label>
                                        <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="139px" AutoPostBack="True" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblremarks" runat="server" Text="Remarks:" Width="122px"></asp:Label>
                                        <asp:TextBox ID="txtremark" runat="server" Enabled="true" Style="width: 397px; margin-left: 10px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpSave" runat="server">
                                            <ContentTemplate>
                                                <center>
                                                    <asp:ImageButton ID="btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btn_Save_Click"
                                                        OnClientClick="return  valid1()" />
                                                    <asp:ImageButton ID="btnexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="btn_exit_Click" />
                                                </center>
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" class="popupstyle popupheight1" visible="false" style="height: 300em;
                    font-family: Book Antiqua;">
                    <br />
                    <center>
                        <div id="div2" runat="server" style="background-color: White; height: 600px; width: 800px;
                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                            margin-left: 150px">
                            <asp:Label ID="Label4" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Journal Pending List </span>
                            </center>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbllibname2" runat="server" Text="Library Name:" Width="120px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddllibname2" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddllibname2_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbljourname1" runat="server" Text="Journal Name:" Width="120px">
                                        </asp:Label>
                                        <asp:Label ID="Label_jc" runat="server" Visible="false">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtjourname1" runat="server" Enabled="false" Style="width: 144px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnjoucode" Text="?" CssClass=" textbox btn1" runat="server" OnClick="btnjoucode_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblsubyr3" runat="server" Text="Subs Year:" Width="122px">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubyr3" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="90px" OnSelectedIndexChanged="ddlsubyr3_SelectedIndexChanged">
                                            <asp:ListItem>2000</asp:ListItem>
                                            <asp:ListItem>2001</asp:ListItem>
                                            <asp:ListItem>2002</asp:ListItem>
                                            <asp:ListItem>2003</asp:ListItem>
                                            <asp:ListItem>2004</asp:ListItem>
                                            <asp:ListItem>2005</asp:ListItem>
                                            <asp:ListItem>2006</asp:ListItem>
                                            <asp:ListItem>2007</asp:ListItem>
                                            <asp:ListItem>2008</asp:ListItem>
                                            <asp:ListItem>2009</asp:ListItem>
                                            <asp:ListItem>2010</asp:ListItem>
                                            <asp:ListItem>2011</asp:ListItem>
                                            <asp:ListItem>2012</asp:ListItem>
                                            <asp:ListItem>2013</asp:ListItem>
                                            <asp:ListItem>2014</asp:ListItem>
                                            <asp:ListItem>2015</asp:ListItem>
                                            <asp:ListItem>2016</asp:ListItem>
                                            <asp:ListItem>2017</asp:ListItem>
                                            <asp:ListItem>2018</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblmonname" runat="server" Text="Month:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtmonth12" Width=" 138px" runat="server" CssClass="textbox txtheight2"
                                            ReadOnly="true"></asp:TextBox>
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
                                        <asp:PopupControlExtender ID="popupExtmonth" runat="server" TargetControlID="txtmonth12"
                                            PopupControlID="pnlmonth" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbl1issueno" runat="server" Text="Issue No:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtisnumber" runat="server" Enabled="true" Style="width: 100px;"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblmonissno" runat="server" Text="Month Issue No:" Width="130px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtmonissno" runat="server" Enabled="true" CssClass="textbox txtheight2"
                                            Style="width: 116px;"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rblStatus" runat="server" Visible="true" RepeatDirection="Horizontal"
                                            AutoPostBack="true" Style="width: 300px">
                                            <asp:ListItem>Received</asp:ListItem>
                                            <asp:ListItem Selected="True">Pending</asp:ListItem>
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpGoPending" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btngopendinglist" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                                    OnClick="btngopendinglist_Click" />
                                                <asp:ImageButton ID="btnexb" runat="server" ImageUrl="~/LibImages/save (2).jpg" OnClick="btnexb_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <div id="div3" runat="server" visible="false" style="height: 500px; width: 500px;
                                overflow: auto">
                                <table>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:GridView ID="grdJournalPending" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                    Font-Names="book antiqua" togeneratecolumns="true" Width="500px" OnSelectedIndexChanged="grdJournalPending_onselectedindexchanged"
                                                    OnRowCreated="grdJournalPending_OnRowCreated">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                                </asp:Label></center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                                </asp:GridView>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                                </br>
                                <table>
                                    <tr>
                                        <asp:UpdatePanel ID="UpdatePanelbtn6" runat="server">
                                            <ContentTemplate>
                                                <td>
                                                    <asp:ImageButton ID="btnk" runat="server" ImageUrl="~/LibImages/ok.jpg" Visible="false"
                                                        OnClick="btnk_okk1_Click" />
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div id="divjourcode" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em; font-family: Book Antiqua;">
                    <br />
                    <center>
                        <div id="divjourcod" runat="server" style="background-color: White; height: 600px;
                            width: 715px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                            margin-left: 150px">
                            <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 10px; bottom: 20px; font-family: Book Antiqua; text-align: center;
                                    color: Green; font-size: large; position: relative; font-weight: bold;">Journal
                                    Code </span>
                            </center>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcode" runat="server" Text="Code:" Style="margin-left: 4px;"></asp:Label>
                                        <asp:TextBox ID="txtcode" runat="server" Enabled="true" Style="width: 130px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:Label ID="lbllang" runat="server" Text="Language:" Style="margin-left: 4px;"></asp:Label>
                                        <asp:DropDownList ID="ddllan1" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddllan1_SelectedIndexChanged">
                                            <asp:ListItem>English</asp:ListItem>
                                            <asp:ListItem>Tamil</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lbltitle1" runat="server" Text="Title:" Style="margin-left: 4px;"></asp:Label>
                                        <asp:TextBox ID="txttit1" runat="server" Enabled="true" Style="width: 180px;" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdgo" runat="server" align="right">
                                        <asp:UpdatePanel ID="UpdatePanelbtn7" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btngo1" runat="server" ImageUrl="~/LibImages/GoWhite.jpg" OnClick="btn_go1_Click" />
                                                <asp:ImageButton ID="btnex" runat="server" ImageUrl="~/LibImages/save (2).jpg" OnClick="btn_ex_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <div id="div1jour" runat="server" visible="false" style="height: 400px; width: 700px;
                                overflow: auto">
                                <center>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                    <asp:GridView ID="grdJournalCode" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="100"
                                        Width="650px" OnSelectedIndexChanged="grdJournalCode_onselectedindexchanged"
                                        OnRowCreated="grdJournalCode_OnRowCreated">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                    </asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                </center>
                            </div>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
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
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
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
    <%--progressBar for Print--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpPrint">
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
    <%--progressBar for Save--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpSave">
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
    <%--progressBar for UpGoPending--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpGoPending">
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
