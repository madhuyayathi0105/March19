<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_HostelMaster.aspx.cs" Inherits="HM_HostelMaster" %>

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
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <%--<script src="Styles/~/Scripts/jquery-latest.min.js" type="text/javascript"></script>--%>
        <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    </head>
    <style type="text/css">
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .col4
        {
            float: right;
            width: 50%;
            height: 210px;
        }
        .col3
        {
            float: right;
            width: 40%;
            height: 40%;
        }
        .lower
        {
            text-transform: lowercase;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
        }
        function Validate() {
            if (document.getElementById("<%=txt_warden.ClientID %>").value == '') {
                alert('Warden Name1 can not be empty.')
                return false;
            }
        }
        function Test() {
            var id = "";
            var idvl = "";
            var empty = "";

            id = document.getElementById("<%=txt_hostelname1.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_hostelname1.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddl_messbill.ClientID %>").value;
            if (id.trim().toUpperCase() == "SELECT") {
                id = document.getElementById("<%=ddl_messbill.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddl_messhed.ClientID %>").value;
            if (id.trim().toUpperCase() == "SELECT") {
                id = document.getElementById("<%=ddl_messhed.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddl_messmaster.ClientID %>").value;
            if (id.trim().toUpperCase() == "SELECT") {
                id = document.getElementById("<%=ddl_messmaster.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_building.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_building.ClientID %>");
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
        function myFunction(x) {
            x.style.borderColor = "#c4c4c4";
        }

        function checkEmail(id) {
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!filter.test(id.value)) {
                id.style.borderColor = 'Red';
                id.value = "";
                email.focus;
            }
            else {
                id.style.borderColor = '#c4c4c4';
            }
        }
        function get(txt1) {
            $.ajax({
                type: "POST",
                url: "HM_HostelMaster.aspx/CheckUserName",
                data: '{HostelName: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function OnSuccess(response) {
            var mesg = $("#msg1")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Hostel Name Not Exist";
                    break;
                case "1":
                    mesg.style.color = "green";
                    document.getElementById('<%=txt_hostelname1.ClientID %>').value = "";
                    mesg.innerHTML = "Hostel Name Available";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter Hostel Name";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

    </script>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="Label2" runat="server" Style="color: Green;" Text="Hostel Master"
                    CssClass="fontstyleheader"></asp:Label>
            </center>
            <br />
        </div>
        <center>
            <div class="maindivstyle" style="height: 900px; width: 1000px;">
                <center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Visible="false" Text="College Name"></asp:Label>
                                <asp:DropDownList ID="ddl_college" runat="server" Visible="false" CssClass="textbox textbox1"
                                    Height="30px" Width="230px" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1" Width="180px"
                                            Height="20px">--select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" Height="250px" Width="200px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_hostelname_ChekedChange" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Visible="true" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lnk_hosteladmissionserialno" Text="Admission Serial No Generation"
                                    runat="server" OnClick="lnk_hosteladmissionserialno_click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbl_errormsg" runat="server" Style="color: Red; float: left;"></asp:Label>
                </center>
                <div>
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="970px" Style="margin-top: -0.1%;">
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
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="980px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -499px;"
                                        Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_border" Visible="false" Width="930px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cbl_columnorder" runat="server" Height="43px" AutoPostBack="true"
                                        Width="930px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                        <%--<asp:ListItem Selected="True" Value="Stud_Name">Name</asp:ListItem>--%>
                                        <asp:ListItem Selected="True" Value="HostelName">Hostel Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="WardenStaff1PK">Warden Name1</asp:ListItem>
                                        <asp:ListItem Value="WardentStaff2PK">Warden Name2</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="HostelBuildingFK">Building Name</asp:ListItem>
                                        <asp:ListItem Value="PhoneNo">Phone No</asp:ListItem>
                                        <asp:ListItem Value="PhoneExtNo">Extension No</asp:ListItem>
                                        <asp:ListItem Value="MobileNo">Mobile No</asp:ListItem>
                                        <asp:ListItem Value="EmailID">Email</asp:ListItem>
                                        <asp:ListItem Value="RoomRentLedgerFK">Room Rent Ledger</asp:ListItem>
                                        <asp:ListItem Value="HostelAdmFeeAmount">Hostel Admission Fee</asp:ListItem>
                                        <asp:ListItem Value="HostelAdmFeeLedgerFK">Student Fee Ledger</asp:ListItem>
                                        <asp:ListItem Value="NessBukkLedgerFK">Mess Bill Ledger</asp:ListItem>
                                        <asp:ListItem Value="MessBillDSLedgerFK">Mess Bill Ledger(Dayscholar)</asp:ListItem>
                                        <%-- <asp:ListItem Value="Pay_Type">Pay Type</asp:ListItem>
                                    <asp:ListItem Value="Mess_FixedFeeAmt">Mess Fee</asp:ListItem>--%>
                                        <asp:ListItem Value="MessBillPayDueDays">Due Days</asp:ListItem>
                                        <asp:ListItem Value="MessBillType">Mess Bill Type</asp:ListItem>
                                        <asp:ListItem Value="MessBillMethod">Fixed Type</asp:ListItem>
                                        <asp:ListItem Value="HostelType">Gender</asp:ListItem>
                                        <asp:ListItem Value="IsHostelGatePassPer">Gate Pass</asp:ListItem>
                                        <asp:ListItem Value="HostelGatePassPerCount">Gate PerCount</asp:ListItem>
                                        <asp:ListItem Value="IsAllowUnApproveStud">Un Approved Students GatePass</asp:ListItem>
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
                <center>
                    <div>
                        <%--style="float: left; width: 400px;"--%>
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false">
                        </asp:Label>
                    </div>
                </center>
                <br />
                <center>
                    <div id="fpreaddiv" visible="false" runat="server" style="overflow: auto; height: 400px;
                        border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" AutoPostBack="true" OnCellClick="FpSpread1_CellClick"
                            OnPreRender="FpSpread1_SelectedIndexChanged" Width="950px">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
                <center>
                    <asp:Label ID="lbl_norec" Visible="False" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text=""></asp:Label>
                </center>
                <br />
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight5"
                            onkeypress="display()"></asp:TextBox>
                        <%--OnTextChanged="txtexcelname_TextChanged"--%>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Font-Bold="true"
                            Width="150px" CssClass="textbox btn2" AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Font-Bold="true" Text="Print" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow" runat="server" visible="false" class="popupstyle popupheight1">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 32px; margin-left: 464px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 705px; width: 947px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lblhstlmstr" runat="server" Text="Hostel Master" Style="font-size: large;
                            color: Green; font-weight: bold;"></asp:Label>
                    </center>
                    <br />
                    <table>
                        <tr>
                            <td colspan='1'>
                                <asp:Label ID="lbl_college1" Visible="false" runat="server" Width="100px" Text="College Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_college1" Visible="false" runat="server" CssClass="textbox textbox1"
                                    Height="30px" Width="230px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_hostelname1" TextMode="SingleLine" runat="server" Height="20px"
                                    CssClass="textbox textbox1" Width="210px" onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                <span style="color: Red;">*</span><span style="font-size: medium;" id="msg1"></span>
                                <asp:FilteredTextBoxExtender ID="filtertextbox" runat="server" TargetControlID="txt_hostelname1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" & -">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_gender" runat="server" Text="Gender"></asp:Label>
                                <asp:RadioButton ID="rdb_male" Text="Male" runat="server" GroupName="gender" />
                                <asp:RadioButton ID="rdb_female" Text="Female" runat="server" GroupName="gender" />
                                <asp:RadioButton ID="rdb_both" Text="Both" runat="server" GroupName="gender" />
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_warden" runat="server" Text="Warden Name1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_warden" TextMode="SingleLine" onfocus="return myFunction(this)"
                                    runat="server" Height="20px" AutoPostBack="true" OnTextChanged="wardendeg" CssClass="textbox textbox1"
                                    Width="165px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_warden"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=". ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_warden"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:Button ID="btn_warden" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_warden_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_department" runat="server" Width="80px" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_department" ReadOnly="true" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="180px" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_department"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td colspan='1'>
                                <asp:Label ID="lbl_designation" runat="server" Text="Designation"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_designation" ReadOnly="true" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="140px" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txt_designation"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_warden1" runat="server" Text="Warden Name2"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_warden1" AutoPostBack="true" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" OnTextChanged="wardendeg1" Width="165px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_warden1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=". ">
                                </asp:FilteredTextBoxExtender>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_warden1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:Button ID="btn_warden1" runat="server" OnClientClick="return Validate()" Text="?"
                                    CssClass="textbox btn" OnClick="btn_warden1_Click" />
                            </td>
                            <td colspan='1'>
                                <asp:Label ID="lbl_department1" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_department1" ReadOnly="true" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="180px" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txt_department1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td colspan='1'>
                                <asp:Label ID="lbl_designation1" runat="server" Text="Designation"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_designation1" ReadOnly="true" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="140px" BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txt_designation1"
                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_building" runat="server" Text="Building"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_building" onfocus="return myFunction(this)" TextMode="SingleLine"
                                    ReadOnly="true" runat="server" Height="20px" CssClass="textbox textbox1" Width="400px"
                                    BackColor="#DCF9D1"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_building"
                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=", ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_building" runat="server" Text="?" CssClass="textbox btn" OnClick="btn_building_click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <asp:Label ID="lbl_phone" runat="server" Text="Phone No"></asp:Label>
                                <asp:TextBox ID="txt_phone" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    MaxLength="12" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_phone"
                                    FilterType="numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_extension" runat="server" Text="Extension No"></asp:Label>
                                <asp:TextBox ID="txt_extension" MaxLength="10" TextMode="SingleLine" runat="server"
                                    Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_extension"
                                    FilterType="numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_mobile" runat="server" Text="Mobile"></asp:Label>
                                <asp:TextBox ID="txt_mobile" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                    MaxLength="12" Width="120px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_mobile"
                                    FilterType="numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_email" runat="server" Text="Email"></asp:Label>
                                <asp:TextBox ID="txt_email" TextMode="SingleLine" runat="server" Height="20px" CssClass="lower textbox textbox1"
                                    Width="170px" onfocus="return myFunction(this)" onblur="return checkEmail(this)"
                                    MaxLength="30"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_email"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=".@">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lbl_rrh" runat="server" Text="Room Rent Header"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddl_rrh" Width="200px" Height="30px" AutoPostBack="true" runat="server"
                                    CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_rrh_Selectedindex_Changed">
                                </asp:DropDownList>
                                <asp:Label ID="lbl_rrl" runat="server" Text="Room Rent Ledger"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddl_rrl" AutoPostBack="true" Width="200px" Height="30px" runat="server"
                                    CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_hosteladjexe_Selectedindex_Changed">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Label ID="lbl_hostelheader" runat="server" Text="Hostel Admission Fee Header"></asp:Label>
                                <asp:DropDownList ID="ddl_hosteladdheader" AutoPostBack="true" Width="200px" Height="30px"
                                    runat="server" CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_hosteladjexe_Selectedindex_Changed">
                                </asp:DropDownList>
                                <asp:Label ID="lbl_hosteledger" runat="server" Text="Hostel Admission Fee Ledger"></asp:Label>
                                <asp:DropDownList ID="ddl_hosteledger" AutoPostBack="true" Width="200px" Height="30px"
                                    runat="server" CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_hosteledger_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbl_studentledger" runat="server" Text="Student Admission Fee"></asp:Label>
                                <asp:TextBox ID="txt_studentledger" Enabled="false" TextMode="SingleLine" MaxLength='13'
                                    runat="server" Height="20px" CssClass="textbox textbox1" Width="100px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_studentledger"
                                    FilterType="numbers,custom" ValidChars=".@">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Select The Mess Name" Width="158px"></asp:Label>
                            </td>
                         
                             <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Mess" runat="server" Visible="true" CssClass="textbox textbox1"
                                    Width="82px" ReadOnly="true" Height="20px" >--Select--</asp:TextBox>
                                <asp:Panel ID="pflrnm" runat="server" Visible="true" CssClass="multxtpanel" Width="155px"
                                    Height="250px">
                                    <asp:CheckBox ID="cb_Mess" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_Mess_CheckedChange" />
                                    <asp:CheckBoxList ID="cbl_Mess" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Mess_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_Mess"
                                    PopupControlID="pflrnm" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    
                                <asp:DropDownList ID="ddl_messmaster" runat="server" CssClass="textbox ddlheight4" Visible="false"
                                    AutoPostBack="true" onfocus="return myFunction(this)">
                                </asp:DropDownList>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                    </table>
                    <div id="div6" runat="server" class="col1" visible="false">
                        <fieldset>
                            <legend>Mess Bill Type </legend>
                            <div id="div2" runat="server" class="col1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_fixed" Text="Fixed" runat="server" GroupName="bill" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_fixedpur" Text="Fixed & additional purchase" runat="server"
                                                GroupName="bill" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="div1" runat="server" class="col3">
                                <fieldset>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_div" Text="Dividend" AutoPostBack="true" OnCheckedChanged="rdb_div_CheckedChanged"
                                                    runat="server" GroupName="mbill" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdb_nondiv" Text="Non-Dividend" OnCheckedChanged="rdb_div_CheckedChanged"
                                                    AutoPostBack="true" runat="server" GroupName="mbill" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <table style="width: 439px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_messheader" runat="server" Text="Mess Bill Header"></asp:Label>
                                        <asp:DropDownList ID="ddl_messhed" Width="110px" Height="30px" runat="server" CssClass="textbox textbox1"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_messhedSelectedindex_Changed"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbl_messledger" runat="server" Text="Ledger"></asp:Label>
                                        <asp:DropDownList ID="ddl_messbill" Width="120px" Height="30px" runat="server" CssClass="textbox textbox1"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_paytype" runat="server" Text="Pay Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_monthly" Text="Monthly" runat="server" Enabled="false" GroupName="type" />
                                            <asp:RadioButton ID="rdb_yearly" Text="Yearly" runat="server" Enabled="false" GroupName="type" />
                                            <asp:RadioButton ID="rdb_sem" Text="Semester" runat="server" Enabled="false" GroupName="type" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_messfee" runat="server" Text="Mess Fee"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_messfee" Enabled="false" TextMode="SingleLine" MaxLength='13'
                                                runat="server" Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_messfee"
                                                FilterType="numbers,custom" ValidChars=".@">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_duedate" runat="server" Text="Days"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_duedate" TextMode="SingleLine" Visible="false" runat="server"
                                                Height="20px" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                            <asp:DropDownList ID="ddl_days" Width="120px" Height="30px" runat="server" Enabled="false"
                                                CssClass="textbox textbox1">
                                            </asp:DropDownList>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_duedate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_rebate" runat="server" Enabled="false" Text="Include Rebate Days" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </fieldset>
                    </div>
                    <div id="div5" runat="server" class="col2" visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_messbillhed" runat="server" Text="Mess Bill Header"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_messbillded" Width="150px" Height="30px" runat="server"
                                        AutoPostBack="true" CssClass="textbox textbox1" OnSelectedIndexChanged="ddl_messbillhedSelectedindex_Changed">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_messdayscholar" runat="server" Text="Mess Bill Ledger
                                                                           (For Day scholar)"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_messdayscholar" Width="150px" Height="30px" runat="server"
                                        CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="cb_gatepass" runat="server" Text="Gate Pass Permission" AutoPostBack="True"
                                        OnCheckedChanged="cb_gatepass_CheckedChanged" />
                                    <asp:TextBox ID="txt_gatepass" TextMode="SingleLine" runat="server" Height="20px"
                                        CssClass="textbox textbox1" Width="100px" MaxLength="2"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_gatepass"
                                        FilterType="numbers,custom" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_unappgatepass" Text="Allow Un Approved Students in Gate Pass"
                                        AutoPostBack="True" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_errormsg1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <center>
                        <div class="col4">
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Button ID="btn_save" runat="server" Visible="false" CssClass="textbox btn2"
                                Text="Save" OnClick="but_save_Click" OnClientClick="return Test()" />
                            <asp:Button ID="btn_update" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Update" onfocus="return checkEmail()" OnClick="btn_update_Click" />
                            <asp:Button ID="btn_delete" Visible="false" runat="server" CssClass="textbox btn2"
                                Text="Delete" OnClick="btn_delete_Click" />
                            <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 32px; margin-left: 439px"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_selectstaffcode" runat="server" Style="color: green;" Text="Select the Staff Name"
                            CssClass="fontstyleheader"></asp:Label>
                    </center>
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" style="width: 72%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college2" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college2" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_college2_selectedindexchange" CssClass="textbox textbox1">
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
                                        <asp:DropDownList ID="ddl_searchby" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_searchby_SelectedIndexChanged" CssClass="textbox textbox1">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_searchby" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_searchby"
                                            FilterType="uppercaseletters,lowercaseletters,custom" ValidChars=" .">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_wardencode" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_wardencode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:Button ID="btn_go2" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go2_Click" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <p>
                                    <%--style="width: 691px;" align="right"--%>
                                    <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </p>
                                <p>
                                    <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                        ForeColor="Red"></asp:Label>
                                </p>
                                <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="846px" Style="overflow: auto;
                                    height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0099CC">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_save1" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save1_Click" />
                                    <asp:Button ID="btn_exit2" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_exit2_Click" />
                                </div>
                            </center>
                        </center>
                        <center>
                            <div style="width: 425px; float: left;">
                                <asp:Label ID="err" ForeColor="Red" Visible="false" runat="server"></asp:Label>
                            </div>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="popupbuild1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 98px; margin-left: 240px;"
                    OnClick="imagebtnpopclose3_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 400px; width: 500px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <br />
                        <asp:Label ID="lbl_selectbuild" runat="server" Style="color: green;" CssClass="fontstyleheader"
                            Text="Select the Building"></asp:Label>
                        <br />
                        <br />
                    </center>
                    <div>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_building1" runat="server" Text="Building Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_building1" TextMode="SingleLine" runat="server" Height="20px"
                                            CssClass="textbox textbox1" Width="200px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txt_building1"
                                            FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="getbuilding" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_building1"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go3" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                            <center>
                                <div>
                                    <p>
                                        <asp:Label ID="lbl_error3" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </p>
                                    <FarPoint:FpSpread ID="fpbuild" runat="server" Visible="false" OnUpdateCommand="fpbuild_Command"
                                        Width="700px" Style="overflow: auto; height: 500px; border: 0px solid #999999;
                                        border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <br />
                                <br />
                                <center>
                                    <div>
                                        <asp:Button ID="btn_ok" runat="server" Visible="false" CssClass="textbox btn2" Text="Ok"
                                            OnClick="btn_ok_Click" />
                                        <asp:Button ID="btn_exit3" runat="server" Visible="false" CssClass="textbox btn2"
                                            Text="Exit" OnClick="btn_exit3_Click" />
                                    </div>
                                </center>
                            </center>
                        </center>
                    </div>
                </div>
            </div>
        </center>
        <center>
            <%--barath 31.03.17 --%>
            <div id="serial_nogen_div" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -9px; margin-left: 196px;"
                        OnClick="imagebtnpopclose4_Click" />
                    <div id="Div7" runat="server" class="table" style="background-color: White; height: 175px;
                        width: 415px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <center>
                                Admission Number Generation settings</center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox textbox1" Width="80px"
                                            OnTextChanged="txt_frmdate_onchange" AutoPostBack="true"></asp:TextBox>
                                        <asp:CalendarExtender ID="calfrmdate" TargetControlID="txt_frmdate" runat="server"
                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label49" runat="server" Text="Acronym"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_acronym" runat="server" CssClass="textbox txtheight" onfocus="return myFunction2(this)"
                                            placeholder="HOSTEL" MaxLength="6" Style="text-transform: uppercase;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterextenderacr" runat="server" TargetControlID="txt_acronym"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label50" runat="server" Text="Start No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_startno" runat="server" CssClass="textbox txtheight" onfocus="return myFunction2(this)"
                                            placeholder="1" MaxLength="6"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterextender" runat="server" TargetControlID="txt_startno"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label51" runat="server" Text="Size"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_size" runat="server" CssClass="textbox txtheight" Width="30px"
                                            placeholder="1 - 6" MaxLength="1" onfocus="return myFunction2(this)" onchange="myFunction(this)"></asp:TextBox>
                                        <asp:RangeValidator ID="Range1" ControlToValidate="txt_size" MinimumValue="1" MaximumValue="6"
                                            Type="Integer" runat="server" ToolTip="Enter Only 1 - 6" SetFocusOnError="False" />
                                        <asp:FilteredTextBoxExtender ID="filterextender1" runat="server" TargetControlID="txt_size"
                                            FilterType="Numbers" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <center>
                                            <asp:Button ID="btn_admissionserialno_save" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_save_serial_nogen_Click" Text="Save" runat="server" />
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
                                        <asp:Label ID="lbl_alerterror" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
