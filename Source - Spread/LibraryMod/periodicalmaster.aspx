<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="periodicalmaster.aspx.cs" Inherits="LibraryMod_periodicalmaster" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function frelig1() {
            document.getElementById('<%= Btnaddcurrency.ClientID%>').style.display = 'block';
            document.getElementById('<%=Btnsubcurrency.ClientID%>').style.display = 'block';
        }
        function frelig2() {
            document.getElementById('<%= btnadddept.ClientID%>').style.display = 'block';
            document.getElementById('<%=Btnsubdept.ClientID%>').style.display = 'block';
        }
        function frelig3() {
            document.getElementById('<%= btnadpub1 .ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsubpub.ClientID%>').style.display = 'block';
        }
        function frelig4() {
            document.getElementById('<%= btnadjourtype .ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsubjourtype.ClientID%>').style.display = 'block';
        }
        function frelig5() {
            document.getElementById('<%= Btnaddsubj .ClientID%>').style.display = 'block';
            document.getElementById('<%=Btnsubsubject.ClientID%>').style.display = 'block';
        }
        function frelig6() {
            document.getElementById('<%= btnlanuage .ClientID%>').style.display = 'block';
            document.getElementById('<%=Btnsublanguage.ClientID%>').style.display = 'block';
        }
        function frelig7() {
            document.getElementById('<%= btnadddelivery .ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsubdel.ClientID%>').style.display = 'block';
        }
        function frelig8() {
            document.getElementById('<%= btnaddperiod .ClientID%>').style.display = 'block';
            document.getElementById('<%=btnsubperiod.ClientID%>').style.display = 'block';
        }

        function valid2() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=txtpercode.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtpercode.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtpertitle.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtpertitle.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlpublish.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlpublish.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=ddldepartment.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddldepartment.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlpri.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlpri.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlis.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlis.ClientID %>");
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
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
                    margin-bottom: 15px; margin-top: 10px;">Periodical Master Entry</span>
            </div>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div class="maintablestyle" style="width: 930px; margin-left: 0px; margin-top: 10px;
                    margin-bottom: 10px; padding: 6px; font-family: Book Antiqua; font-weight: bold;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College" Width="80px" Style="margin-left: 0px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 140px; margin-left: -20px;" AutoPostBack="true" OnSelectedIndexChanged="ddlclg_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblLibrary" runat="server" Text="Library" Width="105px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 140px; height: 30px;" AutoPostBack="true" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblissueby" runat="server" Text="IssueBy" Width="78px"></asp:Label>
                                <asp:DropDownList ID="ddlissueby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 140px; height: 30px;" OnSelectedIndexChanged="ddlissueby_SelectedIndexChanged">                                    
                                    <asp:ListItem Value="1">Daily</asp:ListItem>
                                    <asp:ListItem Value="2">Weekly</asp:ListItem>
                                    <asp:ListItem Value="3">Monthly</asp:ListItem>
                                    <asp:ListItem Value="4">Yearly</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="5">All</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblpubtype" runat="server" Text="PubType"></asp:Label>
                                <asp:DropDownList ID="ddlpubtype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: 140px; height: 30px;" OnSelectedIndexChanged="ddlpubby_SelectedIndexChanged">
                                    <asp:ListItem>National</asp:ListItem>
                                    <asp:ListItem>International</asp:ListItem>
                                    <asp:ListItem Selected="True">Both</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbltitle" runat="server" Text="Title" Width="78px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttitle" runat="server" Style="width: 136px; height: 25px; margin-left: -20px;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getsearchtitle" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txttitle"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblJournal" runat="server" Text="JournalType" Width="105px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddljournaltype" runat="server" Style="width: 140px; height: 30px;"
                                    OnSelectedIndexChanged="ddljournaltype_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <asp:UpdatePanel ID="updatepanel4" runat="server">
                                <ContentTemplate>
                                    <td colspan="2">
                                        <asp:Label ID="lblsearchby" runat="server" Text="SearchBy" Width="80px"></asp:Label>
                                        <asp:DropDownList ID="ddlsearchby" runat="server" Style="width: 140px; height: 30px;"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                            <asp:ListItem>Journal Code</asp:ListItem>
                                            <asp:ListItem>Language</asp:ListItem>
                                            <asp:ListItem>Title Language</asp:ListItem>
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="Txtsearchby" runat="server" AutoPostBack="true" Width="99px" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getsearchjournalcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txtsearchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:DropDownList ID="ddlsearch" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlsearch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblType" runat="server" Text="Type" Width="78px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Font-Bold="false" Style="width: 140px;
                                    height: 30px; margin-left: -20px;" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                    <asp:ListItem>Newspaper</asp:ListItem>
                                    <asp:ListItem>Journals</asp:ListItem>
                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSub" runat="server" Text="SubscribeIn"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkSubscribe" Visible="true" runat="server" AutoPostBack="True"
                                            OnCheckedChanged="chkSub_CheckedChanged" />
                                        <asp:DropDownList ID="ddlYear" runat="server" Style="width: 115px; height: 30px;"
                                            OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblstatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblStatus" runat="server" Style="margin-left: -155px;" Visible="true"
                                    RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>InActive</asp:ListItem>
                                    <asp:ListItem Selected="True">Both</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="right">
                                <asp:UpdatePanel ID="updatepanel52" runat="server">
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
    <center>
        <asp:UpdatePanel ID="updatepanel51" runat="server">
            <ContentTemplate>
                <div id="divtable" runat="server" visible="false">
                    <center>
                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                        <asp:GridView ID="grdperiodical" Width="1200px" runat="server" ShowFooter="false"
                            AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="false"
                            AllowPaging="true" PageSize="100" OnPageIndexChanging="grdperiodical_OnPageIndexChanged"
                            ShowHeader="true" OnRowCreated="grdperiodical_OnRowCreated" OnSelectedIndexChanged="grdperiodical_SelectedIndexChanged">
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                        <center>
                            <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                            </asp:Label></center>
                        <asp:UpdatePanel ID="updatepanel15" runat="server">
                            <ContentTemplate>
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
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="grdperiodical" />
                                <asp:PostBackTrigger ControlID="btn_Excel" />
                                <asp:PostBackTrigger ControlID="btn_printmaster" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel12" runat="server">
            <ContentTemplate>
                <div id="addtbl" runat="server" visible="false" style="height: 70em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <center>
                        <div id="divsaventry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 920px; height: auto; z-index: 1000; border-radius: 5px;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-family: Book Antiqua;
                                    font-size: large; position: relative; font-weight: bold;">Periodical Entry </span>
                            </center>
                            <table style="margin: 10px; margin-bottom: 10px; margin-top: 10px; margin-left: 5px;
                                position: relative; width: 832px; height: 658px; font-family: Book Antiqua; font-weight: bold">
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblperiodicalcode" runat="server" Width="100px" Text="PeriodicalCode">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpercode" runat="server" Width="200px" Style="margin-left: 1px;"
                                            CssClass="textbox txtheight2" AutoPostBack="true" OnTextChanged="txtpercode_OnTextChanged"
                                            MaxLength="25"></asp:TextBox>
                                    </td>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbllibraryname" runat="server" Text="Library Name">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddllibararyname" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddllibararyname_SelectedIndexChanged"
                                            Style="width: 185px; height: 30px;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbnltitle" runat="server" Text="Title">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtpertitle" runat="server" Width="200px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlengtam" runat="server" Style="width: 113px; height: 30px;"
                                            CssClass="textbox ddlstyle ddlheight3" OnSelectedIndexChanged="ddlengtam_SelectedIndexChanged">
                                            <asp:ListItem>English</asp:ListItem>
                                            <asp:ListItem>Tamil</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldepartment" runat="server" Text="Department">
                                        </asp:Label>
                                        <span style="color: Red;">^</span>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnadddept" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 115px; position: absolute; top: 87px;
                                            width: 28px;" OnClick="btnadddepartment_Click" Text="+" />
                                        <asp:DropDownList ID="ddldepartment" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="Btnsubdept" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 341px; position: absolute; top: 87px;
                                            width: 27px;" OnClick="btnsubdepartment_Click" Text="-" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblpubl" runat="server" Text="Publisher">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnadpub1" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 115px; position: absolute; top: 127px;
                                            width: 27px;" OnClick="btnadddpub_Click" Text="+" />
                                        <asp:DropDownList ID="ddlpublish" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlpublish_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnsubpub" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 341px; position: absolute; top: 127px;
                                            width: 27px;" OnClick="btnsubpub_Click" Text="-" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbljour" runat="server" Text="Journal Type">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnadjourtype" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Style="height: 30px; display: none; left: 573px; position: absolute; top: 127px;
                                            width: 27px;" OnClick="btnaddjourtype_Click" Text="+" />
                                        <asp:DropDownList ID="ddljour" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="170px" OnSelectedIndexChanged="ddljour_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnsubjourtype" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 797px; position: absolute; top: 127px;
                                            width: 27px;" OnClick="btnsubjourtype_Click" Text="-" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="Btnaddsubj" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 117px; position: absolute; top: 167px;
                                            width: 27px;" OnClick="btnaddsubject_Click" Text="+" />
                                        <asp:DropDownList ID="ddlsubject" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="Btnsubsubject" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 339px; position: absolute; top: 167px;
                                            width: 27px;" OnClick="Btnsubsubject_Click" Text="-" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbllang" runat="server" Text="Language">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnlanuage" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 573px; position: absolute; top: 169px;
                                            width: 27px;" OnClick="btnaddlanguage_Click" Text="+" />
                                        <asp:DropDownList ID="ddllanguage" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddllanguage_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="Btnsublanguage" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 798px; position: absolute; top: 169px;
                                            width: 27px;" OnClick="btnsublanguage_Click" Text="-" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcurrencytype" runat="server" Text="Currency Type">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="Btnaddcurrency" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 116px; position: absolute; top: 207px;
                                            width: 27px;" OnClick="btnaddtypecurrency_Click" Text="+" />
                                        <asp:DropDownList ID="ddlcurrencytype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlcurrencytype_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px;">
                                        </asp:DropDownList>
                                        <asp:Button ID="Btnsubcurrency" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 341px; position: absolute; top: 207px;
                                            width: 27px;" OnClick="btnsubcurrencytype_Click" Text="-" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblcurrencyvalue" runat="server" Text="Currency Value">
                                        </asp:Label>
                                        <asp:TextBox ID="txtcurrencyvalue" runat="server" Width="65px" CssClass="textbox txtheight2"
                                            Style="width: 50px; height: 20px; margin-left: 3px;" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtcurrencyvalue"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lblindaian" runat="server" Text="Indian Price">
                                        </asp:Label>
                                        <asp:TextBox ID="Txtindaianprice" runat="server" CssClass="textbox txtheight2" Style="width: 40px;
                                            height: 20px; margin-left: 15px" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtIndianpriceFilteredTextBoxExtender" runat="server"
                                            TargetControlID="Txtindaianprice" FilterType="Custom,Numbers" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubs" runat="server" Text="Subs Amount">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsubsam" runat="server" CssClass="textbox txtheight2" Style="width: 200px;
                                            height: 20px;" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtsubsam"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsupp" runat="server" Text="Supplier:">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel30" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlsupp" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Style="width: 200px; height: 30px;">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblpubtyp" runat="server" Text="PublishType">
                                        </asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButtonList ID="rblpubty" runat="server" Visible="true" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">National Periodical</asp:ListItem>
                                            <asp:ListItem>International Periodical</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Country">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlccountry" CssClass="textbox3 textbox1" runat="server" onchange="nation1change()"
                                            onblur="blurFunction(this)" onkeydown="return (event.keyCode!=13);" onfocus="myFunction(this)"
                                            Style="width: 200px; float: left;">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_ccountry" CssClass="textbox textbox1" runat="server" onblur="blurFunction(this)"
                                            Style="display: none; float: left;" placeholder="Other" MaxLength="50" onfocus="myFunction(this)"
                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender38" runat="server" TargetControlID="txt_ccountry"
                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="ddldev" runat="server" Text="Delivery Type"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnadddelivery" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 117px; position: absolute; top: 366px;
                                            width: 27px;" OnClick="btnadddeliv_Click" Text="+" />
                                        <asp:DropDownList ID="ddldevt" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddldevt_SelectedIndexChanged" Style="width: 200px; height: 30px">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnsubdel" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 340px; position: absolute; top: 366px;
                                            width: 27px;" OnClick="btnsubdel_Click" Text="-" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblissn" runat="server" Text="ISSN"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtissn" runat="server" CssClass="textbox txtheight2" Style="width: 147px;
                                            height: 20px" MaxLength="25"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblpri" runat="server" Text="Prioridicity" CssClass="commonHeaderFont"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnaddperiod" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 117px; position: absolute; top: 406px;
                                            width: 27px;" OnClick="btnaddperiod_Click" Text="+" />
                                        <asp:DropDownList ID="ddlpri" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            OnSelectedIndexChanged="ddlpri_SelectedIndexChanged" Style="width: 200px; height: 30px">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnsubperiod" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="30px" Style="display: none; left: 340px; position: absolute; top: 406px;
                                            width: 27px;" OnClick="btnsubperiod_Click" Text="-" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="Lnkbutton" Text="Selectlist" Font-Size="11pt" OnClick="Lnkbutton_Click"
                                                    runat="server" Width="63px" Visible="false" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblis" runat="server" Text="Issue By"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlis" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlis_SelectedIndexChanged" Style="width: 200px;
                                            height: 30px">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblperis" runat="server" Text="Per Issue"> </asp:Label>
                                        <asp:TextBox ID="txtperis" runat="server" CssClass="textbox txtheight2" Style="width: 42px;
                                            height: 20px" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtperis"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lbltotnois" runat="server" Text="Total No Of Issue:"> </asp:Label>
                                        <asp:TextBox ID="txttotnois" runat="server" CssClass="textbox txtheight2" Style="width: 59px;
                                            height: 20px;" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txttotnois"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="Lblyear" runat="server" Text="Year:" Visible="false"> </asp:Label>
                                        <asp:TextBox ID="txtyear" runat="server" CssClass="textbox txtheight2" Style="width: 76px;
                                            height: 20px;" MaxLength="10" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblisty" runat="server" Text="Issue Type"> </asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList ID="rblisty" runat="server" Visible="true" RepeatDirection="Horizontal">
                                            <asp:ListItem>Datewise</asp:ListItem>
                                            <asp:ListItem>Daywise</asp:ListItem>
                                            <asp:ListItem>Monthwise</asp:ListItem>
                                            <asp:ListItem Selected="True">For Every</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdays" runat="server" CssClass="textbox txtheight2" Style="width: 60px;
                                            height: 20px" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtdays"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Label ID="lbldays" runat="server" Text="Days"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblremark" runat="server" Text="Remarks"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtremark" runat="server" CssClass="textbox txtheight2" Style="width: 200px;
                                            height: 20px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbljoweb" runat="server" Text="Journal Website"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtjoweb" runat="server" CssClass="textbox txtheight2" Style="width: 200px;
                                            height: 20px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rbljrweb" runat="server" Visible="true" RepeatDirection="Horizontal"
                                            Style="width: 147px; height: 30px">
                                            <asp:ListItem Selected="True">Active</asp:ListItem>
                                            <asp:ListItem>Inactive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblty" runat="server" Text="Type"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblty" runat="server" Visible="true" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True">Journals</asp:ListItem>
                                            <asp:ListItem>Newspaper</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                                    <center>
                                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                            margin-top: 200px; border-radius: 10px;">
                                            <table style="line-height: 30px">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_addgroup" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox txtheight2"
                                                            onkeypress="display1()"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:UpdatePanel ID="updatepanel7" runat="server">
                                                        <ContentTemplate>
                                                            <td align="center" style="line-height: 35px">
                                                                <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                                                <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
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
                            <table style="margin-bottom: 16px; margin-top: -2px;">
                                <tr>
                                    <asp:UpdatePanel ID="updatepanel8" runat="server">
                                        <ContentTemplate>
                                            <td align="center">
                                                <asp:ImageButton ID="Btnpersave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="Btnpersave_Click"
                                                    OnClientClick="return valid2()" />
                                                <asp:ImageButton ID="btnupdate" runat="server" ImageUrl="~/LibImages/update.jpg"
                                                    OnClick="Btnperupdate_Click" Visible="false" />
                                                <asp:ImageButton ID="btnperclose" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                    OnClick="btnperclose_Click" />
                                                <asp:ImageButton ID="btndelete" runat="server" ImageUrl="~/LibImages/delete.jpg"
                                                    OnClick="btndelete_Click" Visible="false" />
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
        <asp:UpdatePanel ID="updatepanel2" runat="server">
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
                                            <center>
                                                <asp:UpdatePanel ID="updatepanel9" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
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
    <center>
        <asp:UpdatePanel ID="updatepanel3" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbldeletealter" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <asp:UpdatePanel ID="updatepanel10" runat="server">
                                            <ContentTemplate>
                                                <td align="center">
                                                    <center>
                                                        <asp:ImageButton ID="btnyes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnPopAlertyes_Click" />
                                                        <asp:ImageButton ID="btnNo" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnPopAlertNo_Click" />
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
    <asp:UpdatePanel ID="updatepanel11" runat="server">
        <ContentTemplate>
            <div id="popupselect" runat="server" visible="false" class="popupstyle popupheight1">
                <div style="background-color: White; height: 480px; width: 531px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px; margin-left: 662px; margin-top: 96px;">
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="FpSpreadissue" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Visible="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <asp:ImageButton ID="btn_ok" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btn_ok_Click"
                            Visible="false" />
                        <asp:ImageButton ID="btn_exit1" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                            OnClick="btn_exit1_Click" Visible="false" />
                    </center>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--Progress bar for add and go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatepanel52">
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
    <%--Progress bar for save and exit--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updatepanel8">
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
