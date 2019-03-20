<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="nonbookmaterial.aspx.cs" EnableEventValidation="false"
    Inherits="LibraryMod_nonbookmaterial" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">


        function frelig5() {
            document.getElementById('<%= btn_pls_mat .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_mat.ClientID%>').style.display = 'block';

        }
        function frelig6() {
            document.getElementById('<%= btn_pl_currn .ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_min_currn.ClientID%>').style.display = 'block';

        }
        function valid() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=ddl_Library.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_Library.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txacc.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txacc.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddl_mat.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddl_mat.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=txtitle.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtitle.ClientID %>");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Non Book Materials</span></div>
        </center>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel36" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; font-family: Book Antiqua;
                            font-weight: bold; margin-top: 10px; margin-bottom: 10px; padding: 6px; width: -20px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
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
                                                Style="margin-left: 5px;" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblaccess" runat="server" Text="Non Mat.Access No" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtaccess" runat="server" CssClass="textbox txtheight2" Style="width: 100px;"
                                                AutoPostBack="True" OnTextChanged="txtaccess_TextChanged"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getsearchaccess" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtaccess"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsearch" runat="server" Text="BySearch" CssClass="commonHeaderFont">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Style="width: 100px; margin-left: -1px;" AutoPostBack="True" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtsearch" runat="server" Visible="false" CssClass="textbox txtheight2"
                                        Style="width: 125px;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:DropDownList ID="ddldept" runat="server" Visible="false" CssClass="textbox ddlstyle ddlheight3"
                                        Width="125px" AutoPostBack="True" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From: " Visible="false"></asp:Label>
                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                        onchange="return checkDate()" Visible="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To:" Style="margin-left: 4px;" Visible="false"></asp:Label>
                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"
                                        Visible="false"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_todate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="2" align="right">
                                    <asp:UpdatePanel ID="UpdatePanebtngo1" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngoClick" />
                                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnAdd_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="divtable" runat="server" visible="false" style="width: 1000px; background-color: White;
                    border-radius: 10px; margin-top: 30px; margin-left: 2px;">
                    <label id="lbl_totrecord" runat="server" style="background-color: Green; font-family: Book Antiqua;">
                    </label>
                    <br />
                    <asp:HiddenField ID="HiddenFieldNonBook" runat="server" Value="-1" />
                    <asp:GridView ID="grdNonBook" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                        ShowHeader="true" Font-Names="Book Antiqua" toGenerateColumns="false" AllowPaging="true"
                        PageSize="1000" OnPageIndexChanging="grdNonBook_OnPageIndexChanged" OnRowCreated="grdNonBook_OnRowCreated"
                        OnSelectedIndexChanged="grdNonBook_SelectedIndexChanged">
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdNonBook" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div id="DivNonBookpopup" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 14px; margin-left: 390px;"
                        OnClick="btn_DivNonBookpopup_popclose_Click" />
                    <br />
                    <div style="background-color: White; height: auto; width: 1000px; border: 5px solid #0CA6CA;
                        font-family: Book Antiqua; font-weight: bold; border-top: 30px solid #0CA6CA;
                        border-radius: 10px;">
                        <br />
                        <center>
                            <span class="fontstyleheader" style="color: #008000;">Non Book Materials Entry</span>
                        </center>
                        <div>
                            <table width="855px">
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span> Library:
                                        <asp:DropDownList ID="ddl_Library" runat="server" Style="width: 185px; height: 30px; margin-left: 94px;"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_Library_SelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <fieldset style="width: 200px; height: 7px;">
                                            <asp:RadioButton ID="rbl_non_Single" runat="server" Text="Single" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnCheckedChanged="rbl_non_Single_Selected" Enabled="True"
                                                Font-Names=" Book antiqua" Checked="true" />
                                            <asp:RadioButton ID="rbl_non_mul" runat="server" Text="Multiple" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnCheckedChanged="rbl_non_mul_Selected" Enabled="True" Font-Names=" Book antiqua" />
                                            <asp:TextBox ID="txcopy" runat="server" AutoPostBack="true" Width="50px" Height="10px "
                                                CssClass="textbox textbox1" MaxLength="4" Visible="false"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txcopy"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date Of Accession:
                                        <asp:TextBox ID="txDate_Acc" runat="server" AutoPostBack="true" Style="width: 80px;
                                            height: 20px; margin-left: 28px;" CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txDate_Acc" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span> AccessNo:
                                        <asp:TextBox ID="txacc" runat="server" Style="width: 87px; height: 20px; margin-left: 81px;
                                            background-color: lightyellow" CssClass="textbox ddlstyle ddlheight3" AutoPostBack="true"
                                            MaxLength="25" OnTextChanged="txtacc_TextChanged"></asp:TextBox>
                                    </td>
                                    <td>
                                        Department:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDepart" runat="server" Style="width: 185px; height: 30px;"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddDepart_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span> MaterialName:
                                        <asp:Button ID="btn_pls_mat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="22px" Style="height: 23px; display: none; left: 423px; position: absolute;
                                            top: 177px; width: 27px;" OnClick="btn_pls_mat_Click" Text="+" />
                                        <asp:DropDownList ID="ddl_mat" runat="server" Style="width: 177px; height: 30px;
                                            margin-left: 45px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_mat_SelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_min_mat" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="22px" Style="height: 23px; display: none; left: 633px; position: absolute;
                                            top: 177px; width: 27px;" OnClick="btn_min_mat_Click" Text="-" />
                                    </td>
                                    <td>
                                        Budget Head:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_non_budget" runat="server" Style="width: 185px; height: 30px;"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_non_budget_SelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Journal Acc.No:
                                        <asp:TextBox ID="txt_jour" runat="server" AutoPostBack="true" Style="width: 135px;
                                            height: 20px;  margin-left: 49px;" CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:Button ID="btn_jour_popup" runat="server" Text="?" Style="width: 25px; height: 30px;"
                                            OnClick="btn_jour_popup_OnClick" />
                                    </td>
                                    <td>
                                        Book Access No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbook_accno" runat="server" AutoPostBack="true" Style="width: 148px;
                                            height: 20px; margin-left: -6px;" CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:Button ID="btn_book_accnopopup" runat="server" Text="?" Style="width: 25px;
                                            height: 30px;" OnClick="btn_book_accnopopup_OnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span> Title:
                                        <asp:TextBox ID="txtitle" runat="server" Style="width: 169px; height: 20px; margin-left: 113px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                    </td>
                                    <td>
                                        Author:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txauthor" runat="server" Style="width: 148px; height: 20px; margin-left: 3px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txauthor"
                                            FilterType="LowercaseLetters,UppercaseLetters">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Month&Year:
                                        <asp:DropDownList ID="ddl_monYear" runat="server" Style="width: 100px; height: 30px;
                                            margin-left: 62px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_monYear_SelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtyear" runat="server" Style="width: 75px; height: 20px;" CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtyear"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        Currency Type:
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_pl_currn" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="22px" Style="height: 23px; display: none; left: 421px; position: absolute;
                                            top: 348px; width: 27px;" OnClick="btn_pl_currn_Click" Text="+" />
                                        <asp:DropDownList ID="ddcurrency" runat="server" Style="width: 147px; height: 30px;
                                            margin-left: 3px;" AutoPostBack="true" OnSelectedIndexChanged="ddcurrency_SelectedIndexChanged"
                                            CssClass="textbox ddlstyle ddlheight3">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_min_currn" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                                            Height="22px" Style="height: 23px; display: none; left: 630px; position: absolute;
                                            top: 348px; width: 27px;" OnClick="btn_min_currn_Click" Text="-" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Publisher:
                                        <asp:TextBox ID="txpublish" runat="server" Style="width: 173px; height: 20px; margin-left: 88px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txpublish"
                                            FilterType="LowercaseLetters,UppercaseLetters">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        Currency Value:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txcurrval" runat="server" Style="width: 148px; height: 20px; margin-left: 3px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txcurrval"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Supplier
                                        <asp:DropDownList ID="ddlsupp" runat="server" AutoPostBack="true" CssClass="textbox ddlstyle ddlheight3"
                                            Style="width: 177px; height: 30px; margin-left: 99px;">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Status:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="dd_sts" runat="server" Style="width: 147px; height: 30px; margin-left: 3px;"
                                            AutoPostBack="true" OnSelectedIndexChanged="dd_sts_SelectedIndexChanged" CssClass="textbox ddlstyle ddlheight3">
                                            <asp:ListItem Value="0">Available</asp:ListItem>
                                            <asp:ListItem Value="1">Condemn</asp:ListItem>
                                            <asp:ListItem Value="2">Issuable</asp:ListItem>
                                            <asp:ListItem Value="3">Lost</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Bill/Invoice No & Date
                                        <asp:TextBox ID="txt_billno" runat="server" AutoPostBack="true" Width="75px" Height="20px"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CheckBox ID="chk_date" runat="server" AutoPostBack="true" OnCheckedChanged="chk_date_CheckedChanged" />
                                        <asp:TextBox ID="Txtbilldate" runat="server" AutoPostBack="true" Width="80px" Height="20px"
                                            CssClass="textbox txtheight2" Enabled="false"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="Txtbilldate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        Volume No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvol" runat="server" Style="width: 90px; height: 20px; margin-left: 0px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Total Price:
                                        <asp:TextBox ID="txttolprice" Text="0.00" runat="server" Style="width: 108px; height: 20px;
                                            margin-left: 83px" CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txttolprice"
                                            FilterType="Numbers,custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        Issue No:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtissueno" runat="server" Style="width: 148px; height: 20px; margin-left: 2px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Contents Of Parts:
                                    </td>
                                    <td>
                                        ISBN:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtisbn" runat="server" Style="width: 148px; height: 20px; margin-left: 1px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <textarea id="textarea_contentpart" runat="server" cols="35" rows="3"> </textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Running Time:
                                        <asp:TextBox ID="txt_time" runat="server" Style="width: 90px; height: 20px; margin-left: 1px"
                                            CssClass="textbox ddlstyle ddlheight3"></asp:TextBox>Min.
                                    </td>
                                    <asp:UpdatePanel ID="UpdatePanelbtn2" runat="server">
                                        <ContentTemplate>
                                            <td>
                                                <asp:ImageButton ID="btn_save_Non_book" runat="server" Visible="false" ImageUrl="~/LibImages/save.jpg"
                                                    OnClick="btn_save_Non_book_Click" OnClientClick="return valid()" />
                                                <asp:ImageButton ID="btnupdate" runat="server" Visible="false" ImageUrl="~/LibImages/update (2).jpg"
                                                    OnClick="btnupdate_Click" />
                                                <asp:ImageButton ID="btndelete" runat="server" Visible="false" ImageUrl="~/LibImages/delete.jpg"
                                                    OnClick="btndelete_Click" />
                                                <asp:ImageButton ID="btn_Exit_Non" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                    OnClick="btn_Exit_Non_book_Click" />
                                                <asp:Button ID="BtnClear" runat="server" Text="Clear" OnClick="BtnClear_OnClick"
                                                    Font-Bold="true" Style="background-color: LightGreen; font-weight: bold; height: 26px;
                                                    width: 83px;" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </table>
                        </div>
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="popwindowjournalaccno" runat="server" class="popupstyle" visible="false"
                    style="height: 50em; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2);
                    position: absolute; top: 0; left: 0;">
                    <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                        OnClick="imagebtnpop2close_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 592px; width: 840px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader">Select the Journal</span></div>
                            <br />
                        </center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_acc_code" Text="Access Code" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_acc_coe" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_search" Text="Search By" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_Search_By" runat="server" CssClass="textbox ddlheight2 textbox1"
                                        AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_Search_By_OnSelectedIndexChanged">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">Journal Code</asp:ListItem>
                                        <asp:ListItem Value="2">Journal Title</asp:ListItem>
                                        <asp:ListItem Value="3">Dept Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_bysearch" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upjournalaccno_go" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_journalaccno_go" runat="server" ImageUrl="~/LibImages/Go.jpg"
                                                OnClick="btn_journalaccno_go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Label ID="lblpop2error" runat="server" ForeColor="Red" Visible="false">
                                </asp:Label>
                            </div>
                        </center>
                        <br />
                        <center>
                            <div id="divTreeView" runat="server" visible="false" align="left" style="overflow: auto;
                                width: 800px; height: 350px; border-radius: 10px; border: 1px solid Gray;">
                                <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                <asp:GridView ID="GrdJourAccNo" Width="780px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    Font-Names="Book Antiqua" toGenerateColumns="false" OnRowCreated="GrdJourAccNo_OnRowCreated"
                                    OnSelectedIndexChanged="GrdJourAccNo_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                        </center>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanelbtn5" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_pop2exit" Text="Exit" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_pop2exit_Click" Visible="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="GrdJourAccNo" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <div id="DivBookAccessNo" runat="server" class="popupstyle" visible="false" style="height: 50em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="ImageButton7" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                        OnClick="image_DivBookAccessNoclose_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; font-family: Book Antiqua; height: 592px; width: 840px;
                        border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader">Select Access Number</span></div>
                            <br />
                        </center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_boaccno" Text="Access Code" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_boaccno" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lb_Search" Text="Search By" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_search_book" runat="server" CssClass="textbox ddlheight2 textbox1"
                                        AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_search_book_OnSelectedIndexChanged">
                                        <asp:ListItem Value="0">All</asp:ListItem>
                                        <asp:ListItem Value="1">Title</asp:ListItem>
                                        <asp:ListItem Value="2">Author</asp:ListItem>
                                        <asp:ListItem Value="3">Publisher</asp:ListItem>
                                        <asp:ListItem Value="4">Edition</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_book_search" runat="server" AutoPostBack="true" Style="width: 120px;
                                        height: 20px; margin-left: -5px" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upbook_go" runat="server">
                                        <ContentTemplate>
                                            <asp:ImageButton ID="btn_book_go" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btn_book_go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="false">
                                </asp:Label>
                            </div>
                        </center>
                        <br />
                        <center>
                            <div id="div1" runat="server" visible="false" align="left" style="overflow: auto;
                                width: 800px; height: 350px; border-radius: 10px; border: 1px solid Gray;">
                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <asp:GridView ID="grdAccessNo" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    Font-Names="Book Antiqua" toGenerateColumns="true" OnRowCreated="grdAccessNo_OnRowCreated"
                                    AllowPaging="true" PageSize="5000" OnPageIndexChanging="grdAccessNo_OnPageIndexChanging"
                                    OnSelectedIndexChanged="grdAccessNo_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                        </center>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanelbtn6" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btn_book_exit" Text="Exit" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_book_ok_exit" Visible="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdAccessNo" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                            onkeypress="display1()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <td align="center" style="line-height: 35px">
                                                <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                                <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                            </td>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <div id="print2" runat="server" visible="false">
                    <asp:Label ID="lblvalidation3" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                    <asp:Label ID="lblrptname2" runat="server" Visible="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname2" runat="server" Visible="true" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>--%>
                    <asp:Button ID="btnExcel2" runat="server" Visible="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnExcel_Click2" Text="Export To Excel" Width="127px"
                        Height="32px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmasterhed2" runat="server" Visible="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click2" Height="32px"
                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
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
    <%--progressBar for NonBookgo&Add--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanebtngo1">
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
    <%--progressBar for NonBookgo&Add--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn2">
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
    <%--progressBar for Upjournalaccno_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="Upjournalaccno_go">
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
            PopupControlID="UpdateProgres32">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Upbook_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn2">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn2">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn2">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="UpdatePanelbtn2">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
