<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="request_vendor.aspx.cs" Inherits="request_vendor" %>

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
                height: 950px;
                width: 1000px;
            }
            
            /*input[type=checkbox]
        {
            display: none;
        }
        input[type=checkbox] + label:before
        {
            content: "";
            display: inline-block;
            width: 15px;
            height: 15px;
            vertical-align: middle;
            margin-right: 8px;
            background-color: #aaa;
            box-shadow: inset 0px 2px 2px rgba(0, 0, 0, .3);
            border-radius: 4px;
        }
        input[type=checkbox]:checked + label:before
        {
            content: "\2714";
            color: white;
            background-color: #666;
            text-align: center;
            line-height: 15px;
            text-shadow: 0px 0px 3px #eee;
        }*/
            .header
            {
                width: 950px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lbl_error2.ClientID %>').innerHTML = "";
            }
            function display1() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function SelLedgers1() {
                var chkSelAll = document.getElementById("<%=chkselecall.ClientID %>");
                var tbl = document.getElementById("<%=Showgrid.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                for (var i = 0; i < (tbl.rows.length); i++) {
                    var chkSelectid = document.getElementById('MainContent_Showgrid_selectchk_' + i.toString());
                    if (chkSelAll.checked == false) {
                        chkSelectid.checked = false;
                    } else {
                        chkSelectid.checked = true;
                    }
                }
            }
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <br />
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Request For Supplier</span>
                        </div>
                        <br />
                    </center>
                </div>
            </div>
            <%--base screen--%>
            <div>
                <center>
                    <div class="maindivstyle maindivstylesize" style="width: 1000px; height: 530px;">
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="Rad_item" AutoPostBack="true" runat="server" Text="Item" GroupName="d"
                                            OnCheckedChanged="Rad_item_Click" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="Rad_book" AutoPostBack="true" runat="server" Text="Book" GroupName="d"
                                            OnCheckedChanged="Rad_book_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_venname" runat="server" Text="Supplier Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_venname" runat="server" CssClass="textbox textbox1 txtheight1"
                                                    ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                                    height: 200px;">
                                                    <asp:CheckBox ID="cb_venname" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_venname_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_venname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_venname_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_venname"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_basego" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_basescreen_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_baseaddnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                            OnClick="btn_baseaddnew_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <div>
                                    <asp:Label ID="lbl_baseerror" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </div>
                            </center>
                            <center>
                                <div id="spreaddiv1" runat="server" visible="false" style="width: 877px; height: 372px;"
                                    class="spreadborder">
                                    <br />
                                    <FarPoint:FpSpread ID="FpSpread5" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="867px" Height="350px" ShowHeaderSelection="false" OnButtonCommand="btnDelete_Click">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                        </center>
                        <%--    <br />--%>
                        <center>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <br />
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display1()"
                                    CssClass="textbox textbox1"></asp:TextBox>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                    Width="127px" CssClass="textbox btn1" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                        <br />
                    </div>
                </center>
                <center>
                    <div id="popdiv" runat="server" visible="false" style="height: 55em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 470px;"
                            OnClick="imagebtnpopclose_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 785px; width: 970px; border: 3px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <div>
                                    <br />
                                    <center>
                                        <div class="header">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="Radioitem" AutoPostBack="true" runat="server" Text="Item" GroupName="c"
                                                            OnCheckedChanged="Radioitem_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioBook" AutoPostBack="true" runat="server" Text="Book" GroupName="c"
                                                            OnCheckedChanged="RadioBook_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--<fieldset style="float: left;width: 350px;">--%>
                                            <table id="tditem_wise" runat="server" style="float: left; width: 421px;">
                                                <%--style="width: 385px;"--%>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_deptwise" AutoPostBack="true" runat="server" Text="Department wise"
                                                            GroupName="a" OnCheckedChanged="rdo_deptwise_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_itemwise" AutoPostBack="true" runat="server" Text="Item wise"
                                                            GroupName="a" OnCheckedChanged="rdo_itemwise_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_approvedwise" AutoPostBack="true" runat="server" Text="Approved wise"
                                                            GroupName="a" OnCheckedChanged="rdo_approvedwise_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="tdbook_wise" runat="server" visible="false" style="float: left; width: 421px;">
                                                <%--style="width: 385px;"--%>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_bookdeptwise" AutoPostBack="true" runat="server" Text="Library wise"
                                                            GroupName="a" OnCheckedChanged="rdo_bookdeptwise_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_bookwise" AutoPostBack="true" runat="server" Text="Book wise"
                                                            GroupName="a" OnCheckedChanged="rdo_bookwise_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdo_approvedbookwise" AutoPostBack="true" runat="server" Text="Approved wise"
                                                            GroupName="a" OnCheckedChanged="rdo_approvedbookwise_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- </fieldset>--%>
                                            <%--  <fieldset style="float: left; width: 250px;">--%>
                                            <table style="float: left; width: 250px;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cb_request" Visible="false" runat="server" Text="Request" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="cb_notrequest" runat="server" Visible="false" Text="Not Request" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--  </fieldset>--%>
                                            <table style="line-height: 16px; float: right; width: 195px;">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_suggested" runat="server" Text="Suggested" GroupName="b"
                                                            AutoPostBack="true" OnCheckedChanged="rdb_suggested_CheckedChanged" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdb_all" runat="server" Text="All" AutoPostBack="true" GroupName="b"
                                                            OnCheckedChanged="rdb_allChecked_CheckedChange" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_go1" Text="Go" Visible="false" runat="server" CssClass="textbox btn1"
                                                            OnClick="btn_go1_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </center>
                                    <br />
                                    <br />
                                    <center>
                                        <table id="tbl_ItemRequest" runat="server" class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_deptname" Visible="false" runat="server" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_deptname" Visible="false" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                            <%-- Style="top: 10px; left: 519px; position: absolute;"--%>
                                                            <asp:Panel ID="p1" Visible="false" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                                width: 160px;">
                                                                <%--Style="height: 250px; width: 200px;"--%>
                                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_deptname_CheckedChange" />
                                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_deptname_SelectedIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_deptname"
                                                                PopupControlID="p1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_itemname" Visible="false" runat="server" Text="Item Name"></asp:Label>
                                                    <%--Style="top: 15px;
                                    left: 427px; position: absolute;"--%>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_itemname" Visible="false" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                            <asp:Panel ID="p2" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                                                width: 160px;">
                                                                <asp:CheckBox ID="cb_item" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_item_CheckedChange" />
                                                                <asp:CheckBoxList ID="cbl_item" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_item_SelectedIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemname"
                                                                PopupControlID="p2" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" Visible="false" runat="server" Text="From Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" Visible="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" Visible="false" runat="server" Text="To Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" Visible="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <%--<asp:Label ID="lbl_search" Visible="false" runat="server" Text="Search by"></asp:Label>--%>
                                                    <asp:DropDownList ID="ddl_search" Visible="false" runat="server" CssClass="textbox  ddlheight1"
                                                        OnSelectedIndexChanged="ddl_search_SelectedIndexChanged" AutoPostBack="True">
                                                        <asp:ListItem Value="0">Item Name</asp:ListItem>
                                                        <asp:ListItem Value="1">Department</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txt_searchitem" Visible="false" runat="server" placeholder="Search Item Name"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="searchitemname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitem"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txt_searchdept" Visible="false" runat="server" placeholder="Search Department"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="searchdepartmentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchdept"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="txt_searchreq" Visible="false" runat="server" placeholder="Search Request ID"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="request_search" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchreq"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                                    <%--Style="top: 10px; left: 646px; position: absolute;"--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <div id="divitemreqid" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Ref_ID" runat="server" Text="Request ID"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_ref_id" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_reqdate" runat="server" Text="Request Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_reqdate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_reqdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <%--     Book Request Filter--%>
                                    <center>
                                        <table id="Tbl_Book_Request" runat="server" visible="false" class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label_book_Department" Visible="false" runat="server" Text="Library"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TextDepartment" Visible="false" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                            <%-- Style="top: 10px; left: 519px; position: absolute;"--%>
                                                            <asp:Panel ID="PanelDepartment" Visible="false" runat="server" CssClass="multxtpanel"
                                                                Style="height: 200px; width: 160px;">
                                                                <%--Style="height: 250px; width: 200px;"--%>
                                                                <asp:CheckBox ID="cb_Department" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_Department_CheckedChange" />
                                                                <asp:CheckBoxList ID="cbl_Department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Department_SelectedIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="TextDepartment"
                                                                PopupControlID="PanelDepartment" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label_Book" Visible="false" runat="server" Text="Book Name"></asp:Label>
                                                    <%--Style="top: 15px;
                                    left: 427px; position: absolute;"--%>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="TextBook" Visible="false" runat="server" CssClass="textbox textbox1 txtheight1">--Select--</asp:TextBox>
                                                            <asp:Panel ID="PanelBook" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                                                width: 160px;">
                                                                <asp:CheckBox ID="cb_Book" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_Book_CheckedChange" />
                                                                <asp:CheckBoxList ID="cbl_Book" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Book_SelectedIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="TextBook"
                                                                PopupControlID="PanelBook" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label_fromdate" Visible="false" runat="server" Text="From Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Text_fromdate" Visible="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="Text_fromdate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label_Todate" Visible="false" runat="server" Text="To Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Text_Todate" Visible="false" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="Text_Todate" runat="server"
                                                        Format="dd/MM/yyyy">
                                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TextBox_book" Visible="false" runat="server" placeholder="Search Book Name"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="searchitemname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox_book"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="TextBox_dept" Visible="false" runat="server" placeholder="Search Library"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="searchdepartmentname" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox_dept"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:TextBox ID="TextBox_bookreqid" Visible="false" runat="server" placeholder="Search Request ID"
                                                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="request_search" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox_bookreqid"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <asp:Button ID="btn_book_request_Go" Text="Go" runat="server" CssClass="textbox btn1"
                                                        OnClick="btn_book_request_Go_Click" />
                                                    <%--Style="top: 10px; left: 646px; position: absolute;"--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <div id="divbookreid" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Reff_ID" runat="server" Text="Request ID"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_reff_id" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblreqdate" runat="server" Text="Request Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtreqdate" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtreqdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <%--   End--%>
                                    <br />
                                    <center>
                                        <div>
                                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                        </div>
                                    </center>
                                    <center>
                                        <%-- <div id="spreaddiv" runat="server" visible="false" style="width: 975px; height: 350px;"
                        class="spreadborder">--%>
                                        <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" Height="340px" CssClass="spreadborder"
                                            OnUpdateCommand="Fpspread1_Command">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                        <%--  </div>--%>
                                        <br />
                                        <center>
                                            <div>
                                                <%--        <div id="sug_all" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                    border-radius: 10px;">
                                    <table style="line-height: 58px; width=80%;">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cb_suggested" Visible="false" runat="server" Text="Suggested" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_all" Visible="false" runat="server" Text="All" />
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 45px;" OnClick="btn_errorclose_Click"
                                            runat="server" ImageUrl="~/images/okimg.jpg" />
                                    </center>
                                </div>
                            </center>
                        </div>--%>
                                            </div>
                                        </center>
                                        <center>
                                            <div id="vendorspread_div" runat="server" visible="false" style="width: 770px; height: 350px;"
                                                class="spreadborder">
                                                <FarPoint:FpSpread ID="FpSpread2" Visible="false" runat="server" Width="750px" Height="350px">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </div>
                                            <div id="chkallbook" runat="server" visible="false" style="margin-left: -586px; margin-top: -18px;">
                                                <asp:CheckBox ID="chkselecall" runat="server" Text="SelectAll" onchange="return SelLedgers1();" />
                                            </div>
                                            <div id="div_book" runat="server" visible="false" style="display: table; margin: 0px;
                                                height: 300px; margin-bottom: 20px; margin-top: 10px; position: relative; width: 800px;
                                                text-align: left;">
                                                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                                    HeaderStyle-BackColor="#0CA6CA" Style="width: 800px;" AutoGenerateColumns="false"
                                                    Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:Label ID="lbl_snum" runat="server" Visible="true" Text='<%#Eval("SNo") %>'>
                                                                    </asp:Label>
                                                                </center>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="selectchk" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Request" runat="server" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("RequestCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Title" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Title" runat="server" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("Title") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Author" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Author" runat="server" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("Author") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approved Copies" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Approved" runat="server" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("Approved") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Request Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_RequestDate" runat="server" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("RequestDate") %>'></asp:Label>
                                                                <asp:Label ID="lbl_libcode" runat="server" Visible="false" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("libcode") %>'></asp:Label>
                                                                <asp:Label ID="lbl_bookid" runat="server" Visible="false" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("bookid") %>'></asp:Label>
                                                                <asp:Label ID="LabelReqPk" runat="server" Visible="false" Style="width: auto; text-align: right;"
                                                                    Text='<%#Eval("ReqPk") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" />
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                    <RowStyle ForeColor="#333333" />
                                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                </asp:GridView>
                                            </div>
                                            <center>
                                                <div id="reqdiv" runat="server" visible="false">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btn_selectitem" runat="server" Visible="false" Text="Selected Item"
                                                                    CssClass="textbox btn3" Height="30px" OnClick="btn_selectitem_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblvendor" runat="server" Text="Select Vendor"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_searchvendor" Visible="false" runat="server" CssClass="textbox  txtheight4"
                                                                    AutoPostBack="true" OnTextChanged="txt_searchvendor_txt_change"></asp:TextBox>
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_vendorname" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                            ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                                                        <asp:Panel ID="pvendorname" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                                                            height: 200px;">
                                                                            <asp:CheckBox ID="cb_vendorname" runat="server" Text="Select All" AutoPostBack="True"
                                                                                OnCheckedChanged="cb_vendorname_CheckedChanged" />
                                                                            <asp:CheckBoxList ID="cbl_vendorname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_vendorname_SelectedIndexChange">
                                                                            </asp:CheckBoxList>
                                                                        </asp:Panel>
                                                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_vendorname"
                                                                            PopupControlID="pvendorname" Position="Bottom">
                                                                        </asp:PopupControlExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_vendorqmark" Visible="false" Text="?" runat="server" CssClass="textbox btn"
                                                                    OnClick="btn_vendorqmark_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_request" Text="Request" runat="server" CssClass="textbox btn2"
                                                                    OnClick="btn_Request_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_Online" runat="server" Text="Online Entry"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <fieldset id="fdrblsmsmail" runat="server" style="width: 150px; height: 15px;">
                                                                    <asp:CheckBoxList ID="rblsmsmail" runat="server" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="SMS"></asp:ListItem>
                                                                        <asp:ListItem Text="EMAIL"></asp:ListItem>
                                                                    </asp:CheckBoxList>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <center>
                                                        <div>
                                                            <asp:Label ID="lbl_error1" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </center>
                                                </div>
                                                <div id="smsmcontent" runat="server" visible="false">
                                                    <table>
                                                        <tr>
                                                        <td>
                                                        Sms And Mail
                                                        </td>
                                                            <td>
                                                                <textarea id="textarea_smsemailcontent" runat="server" cols="40" rows="5">  </textarea>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                        </center>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="pop_vendor" runat="server" visible="false" style="height: 48em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 47px; margin-left: 405px;"
                                        OnClick="imagebtnpopclose3_Click" />
                                    <br />
                                    <br />
                                    <br />
                                    <div style="background-color: White; height: 500px; width: 830px; border: 5px solid #0CA6CA;
                                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                        <br />
                                        <center>
                                            <span style="color: Green; font-size: large;">Select the vendor</span>
                                        </center>
                                        <br />
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_popsearchvendor" runat="server" Text="Select Vendor"></asp:Label>
                                                </td>
                                                <td>
                                                    <%-- <asp:DropDownList ID="ddl_vendor" runat="server" CssClass="textbox textbox1">
                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txt_popsearchvendor" runat="server" CssClass="textbox  txtheight5"
                                                        AutoPostBack="true" OnTextChanged="txt_popsearchvendor_txt_change" onkeypress="display()"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_popsearchvendor"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_popsearchvendor"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" >
                                        </asp:FilteredTextBoxExtender>--%>
                                                    <asp:Button ID="btn_popgo" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_popgo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <center>
                                            <div>
                                                <asp:Label ID="lbl_error2" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                        </center>
                                        <br />
                                        <div>
                                            <center>
                                                <div id="vendorsearch_div" runat="server" visible="false" style="width: 590px; height: 350px;"
                                                    class="spreadborder">
                                                    <FarPoint:FpSpread ID="FpSpread3" Visible="false" runat="server" Width="570px" Height="348px"
                                                        OnCellClick="FpSpread3_CellClick" OnPreRender="FpSpread3_render">
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </div>
                                            </center>
                                        </div>
                                    </div>
                                </div>
                            </center>
                            <div>
                                <center>
                                    <div id="Div1" runat="server" visible="false" style="width: 867px; margin-left: 20px;
                                        margin-top: -382px; height: 350px;" class="spreadborder">
                                        <FarPoint:FpSpread ID="FpSpread4" Visible="false" runat="server" Width="867px" Height="348px"
                                            Style="border-radius: 10px;">
                                            <%--OnCellClick="FpSpread3_CellClick" OnPreRender="FpSpread3_render"--%>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </center>
                            </div>
                        </div>
                    </div>
                </center>
                <center>
                    <div id="alertmessage" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                <asp:Label ID="lbl_alerterror" Visible="false" runat="server" Text="" Style="color: Red;"
                                                    Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                                    <%-- <asp:ImageButton ID="btn_errorclose" Style="height: 40px; width: 40px;" OnClick="btn_errorclose_Click"
                                                ImageUrl="~/images/okimg.jpg" runat="server" />--%>
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
            <center>
                <div id="divPopAlertNEW" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
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
                                            <center>
                                                <asp:Button ID="btn_yes" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: 65px; margin-left: -80px;"
                                                    OnClick="btn_yes_Click" Text="Yes" runat="server" />
                                            </center>
                                        </td>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_No" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    Visible="true" CssClass="textbox textbox1" Style="height: auto; width: 65px;
                                                    margin-left: -175px;" OnClick="btn_No_Click" Text="No" runat="server" />
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
