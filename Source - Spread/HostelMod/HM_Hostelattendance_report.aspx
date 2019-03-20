<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_Hostelattendance_report.aspx.cs" Inherits="HM_Hostelattendance_report" %>

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


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <script type="text/javascript">
            var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            function BeginRequestHandler(sender, args) {
                if ($get('<%=panel_room.ClientID%>') != null) {
                    xPos = $get('<%=panel_room.ClientID%>').scrollLeft;
                    yPos = $get('<%=panel_room.ClientID%>').scrollTop;
                }
            }

            function EndRequestHandler(sender, args) {
                if ($get('<%=panel_room.ClientID%>') != null) {
                    $get('<%=panel_room.ClientID%>').scrollLeft = xPos;
                    $get('<%=panel_room.ClientID%>').scrollTop = yPos;
                }
            }

            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span style="color: #008000;" class="fontstyleheader">Hostel Absentees Attendance Report</span>
                        <br />
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="height: auto;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="Institution Name" runat="server" CssClass="txtheight"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCollege" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                            placeholder="Batch" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbCollege" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbCollegeCheckedChanged" />
                                            <asp:CheckBoxList ID="cblCollege" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblCollegeSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtCollege"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upBatch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" Width="60px"
                                            ReadOnly="true" placeholder="Batch" onfocus="return myFunction1(this)"></asp:TextBox>
                                        <asp:Panel ID="pnlBatch" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_Batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_Batch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_Batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Batch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceBatch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="pnlBatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_degree_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="p3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                        <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" CssClass="multxtpanel" Width="250px" Height="200px" Style="position: absolute;">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_branch_checkedchange" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="p4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_hostelname" runat="server" Text="Hostel Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_hostelname" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                        <asp:Panel ID="panel_hostelname" runat="server" Width="200px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_hostelname"
                                            PopupControlID="panel_hostelname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="Label2" Text="Buildind Name" runat="server" Width="52px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="drbbuilding" runat="server" CssClass="textbox textbox1 ddlheight1"
                                            AutoPostBack="true" OnSelectedIndexChanged="drbbuilding_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_floor" runat="server" Text="Floor Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_floor" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_floor" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_floor" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_floor" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbfloor_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_floor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblfloor_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_floor"
                                            PopupControlID="panel_floor" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_room" runat="server" Text="Room"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_room" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_room" runat="server" CssClass="textbox textbox1 txtheight3"
                                            ReadOnly="true" Width="102px">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_room" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight"
                                            Style="position: absolute;">
                                            <asp:CheckBox ID="cb_room" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_room_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_room"
                                            PopupControlID="panel_room" Position="Bottom">
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
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                    OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                    OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_criteria" runat="server" Text="Criteria"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_building" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_criteria" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_building" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_criteria" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_criteria_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_criteria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_criteria_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_criteria"
                                            PopupControlID="panel_building" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--  <td>
                            <asp:RadioButton ID="rdb_count" Text="Count" Visible="false" runat="server" GroupName="same" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdb_det" Text="Detail" Visible="false" runat="server" GroupName="same" />
                        </td>--%>
                            <td colspan="3">
                                <asp:RadioButton ID="rdb_Hostel" Text="Hostel Absentees" OnCheckedChanged="rdb_hostel_SelectedIndexchange"
                                    AutoPostBack="true" runat="server" GroupName="same" />
                                <asp:RadioButton ID="rdo_guest" AutoPostBack="true" Text="Guest Absentees" OnCheckedChanged="rdb_guest__SelectedIndexchange"
                                    runat="server" GroupName="same" />
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdbCumulative" Text="Cumulative" OnCheckedChanged="rdbDetails_OnCheckedChanged"
                                    runat="server" AutoPostBack="true" GroupName="s" Checked="true" />
                                <asp:RadioButton ID="rdbDetails" Text="Details" OnCheckedChanged="rdbDetails_OnCheckedChanged"
                                    runat="server" AutoPostBack="true" GroupName="s" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="cbmor" runat="server" Text="Morning" GroupName="ty" Checked="true">
                                </asp:RadioButton>
                                <asp:RadioButton ID="cbeve" runat="server" Text="Evening" GroupName="ty"></asp:RadioButton>
                                <asp:RadioButton ID="cbboth" runat="server" Text="Both" GroupName="ty"></asp:RadioButton>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblorder" runat="server" Text="OrderBy" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddlorder" runat="server" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                    <asp:ListItem Text="Roll No"></asp:ListItem>
                                    <asp:ListItem Text="Room"></asp:ListItem>
                                </asp:DropDownList>
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
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <%-- GUEST COLUMN ORDER--%>
                        <br />
                        <div>
                            <center>
                                <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="889px">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="Label1" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                        ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                        </div>
                        <br />
                        <%--end guest--%>
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
                                            <asp:ListItem Value="Roll_No" Selected="true">Roll No</asp:ListItem>
                                            <asp:ListItem Value="Reg_No" Selected="true">Reg No</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name" Selected="true">Student Name</asp:ListItem>
                                            <asp:ListItem Value="Stud_Type" Selected="true">Student Type</asp:ListItem>
                                            <asp:ListItem Value="Course_Name">Degree</asp:ListItem>
                                            <asp:ListItem Value="Dept_Name">Department</asp:ListItem>
                                            <asp:ListItem Value="Current_Semester">Semester</asp:ListItem>
                                            <asp:ListItem Value="Sections">Section</asp:ListItem>
                                            <asp:ListItem Value="Hostel_Name">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room No</asp:ListItem>
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
                    <%-- GUEST COLUMN ORDER--%>
                    <%--<div>
                        <center>
                            <asp:Panel ID="pheaderfilter1" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="889px">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Labelfilter1" Text="Column Order1" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter1" runat="server" CssClass="cpimage" ImageUrl="~/images/right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                    </div>--%>
                    <center>
                        <asp:Panel ID="pcolumnorder1" runat="server" CssClass="maintablestyle" Width="890px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                            Visible="false" Width="111px" OnClick="lb_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tborder1" Visible="false" Width="867px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="6" RepeatDirection="Horizontal" OnSelectedIndexChanged="cbl_columnorder_SelectedIndexChanged">
                                            <asp:ListItem Value="Hostel_Name">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Guest_Name">Guest Name</asp:ListItem>
                                            <asp:ListItem Value="Guest_Address">Guest Address</asp:ListItem>
                                            <asp:ListItem Value="MobileNo">Mobile No</asp:ListItem>
                                            <asp:ListItem Value="From_Company">From Company</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room Name</asp:ListItem>
                                            <%-- <asp:ListItem Value="Hostel_Code">Hostel Code</asp:ListItem>--%>
                                            <asp:ListItem Value="Admission_Date">Admission_Date</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building Name</asp:ListItem>
                                            <%-- <asp:ListItem Value="Floor_Name">Floor Name</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room No</asp:ListItem>--%>
                                            <asp:ListItem Value="Guest_Street">Guest Street</asp:ListItem>
                                            <asp:ListItem Value="Guest_City">Guest City</asp:ListItem>
                                            <asp:ListItem Value="Guest_PinCode">Guest Pincode</asp:ListItem>
                                            <asp:ListItem Value="Purpose">Purpose</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                        CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                        TextLabelID="Labelfilter1" CollapsedSize="0" ImageControlID="Imagefilter1" CollapsedImage="~/images/right.jpeg"
                        ExpandedImage="~/images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <%--end column order--%>
                    <br />
                    <%--<br />--%>
                    <%--<asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>--%>
                    <div id="div1" runat="server" visible="false" style="width: 950px; height: 550px;
                        overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="1px" Width="900px" Style="height: 500px; overflow: auto; background-color: White;"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <center>
                        <FarPoint:FpSpread ID="Fphostelcount" Visible="false" runat="server" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" CssClass="spreadborder">
                            <%--Width="966px" Height="500px"--%>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Width="180px"
                            onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                            Width="127px" CssClass="textbox btn1 " />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="60px" CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
        </center>
        <p style="width: 691px;" align="right">
            <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                ForeColor="Red"></asp:Label>
        </p>
        <br />
        <center>
            <div id="dat" visible="false" runat="server" style="width: 852px; overflow: auto;
                height: 332px;" class="reportdivstyle table">
                <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate>
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" Width="850px" Height="330px"
                            class="spreadborder table">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </center>
        <br />
        <%--GUEST END--%>
        </div> </div> </center>
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
        </form>
    </body>
    </html>
</asp:Content>
