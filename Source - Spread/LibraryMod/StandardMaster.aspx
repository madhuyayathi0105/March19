<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="StandardMaster.aspx.cs" Inherits="LibraryMod_StandardMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <link href="~/Styles/css/Style.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function valid() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=ddllib.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddllib.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtreceiveddate.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtreceiveddate.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtaccessno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtaccessno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=txtcodeno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtcodeno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=ddltitle.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddltitle.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlstatus.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlstatus.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=dlldepartment.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=dlldepartment.ClientID %>");
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Standard Master</span></div>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
            <ContentTemplate>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <div>
                        <table class="maintablestyle" style="width: 980px; height: auto; background-color: #0CA6CA;
                            padding: 6px; margin: 0px; margin-bottom: 15px; margin-top: 10px; font-family: Book Antiqua;
                            font-weight: bold">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbllibraryname" runat="server" Text="Library Name">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Search By">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsearchname" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtsearchvalue" runat="server" Visible="false" AutoPostBack="true"
                                        CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchvalue"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:CheckBox ID="cbfrom" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="cbfrom_OnCheckedChanged" />
                                    <asp:Label ID="lbl_fromdate1" runat="server" Text="From Date:"></asp:Label>
                                    <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                        onchange="return checkDate()"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate1" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbl_todate1" runat="server" Text="To Date:" Style="margin-left: 4px;"></asp:Label>
                                    <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                        onchange="return checkDate()"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate1" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelbtn1" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btnMainGo" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                                OnClick="btnMainGo_Click" />
                                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/LibImages/Add new.jpg" Style="margin-top: 10px;"
                                                OnClick="btnAdd_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblerrmainapp" runat="server" Text="" Visible="false" ForeColor="Red"
                            CssClass="font"></asp:Label>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <br />
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="GrdstandMas" runat="server" ShowFooter="false" Width="900px" AutoGenerateColumns="true"
                    Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="true"
                    ShowHeader="true" OnRowDataBound="GrdstandMas_OnRowDataBound" OnSelectedIndexChanged="GrdstandMas_onselectedindexchanged"
                    OnRowCreated="GrdstandMas_OnRowCreated" AllowPaging="true" PageSize="500">
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div style="height: 20px;">
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div style="text-align: center;">
                <asp:Label ID="lbprint" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="return keyvalue(this)"
                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:ImageButton ID="btn_excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                    OnClick="btn_excel_Click" />
                <asp:ImageButton ID="btnprintmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                    OnClick="btnprintmaster_Click" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_excel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
        </Triggers>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divTarvellerEntryDetails" runat="server" visible="false" style="height: 70em;
                    z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divTarvellerEntry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 20px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 830px; height: 550px; font-family: Book Antiqua; font-weight: bold;
                            z-index: 1000; border-radius: 5px;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 3px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Standard Master </span>
                            </center>
                            <center>
                                <center>
                                    <table style="height: 100px; width: 100%; padding: 5px;">
                                        <tr>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lbllib" runat="server" Text="Library" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddllib" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblreceiveddate" runat="server" Text="Received Date" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtreceiveddate" runat="server" CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtreceiveddate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblaccessno" runat="server" Text="Access No." CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtaccessno" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                                    OnTextChanged="txtaccessno_OnTextChanged"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblcodeno" runat="server" Text="Code No." CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcodeno" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lbltitle" runat="server" Text="Title" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddtitle" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddtitle_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddltitle" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubtitle" runat="server" OnClick="btnsubtitle_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblcallno" runat="server" Text="Call No." CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcallno" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblothertitle" runat="server" Text="Other Title" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddothertitle" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddothertitle_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlothertitle" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubothertitle" runat="server" OnClick="btnsubothertitle_Click"
                                                    Font-Names="Book Antiqua" Font-Size="Small" Height="22px" Style="height: 23px;
                                                    position: absolute; width: 27px;" Text="-" />
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtyear" runat="server" CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtyear" runat="server"
                                                    Format="yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSubject" runat="server" Text="Subject" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddsubject" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddsubject_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlsubject" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubsubject" runat="server" OnClick="btnsubsubject_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lbledition" runat="server" Text="Edition" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtedition" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpublisher" runat="server" Text="Publisher" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddpublisher" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddpublisher_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlpublisher" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubpublisher" runat="server" OnClick="btnsubpublisher_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblpages" runat="server" Text="Pages" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpages" runat="server" AutoPostBack="true" OnTextChanged="txtprice_TextChanged"
                                                    CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" TargetControlID="txtpages"
                                                    FilterType="Numbers,Custom" ValidChars="." runat="server">
                                                    <%-- rajasekar --%>
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsupplier" runat="server" Text="Supplier" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsupplier" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblcountry" runat="server" Text="Country" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddcountry" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddcountry_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlcounty" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubcountry" runat="server" OnClick="btnsubcountry_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblinvoice" runat="server" Text="Invoice" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtinvoice" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddstatus" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddstatus_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubstatus" runat="server" OnClick="btnsubstatus_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcurrency" runat="server" Text="Currency" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnaddcurrency" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnaddcurrency_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlcurrency" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubcurrency" runat="server" OnClick="btnsubcurrency_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblcost" runat="server" Text="cost" CssClass="commonHeaderFont" Style="margin-left: 25px"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtcost" runat="server" AutoPostBack="true" MaxLength="50" CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" TargetControlID="txtcost"
                                                    FilterType="Numbers,Custom" ValidChars="." runat="server">
                                                    <%-- rajasekar --%>
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblprice" runat="server" Text="Price" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtprice" runat="server" AutoPostBack="true" MaxLength="50" OnTextChanged="txtprice_TextChanged"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtprice"
                                                    FilterType="Numbers,Custom" ValidChars="." runat="server">
                                                    <%-- rajasekar --%>
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lldiscount" runat="server" Text="Discount %" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdiscount" runat="server" AutoPostBack="true" MaxLength="50" OnTextChanged="txtdiscount_TextChanged"
                                                    CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txtdiscount"
                                                    FilterType="Numbers,Custom" ValidChars="." runat="server">
                                                    <%-- rajasekar --%>
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblnetprice" runat="server" Text="Net Price" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtnetprice" runat="server" Enabled="false" AutoPostBack="true"
                                                    CssClass="textbox txtheight2" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span style="color: Red;">*</span>
                                                <asp:Label ID="libdepartment" runat="server" Text="Department" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnadddept" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                                    Height="22px" Style="height: 23px; position: absolute; width: 27px;" OnClick="btnadddept_Click"
                                                    Text="+" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="dlldepartment" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnsubdept" runat="server" OnClick="btnsubdept_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Small" Height="22px" Style="height: 23px; position: absolute; width: 27px;"
                                                    Text="-" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblbudget" runat="server" Text="Budget" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbudget" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblbudgetyear" runat="server" Text="Budget Year" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtbudgetyear" runat="server" CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txtbudgetyear" runat="server"
                                                    Format="yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblkeywords" runat="server" Text="Keywords" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtkeywords" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbllocation" runat="server" Text="Location" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtlocation" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblremarks" runat="server" Text="Remarks" CssClass="commonHeaderFont"
                                                    Font-Names=" Book antiqua">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtremarks" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </center>
                            <center>
                                <table class="tablfont" style="top: 500px; margin-left: 390px; margin-top: 0px; position: absolute;
                                    width: 356px; height: 28px; border-color: Gray;">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanelbtn3" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btnnew" runat="server" ImageUrl="~/LibImages/Add new (2).jpg"
                                                        Enabled="true" OnClick="btnnew_Click" />
                                                    <asp:ImageButton ID="Btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="Btnsave_Click"
                                                        OnClientClick="return valid()" />
                                                    <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                                        OnClick="btndelete_Click" />
                                                    <asp:ImageButton ID="btn_exit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="btn_exit_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <center>
                                    <div id="plusdiv" runat="server" visible="false" style="height: 5680em; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                                                height: auto; width: 367px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 200px; border-radius: 10px;">
                                                <table>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_header1" runat="server" Style="color: Green; font: bold;" Text="REMARKS"></asp:Label>
                                                        </td>
                                                        <br />
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_addgroup" runat="server" Style="text-transform: capitalize;"
                                                                CssClass="textbox txtheight2"></asp:TextBox>
                                                            <%--onfocus=" return display(this)"--%>
                                                        </td>
                                                    </tr>
                                                    <br />
                                                    <tr>
                                                        <td align="center">
                                                            <asp:UpdatePanel ID="UpdatePanelbtn7" runat="server">
                                                                <ContentTemplate>
                                                                    <br />
                                                                    <asp:Button ID="btn_addgroup1" runat="server" Visible="true" CssClass="textbox btn1"
                                                                        Text="Add" OnClientClick="return checkadd()" OnClick="btn_addgroup_Click" />
                                                                    <asp:Button ID="btn_exitgroup1" runat="server" Visible="true" CssClass="textbox btn1"
                                                                        Text="Exit" OnClick="btn_exitaddgroup_Click" />
                                                                    <br />
                                                                    <br />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </div>
                                </center>
                                <center>
                                    <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                                border-radius: 10px;">
                                                <center>
                                                    <table style="height: 100px; width: 100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <center>
                                                                    <asp:UpdatePanel ID="UpdatePanelbtn8" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:ImageButton ID="btn_alertclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                                                OnClick="btn_alertclose_Click" />
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
                                </center>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Go and add--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn1">
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
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn3">
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
