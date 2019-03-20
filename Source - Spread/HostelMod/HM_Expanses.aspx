<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_Expanses.aspx.cs" Inherits="HM_Expanses" %>

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
            .div
            {
                left: 0%;
                top: 0%;
            }
            .table2
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #7bc1f7;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function checkvalue() {
                var fl = 0;
                var id = document.getElementById("<%=SelectdptGrid.ClientID %>");
                var gridViewControls = id.getElementsByTagName("input");
                var len = id.rows.length;
                for (var i = 0; i < gridViewControls.length; i++) {
                    if (gridViewControls[i].name.indexOf("txt_quantity") > 1) {

                        if (gridViewControls[i].value == "") {

                            fl = 1;
                        }
                    }
                }
                if (fl == 1) {
                    alert('Please Fill All Values');

                    return false;
                }
                else {

                    return true;
                }
            }

            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";


                id = document.getElementById("<%=ddl_group.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "Select") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_group.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                id = document.getElementById("<%=ddl_hostelname.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "Select") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_hostelname.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                id = document.getElementById("<%=txt_amount.ClientID %>");
                value1 = id.value;
                if (value1.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=txt_amount.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                id = document.getElementById("<%=ddl_descrip.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim() == "") {
                    empty = "E";
                    id = document.getElementById("<%=ddl_descrip.ClientID %>");
                    id.style.borderColor = 'Red';
                }

                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }

            function display1() {
                document.getElementById('<%=lblerror.ClientID %>').innerHTML = "";
            }

            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function display2() {
                document.getElementById('<%=lblvalidation2.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form2">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <center>
                        <br />
                        <div>
                            <span style="color: #008000;" class="fontstyleheader">Hostel Expenses</span>
                            <br />
                        </div>
                    </center>
                    <br />
                    <div class="maindivstyle" style="height: 550px; overflow: auto; width: 1000px;">
                        <br />
                        <center>
                            <div>
                                <asp:RadioButton ID="rdb_commonwise" Text="Common wise " runat="server" Visible="false"
                                    AutoPostBack="true" OnCheckedChanged="rdb_commonwise_Click" GroupName="h" />
                                <asp:RadioButton ID="rdb_messwise" Text="Mess wise" runat="server" Visible="false"
                                    AutoPostBack="true" OnCheckedChanged="rdb_messwise_Click" GroupName="h" />
                            </div>
                        </center>
                        <br />
                        <%--dsaa--%>
                        <center>
                            <table id="messwisetabel" runat="server" visible="false" class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_mess" runat="server" Text="Mess Name" Width="76px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_messname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                                <asp:Panel ID="panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                    width: 160px">
                                                    <asp:CheckBox ID="cb_messname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_messname_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_messname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_messname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_messname"
                                                    PopupControlID="panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemheadername" runat="server" Text="Item Header Name" Width="122px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_headername" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 190px;">
                                                    <asp:CheckBox ID="cb_headername" runat="server" Width="100px" OnCheckedChanged="cb_headername_CheckedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_headername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_headername_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_headername"
                                                    PopupControlID="pbatch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sub" runat="server" Text="Sub Header Name" Width="120px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_sub" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                    height: 190px;">
                                                    <asp:CheckBox ID="cb_sub" runat="server" Width="100px" OnCheckedChanged="cb_sub_CheckedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_sub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sub_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_sub"
                                                    PopupControlID="Panel3" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                    width: 130px;">
                                                    <asp:CheckBox ID="cb_itemname" runat="server" Width="100px" OnCheckedChanged="cb_itemname_CheckedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_itemname"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_messfromdate" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_messfromdate" runat="server" CssClass="textbox  txtheight2"
                                            ForeColor="Black" AutoPostBack="true" OnTextChanged="txt_messfromdate_Textchanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_messfromdate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_messtodate" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_messtodate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                            OnTextChanged="txt_messtodate_Textchanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_messtodate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="searchby" runat="server" Text="Search by"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox  ddlheight1" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged"
                                            AutoPostBack="True">
                                            <asp:ListItem Value="0">Item Name</asp:ListItem>
                                            <asp:ListItem Value="1">Item Code</asp:ListItem>
                                            <asp:ListItem Value="2">Item Header</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txt_searchby" Visible="false" runat="server" CssClass="textbox  txtheight2"
                                            placeholder="Search Item Name"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchitemcode" Visible="false" runat="server" CssClass="textbox  txtheight2"
                                            placeholder="Search Item Code"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchheadername" Visible="false" runat="server" CssClass="textbox  txtheight2"
                                            placeholder="Search Item Header"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_messgo" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_messgo_Click" />
                                        <asp:Button ID="btn_messaddnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                            OnClick="btn_messaddnew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <%--        aasasas--%>
                        <table id="commontable" runat="server" visible="false" class="maintablestyle" style="width: 950px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel_hostelname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel_hostelname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                 <td>
                                    <asp:Label ID="Label3" runat="server" Text="Mess Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtmess" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                            <asp:Panel ID="panel4" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px">
                                                <asp:CheckBox ID="Chkmess" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="Chkmess_CheckedChanged" />
                                                <asp:CheckBoxList ID="Cblmess" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cblmess_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtmess"
                                                PopupControlID="panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_groupname" runat="server" Text="Group Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel_groupname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_groupname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_groupname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px">
                                                <asp:CheckBox ID="cb_groupname" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_groupname_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_groupname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_groupname_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_groupname"
                                                PopupControlID="panel_groupname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_subgroupname" runat="server" Text="SubGroup Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel_subgroupname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_subgroupname" runat="server" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_subgroupname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px">
                                                <asp:CheckBox ID="cb_subgroupname" runat="server" Width="100px" Text="Select All"
                                                    AutoPostBack="True" OnCheckedChanged="cb_subgroup_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_subgroupname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subgroup_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_subgroupname"
                                                PopupControlID="panel_subgroupname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_description" runat="server" Text="Description"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updatepanel_description" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_description" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_description" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px">
                                                <asp:CheckBox ID="cb_description" runat="server" Width="100px" Text="Select All"
                                                    AutoPostBack="True" OnCheckedChanged="cb_description_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_description" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_description_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_description"
                                                PopupControlID="panel_description" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight2" ForeColor="Black"
                                        AutoPostBack="true" OnTextChanged="txt_fromdate_Textchanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="Cal1" TargetControlID="txt_fromdate" runat="server" Format="dd/MM/yyyy"
                                        CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                        OnTextChanged="txt_todate_Textchanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="Cal2" TargetControlID="txt_todate" runat="server" Format="dd/MM/yyyy"
                                        CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_datewise" Text="Date Wise" AutoPostBack="true" runat="server"
                                        GroupName="same" OnCheckedChanged="rdb_datewise_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_totalwise" Text="Total Wise" AutoPostBack="true" runat="server"
                                        GroupName="same" OnCheckedChanged="rdb_totalwise_CheckedChanged" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                    <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                        OnClick="btn_addnew_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lbl_error1" runat="server" ForeColor="Red"></asp:Label>
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                                    Style="margin-top: -0.1%;" Visible="false">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px" Visible="false">
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
                                        <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Value="Roll_No">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Roll_Admit">Group </asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Sub Group</asp:ListItem>
                                            <asp:ListItem Value="Degree">Description</asp:ListItem>
                                            <asp:ListItem Value="DOB">Amount</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <div id="div1" runat="server" visible="false" class="reportdivstyle" style="width: 900px;">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="750px" Style="overflow: auto;
                                height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <div id="div2" runat="server" visible="false" class="reportdivstyle spreadborder"
                            style="width: 775px; height: 300px;">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Style="width: 750px; height: 280px;
                                overflow: auto;" OnCellClick="Cell_Click" OnPreRender="Fpspread2_render">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <div id="div5" runat="server" visible="false" class="reportdivstyle spreadborder"
                            style="width: 930px; height: 300px;">
                            <FarPoint:FpSpread ID="Fpspread3" runat="server" Style="width: 910px; height: 280px;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                        <div id="rptprint2" runat="server" visible="false">
                            <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname2" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname2" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display2()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtexcelname2"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" .">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel2" runat="server" OnClick="btnExcel2_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster2" runat="server" Text="Print" OnClick="btnprintmaster2_Click"
                                CssClass="textbox btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol2" Visible="false" />
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="popwindow" runat="server" visible="false" style="height: 40em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 12px; margin-left: 390px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 800px;
                        height: 400px;" align="center">
                        <br />
                        <center>
                            <asp:Label ID="lbl_detailsentry" runat="server" Style="font-size: large; color: Green;"
                                Text="Hostel Expenses Entry"></asp:Label>
                        </center>
                        <br />
                        <div align="center" style="height: 300px; width: 739px;" class="spreadborder">
                            <br />
                            <table style="width: 700px;">
                                <tr>
                                    <%--<td>
                                        <asp:RadioButton ID="rdb_veg" runat="server" GroupName="rr" Text="Veg" AutoPostBack="true"
                                            OnCheckedChanged="rdb_veg_CheckedChanged" Checked="true" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdb_NonVeg" runat="server" GroupName="rr" Text="Non Veg" AutoPostBack="true"
                                            OnCheckedChanged="rdb_Nonveg_CheckedChanged" />
                                    </td>--%>
                                    <td>
                                        Expances Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3">
                                           <%-- <asp:ListItem Value="0">Common</asp:ListItem>
                                            <asp:ListItem Value="1">Veg</asp:ListItem>
                                            <asp:ListItem Value="2">Non Veg</asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:UpdatePanel ID="updatepanel7" runat="server">
                                        <ContentTemplate>
                                        <asp:DropDownList ID="ddl_hostelname" CssClass="textbox textbox1 ddlheight2" Style="width: 175px" 
                                            runat="server" OnSelectedIndexChanged="ddl_hostelname_SelectedIndexChanged" AutoPostBack="true" onfocus="return myFunction(this)" >
                                        </asp:DropDownList></ContentTemplate></asp:UpdatePanel>
                                        <span style="color: Red;">*</span>
                                    </td>

                                     <td>
                                        <asp:Label ID="Label2" runat="server" Text="Mess Name"></asp:Label>
                                    </td>
                                    <td>
                                     <asp:UpdatePanel ID="updatepanel8" runat="server">
                                        <ContentTemplate>
                                        <asp:DropDownList ID="ddlmess" CssClass="textbox textbox1 ddlheight2" Style="width: 205px"
                                            runat="server" onfocus="return myFunction(this)">
                                        </asp:DropDownList></ContentTemplate></asp:UpdatePanel>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <div>
                                        </div>
                                    </td>
                                  
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_group1" runat="server" Text="Group"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                        <asp:DropDownList ID="ddl_group" runat="server" CssClass="textbox  ddlheight3" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                        <%--  <asp:TextBox ID="txt_group1"  CssClass="textbox  txtheight2" Style="width: 150px; display: none;
                               float: right; " onfocus="return myFunction(this)" runat="server"></asp:TextBox>--%>
                                        <span style="color: Red;">*</span>
                                    </td>

                                      <td style="text-align: right;">
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbl_todaydate" runat="server" Text="Today Date"></asp:Label>
                                        <asp:TextBox ID="txt_todaydate" runat="server" CssClass="textbox txtheight2" AutoPostBack="true"
                                            OnTextChanged="txt_todaydate_textchanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todaydate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_subgroup1" runat="server" Text="Sub Group"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_plus1" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus1_Click" />
                                        <asp:DropDownList ID="ddl_subgroup" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus1" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus1_Click" />
                                        <%--<asp:TextBox ID="txt_subgroup1" CssClass="textbox  txtheight2" Style="width: 150px; display: none;
                                float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>--%>
                                        <%-- <span style="color: Red;">*</span>--%>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rdb_pophos" Text="Hostel Wise" AutoPostBack="true" Visible="false"
                                            runat="server" GroupName="c" />
                                        <%--  </td>
                                <td>--%>
                                        <asp:RadioButton ID="rdb_popmess" Text="Mess Wise" AutoPostBack="true" Visible="false"
                                            runat="server" GroupName="c" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_des" runat="server" Text="Description"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_plus2" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus2_Click" />
                                        <asp:DropDownList ID="ddl_descrip" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_minus2" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus2_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_amount" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text=" Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_amount" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_amount"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <center>
                                    <asp:Button ID="btn_update" runat="server" CssClass="textbox btn2" OnClientClick="return Test()"
                                        Text="Update" OnClick="btn_Update_Click" Visible="false" />
                                    <asp:Button ID="btn_delete" runat="server" CssClass="textbox btn2" OnClientClick="return Test()"
                                        Text="Delete" OnClick="btn_delete_Click" Visible="false" />
                                    <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClientClick="return Test()"
                                        OnClick="btn_save_Click" />
                                    <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                                </center>
                            </div>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="pop_mess" runat="server" visible="false" style="height: 40em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 100px;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 12px; margin-left: 438px;"
                        OnClick="imagemessbtnpopclose_Click" />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 900px;
                        height: 450px;" align="center">
                        <br />
                        <center>
                            <asp:Label ID="Label1" runat="server" Style="font-size: large; color: Green;" Text="Mess Expenses Entry"></asp:Label>
                        </center>
                        <br />
                        <div align="center" style="height: 330px; width: 835px;" class="spreadborder">
                            <br />
                            <table style="width: 700px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_mess1" runat="server" Text="Mess Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_messname" CssClass="textbox textbox1" Style="width: 205px"
                                            runat="server" OnSelectedIndexChanged="ddl_search_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                    <td>
                                        <div>
                                        </div>
                                    </td>
                                    <td style="text-align: right;">
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbl_messtodate1" runat="server" Text="Today Date"></asp:Label>
                                        <asp:TextBox ID="txt_messtodate1" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_messtodate1" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_messitemname" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_messitemname" runat="server" CssClass="textbox txtheight4"></asp:TextBox>
                                        <asp:Button ID="qmark1" runat="server" CssClass="textbox btn" Text="?" OnClick="itemlookup_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div>
                                <center>
                                    <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                        HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                        <%--     OnRowDataBound="typegrid_OnRowDataBound" OnRowCommand="SelectdptGrid_RowCommand"--%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_select" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Item Measure">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_itemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hand on Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_avlqty" runat="server" Text='<%# Eval("Hand on quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_quantity" AutoPostBack="true" OnTextChanged="txt_quantity_textchange"
                                                        runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>' Width="80px"
                                                        CssClass="textbox"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_quantity"
                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rpu">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_rpu" runat="server" Text='<%# Eval("rpu") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </center>
                            </div>
                            <br />
                        </div>
                        <br />
                        <div>
                            <center>
                                <asp:Button ID="btn_messsave" Visible="false" runat="server" CssClass="textbox btn2"
                                    Text="Save" OnClick="btn_messsave_Click" onfocus="checkvalue()" />
                                <asp:Button ID="btn_messexit" Visible="false" runat="server" CssClass="textbox btn2"
                                    Text="Exit" OnClick="btn_messexit_Click" />
                            </center>
                        </div>
                    </div>
                </div>
            </center>
            <center>
                <div id="pop_messitemlookup" runat="server" visible="false" class="popupstyle popupheight">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <div class="subdivstyle" style="background-color: White; height: 615px; width: 900px;">
                        <br />
                        <div>
                            <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                                Text="Select the Item" Font-Bold="true"></asp:Label>
                        </div>
                        <br />
                        <%-- <asp:UpdatePanel ID="sdf" runat="server">
                    <ContentTemplate>--%>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemheader3" runat="server" Text="Item Header"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_itemheader3" runat="server" CssClass="textbox" ReadOnly="true"
                                                Width="106px" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 160px;">
                                                <asp:CheckBox ID="cb_itemheader3" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_itemheader3_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemheader_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txt_itemheader3"
                                                PopupControlID="p5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                height: 190px;">
                                                <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                    Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_subheadername"
                                                PopupControlID="Panel5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_itemtype3" runat="server" Text="Item Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_itemname3" runat="server" CssClass="textbox" ReadOnly="true"
                                                Width="106px" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 300px; width: 200px;">
                                                <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkitemtyp" />
                                                <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemtyp">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_itemname3"
                                                PopupControlID="p51" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span>Search By</span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_searchitems" runat="server" CssClass="textbox ddlstyle"
                                        Height="30px" OnSelectedIndexChanged="ddl_search_SelectedIndexChanged" AutoPostBack="True">
                                        <asp:ListItem Value="0">Item Name</asp:ListItem>
                                        <asp:ListItem Value="1">Item Code</asp:ListItem>
                                        <asp:ListItem Value="2">Item Header</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_messitem1" Visible="false" placeholder="Search Item Name" runat="server"
                                        CssClass="textbox " Height="20px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_messitem1"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txt_messitemcode1" Visible="false" placeholder="Search Item Code"
                                        runat="server" CssClass="textbox " Height="20px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_messitemcode1"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txt_messitemheader1" Visible="false" placeholder="Search Item Header"
                                        runat="server" CssClass="textbox" Height="20px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_messitemheader1"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go3" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                </td>
                            </tr>
                        </table>
                        <%--  </ContentTemplate>
                    <triggers>
                            <asp:PostBackTrigger ControlID="btn_go3" />
                        </triggers>
                    </asp:UpdatePanel>--%>
                        <br />
                        <asp:Label ID="lbl_error3" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                        <%--<center>
                        <span>Item Name: </span>
                        <asp:Label ID="menulbl" runat="server" ForeColor="#0099CC
"></asp:Label></center>--%>
                        <br />
                        <div id="div4" runat="server" visible="false" style="width: 850px; height: 318px;
                            background-color: White;" class="spreadborder">
                            <div style="width: 550px; float: left;">
                                <br />
                                <asp:DataList ID="gvdatass" runat="server" Font-Size="Small" RepeatColumns="5" Width="500px"
                                    ForeColor="#333333">
                                    <AlternatingItemStyle BackColor="White" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="CheckBox2" AutoPostBack="true" runat="server" OnCheckedChanged="selectedmenuchk" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                    <asp:Label ID="lbl_itemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                        Text='<%# Eval("ItemHeaderName") %>'></asp:Label>
                                                    <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                        Text='<%# Eval("ItemHeaderCode") %>'></asp:Label>
                                                    <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                                    <%-- <asp:Label ID="lbl_avlqty" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("AvlQty") %>'></asp:Label>
                                                <asp:Label ID="lbl_rpu" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("rpu") %>'></asp:Label>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                </asp:DataList>
                            </div>
                            <br />
                            <div style="width: 200px; float: right;">
                                <asp:GridView ID="selectitemgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                    AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="snogv" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Name">
                                            <ItemTemplate>
                                                <asp:Label ID="itemnamegv" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BorderWidth="1px" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="itemcodegv" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />--%>
                                        <asp:TemplateField HeaderText="Item Headername" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_headername" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("Header Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Headercode" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                    Text='<%# Eval("Header code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item Unit" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Item unit") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hand on quantity" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_avlqty" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Hand on quantity") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="rpu" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_rpu" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("rpu") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:Label ID="itemcodegv" runat="server" Text='<%# Eval("item_code") %>'></asp:Label>--%>
                                        <%-- </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <br />
                        <center>
                            <asp:Button ID="btn_itemsave4" runat="server" Text="Save" CssClass="textbox btn2"
                                OnClick="btn_itemsave4_Click" />
                            <asp:Button ID="btn_conexist4" runat="server" Text="Exit" CssClass="textbox btn2"
                                OnClick="btn_conexit4_Click" />
                        </center>
                    </div>
                </div>
            </center>
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
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
            <center>
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
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                        <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
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
            <%--  09.10.15--%>
            <center>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                                <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_sureno_Click" Text="no" runat="server" />
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
        <div id="surediv_del" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div6" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_suredel" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_suredel" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_suredel_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btn_delno" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_delno_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
