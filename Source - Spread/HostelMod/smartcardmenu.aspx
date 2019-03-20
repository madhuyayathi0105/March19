<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="smartcardmenu.aspx.cs" Inherits="smartcardmenu" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
        <style type="text/css">
            .rdbstyle input[type=radio]
            {
                display: none;
            }
            .rdbstyle input[type=radio] + label
            {
                display: inline-block;
                margin: -2px;
                padding: 4px 12px;
                margin-bottom: 0;
                font-size: 14px;
                line-height: 20px;
                color: #993399;
                text-align: center;
                text-shadow: 0 1px 1px rgba(255,255,255,0.75);
                vertical-align: middle;
                cursor: pointer;
                background-color: #f5f5f5;
                background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
                background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
                background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
                background-image: -o-linear-gradient(top,#fff,#e6e6e6);
                background-image: linear-gradient(to bottom,#fff,#e6e6e6);
                background-repeat: repeat-x;
                border: 1px solid #ccc;
                border-color: #e6e6e6 #e6e6e6 #bfbfbf;
                border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
                border-bottom-color: #b3b3b3;
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
                filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
                -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
                -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
                box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            }
            .rdbstyle input[type=radio]:checked + label
            {
                background-image: none;
                outline: 0;
                -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
                -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
                box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
                border-bottom-color: #b3b3b3;
                border-bottom-style: solid;
                border-bottom-color: #89D17C;
                border-bottom-width: medium;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">

        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <center>
                    <asp:Label ID="lbl_header" Text="Purchase Order Menu Item" Font-Bold="true" ForeColor="Green"
                        Font-Size="Larger" runat="server"></asp:Label>
                </center>
                <br />
                <br />
            </center>
            <center>
                <div class="maindivstyle " style="height: auto; width: 1000px;">
                    <br />
                    <asp:RadioButtonList ID="rdbtype" runat="server" OnSelectedIndexChanged="rdbtype_SelectedIndexChanged"
                        AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" Style="margin-left: -565px;"
                        BorderColor="#999999" Font-Bold="True" CssClass="rdbstyle">
                        <asp:ListItem Value="0">Student</asp:ListItem>
                        <asp:ListItem Value="1">Staff</asp:ListItem>
                        <asp:ListItem Value="2">Others</asp:ListItem>
                    </asp:RadioButtonList>
                    <div style="margin-left: 515px; margin-top: -30px; width: 300px;">
                        <asp:Label ID="lbl_canteenname" runat="server" Text="Canteen Name"></asp:Label>
                        <asp:DropDownList ID="ddl_canteenname" runat="server" CssClass="textbox1  ddlheight4" OnSelectedIndexChanged="ddl_canteenname_selectedindex" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="Panel1" BorderColor="#993333" runat="server" BorderStyle="Solid" Style="height: 181px;
                        width: 766px;">
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                            <asp:View ID="Student" runat="server">
                                <center>
                                    <div style="width: 850px;">
                                        <div>
                                            <div style="float: left; width: 350px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlstudenttype" runat="server" CssClass="textbox1 ddlheight2 "
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlstudenttype_onselectedindexchange">
                                                                <asp:ListItem Value="0">Roll No</asp:ListItem>
                                                                <asp:ListItem Value="1">Student Name</asp:ListItem>
                                                                <asp:ListItem Value="2">Reg No</asp:ListItem>
                                                                <asp:ListItem Value="3">Smart Card</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_studrollsearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" AutoPostBack="true" OnTextChanged="txt_studrollsearch_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_studrollsearch"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studrollsearch"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_studnamesearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" Visible="false" AutoPostBack="true" OnTextChanged="txt_studnamesearch_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_studnamesearch"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstudentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studnamesearch"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_studregno" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" Visible="false" AutoPostBack="true" OnTextChanged="txt_studregno_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_studregno"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstudentreg" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studregno"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_smartcardsearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TextMode="password" TabIndex="1" Visible="false" AutoPostBack="true" OnTextChanged="txt_smartcardsearch_TextChange"></asp:TextBox>
                                                            <asp:Button ID="btn1" Text="?" runat="server" OnClick="btn1_Click" CssClass="textbox btn" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Choose Menu Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_studmenuname" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_studmenuname"
                                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="true" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studmenuname"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:Button ID="menunamelookup" Text="?" runat="server" OnClick="menunamelookup_Click"
                                                                CssClass="textbox btn" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Menu Quantity
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_studmenuqty" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="3" MaxLength="2"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_studmenuqty"
                                                                FilterType="numbers" ValidChars="">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button ID="btn_studadd" Font-Bold="true" ForeColor="#993399" Text="Add" runat="server"
                                                                CssClass="textbox btn2" TabIndex="4" OnClick="btn_studadd_Onclick" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btn_studsave" Font-Bold="true" ForeColor="#993399" Text="Save" runat="server"
                                                                CssClass="textbox btn2" TabIndex="5" OnClick="btn_save_Onclick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="float: left; width: 377px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Session Name
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="ddl_sessionname" ForeColor="#993399" CssClass="textbox1 ddlheight3"
                                                                Width="145px" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_smartroll" Visible="false" runat="server" Text="Roll No"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_smartrollno" Visible="false" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Name
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                        </td>
                                                        <td rowspan="4">
                                                            <asp:Image ID="lbl_studimage" runat="server" Width="120px" Height="110px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Student Type
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_studenttype" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Degree
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                </center>
                            </asp:View>
                            <asp:View ID="Staff" runat="server">
                                <center>
                                    <div style="width: 770px;">
                                        <div style="float: left; width: 350px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_stafftype" runat="server" CssClass="textbox1 ddlheight2 "
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_stafftype_onselectedindexchange">
                                                            <asp:ListItem Value="0">Staff Code</asp:ListItem>
                                                            <asp:ListItem Value="1">Staff Name</asp:ListItem>
                                                            <asp:ListItem Value="2">Smart Card</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffcodesearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                            TabIndex="1" AutoPostBack="true" OnTextChanged="txt_staffcodesearch_textchange"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_staffcodesearch"
                                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:TextBox ID="txt_staffnamesearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                            TabIndex="1" Visible="false" AutoPostBack="true" OnTextChanged="txt_staffnamesearch_textchange"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_staffnamesearch"
                                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:TextBox ID="txt_staffsmartcardsearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                            TabIndex="1" Visible="false"></asp:TextBox>
                                                        <asp:Button ID="Button1" Text="?" runat="server" OnClick="btn_staffpop_Click" CssClass="textbox btn" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Choose Menu Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffmenu" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                            TabIndex="2"></asp:TextBox>
                                                        <asp:Button ID="Button2" Text="?" runat="server" OnClick="menunamelookup_Click" CssClass="textbox btn" />
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_staffmenu"
                                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffmenu"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Menu Quantity
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffmenuqty" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                            TabIndex="3"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_staffmenuqty"
                                                            FilterType="numbers" ValidChars="">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btn_staffadd" Font-Bold="true" ForeColor="#993399" TabIndex="4" Text="Add"
                                                            runat="server" CssClass="textbox btn2" OnClick="btn_staffadd_Onclick" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btn_staffsave" Font-Bold="true" ForeColor="#993399" TabIndex="5"
                                                            Text="Save" runat="server" CssClass="textbox btn2" OnClick="btn_save_Onclick" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="float: left; width: 377px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Session Name
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="ddl_staffsessionname" ForeColor="#993399" CssClass="textbox1 ddlheight3"
                                                            Width="145px" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Name
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffname" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                    </td>
                                                    <td rowspan="4">
                                                        <asp:Image ID="staffimg" runat="server" Width="120px" Height="110px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Designation
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_staffdegree" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </center>
                            </asp:View>
                            <asp:View ID="Others" runat="server">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_othersname" TabIndex="1" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                            </td>
                                            <td>
                                                Session Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_othersessionname" ForeColor="#993399" CssClass="textbox1 ddlheight3"
                                                    Width="145px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Choose Menu Name
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_othermenu" TabIndex="2" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                                <asp:Button ID="Button3" Text="?" runat="server" OnClick="menunamelookup_Click" CssClass="textbox btn" />
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_othermenu"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othermenu"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                Menu Quantity
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_othermenuqty" TabIndex="3" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                    OnTextChanged="btn_otheradd_Onclick"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_othermenuqty"
                                                    FilterType="numbers" ValidChars="">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Button ID="btn_otheradd" Font-Bold="true" ForeColor="#993399" Text="Add" TabIndex="4"
                                                    runat="server" CssClass="textbox btn2" />
                                            </td>
                                            <td align="left">
                                                <asp:Button ID="btn_othersave" Font-Bold="true" ForeColor="#993399" Text="Save" TabIndex="5"
                                                    runat="server" CssClass="textbox btn2" OnClick="btn_save_Onclick" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </asp:View>
                        </asp:MultiView>
                    </asp:Panel>
                    <br />
                    <div>
                        <asp:GridView ID="SelectMenuitemGrid" runat="server" AutoGenerateColumns="false"
                            Width="800px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" HeaderStyle-Height="35px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" Height="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Menu Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("Menu Name") %>'></asp:Label>
                                        <asp:Label ID="lbl_menumasterfk" Visible="false" runat="server" Text='<%# Eval("MenumasterFK") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Cost" runat="server" Text='<%# Eval("Cost") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_quantity" runat="server" Style="text-align: center;" Text='<%# Eval("Menu Quantity") %>'
                                            Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Cost">
                                    <ItemTemplate>
                                        <asp:Label ID="txt_totalcost" runat="server" Style="text-align: center;" Text='<%# Eval("Total Cost") %>'
                                            Width="100px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                </div>
            </center>
            <center>
                <div id="popupselectstd" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 580px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label1" runat="server" Text="Select the Student" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname2" runat="server" Text="College Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_collgname_selectedindexchange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree1" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pdegree" runat="server" Width="150px" Height="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_degree" runat="server" OnCheckedChanged="cb_degree_ChekedChange"
                                                    Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree1"
                                                PopupControlID="pdegree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_branch" runat="server" OnCheckedChanged="cb_branch_ChekedChange"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="pbranch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_batch" Width="70px" Height="30px" runat="server" CssClass="textbox1"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lbl_errormsg1" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <p style="width: 691px;" align="right">
                            <asp:Label ID="lbl_count" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </p>
                        <div>
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" AutoPostBack="true"
                                BorderWidth="0px" Style="overflow: auto; height: 300px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_ok" Visible="false" runat="server" CssClass="textbox btn2" Text="Ok"
                                    OnClick="btn_ok_Click" />
                                <asp:Button ID="btn_exit1" Visible="false" runat="server" CssClass="textbox btn2"
                                    Text="Exit" OnClick="btn_exit1_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="menuloop" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 32px; margin-left: 354px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 730px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="Label2" runat="server" Text="Select the Menu Name" class="fontstyleheader"
                                Style="color: Green;"></asp:Label>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    Menu Type
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_menutype" runat="server" CssClass="textbox textbox1 ddlheight2">
                                        <asp:ListItem Value="2">All</asp:ListItem>
                                        <asp:ListItem Value="0">Veg</asp:ListItem>
                                        <asp:ListItem Value="1">Non-Veg</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnmenugo" CssClass=" textbox btn1" OnClick="btnmenugo_Click" Text="Go"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <div>
                            <asp:Label ID="Label3" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <br />
                        <div>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" AutoPostBack="true"
                                BorderWidth="0px" Style="overflow: auto; height: 300px; width: 470px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        </br>
                        <center>
                            <div>
                                <asp:Button ID="btn_ok1" Visible="false" runat="server" CssClass="textbox btn2" Text="Ok"
                                    OnClick="btn_ok1_Click" />
                                <asp:Button ID="btn_exit2" Visible="false" runat="server" CssClass="textbox btn2"
                                    Text="Exit" OnClick="btn_exit2_Click" />
                            </div>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="popupstaffcode1" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                        OnClick="imagebtnpopclose2_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_selectstaffcode" CssClass="fontstyleheader" runat="server" Style="color: Green;"
                                Text="Select the Staff Name"></asp:Label>
                        </center>
                        <br />
                        <div>
                            <center>
                                <table class="maintablestyle" style="width: 69%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_college2" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_college2" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_college2_selectedindexchange" CssClass="textbox1 ddlheight5">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_department3" runat="server" Text="Department"></asp:Label>
                                            <asp:DropDownList ID="ddl_department3" Width="180px" Height="30px" runat="server"
                                                AutoPostBack="true" CssClass="textbox1 ddlheight6">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_searchbystaff" Width="250px" Height="30px" runat="server"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_searchbystaff_SelectedIndexChanged"
                                                CssClass="textbox1 ddlheight6">
                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_staffnamesearch1" Visible="false" TextMode="SingleLine" runat="server"
                                                Height="20px" CssClass="textbox textbox1" Width="180px" placeholder="Staff Name"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_staffcodesearch1" placeholder="Staff Code" Visible="false" TextMode="SingleLine"
                                                runat="server" Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetStaffCode1" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:Button ID="btn_staffselectgo" runat="server" CssClass="textbox btn1" Text="Go"
                                                OnClick="btn_staffselectgo_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <p>
                                        <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <p style="width: 691px;" align="right">
                                        <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                            ForeColor="Red"></asp:Label>
                                    </p>
                                    <div id="div1" runat="server" visible="false" style="width: 877px; height: 368px;
                                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                        box-shadow: 0px 0px 8px #999999;">
                                        <br />
                                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                            border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                            ShowHeaderSelection="false">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <center>
                                    <div>
                                        <asp:Button ID="btn_staffsave1" Visible="false" runat="server" CssClass="textbox btn2"
                                            Text="Ok" OnClick="btn_staffsave_Click" />
                                        <asp:Button ID="btn_staffexit" runat="server" Visible="false" CssClass="textbox btn2"
                                            Text="Exit" OnClick="btn_staffexit_Click" />
                                    </div>
                                </center>
                            </center>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="alertwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
