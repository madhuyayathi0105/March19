<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HT_StaffRegistration.aspx.cs" Inherits="HT_StaffRegistration" %>

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
        <%-- <script src="Styles/~/Scripts/jquery-latest.min.js" type="text/javascript"></script>--%>
        <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
        <style type="text/css">
            .table2
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #7bc1f7;
            }
            
            .watermark
            {
                color: #999999;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function valid1() {
                var idval = "";
                var empty = "";

                idval = document.getElementById("<%=txt_pop1staffname.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_pop1staffname.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }

                idval = document.getElementById("<%=txt_pop1staffcode.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_pop1staffcode.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }


                idval = document.getElementById("<%=txt_pop1roomno.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_pop1roomno.ClientID %>");
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
            function valid() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=txt_nameguest.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_nameguest.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_room.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_room.ClientID %>");
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
        <br />
        <center>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: #008000;">Hostel Staff / Guest Registration</span>
                </div>
                <br />
            </center>
            <div class="maindivstyle" style="height: 929px; width: 1000px;">
                <center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_hostelname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel" Width="180px"
                                            Height="200px">
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
                                <asp:Label ID="lbl_building" runat="server" Text="Building"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_building" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_building" runat="server" CssClass="multxtpanel" Width="180px"
                                            Height="200px">
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
                                <asp:Label ID="lbl_floorname" runat="server" Text="Floor"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_floorname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            Height="20px" ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_floorname" runat="server" CssClass="multxtpanel" Width="180px"
                                            Height="200px">
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
                                <asp:Label ID="lbl_roomname" runat="server" Text="Room"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_roomname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true" Width="80px">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_roomname" runat="server" CssClass="multxtpanel" Width="180px"
                                            Height="200px">
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
                                <asp:Label ID="lbl_department" Text="Department" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_department" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_department" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_department" runat="server" Height="200px" Width="180px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_department" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_department_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_department" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_department_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_department" runat="server" TargetControlID="txt_department"
                                            PopupControlID="panel_department" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_designation" Text="Designation" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_designation" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_designation" runat="server" CssClass="textbox textbox1 txtheight1"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_designation" Height="200px" Width="180px" runat="server" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_designation" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_designation_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_designation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_designation_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_designation" runat="server" TargetControlID="txt_designation"
                                            PopupControlID="panel_designation" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stafftype" Text="Staff Type" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp_stafftype" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stafftype" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">-- Select--</asp:TextBox>
                                        <asp:Panel ID="panel_stafftype" runat="server" Height="100px" Width="180px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_stafftype" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_stafftype_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_stafftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftype_checkedchange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupext_stafftype" runat="server" TargetControlID="txt_stafftype"
                                            PopupControlID="panel_stafftype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_searchbystaff" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_staffname" runat="server" Text="Staff Name"></asp:Label>
                                <%--</td>
                        <td>--%>
                                <asp:TextBox ID="txt_staffname" runat="server" placeholder="Staff Name" CssClass=" textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="acext_staffname" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffNamego" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_searchbystaffcode" runat="server" Style="float: right;" Text="Search By"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbl_staffcode" runat="server" Text="Staff Code"></asp:Label>
                                <%--</td>
                        <td>--%>
                                <asp:TextBox ID="txt_staffcode" runat="server" placeholder="Staff Code" CssClass=" textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="acext_staffcode" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffcode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <%--  <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>--%>
                            <td colspan="2">
                                <asp:RadioButton ID="rdb_staffe" runat="server" Text="Staff"  GroupName="day" OnCheckedChanged="rdb_staffe_Select"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rdb_gueste" runat="server" Text="Guest"  GroupName="day" OnCheckedChanged="rdb_gueste_select"
                                    AutoPostBack="true" />
                            <td colspan="2">
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btnaddnew_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <div>
                        <asp:Label ID="lbl_error" runat="server" Text="" Visible="true" ForeColor="red"></asp:Label>
                    </div>
                </center>
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="940px" Style="margin-top: -0.1%;">
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
                    <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="940px">
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
                                    <asp:TextBox ID="tborder" Visible="false" Width="930px" TextMode="MultiLine" CssClass="style1"
                                        AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                        Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                        <asp:ListItem Value="staff_code">Staff Code</asp:ListItem>
                                        <asp:ListItem Value="staff_name">Name</asp:ListItem>
                                        <asp:ListItem Value="desig_name">Designation</asp:ListItem>
                                        <asp:ListItem Value="dept_name">Department</asp:ListItem>
                                        <asp:ListItem Value="staffcategory">Staff Type</asp:ListItem>
                                        <asp:ListItem Value="Admin_Date">Admit Date</asp:ListItem>
                                        <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                        <asp:ListItem Value="BuildingFK">Building</asp:ListItem>
                                        <asp:ListItem Value="FloorFK">Floor</asp:ListItem>
                                        <asp:ListItem Value="RoomFK">Room</asp:ListItem>
                                        <%--  <asp:ListItem Value="Room_Type">Room Type</asp:ListItem>--%>
                                        <asp:ListItem Value="DiscontinueDate">Discontinue</asp:ListItem>
                                        <asp:ListItem Value="VacatedDate">Vacated</asp:ListItem>
                                        <asp:ListItem Value="Reason">Reason</asp:ListItem>
                                        <asp:ListItem Value="StudMessType">StudMessType</asp:ListItem>
                                        <asp:ListItem Value="id">Staff Id</asp:ListItem>

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
                <center>
                    <div id="Divspread" runat="server" visible="false" style="width: 900px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White;" class="spreadborder">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" OnCellClick="Cell_Click1" OnPreRender="Fpspread_render"
                            Visible="false" BorderWidth="1px" Style="overflow: auto; position: relative;
                            border: 0px solid #999999; border-radius: 10px; background-color: White; width: 880px;
                            height: 350px;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
                <div>
                    <br />
                    <center>
                        <asp:Panel ID="pheaderfilterguest" runat="server" CssClass="maintablestyle" Height="22px"
                            Width="940px" Style="margin-top: -7.1%;">
                            <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                            <asp:Label ID="Labelfilterguest" Text="Column Order" runat="server" Font-Size="Medium"
                                Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                            <asp:Image ID="Imagefilterguest" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                ImageAlign="Right" />
                        </asp:Panel>
                    </center>
                    <br />
                </div>
                <center>
                    <asp:Panel ID="pcolumnorderguest" runat="server" CssClass="maintablestyle" Width="940px">
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox_columnguest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_columnguest_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnk_columnorderguest" runat="server" Font-Size="X-Small" Height="16px"
                                        Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                        Visible="false" Width="111px" OnClick="LinkButtonsremoveguest_Click">Remove  All</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                    <asp:TextBox ID="tborderguest" Visible="false" Width="930px" TextMode="MultiLine"
                                        CssClass="style1" AutoPostBack="true" runat="server" Enabled="false">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cblcolumnorderguest" runat="server" Height="43px" AutoPostBack="true"
                                        Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                        RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorderguest_SelectedIndexChanged">
                                        <asp:ListItem Value="Admission_Date">Admission Date</asp:ListItem>
                                        <asp:ListItem Value="VenContactName">Guest Name</asp:ListItem>
                                        <asp:ListItem Value="VendorCompName">Company Name</asp:ListItem>
                                        <asp:ListItem Value="VenContactDesig">Designation</asp:ListItem>
                                        <asp:ListItem Value="VenContactDept">Department</asp:ListItem>
                                        <asp:ListItem Value="VendorAddress">Address</asp:ListItem>
                                        <asp:ListItem Value="VendorCity">City</asp:ListItem>
                                        <asp:ListItem Value="VendorDist">District</asp:ListItem>
                                        <asp:ListItem Value="VendorState">State</asp:ListItem>
                                        <asp:ListItem Value="VendorMobileNo">Mobile Number</asp:ListItem>
                                        <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                        <asp:ListItem Value="BuildingFK">Building</asp:ListItem>
                                        <asp:ListItem Value="FloorFK">Floor</asp:ListItem>
                                        <asp:ListItem Value="RoomFK">Room</asp:ListItem>
                                        <%--   <asp:ListItem Value="room_type">Room Type</asp:ListItem>--%>
                                        <asp:ListItem Value="IsVacated">Is Vacate</asp:ListItem>
                                        <asp:ListItem Value="vacate_date">Vacated Date</asp:ListItem>
                                        <asp:ListItem Value="StudMessType">Mess Type</asp:ListItem>
                                         <asp:ListItem Value="id">Guest Id</asp:ListItem>
                                        <%--  <asp:ListItem Value="vacate_date">Vacated</asp:ListItem>--%>
                                        <%--   <asp:ListItem Value="Relived_Date">Relieved</asp:ListItem>
                             
                                <asp:ListItem Value="Reason">Reason</asp:ListItem>--%>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorderguest"
                    CollapseControlID="pheaderfilterguest" ExpandControlID="pheaderfilterguest" Collapsed="true"
                    TextLabelID="Labelfilterguest" CollapsedSize="0" ImageControlID="Imagefilterguest"
                    CollapsedImage="right.jpeg" ExpandedImage="down.jpeg">
                </asp:CollapsiblePanelExtender>
                <br />
                <center>
                    <div id="Div4" runat="server" visible="false" style="width: 900px; height: 350px;"
                        class="spreadborder ">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" OnCellClick="Cell_Clickguest" OnPreRender="Fpspread2_render"
                            Visible="false" BorderWidth="1px" Style="overflow: auto; position: relative;
                            border: 0px solid #999999; border-radius: 10px; background-color: White; width: 850px;
                            height: 350px;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <br />
                <center>
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
            </div>
        </center>
        <center>
            <div id="popwindow1" runat="server" class="popupstyle" visible="false" style="height: 61em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 443px;"
                    OnClick="imagebtnpop1close_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; height: 680px; width: 915px;
                    border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <asp:Label ID="lbl_itemcostmaster" runat="server" CssClass="fontstyleheader" Style="color: Green;"
                            Text="Hostel Staff / Guest Registration"></asp:Label>
                    </center>
                    <br />
                    <center>
                        <div align="left" style="overflow: auto; width: 860px; height: 595px; border-radius: 10px;
                            border: 1px solid Gray;">
                            <br />
                            <center>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdb_staff" Text="Staff" runat="server" GroupName="same" AutoPostBack="true"
                                                OnCheckedChanged="rdb_staff_CheckedChanged" />
                                            <asp:RadioButton ID="rdb_guest" Text="Guest" runat="server" GroupName="same" AutoPostBack="true"
                                                OnCheckedChanged="rdb_guest_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <br />
                            <center>
                                <table style="width: 848px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1collegename" Text="College Name" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddl_pop1collegename" runat="server" CssClass="textbox ddlheight6 textbox1"
                                                Width="320px" AutoPostBack="true" onfocus="return myFunction(this)" Visible="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1hostelname" Text="Hostel Name" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_pop1hostelname" runat="server" CssClass="textbox ddlheight4 textbox1"
                                                Width="190px" OnSelectedIndexChanged="ddl_pop1hostelname_SelectedIndexChanged"
                                                AutoPostBack="true" onfocus="return myFunction(this)" Visible="false">
                                            </asp:DropDownList>
                                            <span id="staff" runat="server" visible="false" style="color: Red;">*</span>
                                        </td>
                                         <td>
                                        <asp:Label ID="Lblmess" Text="Mess Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlmess" runat="server" CssClass="textbox ddlheight4 textbox1"
                                            Width="152px" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1staffname" Text="Staff Name" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1staffname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                BackColor="#DCF9D1" onfocus="return myFunction(this)" AutoPostBack="true" OnTextChanged="txt_pop1staffname_Text_Changed"
                                                Visible="false"></asp:TextBox>
                                            <span id="staffnamebtn" runat="server" visible="false" style="color: Red;">*</span>
                                            <asp:Button ID="btn_staffquestion" Text="?" runat="server" OnClick="btn_staff_question_Click"
                                                CssClass="textbox btn" Visible="false" />
                                        
                                       
                                            <%--  <asp:TextBox ID="txtstaffname" TextMode="SingleLine" runat="server" Height="20px"
                                    CssClass="textbox textbox1" Width="180px"></asp:TextBox>--%>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1staffname"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <%--<asp:TextBoxWatermarkExtender ID="wateritemname" runat="server" TargetControlID="txtstaffname"
                                    WatermarkText="Search Staff Name" WatermarkCssClass="watermark textbox textbox1">
                                </asp:TextBoxWatermarkExtender>--%>
                                        </td>

                                           <td>
                                        <asp:Label ID="lblid" Text="Staff Id" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px"  Enabled="false"
                                             MaxLength="30"></asp:TextBox>
                                      
                                        
                                    </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1staffcode" Text="Staff Code" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1staffcode" runat="server" CssClass="textbox txtheight3 textbox1"
                                                BackColor="#DCF9D1" ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                            <span id="staffcode" runat="server" visible="false" style="color: Red;">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1department" Text="Department" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1department" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1designation" Text="Designation" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1designation" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1dob" Text="DOB" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1dob" runat="server" Width="80px" BackColor="#DCF9D1" ReadOnly="true"
                                                CssClass="textbox txtheight3 textbox1" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1admindate" Text="Admit Date" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1admindate" runat="server" Width="131px" BackColor="#DCF9D1"
                                                CssClass="textbox txtheight3" Visible="false"></asp:TextBox>
                                            <asp:CalendarExtender ID="caladmin" TargetControlID="txt_pop1admindate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1roomno" Text="Room No" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1roomno" runat="server" BackColor="#DCF9D1" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                            <span id="roomno" runat="server" visible="false" style="color: Red;">*</span>
                                            <asp:Button ID="btn_roomques" Text="?" runat="server" OnClick="btn_roomques_Click"
                                                CssClass="textbox btn" Visible="false" />
                                        </td>
                                          <%--magesh 12.3.18
                                        <%--<td>
                                            <asp:Label ID="lbl_pop1messtype" Text="Mess Type" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:RadioButton ID="rdbveg" runat="server" Text="Veg" GroupName="same2" />
                                <asp:RadioButton ID="rdbnonveg" runat="server" Text="Non Veg" GroupName="same2" del />--%>
                                            <%--<asp:RadioButtonList ID="rbl_messtype" runat="server" AutoPostBack="false" Font-Names="Book Antiqua"
                                                Style="margin-left: 0px;" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbl_messtype_RadiobtnChanged"
                                                Visible="false">
                                                <asp:ListItem Value="0" Selected="True">Veg</asp:ListItem>
                                                <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>--%>
                                      
                                        <td>
                                        <asp:Label ID="lbl_pop1messtype" Text="Student Type" runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2px">
                                        <%--<asp:RadioButton ID="rdbveg" runat="server" Text="Veg" GroupName="same2" />
                                <asp:RadioButton ID="rdbnonveg" runat="server" Text="Non Veg" GroupName="same2" />
                                        <asp:RadioButtonList ID="Radiobtnstype" runat="server" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Value="0">Veg</asp:ListItem>
                                            <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <asp:DropDownList ID="ddlStudType" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>  <%--magesh 12.3.18--%>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1building" Text="Building Name" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1building" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_pop1building"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_pop1floor" Text="Floor" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1floor" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_pop1roomtype" Text="Room Type" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1roomtype" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight2 textbox1"
                                                ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1discontinue" Text="Discontinue" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_discontinue" runat="server" OnCheckedChanged="cb_discontinue_CheckedChanged"
                                                AutoPostBack="true" Visible="false" />
                                            <asp:Label ID="lbl_pop1date" Text="Date" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txt_discontinuedate" runat="server" CssClass="textbox textbox1"
                                                Width="80px" Visible="false"></asp:TextBox>
                                            <asp:CalendarExtender ID="caldisdate" TargetControlID="txt_discontinuedate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_pop1reason" Text="Reason" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_pop1reason" runat="server" CssClass="textbox txtheight3 textbox1"
                                                onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_pop1vacate" Text="Vacated" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cb_pop1vacate" runat="server" OnCheckedChanged="cb_pop1vacate_CheckedChange"
                                                AutoPostBack="true" Visible="false" />
                                            <asp:Label ID="lbl_pop1date1" Text="Date" runat="server" Visible="false"></asp:Label>
                                            <asp:TextBox ID="txt_vacatedate" runat="server" CssClass="textbox txtheight2" Width="80px"
                                                Visible="false"></asp:TextBox>
                                            <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_vacatedate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                                <center>
                                    <div>
                                        <asp:Button ID="btn_pop1save" Text="Save" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClientClick="return valid1()" OnClick="btn_pop1save_Click" />
                                        <asp:Button ID="btn_pop1exit" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1exit_Click" />
                                        <asp:Button ID="btn_pop1update" Text="Update" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1update_Click" />
                                        <asp:Button ID="btn_pop1delete" Text="Delete" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1delete_Click" />
                                        <asp:Button ID="btn_pop1exit1" Text="Exit" runat="server" Visible="false" CssClass="textbox btn2"
                                            OnClick="btn_pop1exit1_Click" />
                                    </div>
                                </center>
                                <div style="margin-top: -50px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_messname" runat="server" Text="Hostel Name" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_messname" runat="server" CssClass="textbox1  ddlheight4"
                                                    OnSelectedIndexChanged="ddl_messname_SelectedIndexChanged" AutoPostBack="True"
                                                    Visible="false">
                                                </asp:DropDownList>
                                            </td>
                                             <td>
                                        <asp:Label ID="lbmess" Text="Mess Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlmess1" runat="server" CssClass="textbox ddlheight4 textbox1"
                                            Width="152px" onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                        <span style="color: Red;">*</span>
                                    </td>
                                         
                                            <td>
                                                <asp:Label ID="lbl_code" runat="server" Text="Code" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_code" runat="server" CssClass="textbox textbox1" Visible="false"
                                                    Enabled="false" Width="110px"></asp:TextBox>
                                                <%--  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_code"
                                                FilterType="numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_name4" runat="server" Text="Name" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_nameguest" runat="server" onfocus="return myFunction(this)"
                                                    Height="20px" CssClass="textbox textbox1" Width="183px" Visible="false"></asp:TextBox>
                                                <span id="guest" runat="server" visible="false" style="color: Red;">*</span>
                                                <asp:FilteredTextBoxExtender ID="ftext_name4" runat="server" TargetControlID="txt_nameguest"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                             <td>
                                        <asp:Label ID="Llid" Text="Guest Id" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtid1" runat="server" CssClass="textbox textbox1" BackColor="#DCF9D1"
                                            onfocus="return myFunction(this)" Width="190px" Height="20px"  Enabled="false"
                                             MaxLength="30"></asp:TextBox>
                                      
                                        
                                    </td>
                                               <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="Admin Date" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_admindate" runat="server" CssClass="textbox  txtheight3" ForeColor="Black"
                                                    Visible="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_admindate" runat="server"
                                                    Format="dd/MM/yyyy">
                                                    <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                </asp:CalendarExtender>
                                            </td>
                                            <%--  <td>
                                            <asp:Label ID="lbl_phno" runat="server" Text="Phone No" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_phno" runat="server" CssClass="textbox textbox1" Visible="false"
                                                MaxLength="15"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_phno" runat="server" TargetControlID="txt_phno"
                                                FilterType="numbers" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>--%>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_compname" runat="server" Text="Company Name" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_compname" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_compname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-&@">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_str" runat="server" Visible="false" Text="Address"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_str" runat="server" CssClass="textbox textbox1" Visible="false"
                                                    Width="184px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_str" runat="server" TargetControlID="txt_str"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" .-&/">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_desgn" runat="server" Text="Designation" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_desgn" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_desgn" runat="server" TargetControlID="txt_desgn"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getdesi" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_desgn"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_cty" runat="server" Visible="false" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_cty" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_cty" runat="server" TargetControlID="txt_cty"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_dep" runat="server" Text="Department" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_dep" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_dep" runat="server" TargetControlID="txt_dep"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-&">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getdept" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_dep"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_dis" runat="server" Visible="false" Text="District"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_dis" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_dis" runat="server" TargetControlID="txt_dis"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getdist" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_dis"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_mno" runat="server" Text="Mobile No" MaxLength="10" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_mno" runat="server" CssClass="textbox textbox1" Visible="false"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_mno" runat="server" TargetControlID="txt_mno"
                                                    FilterType="numbers" ValidChars="">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_stat" runat="server" Visible="false" Text="State"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_stat" runat="server" CssClass="textbox textbox1" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_stat" runat="server" TargetControlID="txt_stat"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getstate" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stat"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_room" Text="Room No" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_room" runat="server" onfocus="return myFunction(this)" BackColor="#DCF9D1"
                                                    CssClass="textbox txtheight3 textbox1" ReadOnly="true" Visible="false"></asp:TextBox>
                                                <span id="roomnum" runat="server" visible="false" style="color: Red;">*</span>
                                                <asp:Button ID="btn2" Text="?" runat="server" OnClick="btn2_Click" CssClass="textbox btn"
                                                    Visible="false" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGuestType" Text="Mess Type" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <%--<td>
                                                <asp:RadioButton ID="rdb_veg" runat="server" GroupName="rr" Text="Veg" Visible="false" AutoPostBack="true"
                                                    Checked="true" />
                                                <asp:RadioButton ID="rdb_NonVeg" runat="server" GroupName="rr" Text="Non Veg" Visible="false" AutoPostBack="true" />
                                            </td>--%>
                                             <td colspan="2px">
                                        <%--<asp:RadioButton ID="rdbveg" runat="server" Text="Veg" GroupName="same2" />
                                <asp:RadioButton ID="rdbnonveg" runat="server" Text="Non Veg" GroupName="same2" />
                                        <asp:RadioButtonList ID="Radiobtnstype" runat="server" Font-Names="Book Antiqua"
                                            Style="margin-left: 0px;" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Value="0">Veg</asp:ListItem>
                                            <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                        <asp:DropDownList ID="ddlguest" runat="server" CssClass="textbox  ddlheight3"
                                            onfocus="return myFunction(this)">
                                        </asp:DropDownList>
                                    </td>  <%--magesh 12.3.18--%>
                                            <%-- <asp:RadioButtonList ID="rdbGuestMessType" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                                    Style="margin-left: 0px;" RepeatDirection="Horizontal" Visible="false">
                                                    <asp:ListItem Value="0" Selected="True">Veg</asp:ListItem>
                                                    <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                                </asp:RadioButtonList>--%>
                                           
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_buildingguest" Text="Building Name" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_building" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                    ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_building"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_floorguest" Text="Floor" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_floor" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight3 textbox1"
                                                    ReadOnly="true" onfocus="return myFunction(this)" Visible="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_roomtype" Text="Room Type" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_roomtype" BackColor="#DCF9D1" runat="server" CssClass="textbox txtheight2 textbox1"
                                                    Visible="false" ReadOnly="true"></asp:TextBox><%--onfocus="return myFunction(this)"--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_vacate" Text="Vacated" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cb_vacate" runat="server" OnCheckedChanged="cb_vacate_CheckedChange"
                                                    AutoPostBack="true" Visible="false" />
                                                <asp:Label ID="lbl_vacatedate" Text="Date" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txt_vacatedateguest" runat="server" CssClass="textbox txtheight2"
                                                    Width="80px" Visible="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_vacatedateguest"
                                                    runat="server" Format="dd/MM/yyyy">
                                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                            <center>
                                <div>
                                    <asp:Button ID="btn_saveguest" runat="server" CssClass="textbox btn2" Text="Save"
                                        OnClick="btn_saveguest_Click" OnClientClick="return valid()" Visible="false" />
                                    <asp:Button ID="btn_updateguest" runat="server" CssClass="textbox btn2" Text="Update"
                                        OnClick="btn_updateguest_Click" Visible="false" />
                                    <asp:Button ID="btn_delguest" runat="server" CssClass="textbox btn2" Text="Delete"
                                        OnClick="btn_delguest_Click" Visible="false" />
                                    <asp:Button ID="btn_exitguest" runat="server" CssClass="textbox btn2" Text="Exit"
                                        OnClick="btn_exitguest_Click" Visible="false" />
                                </div>
                            </center>
                            <center>
                                <asp:Label ID="errmsg" Style="color: Red;" runat="server"></asp:Label></center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popupstaffcode1" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
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
                            <table class="maintablestyle" style="width: 69%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college2" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college2" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_college2_selectedindexchange" CssClass="textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_department3" runat="server" Text="Department"></asp:Label>
                                        <asp:DropDownList ID="ddl_department3" Width="180px" Height="30px" runat="server"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight6">
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
                                            CssClass="textbox1 ddlheight6">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_staffnamesearch" Visible="false" TextMode="SingleLine" runat="server"
                                            Height="20px" CssClass="textbox textbox1" Width="180px" placeholder="Staff Name"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffnamesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_staffcodesearch" placeholder="Staff Code" Visible="false" TextMode="SingleLine"
                                            runat="server" Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="acext_staffcodesearch" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCode1" MinimumPrefixLength="0" CompletionInterval="100"
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
                                    <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                        border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                        OnCellClick="Cell_Click" OnPreRender="Fpspread1_render" ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="LightBlue">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </div>
                            <br />
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
        <center>
            <div id="roomlookup" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 394px;"
                    OnClick="imagebtnpop3close_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 820px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select the Room</span></div>
                        <br />
                    </center>
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pop3buildingname" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp_pop3build" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3build" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_pop3build" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_pop3build" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3build_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3build" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3build_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Popupextl_pop3build" runat="server" TargetControlID="txt_pop3build"
                                                PopupControlID="panel_pop3build" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%--<td><asp:DropDownList ID="ddlbuildingname" runat="server" CssClass="textbox ddlstyle"  OnSelectedIndexChanged="ddlbuildingname_SelectedIndexChanged"></asp:DropDownList></td>--%>
                                <td>
                                    <asp:Label ID="lbl_pop3floor" Text="Floor" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp_pop3floor" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3floor" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_pop3floor" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_pop3floor" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3floor_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3floor_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupext_pop3floor" runat="server" TargetControlID="txt_pop3floor"
                                                PopupControlID="panel_pop3floor" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_pop3roomtype" Text="Room Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp_pop3roomtype" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_pop3roomtype" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_pop3roomtype" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_pop3roomtype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_pop3roomtype_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_pop3roomtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3roomtype_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Popupext_pop3roomtype" runat="server" TargetControlID="txt_pop3roomtype"
                                                PopupControlID="panel_pop3roomtype" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_pop3vaccant" Text="Vacant Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_pop3vaccant" runat="server" Width="125px" CssClass="textbox1 ddlheight2 "
                                        OnSelectedIndexChanged="ddl_pop3vaccant_SelectedIndexChanged">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Filled</asp:ListItem>
                                        <asp:ListItem>Un Filled</asp:ListItem>
                                        <asp:ListItem>Partialy Filled</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblinclude" Text="Include:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_include" runat="server" Text="All" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="cb_include_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="cbl_roomlist" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_roomlist_SelectedIndexChanged"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                        <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                        <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_roomlookupgo" Text="Go" runat="server" CssClass="textbox btn1"
                                        OnClick="btn_roomlookupgo_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lblpop3err" runat="server" Style="color: Red;"></asp:Label></center>
                    <br />
                    <div id="div3" runat="server" style="width: 780px; height: 180px; overflow: auto">
                        <center>
                            <FarPoint:FpSpread ID="Froomspread" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread></center>
                    </div>
                    <br />
                    <center>
                        <table class="maintablestyle" runat="server" id="tblStatus" style="border-bottom-style: solid;
                            border-top-style: solid; border-left-style: solid; border-width: 0px;">
                            <tr>
                                <td>
                                    <asp:Label ID="toalrooms" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="totalvaccants" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="Btn4" runat="server" Width="20px" BackColor="GreenYellow" />
                                    <asp:Label ID="fill" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="109px"></asp:Label>
                                    <asp:Button ID="Btn5" runat="server" Width="20px" BackColor="Coral" />
                                    <asp:Label ID="partialfill" runat="server" Text="Partialy Filled" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                    <asp:Button ID="Btn6" runat="server" Width="20px" BackColor="MistyRose" />
                                    <asp:Label ID="unfill" runat="server" Text="UnFilled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="145px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Button ID="btn_roomlookupsave" Text="Save" runat="server" CssClass="textbox btn2"
                            OnClick="btn_roomlookupsave_Click" />
                        <asp:Button ID="btn_roomlookupexit" Text="Exit" runat="server" CssClass="textbox btn2"
                            OnClick="btn_roomlookupexit_Click" />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow3" runat="server" class="popupstyle" visible="false" style="height: 50em;
                z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                top: 0; left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 394px;"
                    OnClick="imagebtnpop3closeguest_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 550px; width: 820px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Select the Room</span></div>
                        <br />
                    </center>
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel33" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_build" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p4444" runat="server" CssClass="multxtpanel" Height="200px" Width="180px">
                                                <asp:CheckBox ID="cb_build" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_build_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_build" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_build_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender123" runat="server" TargetControlID="txt_build"
                                                PopupControlID="p4444" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%--<td><asp:DropDownList ID="ddlbuildingname" runat="server" CssClass="textbox ddlstyle"  OnSelectedIndexChanged="ddlbuildingname_SelectedIndexChanged"></asp:DropDownList></td>--%>
                                <td>
                                    <asp:Label ID="Label2" Text="Floor" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floorguest" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="P1111" runat="server" CssClass="multxtpanel" Height="200px" Width="150px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_floor" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_floor_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floor_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txt_floorguest"
                                                PopupControlID="p1111" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" Text="Room Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelro" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_roomtypeguest" runat="server" CssClass="textbox textbox1 txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2222" runat="server" CssClass="multxtpanel" Height="200px" Width="180px">
                                                <asp:CheckBox ID="cb_roomtype" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_roomtype_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_roomtype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_pop3roomtype_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txt_roomtypeguest"
                                                PopupControlID="p2222" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" Text="Vacant Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_vacant" runat="server" Width="125px" CssClass="ddlheight2 textbox1">
                                        <asp:ListItem>All</asp:ListItem>
                                        <asp:ListItem>Filled</asp:ListItem>
                                        <asp:ListItem>Un Filled</asp:ListItem>
                                        <asp:ListItem>Partially Filled</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" Text="Include:" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chck1" runat="server" Text="All" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnCheckedChanged="chck1_CheckedChanged" AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="roomchecklist" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" Font-Names="Book Antiqua" OnSelectedIndexChanged="roomchecklist_SelectedIndexChanged"
                                        Font-Size="Medium">
                                        <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                        <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                        <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_gopop3" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_gopop3_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lblerr" runat="server" Style="color: Red;"></asp:Label></center>
                    <br />
                    <div id="div2" runat="server" style="width: 810px; height: 180px; overflow: auto">
                        <center>
                            <FarPoint:FpSpread ID="FpSpread3" runat="server" Visible="false" BorderStyle="NotSet"
                                BorderWidth="0px" ActiveSheetViewIndex="0" Style="width: 810px; height: 180px;
                                overflow: auto">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA" AutoPostBack="true">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread></center>
                    </div>
                    <br />
                    <center>
                        <table class="maintablestyle" runat="server" id="tblStatusguest" style="border-bottom-style: solid;
                            border-top-style: solid; border-left-style: solid; border-width: 0px;" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="toalroomsguest" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="totalvaccantsguest" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%-- <asp:Button ID="Button4" runat="server" Width="20px" BackColor="GreenYellow" />--%>
                                    <asp:Label ID="gy" runat="server" Width="20px" Height="20px" BackColor="GreenYellow"></asp:Label>
                                    <asp:Label ID="fillguest" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="109px"></asp:Label>
                                    <%-- <asp:Button ID="Button5" runat="server" Width="20px" BackColor="Coral" />--%>
                                    <asp:Label ID="cor" runat="server" Width="20px" Height="20px" BackColor="Coral"></asp:Label>
                                    <asp:Label ID="partialfillguest" runat="server" Text="Partially Filled" Font-Bold="True"
                                        Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                    <%-- <asp:Button ID="Button6" runat="server" Width="20px" BackColor="MistyRose" />--%>
                                    <asp:Label ID="mis" runat="server" Width="20px" Height="20px" BackColor="MistyRose"></asp:Label>
                                    <asp:Label ID="unfillguest" runat="server" Text="Unfilled" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="145px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Button ID="btn_pop3save" Text="Save" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_pop3save_Click" />
                            <asp:Button ID="btn_pop3exit" Text="Exit" Visible="false" runat="server" CssClass="textbox btn2"
                                OnClick="btn_pop3exit_Click" />
                        </div>
                    </center>
                </div>
            </div>
        </center>
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
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
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
            <div id="suredivstaff" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div7" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_surestaff" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yesstaff" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyesstaff_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_surenostafff" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_surenostaff_Click" Text="no" runat="server" />
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
