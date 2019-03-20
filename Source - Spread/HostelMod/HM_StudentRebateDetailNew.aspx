<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_StudentRebateDetailNew.aspx.cs" Inherits="HM_StudentRebateDetailNew" %>

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
                position: fixed;
                width: 100%;
                z-index: 1000;
                height: 100px;
                background-color: lightblue;
                border-style: 1px;
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

            function daysvalidate() {
                var id = "";
                var id1 = "";
                id = document.getElementById("<%=txt_leavedays.ClientID %>").value;
                id1 = document.getElementById("<%=txt_rebatedays.ClientID %>").value;
                if (id.trim() < id1.trim()) {
                    var id1new = document.getElementById("<%=txt_rebatedays.ClientID %>");
                    id1new.value = "";
                    alert('Rebate days should be less than Leave days.')
                    return false;
                }
                else if (id.trim() >= id1.trim()) {
                    return true;
                }
            }

            function valid1() {
                var idval = "";
                var idval1 = "";
                var empty = "";

                idval = document.getElementById("<%=txt_rollno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_rollno.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_fromdate1.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_fromdate1.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txt_todate1.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_todate1.ClientID %>");
                    idval.style.borderColor = 'Red';
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
        <div>
            <br />
            <center>
                <center>
                    <div>
                        <asp:Label ID="lblstu" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Student Rebate Details"></asp:Label>
                    </div>
                </center>
                <br />
                <div class="maindivstyle" style="width: 1000px; height: 500px;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td width="160px">
                                <asp:UpdatePanel ID="upp_hostel" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_hostel" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="chkhstlname_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklsthstlname_Change">
                                            </asp:CheckBoxList>
                                            &nbsp;
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_hostel" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="panel_hostel" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_fromdate" runat="server" OnCheckedChanged="chkfrdate_CheckedChanged"
                                    AutoPostBack="true" />
                                &nbsp;
                                <asp:Label ID="lbl_fromdate" Text="From Date" runat="server">
                                </asp:Label>
                                &nbsp;<asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                &nbsp;
                                <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                                &nbsp;
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight"
                                    AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calext_todate" TargetControlID="txt_todate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_rebatetype" Text="Rebate Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_rebatetype" runat="server" CssClass="textbox textbox1 ddlheight4"
                                    Width="171px">
                                    <asp:ListItem Text="Rebate Days" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Rebate Amount" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td> <asp:RadioButton ID="Rdbst"  Text="Student" runat="server" AutoPostBack="true" Checked="true"
                                        GroupName="fix1" OnCheckedChanged="rdb_guest_CheckedChange" />
                               
                                    <asp:RadioButton ID="rdbsta"  Text="Staff" runat="server" AutoPostBack="true"
                                        GroupName="fix1" OnCheckedChanged="rdb_guest_CheckedChange"  />
                                
                                    <asp:RadioButton ID="rdbgue"  Text="Guest" runat="server" AutoPostBack="true"
                                        GroupName="fix1" OnCheckedChanged="rdb_guest_CheckedChange" /></td>
                            <td>
                            
                                <asp:Button ID="btn_go" Text="Go" OnClick="btngo_Click" CssClass="textbox btn1" runat="server" />
                                &nbsp;
                                <asp:Button ID="btn_addnew" Text="Add New" runat="server" CssClass="textbox btn2"
                                    OnClick="btnaddnew_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <asp:Label ID="lbl_errrepor" runat="server" ForeColor="Red" Visible="false"></asp:Label></center>
                    <br />
                    <center>
                        <div id="Divspread" runat="server" visible="false" style="width: 850px; height: 300px;
                            overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                            box-shadow: 0px 0px 8px #999999;">
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" OnCellClick="Cell_Click1" OnPreRender="Fpspread_render"
                                Visible="false" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Width="680px"
                                Style="height: 350px; overflow: auto; background-color: White;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                onkeypress="display()"></asp:TextBox>
                            <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                                Text="Export To Excel" Width="127px" Height="30px" />
                            <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                                Width="60px" Height="30px" CssClass="textbox" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                    <br />
                    <div>
                        <center>
                            <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btnsave_Click" />
                        </center>
                    </div>
                </div>
            </center>
        </div>
        <center>
            <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: 486px; width: 815px;">
                    <asp:ImageButton ID="imgbtn_popclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -38px; margin-left: 400px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lblstu1" runat="server" class="fontstyleheader" Style="color: Green;"
                                Text="Student Rebate Details"></asp:Label>
                        </div>
                        <p style="width: 691px;" align="center">
                            <asp:Label ID="lblstudent" runat="server" Visible="false" Font-Bold="true" Text=" No of Students:"
                                ForeColor="Red"></asp:Label>
                            <asp:Label ID="lblstudentcount" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </p>
                    </center>
                    <table>
                    <tr>
                         <td>
                                    <asp:RadioButton ID="rdb_student"  Text="Student" runat="server" AutoPostBack="true" Checked="true"
                                        GroupName="fix" OnCheckedChanged="rdb_guest_CheckedChange" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_staff"  Text="Staff" runat="server" AutoPostBack="true"
                                        GroupName="fix" OnCheckedChanged="rdb_guest_CheckedChange"  />
                                
                                    <asp:RadioButton ID="rdb_guest"  Text="Guest" runat="server" AutoPostBack="true"
                                        GroupName="fix" OnCheckedChanged="rdb_guest_CheckedChange" />
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_rollno" Text="Roll No" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox textbox1 txtheight4"
                                    OnTextChanged="txt_rollno_Changed" AutoPostBack="true" onfocus="return myFunction(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ()@-">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <span style="color: Red;">*</span>
                                <asp:Button ID="btn_question" Text="?" runat="server" CssClass="textbox btn" OnClick="btnquestion_Click" />
                                 <td>
                                <asp:Label ID="Label4" Text="Roll No" runat="server" Visible="false"></asp:Label>
                            </td>
                            </td>
                            </tr>
                            <tr>
                            
                            <td>
                                <asp:Label ID="Label3" Text="Student Id" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="Txtid" runat="server" CssClass="textbox textbox1 txtheight4"
                                   onfocus="return myFunction(this)" AutoPostBack="true" OnTextChanged="txt_rollno_Changed"></asp:TextBox>
                                   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txtid"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=". ()@-">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrno1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txtid"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <span style="color: Red;">*</span>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_name" Text="Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_name" runat="server" CssClass="textbox textbox1 txtheight5"
                                    OnTextChanged="txt_name_Changed" AutoPostBack="true"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getrname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <%--<asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                </asp:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_degree" Text="Degree" ReadOnly="true" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname1" Text="Hostel Name" ReadOnly="true" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_hostelname1" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_days" Text="Rebate Days" runat="server" AutoPostBack="true"
                                    GroupName="same2" Checked="true" OnCheckedChanged="rdbdays_CheckedChanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_rebateamt" Text="Rebate Amount" runat="server" GroupName="same2"
                                    OnCheckedChanged="rdbrebateamt_CheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_rebatedate" Text="Rebate Date" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rebatedate" runat="server" CssClass="textbox textbox1 txtheight"
                                    Visible="false"></asp:TextBox>
                                <asp:CalendarExtender ID="calext_rebatedate" TargetControlID="txt_rebatedate" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rebateamt" Text="Rebate Amount" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rebateamt" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_rebateamt" runat="server" TargetControlID="txt_rebateamt"
                                    FilterType="Numbers ,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate1" Text="From Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate1" runat="server" CssClass="textbox textbox1 txtheight"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate1_TextChanged" onfocus="return myFunction(this)"></asp:TextBox>
                                <asp:CalendarExtender ID="calext_fromdate1" TargetControlID="txt_fromdate1" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <span style="color: Red;">*</span>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate1" Text="To date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate1" runat="server" CssClass="textbox textbox1 txtheight"
                                    AutoPostBack="true" onfocus="return myFunction(this)" OnTextChanged="txt_todate1_TextChanged1"></asp:TextBox>
                                <asp:CalendarExtender ID="calpoptodate" TargetControlID="txt_todate1" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_leavedays" Text="Leave Days" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_leavedays" runat="server" Enabled="false" onblur="return daysvalidate()"
                                    ReadOnly="true" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_leavedays" runat="server" TargetControlID="txt_leavedays"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_rebatedays" Text="Rebate Days" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rebatedays" runat="server" onblur="return daysvalidate()" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftext_rebatedays" runat="server" TargetControlID="txt_rebatedays"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_description" Text="Description" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                <asp:DropDownList ID="ddl_description" runat="server" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                </asp:DropDownList>
                                <asp:Button ID="btn_minus" runat="server" Text="-" Font-Size="Medium" Font-Names="Book Antiqua"
                                    CssClass="textbox btn" OnClick="btnminus_Click" />
                            </td>
                        </tr>
                    </table>
                    <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                            <asp:TextBox ID="txt_description11" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                text-transform: capitalize; margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:TextBox>
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
                    <br />
                    <center>
                        <asp:Button ID="btn_update" Text="Update" Visible="false" OnClick="btnupdate_Click"
                            CssClass="textbox btn2" OnClientClick="return daysvalidate()" runat="server" />
                        <asp:Button ID="btn_delete" Text="Delete" Visible="false" OnClick="btndelete_Click"
                            CssClass="textbox btn2" runat="server" />
                        <asp:Button ID="btn_exit_fp" Text="Exit" Visible="false" OnClick="btn_exit_fp_Click"
                            CssClass="textbox btn2" runat="server" />
                        <asp:Button ID="btn_save1" Text="Save" runat="server" CssClass="textbox btn2" OnClick="btnpopsave_Click"
                            OnClientClick="return valid1()" />
                        <asp:Button ID="btn_exit1" Text="Exit" runat="server" CssClass="textbox btn2" OnClick="btnpopexit_Click" /></center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupselectstd" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: 650px; width: 850px;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -38px; margin-left: 415px;"
                        OnClick="imagebtnpopclose1_Click" />
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="Label1" runat="server" class="fontstyleheader" Style="color: Green;"
                                Text="Select the Student"></asp:Label></div>
                    </center>
                    <br />
                    <p style="width: 691px;" align="right">
                        <asp:Label ID="Label2" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </p>
                    <center>
                        <table class="maintablestyle">
                        
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname2" runat="server" Text="Hostel Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_hostel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname2" runat="server" CssClass="textbox textbox1 txtheight4"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostel2" runat="server" Height="200px" Width="200px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_hostelname2" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkhostlnm_ChekedChange" />
                                                <asp:CheckBoxList ID="cbl_hostelname2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklsthostlnm_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popext_hostel2" runat="server" TargetControlID="txt_hostelname2"
                                                PopupControlID="panel_hostel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch2" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_batch2" runat="server" CssClass="textbox textbox1 ddlheight1"
                                        onfocus="return myFunction(this)" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree2" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_degree2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree2" runat="server" CssClass="textbox textbox1 txtheight4"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_degree2" runat="server" Height="180px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_degree2" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkdeg_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstdeg_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_degree2" runat="server" TargetControlID="txt_degree2"
                                                PopupControlID="panel_degree2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_branch2" Text="Branch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp_branch2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch2" runat="server" CssClass="textbox textbox1 txtheight4"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_branch2" runat="server" Height="200px" Width="250px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_branch2" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="chkbnch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_branch2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstbnch_Change">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_branch2" runat="server" TargetControlID="txt_branch2"
                                                PopupControlID="panel_branch2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_rollnum2" runat="server" Text="Roll No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_rollnum2" runat="server" CssClass="textbox textbox1 txtheight3">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_rollnum2" runat="server" TargetControlID="txt_rollnum2"
                                        FilterType="Numbers,UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_rollnum2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollnum2"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go2" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btnpopgo_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label></center>
                    <%--theivamani 14.11.15--%>
                    <p style="width: 691px;" align="right">
                        <asp:Label ID="lbl_count" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </p>
                    <center>
                        <div id="spreaddiv" runat="server" visible="false" style="width: 757px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">
                            <br />
                           <%-- <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="Solid" 
                                BorderWidth="1px" Width="730px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread2_render"  OnRowCommand="Fpspread2_RowCommand"   OnUpdateCommand="Fpspread2_UpdateCommand" OnRowDataBound="Fpspread2_RowDataBound">--%>
                                      <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="Solid" 
                                BorderWidth="1px" Width="730px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread2_render" OnUpdateCommand="Fpspread2_UpdateCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <asp:Button ID="btnSelectStudent" CssClass="textbox btn" runat="server" Width="70px"
                            Visible="false" Text="Ok" OnClick="btnSelectStudent_Click" />
                    </center>
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="Button1" runat="server" CssClass="textbox btn2" Text="Save" OnClick="buttonsv_Click" />
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
                    <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
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
        <center>
            <div id="surediv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div31" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sureminus" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yesminus" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyesminus_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_nominus" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btn_surenominus_Click" Text="no" runat="server" />
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
