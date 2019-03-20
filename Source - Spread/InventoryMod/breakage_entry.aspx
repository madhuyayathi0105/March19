<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="breakage_entry.aspx.cs" Inherits="breakage_entry" %>

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
            .watermark
            {
                color: #999999;
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
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <script type="text/javascript">
            function Test() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                idval = document.getElementById("<%=txt_rollno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_rollno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=txt_deptadd.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_deptadd.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_itemnameadd.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_itemnameadd.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_pop1staffname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1staffname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }


                id = document.getElementById("<%=txt_payamt.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_payamt.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                //            id = document.getElementById("<%=txt_staffcode.ClientID %>").value;
                //            if (id.trim() == "") {
                //                id = document.getElementById("<%=txt_staffcode.ClientID %>");
                //                id.style.borderColor = 'Red';
                //                empty = "E";
                //            }
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

        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" CssClass="fontstyleheader" Style="color: Green;"
                            Text="Breakage Entry"></asp:Label>
                        <br />
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="height: 800px; width: 1000px;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 200px;">
                                            <asp:CheckBox ID="cb_itemname" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AutoPostBack="True" OnCheckedChanged="cb_itemname_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_dept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_breakagedby" runat="server" Text="Breakaged By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_breakagedby" runat="server" CssClass="textbox  ddlheight3"
                                    AutoPostBack="true">
                                    <%-- <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Student</asp:ListItem>
                                <asp:ListItem Value="2">Staff</asp:ListItem>
                                <asp:ListItem Value="3">Unknown</asp:ListItem> --%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_status" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_status" runat="server" CssClass="textbox  ddlheight3">
                                    <%-- <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Scrapped</asp:ListItem>
                                <asp:ListItem Value="2">Repair</asp:ListItem>
                                <asp:ListItem Value="3">Missing</asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight2"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="cext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1  txtheight2"
                                    AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="cext_todate" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                    OnClick="btn_addnew_click" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                    <div>
                        <br />
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                                Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="LinkButtonsremove_Click" />
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
                                        <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                            RepeatDirection="Horizontal">
                                            <%--OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged"--%>
                                            <asp:ListItem Value="Roll_No">DeptName</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Item Name</asp:ListItem>
                                            <asp:ListItem Value="Course_Name">AssetNo</asp:ListItem>
                                            <asp:ListItem Value="Dept_Name">Incharge Staff</asp:ListItem>
                                            <asp:ListItem Value="Current_Semester">Breakage By</asp:ListItem>
                                            <asp:ListItem Value="Sections">Mem Code</asp:ListItem>
                                            <asp:ListItem Value="TotItemQty">Item Status</asp:ListItem>
                                            <asp:ListItem Value="TotItemQty">Pay Method</asp:ListItem>
                                            <asp:ListItem Value="Roll_No">Remarks </asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Header Name</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Ledger Name</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Pay Amount</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                        ExpandedImage="~/images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                    <%-- <br />--%>
                    <asp:Label ID="lblnorecr" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                    <br />
                    <div id="fpmain_div" runat="server" visible="false" style="height: 300px; width: 910px;"
                        class="spreadborder">
                        <FarPoint:FpSpread ID="Fpmain" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="890px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 5px; box-shadow: 0px 0px 8px #999999;" OnCellClick="Cell1_Click"
                            OnPreRender="Fpmain_render">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
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
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
        </center>
        <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                            Text="OK" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 396px;"
                    OnClick="imagebtnpopcloseadd_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 830px;
                    height: 695px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_goodsreturn" runat="server" Style="color: Green;" Text="Breakage Entry Detail"
                            CssClass="fontstyleheader"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 760px; height: 610px; border-radius: 10px;
                        border: 1px solid Gray;" class="spreadborder">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_deptadd" runat="server" Text="Department" Style="margin-left: 130px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_deptadd" TextMode="SingleLine" AutoPostBack="true" runat="server"
                                        CssClass="textbox textbox1" Width="180px" onfocus="return myFunction(this)" OnTextChanged="txt_deptadd_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_deptadd"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetDeptName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_deptadd"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:Button ID="btn_dept" runat="server" Text="?" Style="margin-left: 10px;" CssClass="textbox btn"
                                        OnClick="btn_dept_Click" />
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_date" runat="server" Style="margin-left: 10px;" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                    <asp:CalendarExtender ID="caltodate" TargetControlID="txt_date" runat="server" Format="dd/MM/yyyy">
                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_itemnameadd" runat="server" Style="margin-left: 130px;" Text="Item Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_itemnameadd" TextMode="SingleLine" runat="server" AutoPostBack="true"
                                        CssClass="textbox textbox1" Width="180px" onfocus="return myFunction(this)" OnTextChanged="txt_itemnameadd_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_itemnameadd"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetItemName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemnameadd"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:Button ID="btn_itemname" runat="server" Text="?" Style="margin-left: 10px;"
                                        CssClass="textbox btn" OnClick="btn_itemname_Click" />
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_measure" runat="server" Style="margin-left: 10px;" Text="Measure"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_measure" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_breakgedbyadd" runat="server" Style="margin-left: 130px;" Text="BreakagedBy"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_breakgedbyadd" runat="server" CssClass="textbox1  ddlheight3"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_breakgedbyadd_SelectedIndexChanged">
                                        <%--   <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Student</asp:ListItem>
                                    <asp:ListItem Value="2">Staff</asp:ListItem>
                                    <asp:ListItem Value="3">Unknown</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_assetno" runat="server" Text="Asset No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_assetno" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdb_student" Text="If Student" runat="server" Style="margin-left: 130px;"
                                        GroupName="a" AutoPostBack="true" OnCheckedChanged="rdb_student_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_staff" Text="If Staff" runat="server" GroupName="a" AutoPostBack="true"
                                        OnCheckedChanged="rdb_staff_CheckedChanged" />
                                    <%--<asp:RadioButton ID="rdb_unknown" Text="Unknown" runat="server" GroupName="same" AutoPostBack="true"
                             OnCheckedChanged="rdb_unknown_CheckedChanged"  
                                 />
                                    --%>
                                    <asp:RadioButton ID="rdb_Guest" Text="If Guest" runat="server" GroupName="a" AutoPostBack="true"
                                        OnCheckedChanged="rdb_Guest_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2px">
                                    <fieldset id="fs_student" runat="Server" visible="false" style="margin-left: 135px;
                                        width: 200px">
                                        <asp:RadioButton ID="rdb_DayScholer" Text="Day Scholer" runat="server" GroupName="a"
                                            AutoPostBack="true" OnCheckedChanged="rdb_dayscholer_CheckedChanged" />
                                        <asp:RadioButton ID="rdb_hostler" Text="Hostler" runat="server" GroupName="a" AutoPostBack="true"
                                            OnCheckedChanged="rdb_hostler_CheckedChanged" />
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_rollno" Text="Roll No" runat="server" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox textbox1 txtheight3"
                                        AutoPostBack="true" onfocus="return myFunction(this)" Visible="false" OnTextChanged="txt_rollno_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <span id="roll" runat="server" visible="false" style="color: Red;">*</span>
                                    <asp:Button ID="btn_roll" Text="?" runat="server" CssClass="textbox btn" Visible="false"
                                        OnClick="btn_roll_Click" />
                                    <asp:Button ID="btn1sturoll" Text="?" runat="server" OnClick="btnsturollno_Click"
                                        CssClass="textbox btn" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_name" runat="server" Text="Name" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_name" TextMode="SingleLine" ReadOnly="true" runat="server" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_deg" runat="server" Text="Degree" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_deg" TextMode="SingleLine" ReadOnly="true" runat="server" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_deptstu" runat="server" Text="Department" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_deptstu" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_sem" runat="server" Text="Semester" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sem" TextMode="SingleLine" ReadOnly="true" runat="server" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sec" runat="server" Text="Section" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sec" TextMode="SingleLine" ReadOnly="true" runat="server" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pop1staffname" Text="Staff Incharge" runat="server" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_pop1staffname" runat="server" CssClass="textbox txtheight3 textbox1"
                                        Visible="false" AutoPostBack="true" OnTextChanged="txt_pop1staffname_OnTextChanged">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_pop1staffname"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1staffname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <span id="staff" runat="server" visible="false" style="color: Red;">*</span>
                                    <asp:Button ID="btn_staffquestion" Text="?" runat="server" Visible="false" CssClass="textbox btn"
                                        OnClick="btn_staffquestion_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_photo" Text="Photo" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImageButton3"  runat="server" Width="130px" Height="110px" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_staffcode" runat="server" Style="margin-left: 130px;" Text="Staff Code"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_staffcode" CssClass="textbox textbox1 txtheight3" AutoPostBack="true"
                                        Visible="false" runat="server" onfocus="return myFunction(this)" OnTextChanged="txt_staffcode_OnTextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_staffcode"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <span id="code" runat="server" visible="false" style="color: Red;">*</span>
                                    <asp:Button ID="btn_staffcode" Text="?" runat="server" CssClass="textbox btn" Visible="false"
                                        OnClick="btn_staffcode_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_staffname" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_deptstaff" runat="server" Style="margin-left: 130px;" Text="Department"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_deptstaff" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_des" runat="server" Text="Designation" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_des" TextMode="SingleLine" ReadOnly="true" runat="server" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stafftypr" runat="server" Style="margin-left: 130px;" Text="Staff Type"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_stafftype" TextMode="SingleLine" ReadOnly="true" runat="server"
                                        CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffphoto" Text="Photo" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImageButton4"  runat="server" Width="130px" Height="110px" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pay" Text="Pay Method" Visible="false" Style="margin-left: 130px;"
                                        runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdb_sel" Text="Self" Visible="false" runat="server" GroupName="same"
                                        AutoPostBack="true" OnCheckedChanged="rdb_self_CheckedChanged" />
                                    <asp:RadioButton ID="rdb_mgmt" Visible="false" Text="Management" runat="server" GroupName="same"
                                        AutoPostBack="true" OnCheckedChanged="rdb_mgmt_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_sltheader" Visible="false" Text="Header" runat="server" Style="margin-left: 130px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_header" Visible="false" runat="server" CssClass="textbox1  ddlheight3">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_ledger" Text="Ledger" Visible="false" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_ledger" Visible="false" runat="server" CssClass="textbox1  ddlheight3">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <%--delsi0903--%>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_guestCode" runat="server" Text="Guest Code" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_guestcode" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                    <asp:Button ID="btn_guestLookup" Text="?" runat="server" CssClass="textbox btn" Visible="false"
                                        OnClick="btn_guestLookup_click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_guestName" runat="server" Text="Guest Name" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_guestName" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_statuspay" runat="server" Text="Status" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <%--  <asp:TextBox ID="txt_status" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>--%>
                                    <asp:DropDownList ID="txt_status" runat="server" Visible="false" CssClass=" textbox1 ddlheight3">
                                        <%-- <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Scrapped</asp:ListItem>
                                    <asp:ListItem Value="2">Repair</asp:ListItem>
                                    <asp:ListItem Value="3">Missing</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_narr" Text="Narration" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_narr" runat="server" TextMode="MultiLine" CssClass="textbox textbox1"
                                        Visible="false"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_narr"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_payamt" Text="PayAmount" runat="server" Style="margin-left: 130px;"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_payamt" runat="server" TextMode="SingleLine" CssClass="textbox textbox1"
                                        Visible="false" onfocus="return myFunction(this)"></asp:TextBox>
                                    <span id="sppay" runat="server" visible="false" style="color: Red;">*</span>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_payamt"
                                        FilterType="numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Label ID="stdlblerr" ForeColor="red" runat="server" Visible="false"></asp:Label></center>
                        <br />
                        <center>
                            <asp:Button ID="btn_brkdetsave" runat="server" Visible="false" Text="Save" CssClass="textbox btn2"
                                OnClientClick="return Test()" OnClick="btn_brkdetsave_Click" />
                            <asp:Button ID="btn_brkupdate" runat="server" Text="Update" Visible="false" CssClass="textbox btn2"
                                OnClientClick="return Test()" OnClick="btn_brkupdate_Click" />
                            <asp:Button ID="btn_brkdetexit" runat="server" Text="Exit" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_brkdetexit_Click" />
                            <asp:Button ID="btn_brkdelete" runat="server" Text="Delete" Visible="false" CssClass="textbox btn2"
                                OnClientClick="return Test()" OnClick="btn_brkdelete_Click" />
                            <asp:Button ID="btn_exit1" runat="server" Text="Exit" Visible="false" CssClass="textbox btn2"
                                OnClick="imagebtnpopcloseadd_Click" />
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <%--delsi--%>
        <center>
            <div id="popwindowstudent" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 410px;"
                    OnClick="imagebtnpop2close_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span style="color: Green;" class="fontstyleheader">Select the Student</span></div>
                        <br />
                    </center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2collgname" Text="College Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2collgname_selectedindexchange">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2batchyr" Text="Batch Year" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)" OnSelectedIndexChanged="ddl_pop2batchyear_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2degre" Text="Degree" runat="server" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2degre" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    OnSelectedIndexChanged="ddl_pop2degre_SelectedIndexChanged" AutoPostBack="true"
                                    onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop2branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2branch" runat="server" CssClass="textbox ddlheight5 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop2sex" Text="Gender" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop2sex" runat="server" CssClass="textbox ddlheight2 textbox1"
                                    AutoPostBack="true" onfocus="return myFunction1(this)">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Male</asp:ListItem>
                                    <asp:ListItem Value="2">Female</asp:ListItem>
                                    <asp:ListItem Value="3">Transgender</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop1hostelname" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_pop1hostelname" runat="server" CssClass="textbox ddlheight4 textbox1"
                                    Width="123px" onfocus="return myFunction(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_pop2go" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_pop2go_Click" />
                            </td>
                            <td>
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
                    <%-- <div style="width: 250px; float: right;">
                        <asp:Label ID="lblcounttxt" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                        <asp:Label ID="lblcount" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                    </div>--%>
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="fpsturoll" runat="server" Visible="false" Style="overflow: auto;
                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <asp:Button ID="btn_pop2ok" Text="Ok" runat="server" CssClass="textbox btn2" OnClick="btn_pop2ok_Click" />
                    <asp:Button ID="btn_pop2exit" Text="Exit" runat="server" CssClass="textbox btn2"
                        OnClick="btn_pop2exit_Click" />
                </div>
            </div>
        </center>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 20px;
                left: 0px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                Text="Ok" runat="server" OnClick="btnerrclose1_Click" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="Newdiv" runat="server" visible="false" style="height: 50em; z-index: 100000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 55px; margin-left: 338px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <br />
                <center>
                    <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <br />
                        <center>
                            <span style="font-size: large; color: Green;">Department Name</span>
                        </center>
                        <br />
                        <div style="overflow: auto; width: 620px; height: 312px; border: 1px solid Gray;">
                            <asp:Label ID="lblalert" runat="server" Visible="false"></asp:Label>
                            <FarPoint:FpSpread ID="Fpdept" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Width="600px" Style="overflow: auto; border: 0px solid #999999;
                                border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightBlue">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%--   <asp:GridView ID="dptgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                            HeaderStyle-ForeColor="White">
                            
                          
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbcheck" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="DeptCode">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldeptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DeptName">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:CheckBox ID="cbselectall" runat="server" Text="Select All" AutoPostBack="true"
                        OnCheckedChanged="cbselectAll_Change" Style="margin-left: -156px; position: absolute;" />--%>
                        </div>
                        <br />
                        <asp:Button ID="btndeptsave" runat="server" Text="Save" CssClass="textbox btn1" OnClick="btndeptsave_Click" />
                        <asp:Button ID="btndeptexit" runat="server" Text="Exit" CssClass="textbox btn1" OnClick="btndept_exit" />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="stu" runat="server" align="left" style="overflow: auto; width: 700px; height: 200px;
                border-radius: 10px; border: 1px solid Gray;">
                <table>
                </table>
            </div>
        </center>
        <center>
            <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 550px; width: 900px;">
                    <br />
                    <div>
                        <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                            Text="Select Item"></asp:Label>
                    </div>
                    <br />
                    <asp:UpdatePanel ID="upp4" runat="server">
                        <ContentTemplate>
                            <%--style="margin-left: 0px;  position: absolute; width: 800px; --%>
                            <table style="margin-left: 50px; position: absolute; width: 800px; border-radius: 10px;
                                background-color: #0ca6ca; height: 42px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_itemheader3" runat="server" Style="top: 10px; left: 20px; position: absolute;"
                                            Text="Item Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itemheader3" runat="server" Style="top: 6px; left: 120px; position: absolute;"
                                            CssClass="textbox" ReadOnly="true" Width="106px" Height="20px">--Select--</asp:TextBox>
                                        <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 160px;">
                                            <asp:CheckBox ID="cb_itemheader3" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="cb_itemheader3_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemheader3_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txt_itemheader3"
                                            PopupControlID="p5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemname3" runat="server" Style="top: 10px; left: 256px; position: absolute;"
                                            Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Upp5" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_itemname3" runat="server" Style="top: 6px; left: 346px; position: absolute;"
                                                    CssClass="textbox" ReadOnly="true" Width="106px" Height="20px">--Select--</asp:TextBox>
                                                <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 300px; width: 200px;">
                                                    <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chk_pop2itemtyp_CheckedChange" />
                                                    <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="chklst_pop2itemtyp_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_itemname3"
                                                    PopupControlID="p51" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle" Height="30px"
                                            Style="top: 6px; left: 472px; position: absolute;" AutoPostBack="True" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Item Name</asp:ListItem>
                                            <asp:ListItem Value="1">Item Code</asp:ListItem>
                                            <asp:ListItem Value="2">Item Header</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="false" runat="server" PlaceHolder="SearchBy ItemName"
                                            CssClass="textbox textbox1 txtheight3" Style="top: 6px; left: 582px; position: absolute;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchitemcode" Visible="false" runat="server" PlaceHolder="SearchBy ItemCode"
                                            CssClass="textbox textbox1 txtheight3" Style="top: 6px; left: 582px; position: absolute;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_searchheadername" Visible="false" runat="server" PlaceHolder=" ItemHeader Name"
                                            CssClass="textbox textbox1 txtheight3" Style="top: 6px; left: 582px; position: absolute;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go3" runat="server" Style="top: 6px; left: 742px; position: absolute;"
                                            CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btn_go3" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                    <br />
                    <div>
                        <br />
                        <center>
                            <asp:Label ID="lbl_errormsg" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                        </center>
                    </div>
                    <br />
                    <center>
                        <div class="spreadborder" id="Fpitem_div" runat="server" style="width: 700px; height: 280px;">
                            <FarPoint:FpSpread ID="Fpitem" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White" SelectionBackColor="LightBlue">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <br />
                    <center>
                        <asp:Button ID="btn_itemsave4" runat="server" Visible="false" Text="Save" CssClass="textbox btn2"
                            OnClick="btn_itemsave4_Click" />
                        <asp:Button ID="btn_conexist4" runat="server" Visible="false" Text="Exit" CssClass="textbox btn2"
                            OnClick="btn_conexist4_Click" />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupstaffcode1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton6" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" runat="server" Style="font-size: large; color: #0AA7B3;"
                            Text="Select the staff name"></asp:Label>
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
                                            CssClass="textbox textbox1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department3" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department3" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox textbox1">
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
                                            CssClass="textbox textbox1">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_staffnamesearch" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_staffcodesearch" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcodesearch"
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
                                <div id="div1" runat="server" visible="false" style="width: 740px; height: 320px;
                                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <br />
                                    <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                        border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_staffsave" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Save" OnClick="btn_staffsave_Click" />
                                    <asp:Button ID="btn_staffexit" runat="server" Visible="false" CssClass="textbox btn2"
                                        Text="Exit" OnClick="btn_staffexit_Click" />
                                </div>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <%--delsi0903guestLookup--%>
        <center>
            <div id="popwindowguest" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="img_guest" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpop_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <br />
                    <center>
                        <table style="margin-left: 11px; height: 40px; top: 10px" class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_hostelname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostelname" runat="server" BorderStyle="Solid" BorderWidth="2px"
                                                CssClass="multxtpanel" Style="position: absolute; height: 200px; width: 180px;
                                                top: 10px;">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_hostelname" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel_hostelname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                 <td>
                                    <asp:Label ID="lbl_building" runat="server" Text="Building Name"></asp:Label>
                                </td>
                                 
                                <td>
                                    <asp:UpdatePanel ID="upp_building" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_building" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 180px;">
                                                <asp:CheckBox ID="cb_buildingname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbbuildname_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_buildingname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbuildname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_buildingname" runat="server" TargetControlID="txt_buildingname"
                                                PopupControlID="panel_building" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_floorname" runat="server" Text="Floor Name"></asp:Label>
                                </td>

                                 <td>
                                    <asp:UpdatePanel ID="upp_floorname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_floorname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 180px;">
                                                <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbfloorname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblfloorname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_floorname" runat="server" TargetControlID="txt_floorname"
                                                PopupControlID="panel_floorname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>

                                 <td>
                                    <asp:Label ID="lbl_roomname" runat="server" Text="Room Name"></asp:Label>
                                </td>
                                 <td>
                                    <asp:UpdatePanel ID="upp_roomname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="panel_roomname" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Style="height: 200px; width: 180px;">
                                                <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbroomname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroomname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_roomname" runat="server" TargetControlID="txt_roomname"
                                                PopupControlID="panel_roomname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_guestGo" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_guestGo_Click" />
                                </td>
                            </tr>
                        </table>
                        <div>
                            <p>
                                <asp:Label ID="tbl_errors" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </p>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="lbl_guesterror" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </p>
                            <div id="div3" runat="server" visible="false" style="width: 740px; height: 320px;
                                overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                box-shadow: 0px 0px 8px #999999;">
                                <br />
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                    border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    OnCellClick="Cell_Click1" OnPreRender="Fpspread2_render">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </div>

                        <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_guestSave" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Save" OnClick="btn_GuestSave_Click" />
                                    <asp:Button ID="btn_guestClose" runat="server" Visible="false" CssClass="textbox btn2"
                                        Text="Exit" OnClick="btn_GuestExit_Click" />
                                </div>
                            </center>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton7" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 90px; margin-left: 400px;"
                    OnClick="imagebtnpopcloserollno_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 850px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader">Select Student</span></div>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight1">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Width="150px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_degree2" runat="server" OnCheckedChanged="cb_degree2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno3" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno3" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno3"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno3"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="stdbtn_go" Text="Go" OnClick="stdbtn_go_Click" CssClass="textbox btn1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div>
                        <asp:Label ID="Label2" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="0px"
                            Width="820px" Style="overflow: auto; height: 250px; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="LightBlue">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_studOK" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="btn_studOK_Click" />
                            <asp:Button ID="btn_exitstud" runat="server" CssClass="textbox btn2" Text="Exit"
                                OnClick="btn_exitstud_Click" Visible="false" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
