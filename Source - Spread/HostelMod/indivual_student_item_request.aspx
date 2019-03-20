<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="indivual_student_item_request.aspx.cs" Inherits="indivual_student_item_request" %>

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
            function Test() {
                var id = "";
                var empty = "";
                var value1 = "";
                var idval = "";
                id = document.getElementById("<%=txt_rollno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_rollno.ClientID %>");
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
            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                    Text="Individual Student Item Request"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 1000px;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 100px;">
                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 100px;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="Panel2" Position="Bottom">
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
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: 250px;
                                        width: 200px;">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_branch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox  txtheight" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                        width: 80px;">
                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_sec" runat="server" Text="Section"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" runat="server" CssClass="textbox  txtheight" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="height: 150px;
                                        width: 100px;">
                                        <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_sec"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_stutype" runat="server" Text="Student Type"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:RadioButton ID="rdb_both" runat="server" Text="Both" AutoPostBack="true" GroupName="m"
                                OnCheckedChanged="both_OnChecked_Change" />
                            <asp:RadioButton ID="rdb_hos" runat="server" Text="Hostler" AutoPostBack="true" GroupName="m"
                                OnCheckedChanged="hosteler_OnChecked_Change" />
                            <asp:RadioButton ID="rdb_day" runat="server" Text="Dayscholar" AutoPostBack="true"
                                OnCheckedChanged="daysscholor_OnChecked_Change" GroupName="m" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_hostelname" runat="server" Visible="false" Text="Hostel Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_hostelname" runat="server" Visible="false" CssClass="textbox  txtheight3"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel6" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                        width: 150px;">
                                        <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_hostelname"
                                        PopupControlID="Panel6" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_name" runat="server" Text="Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_name" TextMode="SingleLine" placeholder="Search Student Name"
                                runat="server" AutoCompleteType="Search" CssClass="textbox  txtheight3"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_name"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname1" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbl_rollnum" runat="server" Text="Roll No"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_rollnum" placeholder="Search Roll No" TextMode="SingleLine"
                                runat="server" AutoCompleteType="Search" CssClass="textbox  txtheight3" Width="110px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollnum"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollnum"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td colspan="6">
                            <asp:CheckBox ID="cb_date" runat="server" OnCheckedChanged="cb_date_CheckedChanged"
                                AutoPostBack="true" />
                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"
                                AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="cext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                            <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1  txtheight"
                                AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="cext_todate" TargetControlID="txt_todate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                            Style="margin-top: -0.1%;">
                            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                ImageAlign="Right" />
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
                                    <asp:TextBox ID="tborder" Visible="false" Width="838px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                        Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                        <asp:ListItem Value="Roll_No">Roll No </asp:ListItem>
                                        <asp:ListItem Value="Stud_Name">Name</asp:ListItem>
                                        <asp:ListItem Value="Stud_Type">Student Type</asp:ListItem>
                                        <asp:ListItem Value="TotItemQty">Total No Of Item</asp:ListItem>
                                        <asp:ListItem Value="ReqDate">Request Date</asp:ListItem>
                                        <asp:ListItem Value="Student_Mobile">Mobile Number</asp:ListItem>
                                        <asp:ListItem Value="parent_phnop">Phone Number</asp:ListItem>
                                        <asp:ListItem Value="Course_Name">Degree</asp:ListItem>
                                        <asp:ListItem Value="Dept_Name">Branch</asp:ListItem>
                                        <asp:ListItem Value="Current_Semester">Semester</asp:ListItem>
                                        <asp:ListItem Value="Sections">Section</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                    CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                    TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                    ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <br />
                <div id="div1" runat="server" visible="false" class="reportdivstyle" style="width: 767px;">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="750px" Style="overflow: auto;
                        height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;"
                        OnCellClick="Cell_Click1" OnPreRender="Fpspread_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                            onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                            Text="Export To Excel" Width="127px" Height="30px" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                            Width="60px" Height="30px" CssClass="textbox" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </div>
            <div id="poperrjs" runat="server" visible="false" style="height: 68em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 28px; margin-left: 419px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 850px;"
                    align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_stuitemreq" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Student Item Request"></asp:Label>
                    </center>
                    <br />
                    <div align="center" style="overflow: auto; width: 750px; height: 490px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <div style="width: 350px; float: left;">
                            <table align="left" style="width: 300;">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rdb_hostelr" Text="Hostler" runat="server" GroupName="same"
                                            AutoPostBack="true" OnCheckedChanged="rdb_hostelr_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdb_dayscholar" Text="Days Scholar" runat="server" GroupName="same"
                                            AutoPostBack="true" OnCheckedChanged="rdb_dayscholar_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_rollno" runat="server" Text="Roll No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            onfocus="return myFunction(this)" ReadOnly="true" Style="background-color: rgb(220, 249, 209);
                                            border-color: rgb(196, 196, 196);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_rollno"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_rollno" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_rollno_click" />
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_totnoofitem" runat="server" Text="Total No Of Item"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_totnoofitem" TextMode="SingleLine" runat="server" onfocus="return myFunction(this)"
                                            CssClass="textbox  txtheight3" ReadOnly="true" Style="background-color: rgb(220, 249, 209);
                                            border-color: rgb(196, 196, 196);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftext_totnoofitem" runat="server" TargetControlID="txt_totnoofitem"
                                            FilterType="numbers">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_totnoofitem" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_totnoofitem_Click" />
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_date" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1"
                                            runat="server" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                            <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="0px"
                                Width="320px" Style="overflow: auto; height: 200px; border: 0px solid #999999;
                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <asp:Button ID="btn_spreaddelete" Text="Delete" OnClick="btn_spreaddelete_Click"
                                CssClass="textbox btn2" runat="server" />
                        </div>
                        <div style="width: 300px; margin-left: 10px; float: left;">
                            <table align="center" style="width: 400;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_hostelname1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_roomno" runat="server" Text="Room Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_roomno" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name1" runat="server" Text="Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_name1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_degree1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_branch1" runat="server" Text="Branch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_branch1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sem1" runat="server" Text="Semester"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sem1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_sec1" runat="server" Text="Section"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_sec1" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_mono" runat="server" Text="Mobile Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_mono" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_mono"
                                            FilterType="numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_phoneno" runat="server" Text="Phone Number"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_phoneno" TextMode="SingleLine" runat="server" CssClass="textbox  txtheight3"
                                            ReadOnly="true" Style="background-color: rgb(220, 249, 209); border-color: rgb(196, 196, 196);"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_phoneno"
                                            FilterType="numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <br />
                    <div>
                        <center>
                            <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="textbox btn2" Visible="true"
                                OnClick="btn_save_Click" />
                            <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                            <asp:Button ID="btn_update" runat="server" Visible="false" Text="Update" CssClass="textbox btn2"
                                OnClick="btn_update_Click" />
                            <asp:Button ID="btn_delete" runat="server" Text="Delete" Visible="false" CssClass="textbox btn2"
                                OnClick="btn_delete_Click" />
                        </center>
                    </div>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popupselectstd" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 445px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 583px; width: 912px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstudent" runat="server" Text="Select the Student" Style="font-size: large;
                            color: #790D03;"></asp:Label>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td id="hos" runat="server">
                                <asp:Label ID="lbl_hostelname2" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td id="hos1" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname2" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="phstlnm" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 160px;">
                                            <asp:CheckBox ID="cb_hostelname2" runat="server" OnCheckedChanged="cb_hostelname2_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_hostelname2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_hostelname2"
                                            PopupControlID="phstlnm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox   ddlheight2" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged"
                                    AutoPostBack="true" onfocus="return myFunction(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree2" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree2" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel multxtpanleheight" Width="120px">
                                            <asp:CheckBox ID="cb_degree1" runat="server" OnCheckedChanged="cb_degree1_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree2"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_branch2" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch2" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Width="250px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_branch1" runat="server" OnCheckedChanged="cb_branch1_CheckedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_branch2"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rollno1" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno1" placeholder="Search Roll No" TextMode="SingleLine"
                                    runat="server" AutoCompleteType="Search" CssClass="textbox  txtheight3"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_rollno1"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname2" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go2" Text="Go" OnClick="btn_go2_Click" CssClass="textbox btn1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="lbl_errormsg1" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <p style="width: 691px;" align="right">
                        <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                    </p>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" AutoPostBack="true"
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
                            <asp:Button ID="btn_ok" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="btn_ok_Click" />
                            <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit2_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="itemnamediv" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 434px;"
                    OnClick="imagebtnpopclose3_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_pophead" runat="server" Text="Select the Item" Style="font-size: large;
                            color: #790D03;"></asp:Label>
                    </center>
                    <br />
                    <table class="maintablestyle" style="width: 640px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_itemname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_itemname" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 141px;">
                                            <asp:CheckBox ID="cb_itemname" runat="server" OnCheckedChanged="cb_itemname_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popext_itemname" runat="server" TargetControlID="txt_itemname"
                                            PopupControlID="panel_itemname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsearch" runat="server" Text="Search by Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_itemsearch" placeholder="Search Item Name" TextMode="SingleLine"
                                    runat="server" AutoCompleteType="Search" CssClass="textbox  txtheight3"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_itemsearch" runat="server" TargetControlID="txt_itemsearch"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="acext_itemsearch" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetItemName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemsearch"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go3" Text="Go" OnClick="btn_go3_Click" CssClass="textbox btn1"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <asp:Label ID="Label8" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div id="div2" runat="server" visible="false" style="width: 550px; height: 250px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;">
                        <br />
                        <asp:DataList ID="gvdatass" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                            RepeatColumns="5" Width="400px" ForeColor="#333333">
                            <AlternatingItemStyle BackColor="White" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("TextVal") %>'></asp:Label>
                                            <asp:Label ID="lbl_itemcode" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("TextCode") %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                Text='<%# Eval("TextVal") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        </asp:DataList>
                    </div>
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_ok1" runat="server" Visible="false" CssClass="textbox btn2" Text="Ok"
                                OnClick="btn_ok1_Click" />
                            <asp:Button ID="btn_exit3" runat="server" Visible="false" CssClass="textbox btn2"
                                Text="Exit" OnClick="btn_exit3_Click" />
                        </div>
                    </center>
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
        </form>
    </body>
    </html>
</asp:Content>
