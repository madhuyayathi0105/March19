<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="smartcardmenu_report.aspx.cs" Inherits="smartcardmenu_report" EnableEventValidation="false" %>

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
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <center>
                    <asp:Label ID="lbl_header" Text="Purchase Menu Item Report" CssClass="fontstyleheader"
                        Font-Bold="true" ForeColor="Green" runat="server"></asp:Label>
                    <br /> <br />
                </center>
            </center>
            <center>
                <div class="maindivstyle">
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rdbtype1" runat="server" OnSelectedIndexChanged="rdbtype1_SelectedIndexChanged"
                                    AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" BorderColor="#999999"
                                    Font-Bold="True" CssClass="rdbstyle">
                                    <asp:ListItem Value="0">Cumulative</asp:ListItem>
                                    <asp:ListItem Value="1">Detail</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                College Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collgname" runat="server" CssClass="textbox1 ddlheight5 textbox1"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_collgname_selectedindexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Canteen Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_canteenname" runat="server" Height="20px" CssClass="textbox textbox1"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Width="150px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_canteen" runat="server" OnCheckedChanged="cbl_canteen_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_canteen" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_canteen_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_canteenname"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Session Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sessionname" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" Width="150px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_sessionname" runat="server" OnCheckedChanged="cb_sessionname_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_sessionname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sessionname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sessionname"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:MultiView ID="muldetail" runat="server" ActiveViewIndex="0">
                        <asp:View ID="cum" runat="server">
                        </asp:View>
                        <asp:View ID="det" runat="server">
                            <asp:MultiView ID="muldetails" runat="server" ActiveViewIndex="0">
                                <asp:View ID="student" runat="server">
                                    <br />
                                    <%-- 993333--%>
                                    <asp:Panel ID="Panel1" BorderColor="#C0c0c0" runat="server" BorderStyle="Solid" Style="height: 39px;
                                        width: 812px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Search By
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlstudenttype" runat="server" CssClass="textbox1 ddlheight2 "
                                                        AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddlstudenttype_onselectedindexchange">
                                                        <asp:ListItem Value="0">Roll No</asp:ListItem>
                                                        <asp:ListItem Value="1">Reg No</asp:ListItem>
                                                        <asp:ListItem Value="2">Student Name</asp:ListItem>
                                                        <asp:ListItem Value="3">Staff Code</asp:ListItem>
                                                        <asp:ListItem Value="4">Staff Name</asp:ListItem>
                                                        <asp:ListItem Value="5">Others Name</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:MultiView ID="mulddlstud" runat="server" ActiveViewIndex="0">
                                                        <asp:View ID="roll" runat="server">
                                                            <asp:TextBox ID="txt_studrollsearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" AutoPostBack="true" placeholder="Search Roll No" OnTextChanged="txt_studrollsearch_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_studrollsearch"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studrollsearch"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                        <asp:View ID="reg" runat="server">
                                                            <asp:TextBox ID="txt_studregno" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" placeholder="Search Reg no" AutoPostBack="true" OnTextChanged="txt_studregno_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_studregno"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstudentreg" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studregno"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                        <asp:View ID="studentname" runat="server">
                                                            <asp:TextBox ID="txt_studnamesearch" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                                TabIndex="1" placeholder="Search Student Name" AutoPostBack="true" OnTextChanged="txt_studnamesearch_TextChange"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_studnamesearch"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstudentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studnamesearch"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                        <asp:View ID="staffcode" runat="server">
                                                            <asp:TextBox ID="txt_staffcodesearch1" placeholder="Staff Code" TextMode="SingleLine"
                                                                runat="server" Height="20px" CssClass="textbox textbox1 txtheight3" AutoPostBack="true"
                                                                OnTextChanged="txt_staffcodesearch1_txtchange"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetStaffCode1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch1"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                        <asp:View ID="staffname" runat="server">
                                                            <asp:TextBox ID="txt_staffnamesearch1" TextMode="SingleLine" runat="server" Height="20px"
                                                                CssClass="textbox textbox1 txtheight3" placeholder="Staff Name" AutoPostBack="true"
                                                                OnTextChanged="txt_staffnamesearch1_txtchange"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch1"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                        <asp:View ID="othername" runat="server">
                                                            <asp:TextBox ID="txt_othername" TextMode="SingleLine" runat="server" Height="20px"
                                                                CssClass="textbox textbox1 txtheight3" AutoPostBack="true" OnTextChanged="txt_othername_ontextchange"
                                                                placeholder="Others Name"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="getothername" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othername"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </asp:View>
                                                    </asp:MultiView>
                                                </td>
                                                <td>
                                                    Menu Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_studmenuname" runat="server" Height="20px" CssClass="textbox textbox1"
                                                        TabIndex="2" placeholder="Search Menu Name"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_studmenuname"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="true" ServiceMethod="Getmenu" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studmenuname"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    Type
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_type" runat="server" Height="20px" CssClass="textbox textbox1"
                                                                Width="120px" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel3" runat="server" Width="150px" Height="200px" CssClass="multxtpanel">
                                                                <asp:CheckBox ID="cb_type" runat="server" OnCheckedChanged="cb_type_ChekedChange"
                                                                    Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="cbl_type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_type_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0">Student</asp:ListItem>
                                                                    <asp:ListItem Value="1">Staff</asp:ListItem>
                                                                    <asp:ListItem Value="2">Others</asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_type"
                                                                PopupControlID="Panel3" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:View>
                            </asp:MultiView>
                        </asp:View>
                    </asp:MultiView>
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rdbtype" runat="server" OnSelectedIndexChanged="rdbtype_SelectedIndexChanged"
                                    AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" BorderColor="#999999"
                                    Font-Bold="True" CssClass="rdbstyle">
                                    <asp:ListItem Value="0">Date Wise</asp:ListItem>
                                    <asp:ListItem Value="1">Month Wise</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:MultiView ID="mulview" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="datewise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="monthwise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_year" runat="server" Text="Year"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_year" CssClass="textbox1 ddlheight" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_fmon" runat="server" Text="From Month"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_frommonth" CssClass="textbox1 ddlheight" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_tmon" runat="server" Text="To Month"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_tomonth" CssClass="textbox1 ddlheight" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                                <td>
                                    <asp:Button ID="btn_Go" runat="server" Text="Go" CssClass="textbox btn1 " OnClick="btn_Go_Click" />
                                </td>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                ShowHeaderSelection="false" OnCellClick="FpSpread1_cellclick" OnPreRender="FpSpread1_Selectedindexchange">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                            <br />
                            <br />
                            <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                ShowHeaderSelection="false" OnCellClick="FpSpread2_CellClick" OnPreRender="FpSpread2_Selectedindexchange">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <div id="rptprint1" runat="server" visible="false">
                                <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="lblrptname1" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="lblrptname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_excel2" runat="server" OnClick="btnExcel_Click1" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btn_print2" runat="server" Text="Print" OnClick="btnprintmaster_Click1"
                                    CssClass="textbox btn1" />
                                <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                            </div>
                            <br />
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popwindow_menudet" runat="server" visible="false" class="popupstyle popupheight1">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 470px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 500px; width: 965px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <br />
                        <FarPoint:FpSpread ID="FpSpread4" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                            border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <div id="rptprint2" runat="server" visible="false">
                            <asp:Label ID="lblvalidation3" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="lblrptname2" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="lblrptname2"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnpopexcel" runat="server" OnClick="btnExcel_Click2" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnpopprint" runat="server" Text="Print" OnClick="btnprintmaster_Click2"
                                CssClass="textbox btn1" />
                            <Insproplus:printmaster runat="server" ID="Printmaster2" Visible="false" />
                        </div>
                        <div>
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
