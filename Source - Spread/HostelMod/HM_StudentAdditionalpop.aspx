<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_StudentAdditionalpop.aspx.cs" Inherits="HM_StudentAdditionalpop" %>

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
            function display() {
                document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="lbl_studentadd" runat="server" class="fontstyleheader" Style="color: Green;"
                    Text="Student Additional Collection"></asp:Label>
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
                                <asp:RadioButton ID="rdb_cumulative" Text="Cumulative" runat="server" GroupName="amonut"
                                    AutoPostBack="true" OnCheckedChanged="rdb_cumulative_checkedchanged" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_detail" Text="Detailed" runat="server" GroupName="amonut"
                                    AutoPostBack="true" OnCheckedChanged="rdb_detail_checkedchanged" />
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
                    <center>
                        <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red;"></asp:Label></center>
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="770px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                    </div>
                    <br />
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="790px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -450px;"
                                            Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tborder" Visible="false" Width="770px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="750px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                            <%--<asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>--%>
                                            <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Reg_No">Reg No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Stud_Name">Name</asp:ListItem>
                                            <asp:ListItem Value="Degree">Degree</asp:ListItem>
                                            <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Transdate">Date</asp:ListItem>
                                            <asp:ListItem Value="MasterValue">Description</asp:ListItem>
                                            <asp:ListItem Value="Amount">Amount</asp:ListItem>
                                                 <asp:ListItem Selected="True" Value="id">Student Id</asp:ListItem>
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
                </center>
                <center>
                    <div id="mainspread" runat="server" style="overflow: auto; height: 300px; width: 850px;
                        border: 0px solid #999999; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderStyle="NotSet"
                            AutoPostBack="true" OnCellClick="Fpspread1_CellClick" OnPreRender="Fpspread1_SelectedIndexChanged"
                            BorderWidth="0px" Width="800px">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
                <center>
                </center>
                <br />
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" ForeColor="#FF3300" Text="" Visible="False"></asp:Label>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                        <%--   theivamani 15.10.15--%>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupstudaddinl" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 68px; margin-left: 288px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 450px; width: 600px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_studentadd1" runat="server" Text="Student Additional Collection"
                            class="fontstyleheader" Style="color: Green;"></asp:Label>
                        <p style="width: 691px;" align="center">
                            <asp:Label ID="lblstudent" runat="server" Visible="false" Font-Bold="true" Text=" No of Students:"
                                ForeColor="Red"></asp:Label>
                            <asp:Label ID="lblstudentcount" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </p>
                    </center>
                  
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_rollno" runat="server" Text="Roll No"></asp:Label>
                                 <asp:Label ID="lbl_stu" runat="server" Text="No Of Student" Visible="false"></asp:Label>
                                 <asp:Label ID="lbl_Sturollno" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rollno" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    Width="120px" onfocus="return myFunction(this)" OnTextChanged="txt_rollno_txtchange"
                                    AutoPostBack="true"></asp:TextBox>
                                    <asp:TextBox ID="txt_stu" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    Width="120px" Visible="false"
                                    AutoPostBack="true"></asp:TextBox>
                                <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_rollno"
                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                            </asp:FilteredTextBoxExtender>--%>
                                <%-- theivamani 14.11.15--%>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:Button ID="btn_rollno" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_rollno_Click" />
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                           <td>
                                <asp:Label ID="Label3" Text="Student Id" runat="server"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="Txtid" runat="server" CssClass="textbox textbox1 txtheight4" Enabled="false"
                                  ></asp:TextBox>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_regno" runat="server" Text="Reg No"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_regno" TextMode="SingleLine" ReadOnly="true" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_regno"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_name" runat="server" Text="Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_name" TextMode="SingleLine" ReadOnly="true" runat="server" Height="20px"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_name"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=".,-,@,(,), ,">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_degree" TextMode="SingleLine" ReadOnly="true" runat="server"
                                    Height="20px" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_degree"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars="-, ,">
                                </asp:FilteredTextBoxExtender>
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
                            <asp:Button ID="btn_update" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Update" onfocus="return checkEmail()" OnClick="btn_update_Click" />
                            <asp:Button ID="btn_delete" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Delete" OnClick="btn_delete_Click" />
                            <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click"
                                OnClientClick="return Test()" />
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
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_batch" Width="120px" Height="30px" runat="server" CssClass="textbox textbox1"
                                    OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged" AutoPostBack="true" onfocus="return myFunction(this)">
                                </asp:DropDownList>
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
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;" OnUpdateCommand="Fpspread2_UpdateCommand">
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
