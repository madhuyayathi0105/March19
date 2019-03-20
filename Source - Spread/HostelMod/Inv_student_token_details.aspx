<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Inv_student_token_details.aspx.cs" Inherits="Inv_student_token_details" %>

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
            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
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
                        <asp:Label ID="lbl_stutoken" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Token Entry"></asp:Label>
                        <span style="color: #008000; font-size: large;"></span>
                        <br />
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: 658px;">
                    <br />
                    <table class="maintablestyle">
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
                                <asp:Label ID="lbl_building" runat="server" Text="Building"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_building" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_building" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_building" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_building" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_building_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_building" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbuilding_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_building"
                                            PopupControlID="panel_building" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_floor" runat="server" Text="Floor"></asp:Label>
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
                                <asp:Label ID="lbl_room" runat="server" Text="Room" Width="51px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel_room" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_room" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_room" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                            <asp:CheckBox ID="cb_room" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_room_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_room" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_room"
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
                            <td colspan="2">
                                <asp:RadioButton ID="rdb_student1" Text="Student" runat="server" GroupName="rr" AutoPostBack="true"
                                    OnCheckedChanged="rdb_checkedchanged" />
                                <asp:RadioButton ID="rdb_staff1" Text="Staff" runat="server" GroupName="rr" AutoPostBack="true"
                                    OnCheckedChanged="rdb_checkedchanged" />
                                <asp:RadioButton ID="rdb_other1" Text="Others" runat="server" GroupName="rr" AutoPostBack="true"
                                    OnCheckedChanged="rdb_checkedchanged" />
                            </td>
                            <td>
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
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
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <asp:Panel ID="pheaderfilter1" runat="server" CssClass="table2" Height="22px" Width="850px"
                                Style="margin-top: -0.1%;">
                                <asp:Label ID="Labelfilter1" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter1" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                            <asp:Panel ID="pheaderfilter2" runat="server" CssClass="table2" Height="22px" Width="850px"
                                Style="margin-top: -0.1%;">
                                <asp:Label ID="Labelfilter2" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter2" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
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
                                        <asp:TextBox ID="tborder" Visible="false" Width="850px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Value="Roll_No" Selected="True" Enabled="false">Roll No</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name" Selected="True" Enabled="false">Name </asp:ListItem>
                                            <asp:ListItem Value="SessionName">Session Name</asp:ListItem>
                                            <asp:ListItem Value="MenuName">Menu Name</asp:ListItem>
                                            <asp:ListItem Value="TokenQty">Quantity</asp:ListItem>
                                            <%-- <asp:ListItem Value="Mess_Month">Month</asp:ListItem>
                                        <asp:ListItem Value="Mess_Year">Year</asp:ListItem>--%>
                                            <asp:ListItem Value="TokenDate">Token Date</asp:ListItem>
                                            <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room</asp:ListItem>
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
                    <%-- 25.04.16--%>
                    <center>
                        <asp:Panel ID="pcolumnorder1" runat="server" CssClass="table2" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged1" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click1">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder1" Visible="false" Width="850px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged1">
                                            <asp:ListItem Value="Staff_code" Selected="True" Enabled="false">Staff Code</asp:ListItem>
                                            <asp:ListItem Value="Staff_name" Selected="True" Enabled="false">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="SessionName">Session Name</asp:ListItem>
                                            <asp:ListItem Value="MenuName">Menu Name</asp:ListItem>
                                            <asp:ListItem Value="TokenQty">Quantity</asp:ListItem>
                                            <%-- <asp:ListItem Value="Mess_Month">Month</asp:ListItem>
                                        <asp:ListItem Value="Mess_Year">Year</asp:ListItem>--%>
                                            <asp:ListItem Value="TokenDate">Token Date</asp:ListItem>
                                            <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                        CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <center>
                        <asp:Panel ID="pcolumnorder2" runat="server" CssClass="table2" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged2" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder2" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click2">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder2" Visible="false" Width="850px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged2">
                                            <asp:ListItem Value="APP_No" Selected="True" Enabled="false">Guest Code</asp:ListItem>
                                            <asp:ListItem Value="VendorCompName" Selected="True" Enabled="false">Guest Name</asp:ListItem>
                                            <asp:ListItem Value="SessionName">Session Name</asp:ListItem>
                                            <asp:ListItem Value="MenuName">Menu Name</asp:ListItem>
                                            <asp:ListItem Value="TokenQty">Quantity</asp:ListItem>
                                            <%-- <asp:ListItem Value="Mess_Month">Month</asp:ListItem>
                                        <asp:ListItem Value="Mess_Year">Year</asp:ListItem>--%>
                                            <asp:ListItem Value="TokenDate">Token Date</asp:ListItem>
                                            <asp:ListItem Value="HostelName">Hostel Name</asp:ListItem>
                                            <asp:ListItem Value="Building_Name">Building</asp:ListItem>
                                            <asp:ListItem Value="Floor_Name">Floor</asp:ListItem>
                                            <asp:ListItem Value="Room_Name">Room</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pcolumnorder2"
                        CollapseControlID="pheaderfilter2" ExpandControlID="pheaderfilter2" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="767px" Style="overflow: auto;
                        height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                        box-shadow: 0px 0px 8px #999999;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <%--<br />
                <br />
                 </div>--%>
                    <br />
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lbl_validation" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                            onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=",. ">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" CssClass="textbox"
                            Text="Export To Excel" Width="127px" Height="30px" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                            Width="60px" Height="30px" CssClass="textbox" />
                        <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn2"
                            Visible="false" OnClick="btn_delete_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 30px; margin-left: 460px;"
                    OnClick="imagebtnpopclose_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 950px;
                    height: 600px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lblpopstudentstoken" runat="server" class="fontstyleheader" Style="color: Green;"
                            Text="Token Entry"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 947px; height: 500px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <center>
                            <table class="maintablestyle" style="width: 925px; margin-top: 30px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_messname" runat="server" Text="Mess Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_messname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_messname_SelectedIndexChanged"
                                            CssClass="textbox1 ddlheight3">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_hostelname1" runat="server" Text="Hostel Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel_hostelname1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_hostelname1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true"> --Select--</asp:TextBox>
                                                <asp:Panel ID="panel_hostelname1" runat="server" Width="200px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_hostelname1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_hostelname1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_hostelname1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pceSelections1" runat="server" TargetControlID="txt_hostelname1"
                                                    PopupControlID="panel_hostelname1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_building1" runat="server" Text="Building"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel_building1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_building1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_building1" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_building1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_building1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_building1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbuilding1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_building1"
                                                    PopupControlID="panel_building1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_floor1" runat="server" Text="Floor"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel_floor1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_floor1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_floor1" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_floor1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cbfloor1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_floor1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblfloor1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_floor1"
                                                    PopupControlID="panel_floor1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_room1" runat="server" Text="Room"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel_room1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_room1" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_room1" runat="server" Width="150px" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cb_room1" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_room1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_room1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_room1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender31" runat="server" TargetControlID="txt_room1"
                                                    PopupControlID="panel_room1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Token Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_tokendate" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"
                                            OnTextChanged="txt_tokendate_TextChanged">&nbsp;</asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_tokendate" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_sessionname" Text="Session Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_sessionaname" runat="server" CssClass="textbox  ddlheight3"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_sessionname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%-- <asp:UpdatePanel ID="updatepanel_sessionname" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sessionname" runat="server" ReadOnly="true" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_sessionname" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_sessionname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_sessionname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sessionname" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cbl_sessionname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_sessionname"
                                                PopupControlID="panel_sessionname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_menuname" runat="server" Text="Menu Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upp2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_menuname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="p2" runat="server" CssClass="multxtpanel multxtpanleheight" Width="120px">
                                                    <asp:CheckBox ID="cb_menuname" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_menuname_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="Pop2" runat="server" TargetControlID="txt_menuname"
                                                    PopupControlID="p2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_rollnum" runat="server" Text="Roll No"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_rollnum" TextMode="SingleLine" runat="server" AutoCompleteType="Search"
                                            CssClass="textbox textbox1 txtheight2"></asp:TextBox>
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
                                    <td colspan="2">
                                        <asp:RadioButton ID="rdb_student" Text="Student" runat="server" GroupName="r" />
                                        <asp:RadioButton ID="rdb_staff" Text="Staff" runat="server" GroupName="r" />
                                        <asp:RadioButton ID="rdb_other" Text="Others" runat="server" GroupName="r" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go1" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go1_OnClick" />
                                    </td>
                                </tr>
                                <%--       <tr>
                                <td colspan="4">
                                    <asp:Label ID="lbl_monthyear" runat="server" Text="Month & Year" Visible="false"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddl_month" runat="server" CssClass="textbox  ddlheight1" Visible="false">
                                    </asp:DropDownList> &nbsp;
                                   
                                    <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox  ddlheight" Visible="false">
                                    </asp:DropDownList>  &nbsp; &nbsp;
                                  
                                  
                                </td>
                            </tr>--%>
                            </table>
                        </center>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lbl_errormessage" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </center>
                        <div>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                    ForeColor="Red"></asp:Label>
                            </p>
                        </div>
                        <div style="height: 300px; overflow: auto;">
                            <center>
                                <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="NotSet"
                                    BorderWidth="0px" Width="700px" Height="300px" Style="border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                        </div>
                        <br />
                        <div>
                            <center>
                                <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_save_Click" />
                                <asp:Button ID="btn_exit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btnexit_Click" />
                            </center>
                        </div>
                        <br />
                    </div>
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
