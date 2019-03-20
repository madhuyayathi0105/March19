<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="LetterInwardReport.aspx.cs" Inherits="LetterInwardReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .maindivstylesize
            {
                height: 1200px;
                width: 1000px;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <center>
                                <div>
                                    <span class="fontstyleheader" style="color: #008000;">Inward Exit / Entry Report</span></div>
                            </center>
                        </div>
                        <br />
                        <div class="maindivstyle maindivstylesize" style="height: auto; width: 920px;">
                            <br />
                            <div>
                                <center>
                                    <table class="table" width="900px">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_search" Text="Search" runat="server"></asp:Label>
                                                <asp:TextBox ID="txt_search" runat="server" onfocus="return selectpopview();" CssClass="textbox txtheight5">
                                                </asp:TextBox>
                                                <asp:Button ID="btn_go" runat="server" CssClass="textbox1 textbox btn1" Text="Go"
                                                    OnClick="btn_go_Click" OnClientClick="return clearall();" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" Text="From Date" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_fromdate" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_fromdate_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbtimebased" runat="server" Enabled="true" Checked="false" AutoPostBack="true"
                                                    OnCheckedChanged="cbtimebased_OnCheckedChanged" />
                                                <asp:Label ID="lbl_starttime1" Text="From Time" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_hour" Enabled="false" Width="50px" Height="25px" runat="server"
                                                    CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_minits" Enabled="false" Width="50px" Height="25px" runat="server"
                                                    CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_seconds" Enabled="false" Width="50px" Height="25px" Visible="false"
                                                    runat="server" CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_timeformate" Enabled="false" Width="50px" Height="25px"
                                                    runat="server" CssClass="textbox textbox1">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- <td colspan="3" align="center">
                                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                                    height: 30px;">
                                                    <asp:RadioButton ID="rdo_req" runat="server" Checked="true" Text="Request" GroupName="in" />
                                                    <asp:RadioButton ID="rdo_approve" runat="server" Text="Approval" GroupName="in" />
                                                </div>
                                                </td>--%>
                                            <td colspan="2" align="center">
                                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 186px;
                                                    height: 30px; float: left">
                                                    <asp:RadioButton ID="rdo_req" runat="server" AutoPostBack="true" Checked="true" Text="Request"
                                                        GroupName="req" />
                                                    <asp:RadioButton ID="rdo_approve" runat="server" AutoPostBack="true" Text="Approval"
                                                        GroupName="req" />
                                                </div>
                                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 150px;
                                                    height: 30px; float: right">
                                                    <asp:RadioButton ID="rdo_in" runat="server" Checked="true" Text="In" GroupName="in" />
                                                    <asp:RadioButton ID="rdo_out" runat="server" Text="Out" GroupName="in" />
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="Updp_todate" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight2" OnTextChanged="txt_todate_TextChanged"
                                                            AutoPostBack="true"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Cal_date1" TargetControlID="txt_todate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <%-- <td>
                                    <asp:Label ID="lbl_totime" Text="To Time" runat="server"></asp:Label>
                                </td>--%>
                                            <td>
                                                <asp:Label ID="lbl_endtime1" Text="To Time" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_endhour" Enabled="false" Width="50px" Height="25px" runat="server"
                                                    CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_endminit" Enabled="false" Width="50px" Height="25px" runat="server"
                                                    CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_endsecnonds" Enabled="false" Width="50px" Height="25px"
                                                    Visible="false" runat="server" CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddl_endformate" Enabled="false" Width="50px" Height="25px"
                                                    runat="server" CssClass="textbox textbox1">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <%-- <td>
                                    <asp:TextBox ID="txt_totime" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                </td>--%>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <center>
                                                    <table width="800px">
                                                        <tr>
                                                            <td id="td_all" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_all" runat="server" Width="50px" Height="50px" Text="All"
                                                                    ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                                                                <br />
                                                                <asp:Label ID="Label2" runat="server" Visible="true" Style="top: 10px; left: 6px;"
                                                                    Text="All"></asp:Label>
                                                            </td>
                                                            <td id="td_stud" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_stud" runat="server" Width="50px" Height="50px" Text="Student"
                                                                    ImageUrl="~/Hostel Gete Images/download_stud.jpg" OnClick="imgbtn_stud_Click" />
                                                                <br />
                                                                <asp:Label ID="lbl_student" runat="server" Style="top: 10px; left: 6px;" Text="Student"></asp:Label>
                                                            </td>
                                                            <td id="td_staff" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_staff" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/download_staff2.jpg"
                                                                    OnClick="imgbtn_staff_Click" />
                                                                <br />
                                                                <asp:Label ID="lbl_staff" runat="server" Style="top: 10px; left: 6px;" Text="Staff"></asp:Label>
                                                            </td>
                                                            <td id="td_par" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_parents" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/download_parents1.jpg"
                                                                    OnClick="imgbtn_parents_Click" /><br />
                                                                <asp:Label ID="lbl_parents" runat="server" Style="top: 10px; left: 6px;" Text="Parents"></asp:Label>
                                                            </td>
                                                            <td id="td_comp" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_company" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images_company2.jpg"
                                                                    OnClick="imgbtn_company_Click" /><br />
                                                                <asp:Label ID="lbl_company" runat="server" Style="top: 10px; left: 6px;" Text="Company"></asp:Label>
                                                            </td>
                                                            <td id="td_indi" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_individual" runat="server" Width="50px" Height="50px"
                                                                    ImageUrl="~/Hostel Gete Images/indivi.jpg" OnClick="imgbtn_individual_Click" /><br />
                                                                <asp:Label ID="lbl_individual" runat="server" Style="top: 10px; left: 6px;" Text="Individual"></asp:Label>
                                                            </td>
                                                            <td id="td_mag" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_magazine" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images -magazines.jpg"
                                                                    OnClick="imgbtn_magazine_Click" /><br />
                                                                <asp:Label ID="lbl_magazine" runat="server" Style="top: 10px; left: 6px;" Text="Magazine"></asp:Label>
                                                            </td>
                                                            <td id="td_othr" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtn_others" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/images (1)_other1.jpg"
                                                                    OnClick="imgbtn_others_Click" /><br />
                                                                <asp:Label ID="lbl_others" runat="server" Style="top: 10px; left: 6px;" Text="Others"></asp:Label>
                                                            </td>
                                                            <td id="td_search" runat="server" align="center">
                                                                <asp:ImageButton ID="Image_search" runat="server" Width="50px" Height="50px" ImageUrl="~/Hostel Gete Images/file-manager.png"
                                                                    OnClick="imgbtn_serch_Click" /><br />
                                                                <asp:Label ID="lbl_ser" runat="server" Style="top: 10px; left: 6px;" Text="Search"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Action
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_status" runat="server" CssClass="textbox txtheight1" Style="width: 100px;
                                                                            height: 15px;" ReadOnly="true">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 100px;
                                                                            width: 150px;">
                                                                            <asp:CheckBox ID="cb_status" runat="server" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cb_status_CheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_status" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_status_SelectedIndexChanged">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_status"
                                                                            PopupControlID="Panel4" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_pri" runat="server" Style="top: 10px; left: 6px;" Text="priority"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <%--delsi--%>
                                                                <asp:DropDownList ID="ddl_stf_docpri" runat="server" CssClass="textbox  ddlheight4"
                                                                    Style="float: left;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                            <asp:CheckBox ID="cb_finallyCom" runat="server" Text="Finally Completed" Checked="false" />
                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgbtn_tab" runat="server" Width="33px" Visible="false" OnClick="tag_Click"
                                                    Height="34px" ImageUrl="~/image/Menuimage.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                            <asp:Label ID="lbl_erroralert" runat="server" ForeColor="Red"></asp:Label>
                            <div id="popwindow1" runat="server" style="display: none; width: 555px; height: 314px;
                                z-index: 5000; margin-left: 104px; margin-top: -112px; position: absolute;" class="table">
                                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: -10px; margin-left: 259px;"
                                    OnClientClick="return selectpop();" />
                                <div>
                                    <center>
                                        <table style="line-height: 40px;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_stud" Text="Student Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_stud" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changed();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_stud"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstudname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stud"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_stf" Text="Staff Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_stf" runat="server" CssClass="textbox txtheight5" onfocus="return myFunction(this)"
                                                        onchange="return changestf();" Width="390px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_stf"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stf"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_parnt" Text="Parent Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_parnt" runat="server" CssClass="textbox txtheight5" onchange="return changepar();"
                                                        Width="390px">
                                                    </asp:TextBox>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_parnt"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>--%>
                                                    <asp:AutoCompleteExtender ID="auto_parent" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getparentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_parnt"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_comp" Text="Company Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_comp" runat="server" CssClass="textbox txtheight5" onchange="return changecom();"
                                                        Width="390px">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_comp"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" &">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="auto_comp" runat="server" DelimiterCharacters="" Enabled="True"
                                                        ServiceMethod="getcompname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_comp"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_indiv" Text="Individual" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_indiv" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changeindi();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_indiv"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getindivame" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_magaz" Text="Magazine's Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_magaz" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changemag();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_magaz"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getmagname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_magaz"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_othr" Text="Others" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_othr" runat="server" CssClass="textbox txtheight5" Width="390px"
                                                        onchange="return changeothr();">
                                                    </asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_othr"
                                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="getothers" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_othr"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </div>
                            <div id="popup_select" runat="server" class="maindivstyle" style="width: 100px; height: 243px;
                                margin-left: 836px; margin-top: -25px; z-index: 5000; position: absolute;">
                                <br />
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_selectall" runat="server" Width="100px" Text="Select All" OnCheckedChanged="cb_all_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_select" runat="server" OnSelectedIndexChanged="cbl_select_SelectedIndexChanged"
                                                    AutoPostBack="true">
                                                    <asp:ListItem Value="1">All</asp:ListItem>
                                                    <asp:ListItem Value="2">Student</asp:ListItem>
                                                    <asp:ListItem Value="3">Staff</asp:ListItem>
                                                    <asp:ListItem Value="4">Parent</asp:ListItem>
                                                    <asp:ListItem Value="5">Company</asp:ListItem>
                                                    <asp:ListItem Value="6">Individual</asp:ListItem>
                                                    <asp:ListItem Value="7">Magazines</asp:ListItem>
                                                    <asp:ListItem Value="8">Others</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div>
                                <center>
                                    <div id="div_student" runat="server">
                                        <br />
                                        <asp:Label ID="lbl_stud_tit" Visible="false" CssClass="fontstylemedium" runat="server"
                                            Text="Student Report Details" ForeColor="#008000"></asp:Label>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter0" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="lbl_st" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image7" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder0" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column0" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column0_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton8" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove0_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder0" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder0" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder0_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <%-- <asp:ListItem Value="individual_code">Individual</asp:ListItem>
                                                            <asp:ListItem Value="ccindividual_code">CC individual</asp:ListItem>--%>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="ForwardPerson">Received Person Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:ListBox ID="lb_colorderstudent" runat="server" Visible="false"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder0" runat="server" TargetControlID="pcolumnorder0"
                                            CollapseControlID="pheaderfilter0" ExpandControlID="pheaderfilter0" Collapsed="true"
                                            TextLabelID="lbl_st" CollapsedSize="0" ImageControlID="Image8" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_stud" runat="server" ForeColor="Red"></asp:Label>
                                        <%-- <div id="div1" runat="server" visible="false" style="width: 890px; height: 500px;
                                    overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                        <br />
                                        <asp:Label ID="lbl_pending_studClr" runat="server" BackColor="#F0A3CC" ForeColor="#F0A3CC"
                                            Width="10px" Height="16px" Visible="true" Text="1"></asp:Label>
                                        <asp:Label ID="lbl_pending_stud" runat="server" Text="Pending"></asp:Label>
                                        <asp:Label ID="lbl_accept_studClr" runat="server" BackColor="#7FFFD4" ForeColor="#7FFFD4"
                                            Width="10px" Height="16px" Visible="true" Text="2"></asp:Label>
                                        <asp:Label ID="lbl_accept_stud" runat="server" Text="Accepted"></asp:Label>
                                        <asp:Label ID="lbl_reject_studClr" runat="server" BackColor="#CD5C5C" ForeColor="#CD5C5C"
                                            Width="10px" Height="16px" Visible="true" Text="3"></asp:Label>
                                        <asp:Label ID="lbl_reject_stud" runat="server" Text="Rejected"></asp:Label>
                                        <br />
                                        <br />
                                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread1_ButtonCommand"
                                            OnPreRender="FpSpread1_SelectedIndexChanged">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <br />
                                        <%-- </div>--%>
                                    </div>
                                    <div id="div_staff" runat="server">
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Staff Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                                        ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="staff_name">Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Time</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="ForwardPerson">Received Person Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:ListBox ID="lstcolorder" runat="server" Visible="false"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_staff" runat="server" ForeColor="Red"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_pending_staffClr" runat="server" BackColor="#F0A3CC" ForeColor="#F0A3CC"
                                            Width="10px" Height="16px" Visible="true" Text="1"></asp:Label>
                                        <asp:Label ID="lbl_pending_staff" runat="server" Text="Pending"></asp:Label>
                                        <asp:Label ID="lbl_accept_staffClr" runat="server" BackColor="#7FFFD4" ForeColor="#7FFFD4"
                                            Width="10px" Height="16px" Visible="true" Text="2"></asp:Label>
                                        <asp:Label ID="lbl_accept_staff" runat="server" Text="Accepted"></asp:Label>
                                        <asp:Label ID="lbl_reject_staffClr" runat="server" BackColor="#CD5C5C" ForeColor="#CD5C5C"
                                            Width="10px" Height="16px" Visible="true" Text="3"></asp:Label>
                                        <asp:Label ID="lbl_reject_staff" runat="server" Text="Rejected"></asp:Label>
                                        <br />
                                        <br />
                                        <center>
                                            <%-- <div id="div2" runat="server" visible="false" style="width: 890px; height: 350px;
                                    overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread2_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_parent" runat="server">
                                        <br />
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Parent Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <asp:Label ID="lbl_par" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column11" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column1_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove1_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder1" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder1_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="Stud_Name">Student Name </asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="parent_name">Parent Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="individual_code">Individual</asp:ListItem>
                                                            <asp:ListItem Value="ccindividual_code">CC Individual</asp:ListItem>
                                                            <asp:ListItem Value="tostudentno">Student</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference Number</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <%--<asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>--%>
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder1" runat="server" TargetControlID="pcolumnorder1"
                                            CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                                            TextLabelID="lbl_par" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_parent" runat="server" ForeColor="Red"></asp:Label>
                                        <center>
                                            <%-- <div id="div3" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread3_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_company" runat="server">
                                        <br />
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Company Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter2" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="lbl_com" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder2" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column2_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove2_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder2" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder2_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="FromName">Company Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="ForwardPerson">Received Person Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <asp:ListItem Value="Datetime">Completed Date & Time</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:ListBox ID="lb_colordercompany" runat="server" Visible="false"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder2" runat="server" TargetControlID="pcolumnorder2"
                                            CollapseControlID="pheaderfilter2" ExpandControlID="pheaderfilter2" Collapsed="true"
                                            TextLabelID="lbl_com" CollapsedSize="0" ImageControlID="Image3" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_comp" runat="server" ForeColor="Red"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_pending_compClr" runat="server" BackColor="#F0A3CC" ForeColor="#F0A3CC"
                                            Width="10px" Height="16px" Visible="true" Text="1"></asp:Label>
                                        <asp:Label ID="lbl_pending_comp" runat="server" Text="Pending"></asp:Label>
                                        <asp:Label ID="lbl_accept_compClr" runat="server" BackColor="#7FFFD4" ForeColor="#7FFFD4"
                                            Width="10px" Height="16px" Visible="true" Text="2"></asp:Label>
                                        <asp:Label ID="lbl_accept_comp" runat="server" Text="Accepted"></asp:Label>
                                        <asp:Label ID="lbl_reject_compClr" runat="server" BackColor="#CD5C5C" ForeColor="#CD5C5C"
                                            Width="10px" Height="16px" Visible="true" Text="3"></asp:Label>
                                        <asp:Label ID="lbl_reject_comp" runat="server" Text="Rejected"></asp:Label>
                                        <br />
                                        <br />
                                        <center>
                                            <%-- <div id="div4" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread4" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread4_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_individual" runat="server">
                                        <br />
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Individual Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter3" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="lbl_indi" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder3" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column3_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton5" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove3_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder3" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder3" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder3_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="FromName">Company Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="individual_code">Individual</asp:ListItem>
                                                            <asp:ListItem Value="ccindividual_code">CC individual</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <%--<asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>--%>
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder3" runat="server" TargetControlID="pcolumnorder3"
                                            CollapseControlID="pheaderfilter3" ExpandControlID="pheaderfilter3" Collapsed="true"
                                            TextLabelID="lbl_indi" CollapsedSize="0" ImageControlID="Image4" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_indi" runat="server" ForeColor="Red"></asp:Label>
                                        <center>
                                            <%-- <div id="div5" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread5" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread5_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_mag" runat="server">
                                        <br />
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Magazine Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter4" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="lbl_mag" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image5" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder4" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column4_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton6" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove4_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder4" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder4" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder4_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="MagazineName">Magazine Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="MagazineType">Magazine Type</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="individual_code">Individual</asp:ListItem>
                                                            <asp:ListItem Value="ccindividual_code">CC individual</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <%--<asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>--%>
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder4" runat="server" TargetControlID="pcolumnorder4"
                                            CollapseControlID="pheaderfilter4" ExpandControlID="pheaderfilter4" Collapsed="true"
                                            TextLabelID="lbl_mag" CollapsedSize="0" ImageControlID="Image5" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_mag" runat="server" ForeColor="Red"></asp:Label>
                                        <center>
                                            <%-- <div id="div6" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread6" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread6_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%--</div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_others" runat="server">
                                        <br />
                                        <br />
                                        <span class="fontstylemedium" style="color: #008000;">Other Report Details</span>
                                        <br />
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilter5" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="lbl_oth" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Image6" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorder5" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_column5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column5_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton7" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremove5_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborder5" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorder5" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder5_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="FromName">Company Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode">Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="ForwardPerson">Received Person Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:ListBox ID="lb_colorderothers" runat="server" Visible="false"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorder5" runat="server" TargetControlID="pcolumnorder5"
                                            CollapseControlID="pheaderfilter5" ExpandControlID="pheaderfilter5" Collapsed="true"
                                            TextLabelID="lbl_oth" CollapsedSize="0" ImageControlID="Image6" CollapsedImage="right.jpeg"
                                            ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_err_other" runat="server" ForeColor="Red"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_pending_otherClr" runat="server" BackColor="#F0A3CC" ForeColor="#F0A3CC"
                                            Width="10px" Height="16px" Text="1"></asp:Label>
                                        <asp:Label ID="lbl_pending_other" runat="server" Text="Pending"></asp:Label>
                                        <asp:Label ID="lbl_accept_otherClr" runat="server" BackColor="#7FFFD4" ForeColor="#7FFFD4"
                                            Width="10px" Height="16px" Text="2"></asp:Label>
                                        <asp:Label ID="lbl_accept_other" runat="server" Text="Accepted"></asp:Label>
                                        <asp:Label ID="lbl_reject_otherclr" runat="server" BackColor="#CD5C5C" ForeColor="#CD5C5C"
                                            Width="10px" Height="16px" Text="3"></asp:Label>
                                        <asp:Label ID="lbl_reject_other" runat="server" Text="Rejected"></asp:Label>
                                        <br />
                                        <br />
                                        <center>
                                            <%-- <div id="div7" runat="server" visible="false" style="width: 890px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                            <br />
                                            <FarPoint:FpSpread ID="Fpspread7" runat="server" Visible="false" BorderWidth="5px"
                                                BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread7_ButtonCommand">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                            <br />
                                        </center>
                                    </div>
                                    <div id="div_all" runat="server">
                                        <br />
                                        <div>
                                            <center>
                                                <asp:Panel ID="pheaderfilterall" runat="server" CssClass="maintablestyle" Height="22px"
                                                    Width="850px" Style="margin-top: -0.1%;">
                                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                    <asp:Label ID="Labelfilterall" Text="Column Order" runat="server" Font-Size="Medium"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                                    <asp:Image ID="Imagefilterall" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                                        ImageAlign="Right" />
                                                </asp:Panel>
                                            </center>
                                            <br />
                                        </div>
                                        <asp:Panel ID="pcolumnorderall" runat="server" CssClass="maintablestyle" Width="850px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox_columnall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_columnall_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnk_columnorderall" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                            Visible="false" Width="111px" OnClick="LinkButtonsremoveall_Click">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:TextBox ID="tborderall" Visible="false" Width="840px" TextMode="MultiLine" CssClass="style1"
                                                            AutoPostBack="true" runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolumnorderall" runat="server" Height="43px" AutoPostBack="true"
                                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorderall_SelectedIndexChanged">
                                                            <asp:ListItem Selected="True" Value="TapalInwardDate">Date</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="TapalInwardTime">Time</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="MemType">Types</asp:ListItem>
                                                            <%--     <asp:ListItem Selected="True" Value="App_No">Student Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="FromName">Name</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value="staff_code">Staff Name</asp:ListItem>--%>
                                                            <asp:ListItem Value="TapalSubject">Subject</asp:ListItem>
                                                            <asp:ListItem Value="MoveTo">Move To</asp:ListItem>
                                                            <asp:ListItem Value="ToDeptCode"> Department</asp:ListItem>
                                                            <asp:ListItem Value="ToCCDeptCode">CC Department</asp:ListItem>
                                                            <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                                            <asp:ListItem Value="Priority">Document Priority</asp:ListItem>
                                                            <asp:ListItem Value="TapalType">Delivery Mode</asp:ListItem>
                                                            <asp:ListItem Value="type">Type</asp:ListItem>
                                                            <asp:ListItem Value="ActionDate">View Date</asp:ListItem>
                                                            <asp:ListItem Value="ActionDesc">Document Status</asp:ListItem>
                                                            <asp:ListItem Value="ActionStatus">Action Status</asp:ListItem>
                                                            <asp:ListItem Value="ResponseStaff">Response Staff</asp:ListItem>
                                                            <asp:ListItem Value="BankCode">Bank Name</asp:ListItem>
                                                            <asp:ListItem Value="BranchName">Branch Name</asp:ListItem>
                                                            <asp:ListItem Value="PaymentType">Payment Type</asp:ListItem>
                                                            <asp:ListItem Value="DDNo">Cheque/DD No</asp:ListItem>
                                                            <asp:ListItem Value="DDName">Cheque/DD Name</asp:ListItem>
                                                            <asp:ListItem Value="DDDate">Payment Date</asp:ListItem>
                                                            <asp:ListItem Value="Paymentamount">Payment Amount</asp:ListItem>
                                                            <asp:ListItem Value="ForwardPerson">Received Person Name</asp:ListItem>
                                                            <asp:ListItem Value="TapalCode">Unique Code</asp:ListItem>
                                                            <asp:ListItem Value="DateTime">Completed Date & Time</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:ListBox ID="lb_allcollorder" runat="server" Visible="false"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpecolumnorderall" runat="server" TargetControlID="pcolumnorderall"
                                            CollapseControlID="pheaderfilterall" ExpandControlID="pheaderfilterall" Collapsed="true"
                                            TextLabelID="Labelfilterall" CollapsedSize="0" ImageControlID="Imagefilterall"
                                            CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                                        </asp:CollapsiblePanelExtender>
                                        <br />
                                        <asp:Label ID="lbl_all_err" runat="server" ForeColor="Red"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_pending_color" runat="server" BackColor="#F0A3CC" ForeColor="#F0A3CC"
                                            Width="10px" Height="16px" Visible="true" Text="1"></asp:Label>
                                        <asp:Label ID="lbl_pending" runat="server" Text="Pending"></asp:Label>
                                        <asp:Label ID="lbl_ComAcc_color" runat="server" BackColor="#7FFFD4" ForeColor="#7FFFD4"
                                            Width="10px" Height="16px" Visible="true" Text="2"></asp:Label>
                                        <asp:Label ID="lbl_ComAcc" runat="server" Text="Accepted"></asp:Label>
                                        <asp:Label ID="lbl_ComRej_color" runat="server" BackColor="#CD5C5C" ForeColor="#CD5C5C"
                                            Width="10px" Height="16px" Visible="true" Text="3"></asp:Label>
                                        <asp:Label ID="lbl_Rej" runat="server" Text="Rejected"></asp:Label>
                                        <br />
                                        <br />
                                        <FarPoint:FpSpread ID="Fpspread9" runat="server" Visible="false" BorderWidth="5px"
                                            BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread9_ButtonCommand">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <br />
                                    </div>
                                </center>
                            </div>
                            <br />
                            <asp:Button ID="btn_del_stud" Text="Delete" CssClass="btn2 textbox textbox1" runat="server"
                                OnClick="btn_del_stud_Click" />
                            <br />
                            <br />
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
                            <div id="div_report" runat="server" visible="false">
                                <center>
                                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                        CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                                        AutoPostBack="true" OnClick="btnExcel_Click" />
                                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                        AutoPostBack="true" OnClick="btn_printmaster_Click" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </center>
                            </div>
                            <%---------popup window-----%>
                            <%--  *********************8 view popup******************888--%>
                            <div id="popview" runat="server" class="popupstyle popupheight1" visible="false">
                                <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 450px;"
                                    OnClick="btn_popclose_Click" />
                                <br />
                                <br />
                                <div style="background-color: White; height: 652px; width: 920px; border: 5px solid #0CA6CA;
                                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                    <br />
                                    <span class="fontstyleheader" style="color: #008000;">Action Taken Details</span>
                                    <br />
                                    <%-- <div runat="server" visible="true" style="width: 890px; height: 300px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                                    <FarPoint:FpSpread ID="Fpspread8" runat="server" Visible="true" BorderWidth="5px"
                                        BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" OnButtonCommand="fpspread8_ButtonCommand">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                    <%-- </div> delsi2101--%>
                                    <br />
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_doc_state" runat="server" Text="Action Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_ad" runat="server" Text="+" CssClass="btn textbox textbox1" OnClick="btn_ad_Click" />
                                                    <asp:DropDownList ID="ddl_viewdetails1" runat="server" CssClass="textbox textbox1 ddlheight4"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_change">
                                                        <asp:ListItem Value="0">Pending</asp:ListItem>
                                                        <asp:ListItem Value="1">Completed</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_min" runat="server" Text="-" CssClass="btn textbox textbox1"
                                                        OnClick="btn_min_Click" />
                                                    <asp:RadioButton ID="Radio_accept" runat="server" Text="Accept" AutoPostBack="true"
                                                        Checked="true" Visible="false" OnCheckedChanged="radio_acceptEvent" />
                                                    <asp:RadioButton ID="Radio_reject" runat="server" Text="Reject" AutoPostBack="true"
                                                        Visible="false" OnCheckedChanged="radio_rejectEvent" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_remarks" runat="server" Text="Document Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_add1" runat="server" Text="+" CssClass="btn textbox textbox1"
                                                        OnClick="btn_ad1_Click" />
                                                    <asp:DropDownList ID="ddl_viewdetails2" runat="server" CssClass="textbox textbox1 ddlheight4">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_min1" runat="server" Text="-" CssClass="btn textbox textbox1"
                                                        OnClick="btn_min1_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_remrks" Visible="true" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_viewdetails3" Visible="true" runat="server" Width="500px" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_persnname" runat="server" Text="Forward Person Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_viewdetails4" runat="server" Width="226px" onfocus="return myFunction(this)"
                                                        CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_viewdetails4"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-,">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="auto_staff" runat="server" DelimiterCharacters="" Enabled="True"
                                                        ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_viewdetails4"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <span style="color: Red;"></span>
                                                    <asp:Button ID="btn_question" Visible="true" runat="server" CssClass="textbox btn"
                                                        Text="?" OnClick="btnQ_Click" />
                                                </td>
                                            </tr>
                                            <%--delsi--%>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_receiveperson" runat="server" Text="CC Person Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_receiveperson" runat="server" Width="226px" onfocus="return myFunction(this)"
                                                        CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_receiveperson"
                                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-,">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:AutoCompleteExtender ID="auto_receiveperson" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_receiveperson"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                    <span style="color: Red;"></span>
                                                    <asp:Button ID="btn_received_question" runat="server" CssClass="textbox btn" Text="?"
                                                        OnClick="btnQ_Received_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_popviewdate" Text="Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_popviewdate" runat="server" Width="80px" ReadOnly="true" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_popviewdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_view_time" Text="Time" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_viewtime" CssClass="textbox txtheight textbox1" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:CheckBox ID="Iscompleted" runat="server" AutoPostBack="true"  Text="Is Completed" Checked="false" Visible="false" OnCheckedChanged="Iscompleted_check"/> 
                                        <asp:Button ID="btn_pop1go" Text="Approve" runat="server" OnClientClick="return validationstaff()"
                                            OnClick="btn_popsave_Click" CssClass="textbox btn2" />
                                        <asp:Button ID="btn_popexit" Text="Exit" runat="server" OnClick="btn_popexit_Click"
                                            CssClass="textbox btn2" />
                                    </div>
                                </div>
                            </div>
                            <%--pop up add new Itemscheck--%>
                            <center>
                                <div id="popAddStaff" runat="server" visible="false" class="popupstyle popupheight">
                                    <br />
                                    <div class="subdivstyle" style="background-color: White; height: 668px; width: 1000px;">
                                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: -37px; margin-left: 477px;"
                                            OnClick="imagebtnpopclose1_Click" />
                                        <br />
                                        <center>
                                            <asp:Label ID="lbl_selctstaffcode" runat="server" Font-Bold="true" Style="font-size: large;
                                                color: Green;" Text="Select the Staff Name"></asp:Label>
                                        </center>
                                        <br />
                                        <div>
                                            <center>
                                                <table class="maintablestyle">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_collegename2" runat="server" Text="College"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_collegename2" runat="server" AutoPostBack="true" CssClass="textbox textbox1 ddlheight5">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_deptname2" runat="server" Text="Department"></asp:Label>
                                                            <%-- <asp:DropDownList ID="ddl_deptname2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_deptname2_SelectedIndexChanged"
                                                                CssClass="textbox textbox1 ddlheight4">
                                                            </asp:DropDownList>--%>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                                                        border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                                                        box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                                                        <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                                            AutoPostBack="true" />
                                                                        <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                                            AutoPostBack="true">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                                                        PopupControlID="p1" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_search2" runat="server" Text="Search By"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_search2" runat="server" AutoPostBack="true" Width="99px"
                                                                CssClass="textbox textbox1 ddlheight5" OnSelectedIndexChanged="ddl_search2_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txt_searchbyname" TextMode="SingleLine" runat="server" Width="136px"
                                                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_searchbyname" runat="server" TargetControlID="txt_searchbyname"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="acext_searchbyname" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbyname"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_searchbycode" Visible="false" TextMode="SingleLine" runat="server"
                                                                CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_searchbycode" runat="server" TargetControlID="txt_searchbycode"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="acext_searchbycode" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbycode"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_searchgo" runat="server" CssClass="textbox btn1" Text="Go" OnClick="butnsearchbygo_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    <p>
                                                        <asp:Label ID="lbl_search3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    </p>
                                                    <p>
                                                        <asp:Label ID="lbl_error3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                                    </p>
                                                    <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="600px" Style="overflow: auto;
                                                        height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                                        box-shadow: 0px 0px 8px #999999;" OnCellClick="Cell_Click">
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </div>
                                                <br />
                                                <center>
                                                    <div>
                                                        <asp:Button ID="btn_save2" runat="server" CssClass="textbox btn2" Text="Save" Width="100"
                                                            OnClick="btnsav_Click" />
                                                        <%-- <asp:Button ID="btn_save2_received" runat="server" CssClass="textbox btn2" Text="Save Receiver"
                                                            Width="100" OnClick="btnsav_Received_Click" />--%>
                                                        <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btnex_Click" />
                                                    </div>
                                                </center>
                                            </center>
                                        </div>
                                    </div>
                                </div>
                            </center>
                            <%-------end of popup---------%>
                            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_description" runat="server" visible="true" class="table" style="background-color: White;
                                        height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_description11" runat="server" Text="Description" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <br />
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                                    <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                            <%--     ********************************************************88888--%>
                            <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_erroralert1" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_erroralert1" runat="server" Style="color: Red;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_erroralert1" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btnerrclose1_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <div id="imgdiv6" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_erroralert2" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_erroralert2" runat="server" Style="color: Red;" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_erroralert2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btnerrclose22_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <%--  ************************************************************************--%>
                            <div id="imgdiv5" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_description2" runat="server" visible="false" class="table" style="background-color: White;
                                        height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lbl_description12" runat="server" Text="Description" Font-Bold="true"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:TextBox ID="txt_description12" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                        margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <br />
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btn_adddesc2" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_adddesc2_Click" />
                                                    <asp:Button ID="btn_exitdesc2" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_exitdesc2_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                            <div id="Div7" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="di7cnfm" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                        border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label3" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_cls_cnmf" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_cls_cnmf_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <%--************--%>
                            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label1" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_erroralert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <%-- ************--%>
                            <div id="imgdivcnfm" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="pnl2cnfm" runat="server" class="table" style="background-color: White; height: 120px;
                                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                        border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_alertconfm" runat="server" Text="Are You Want To Delete This Record"
                                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_errorclose_cnfm" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_errorclose_cnfm_Click" Text="ok" runat="server" />
                                                            <asp:Button ID="btn_errorclose_cncl" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_errorclose_cncl_Click" Text="Cancel" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <div id="imgdivcnfm2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                left: 0px;">
                                <center>
                                    <div id="pnl2cnfm2" runat="server" class="table" style="background-color: White;
                                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                        margin-top: 200px; border-radius: 10px;">
                                        <center>
                                            <table style="height: 100px; width: 100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_alertconfm2" runat="server" Text="Are You Want To Delete This Record"
                                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <center>
                                                            <asp:Button ID="btn_errorclose_cnfm2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_errorclose_cnfm2_Click" Text="ok" runat="server" />
                                                            <asp:Button ID="btn_errorclose_cncl2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                width: 65px;" OnClick="btn_errorclose_cncl2_Click" Text="Cancel" runat="server" />
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </div>
                                </center>
                            </div>
                            <%--**************************************************************--%>
                        </div>
                        <asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                    </center>
                </div>
            </center>
        </div>
        <script type="text/javascript">

            function searchcheck() {
                var stud = document.getElementById("<%=popwindow1.ClientID %>");

                stud.style.display = "block";

                return false;

            }


            function selectcheck() {
                var stud = document.getElementById("<%=popup_select.ClientID %>");
                stud.style.display = "block";
                return false;

            }
            function selectpop() {
                var stud = document.getElementById("<%=popwindow1.ClientID %>");
                stud.style.display = "none";
                return false;

            }

            function selectpopview() {
                var stud = document.getElementById("<%=popwindow1.ClientID %>");
                stud.style.display = "block";
                return false;

            }




            function studentcheck() {

                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = '#c4c4c4';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';

            }
            function stud_div() {
                studentcheck();
            }
            function staffcheck() {

                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';
            }

            function parentcheck() {
                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';
            }

            function companycheck() {
                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';

            }

            function magazinecheck() {
                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';
            }
            function otherscheck() {
                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';

            }
            function individualcheck() {

                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = '#c4c4c4';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = 'white';
            }

            function searchcheck() {
                document.getElementById("<%=td_stud.ClientID %>").style.backgroundColor = 'white';

                document.getElementById("<%=td_staff.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_par.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_comp.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_indi.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_mag.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_othr.ClientID %>").style.backgroundColor = 'white';
                document.getElementById("<%=td_search.ClientID %>").style.backgroundColor = '#c4c4c4';

            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function changed() {
                if (txt_stud.Text != "") {
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }


            function changestf() {
                if (txt_stf.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }

            function changepar() {
                if (txt_parnt.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }

            function changecom() {
                if (txt_comp.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }

            function changeindi() {
                if (txt_indiv.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }


            function changemag() {
                if (txt_magaz.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_othr.ClientID %>').value = "";
                }
            }

            function changeothr() {
                if (txt_magaz.Text != "") {
                    document.getElementById('<%=txt_stud.ClientID %>').value = "";
                    document.getElementById('<%=txt_stf.ClientID %>').value = "";
                    document.getElementById('<%=txt_parnt.ClientID %>').value = "";
                    document.getElementById('<%=txt_comp.ClientID %>').value = "";
                    document.getElementById('<%=txt_indiv.ClientID %>').value = "";
                    document.getElementById('<%=txt_magaz.ClientID %>').value = "";
                }
            }

            function clearall() {


                var staff = document.getElementById("<%=div_staff.ClientID %>");
                staff.style.display = "none";

                var par = document.getElementById("<%=div_parent.ClientID %>");
                par.style.display = "none";
                var com = document.getElementById("<%=div_company.ClientID %>");
                com.style.display = "none";
                var ind = document.getElementById("<%=div_individual.ClientID %>");
                ind.style.display = "none";
                var mag = document.getElementById("<%=div_mag.ClientID %>");
                mag.style.display = "none";
                var othr = document.getElementById("<%=div_others.ClientID %>");
                othr.style.display = "none";

                return false;
            }


            function checkchange() {
                var i = 0;
                if (cb_selectall.checked == true) {



                    alert("check");

                }

                return false;
            }


            function validationstaff() {
                var idval = "";
                var recstaff = "";
                idval = document.getElementById("<%= txt_viewdetails4.ClientID %>").value;
                recstaff = document.getElementById("<%= txt_receiveperson.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_viewdetails4.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
                if (recstaff.trim() == "") {

                    recstaff = document.getElementById("<%= txt_receiveperson.ClientID %>");
                    recstaff.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }



            }

            //double click

            setInterval(function () {
                document.getElementById("<%=txt_viewtime.ClientID %>").value = (new Date()).toLocaleTimeString();
                return false;
            }, 1000);

      
      

        </script>
        </form>
    </body>
    </html>
</asp:Content>
