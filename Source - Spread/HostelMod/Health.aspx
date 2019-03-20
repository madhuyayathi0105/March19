<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Health.aspx.cs" Inherits="HostelMod_Health" %>

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
    </head>
    <body>
        <script type="text/javascript">
            function change1(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_description.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_description.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function Test1() {
                var id = "";
                var empty = "";
                var value1 = "";
                var idval = "";
                id = document.getElementById("<%=txt_date.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_date.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_amount.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_amount.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_pop1staffname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_pop1staffname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_staffcode.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_staffcode.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_dept.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_dept.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_design.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_design.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_description.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_description.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_description.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_description.ClientID %>");
                    idval.style.borderColor = 'Red';
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function Test2() {
                var id = "";
                var empty = "";
                var value1 = "";
                var idval = "";
                id = document.getElementById("<%=txt_date.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_date.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_amount.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_amount.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_guestname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_guestname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_gustCode.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_gustCode.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=ddl_description.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_description.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_description.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_description.ClientID %>");
                    idval.style.borderColor = 'Red';
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }
            function Test() {
                var id = "";
                var empty = "";
                var value1 = "";
                var idval = "";

                id = document.getElementById("<%=txt_date.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_date.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_amount.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_amount.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_rollno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_rollno.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_regno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_regno.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_name.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_name.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_degree.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_degree.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=ddl_description.ClientID %>");
                value1 = id.options[id.selectedIndex].text;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    idval = document.getElementById("<%=txt_description.ClientID %>").value;
                    if (idval.trim() == "") {
                        empty = "E";
                        idval = document.getElementById("<%=txt_description.ClientID %>");
                        idval.style.borderColor = 'Red';
                    }
                }
                else if (value1.trim().toUpperCase() == "SELECT") {
                    empty = "E";
                    idval = document.getElementById("<%=ddl_description.ClientID %>");
                    idval.style.borderColor = 'Red';
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
             
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="lbl_health" runat="server" class="fontstyleheader" Style="color: Green;"
                    Text="Health Checkup Master"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 700px">
                <center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1" Width="140px"
                                            Height="20px">--select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" Width="200px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_hostelname_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostlname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_from" Text="From Date" runat="server">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1" Width="80px"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txt_fromdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1" Width="80px"
                                    AutoPostBack="true" OnTextChanged="txt_todate_Textchanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" OnClick="btn_go_Click" CssClass="textbox btn1"
                                    runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </center>
                <center>
                    <div id="mainspread" runat="server" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                        Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder"
                                        OnCellClick="Fpspread1_CellClick" OnPreRender="Fpspread1_SelectedIndexChanged">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <div id="printdiv" runat="server" visible="false" style="height: 100%; z-index: -1;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                    </div>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                            Text="Export To Excel" Width="127px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn2" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
            </div>
        </center>
        <center>
            <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red;"></asp:Label>
        </center>
        <center>
            <div id="popupstudaddinl" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 66px; margin-left: 235px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 500px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_studentadd1" runat="server" Text="Health Checkup Master" class="fontstyleheader"
                            Style="color: Green;"></asp:Label>
                        <p style="width: 691px;" align="center">
                            <asp:Label ID="lblstudent" runat="server" Visible="false" Font-Bold="true" Text=" No of Students:"
                                ForeColor="Red"></asp:Label>
                            <asp:Label ID="lblstudentcount" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </p>
                    </center>
                    <table>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <fieldset style="width: 215px; height: 15px;">
                                    <asp:RadioButtonList ID="rblstustaffguest" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblstustaffguest_Selected">
                                        <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Guest" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_rollno" runat="server" Text="Roll No"></asp:Label>
                                <asp:Label ID="lbl_stu" runat="server" Text="No Of Student" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_staff" runat="server" Text="No Of Staff" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_guest" runat="server" Text="No Of Guest" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_Sturollno" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_pop1staffname" Text="Staff Name" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_guestname" Text="Guest Name" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    Width="120px" onfocus="return myFunction(this)" OnTextChanged="txt_rollno_txtchange"
                                    AutoPostBack="true" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:TextBox ID="txt_stu" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    Width="120px" Visible="false" AutoPostBack="true" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:Button ID="btn_rollno" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_rollno_Click" />
                                <asp:TextBox ID="txt_pop1staffname" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                    onfocus="return myFunction(this)" Width="190px" Height="20px" AutoPostBack="true"
                                    OnTextChanged="Staffname_txtchange" MaxLength="30" Visible="false"></asp:TextBox>
                                <asp:Button ID="btnstaffname" Text="?" runat="server" OnClick="btnstaffname_Click"
                                    CssClass="textbox btn" Visible="false" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1staffname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txt_guestname" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                    onfocus="return myFunction(this)" Width="190px" Height="20px" AutoPostBack="true"
                                    OnTextChanged="Guestname_txtchange" MaxLength="30" Visible="false"></asp:TextBox>
                                <span style="color: Red;">*</span>
                                <asp:Button ID="btn_guestname" Text="?" runat="server" OnClick="btn_guestname_Click"
                                    CssClass="textbox btn" Visible="false" />
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getroll1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_guestname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_regno" runat="server" Text="Reg No"></asp:Label>
                                <asp:Label ID="lbl_staffcode" Text="Staff Code" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lbl_guCode" Text="Guest Code" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_regno" TextMode="SingleLine" ReadOnly="true" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="120px" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_regno"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_staffcode" runat="server" ReadOnly="true" BackColor="#DCF9D1"
                                    CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txt_gustCode" runat="server" ReadOnly="true" BackColor="#DCF9D1"
                                    CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr id="dept" runat="server">
                            <td>
                                <asp:Label ID="lbl_name" runat="server" Text="Name"></asp:Label>
                                <asp:Label ID="lbl_dept" Text="Department" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_name" TextMode="SingleLine" ReadOnly="true" runat="server" Height="20px"
                                    CssClass="textbox textbox1 txtheight4" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_name"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=".,-,@,(,), ,">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight5 textbox1"
                                    BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true" Visible="false"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr id="design" runat="server">
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                                <asp:Label ID="lbl_design" Text="Designation" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_degree" TextMode="SingleLine" ReadOnly="true" runat="server"
                                    Height="20px" CssClass="textbox textbox1 txtheight5" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_degree"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars="-, ,">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_design" runat="server" CssClass="textbox txtheight5 textbox1"
                                    BackColor="#DCF9D1" onfocus="return myFunction(this)" ReadOnly="true" Visible="false"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_hostelname1" TextMode="SingleLine" ReadOnly="true" runat="server"
                                    Height="20px" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_hostelname1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="-,@,(,),.,;, ,">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_date" runat="server" Width="80px" CssClass="textbox textbox1"
                                    onfocus="return myFunction(this)"></asp:TextBox>
                                <asp:CalendarExtender ID="caldate" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_description" runat="server" Text="Description"></asp:Label>
                            </td>
                            <td colspan='1'>
                                <asp:DropDownList ID="ddl_description" Width="150px" Height="30px" runat="server"
                                    CssClass="textbox textbox1" Style="float: left;" onchange="change1(this)" onfocus="return myFunction(this)">
                                </asp:DropDownList>
                                <asp:TextBox ID="txt_description" CssClass="textbox textbox1" Style="width: 200px;
                                    display: none; float: left;" onfocus="return myFunction(this)" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_amount" runat="server" Text="Amount"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_amount" TextMode="SingleLine" runat="server" MaxLength="15"
                                    Height="20px" CssClass="textbox textbox1" onfocus="return myFunction(this)" Width="100px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_amount"
                                    FilterType="numbers,Custom" ValidChars=".@">
                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <center>
                        <div>
                            <%-- <asp:Button ID="btn_update" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Update" onfocus="return checkEmail()" OnClick="btn_update_Click" />--%>
                            <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                                OnClientClick="return Test()" OnClientClick1="return Test1()" OnClientClick2="return Test()2" />
                            <asp:Button ID="btn_save1" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save1_Click"
                                OnClientClick="return Test1()" Visible="false" />
                            <asp:Button ID="btn_save2" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save2_Click"
                                OnClientClick="return Test2()" Visible="false" />
                            <asp:Button ID="btn_delete" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Delete" OnClick="btn_delete_Click" />
                            <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                        </div>
                    </center>
                </div>
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
                                <asp:Label ID="lbl_hostelname2" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname2" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="phstlnm" runat="server" Width="200px" Height="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_hostelname2" runat="server" OnCheckedChanged="cb_hostelname2_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_hostelname2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname2_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_hostelname2"
                                            PopupControlID="phstlnm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch" Width="120px" Height="30px" runat="server" CssClass="textbox textbox1"
                                    OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged" AutoPostBack="true" onfocus="return myFunction(this)">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree1" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
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
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
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
                                <asp:Label ID="lbl_rollno1" runat="server" Text="Roll No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno1" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                    Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_rollno1"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
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
                    <%--theivamani 14.11.15--%>
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
                            <asp:Button ID="btn_ok" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="btn_ok_Click" />
                            <asp:Button ID="btn_exit1" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit1_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupwindowstaff" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
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
                            <table class="maintablestyle" style="width: 80%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collegesatff" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegestaff" Width="250px" Height="30px" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_collegestaff_selectedindexchange"
                                            CssClass="textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_hostelname3" runat="server" Text="HostelName"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_hostelname3" runat="server" Height="20px" CssClass="textbox textbox1"
                                                    Width="165px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panelstaffhostel" runat="server" Width="200px" Height="200px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cb_hostelname3" runat="server" OnCheckedChanged="cb_hostelname3_ChekedChange"
                                                        Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbl_hostelname3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname3_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hostelname3"
                                                    PopupControlID="Panelstaffhostel" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--   </tr>
                                <tr>--%>
                                    <td>
                                        <asp:Label ID="lbl_staffdepartment" runat="server" Text="Department"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_staffdepartment" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight6" OnSelectedIndexChanged="ddl_staffdepartment_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_searchby" runat="server" Text="SearchBy"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_searchbystaff" Width="250px" Height="30px" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_searchbystaff_SelectedIndexChanged"
                                            CssClass="textbox1 ddlheight6">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="6">
                                        <asp:TextBox ID="txt_staffnamesearch" TextMode="SingleLine" runat="server" Height="20px"
                                            CssClass="textbox textbox1" Width="180px" placeholder="Staff Name"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_staffcodesearch" placeholder="Staff Code" Visible="false" TextMode="SingleLine"
                                            runat="server" Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCodepopup" MinimumPrefixLength="0" CompletionInterval="100"
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
                                <div id="div1" runat="server" visible="false" style="width: 877px; height: 368px;
                                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <br />
                                    <center>
                                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Style="overflow: auto;
                                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                            box-shadow: 0px 0px 8px #999999;">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </div>
                            </div>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_staffok" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="OK" OnClick="btn_staffok_Click" />
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
            <div id="DivGuestpopupwindow" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                    OnClick="imagebtnpopclose3_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="Label2" CssClass="fontstyleheader" runat="server" Style="color: Green;"
                            Text="Select the Guest Name"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" style="width: 80%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_GuestHostelname" Text="Hostel Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_hostelname" runat="server" Visible="false" AutoPostBack="True"
                                            CssClass="textbox ddlheight4">
                                        </asp:DropDownList>
                                        <asp:UpdatePanel ID="upp_hostelname1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_guesthostelname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                    ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_hostelname" runat="server" BorderStyle="Solid" BorderWidth="2px"
                                                    CssClass="multxtpanel" Style="position: absolute; height: 200px; width: 180px;">
                                                    <asp:CheckBox ID="cb_hostelname1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_hostelname1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_hostelname1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostelname1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popupext_hostelname" runat="server" TargetControlID="txt_guesthostelname"
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
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_guest" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btnguest_go_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <p>
                        <asp:Label ID="lbl_errorsearch2" runat="server" Visible="false" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                    </p>
                    <p style="width: 691px;" align="right">
                        <asp:Label ID="lbl_errorsearch3" runat="server" Visible="false" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                    </p>
                    <div id="divGuest" runat="server" visible="false" style="width: 877px; height: 368px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <center>
                            <FarPoint:FpSpread ID="FpSpreadguest" runat="server" Visible="false" Width="850px"
                                Height="330px" class="spreadborder table" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </div>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_guestok" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="OK" OnClick="btn_guestok_Click" />
                            <asp:Button ID="btn_guestexit" runat="server" Visible="false" CssClass="textbox btn2"
                                Text="Exit" OnClick="btn_guestexit_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
        </form>
    </body>
    </html>
</asp:Content>
