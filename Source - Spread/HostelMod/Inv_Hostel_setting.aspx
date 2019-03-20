<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Inv_Hostel_setting.aspx.cs" Inherits="Inv_Hostel_setting" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title>Hostel Setting</title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .btn
            {
                width: 40px;
                height: 30px;
            }
            .btn1
            {
                width: 80px;
                height: 30px;
            }
            .sty1
            {
                height: 400px;
                width: 750px;
                border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA;
                border-radius: 10px;
            }
            .sty
            {
                height: 550px;
                width: 1000px;
                border: 1px solid Gray;
                background-color: #F0F0F0;
                border-radius: 10px;
            }
            /*.MultipleSelectionDDL
        {
            border: solid 1px #000000;
            overflow-y: scroll;
            background-color: white;
            font-size: 11px;
            font-family: Calibri, Arial, Helvetica;
            line-height: normal;
        }*/
            .multxtpanel
            {
                background: White;
                border-color: Gray;
                border-style: Solid;
                border-width: 2px;
                position: absolute;
                box-shadow: 0px 0px 4px #999999;
                border-radius: 5px;
                overflow: auto;
            }
        </style>
        <%--<script type="text/javascript">

            function valid() {
                var id = "";
                var value1 = "";
                var idval = "";
                var empty = "";
                id = document.getElementById("<%=TextBox1.ClientID %>").value;
                if (id.trim() == "--Select--") {
                    id = document.getElementById("<%=TextBox1.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_sessionname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_sessionname.ClientID %>");
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
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function blurFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
    </script>--%>
    </head>
    <body>
        <script type="text/javascript">

            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <%--Main Content--%>
        <br />
        <div>
            <center>
                <asp:Label ID="lblsessionmaster" class="fontstyleheader" Style="color: Green;" Text=" Hostel Setting"
                    runat="server"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 550px">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:Label ID="lblhostelname" Text="Mess Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txthostelname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="120px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="phostel" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="chkhostelname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chk_hostel_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklsthostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklst_hostel_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txthostelname"
                                        PopupControlID="phostel" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblsessionname" Text="Session Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsessionname" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="120px" Height="18px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Psession" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                        <asp:CheckBox ID="chksessionname" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chksession_checkedchange" />
                                        <asp:CheckBoxList ID="chklstsession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstsession_Change">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsessionname"
                                        PopupControlID="Psession" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblscheduletype" runat="server" Text="Schedule Type"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="rdodatewise" Text="Datewise" runat="server" GroupName="day"
                                AutoPostBack="true" OnCheckedChanged="rdodatewise_CheckedChanged" />
                            <asp:RadioButton ID="rdodaywise" Text="Daywise" runat="server" GroupName="day" AutoPostBack="true"
                                OnCheckedChanged="rdodaywise_CheckedChanged" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" runat="server" CssClass="textbox textbox1" Width="120px"
                                AutoPostBack="true" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtfromdate" runat="server"
                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" runat="server" CssClass="textbox textbox1" Width="120px"
                                AutoPostBack="true" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="caltodate" TargetControlID="txttodate" runat="server" Format="dd/MM/yyyy"
                                CssClass="cal_Theme1 ajax__calendar_active">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Days"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_Daywise" runat="server" Enabled="false" CssClass="textbox textbox1"
                                        Width="110px" Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" Width="150px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cbDaywise" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbDaywise_change" />
                                        <asp:CheckBoxList ID="Cbldaywise" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Cbldaywise_selectIndex">
                                            <asp:ListItem Value="0">Monday</asp:ListItem>
                                            <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                            <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                            <asp:ListItem Value="3">Thursday</asp:ListItem>
                                            <asp:ListItem Value="4">Friday</asp:ListItem>
                                            <asp:ListItem Value="5">Saturday</asp:ListItem>
                                            <asp:ListItem Value="6">Sunday</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_Daywise"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btngo" Text="Go" runat="server" CssClass="textbox btn" OnClick="btngo_Click" />
                            <asp:Button ID="btnaddnew" Text="Add New" runat="server" CssClass="textbox btn1"
                                OnClick="btnaddnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="errorlable" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <div id="div1" runat="server" visible="false" style="width: 950px; height: 350px;"
                    class="spreadborder">
                    <br />
                    <FarPoint:FpSpread ID="Fpspread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="900px" Height="350px" OnCellClick="Cell_Click" OnPreRender="Fpspread1_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1"></asp:TextBox>
                    <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        CssClass="textbox btn1" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
            </div>
        </center>
        <%--Popup content--%>
        <center>
            <div id="divPopper" runat="server" visible="false" class="popupstyle popupheight">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 29px; margin-left: 439px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <br />
                <center>
                    <div class="sty1" style="background-color: White; overflow: auto; width: 900px; height: 533px;"
                        align="center">
                        <br />
                        <center>
                            <asp:Label ID="lblHeader2" runat="server" Style="font-size: large; color: Green;"
                                Text="Hostel Setting"></asp:Label>
                        </center>
                        <br />
                        <div align="center" style="overflow: auto; width: 850px; border-radius: 10px; border: 1px solid Gray;">
                            <br />
                            <table style="height: 50px; border: 1px solid #0CA6CA; border-radius: 10px; background-color: #0CA6CA;
                                font-size: medium;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblhostelname1" Text="Mess Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txthostelname1" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Width="120px" Height="18px">--Select--</asp:TextBox>
                                                <asp:Panel ID="phostel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                    width: 200px">
                                                    <asp:CheckBox ID="chkhostelname1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chk_hostel1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="chklsthostel1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklst_hostel1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pextender1" runat="server" TargetControlID="txthostelname1"
                                                    PopupControlID="phostel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsessionname1" Text="Session Name" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtsessionname1" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Width="120px" Height="18px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Psession1" runat="server" CssClass="multxtpanel" Style="height: 150px;
                                                    width: 150px">
                                                    <asp:CheckBox ID="chksessionname1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="chksession1_checkedchange" />
                                                    <asp:CheckBoxList ID="chklstsession1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklstsession1_Change">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtsessionname1"
                                                    PopupControlID="Psession1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblscheduletype1" runat="server" Text="Schedule Type"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButton ID="rdodatewise1" Text="Datewise" runat="server" GroupName="day1"
                                            AutoPostBack="true" OnCheckedChanged="rdodatewise1_CheckedChanged" />
                                        <asp:RadioButton ID="rdodaywise1" Text="Daywise" runat="server" GroupName="day1"
                                            AutoPostBack="true" OnCheckedChanged="rdodaywise1_CheckedChanged" />
                                    </td>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfromdate1" runat="server" Text="From Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate1" runat="server" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="calfromdate1" TargetControlID="txtfromdate1" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltodate1" runat="server" Text="To Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate1" runat="server" CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                        <asp:CalendarExtender ID="caltodate1" TargetControlID="txttodate1" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldaycom" runat="server" Text="Days"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="uu" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdaycompar" runat="server" CssClass="textbox textbox1" Width="110px"
                                                    Height="20px" ReadOnly="true" Enabled="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="p00" runat="server" Width="150px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chkdaycompar" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="chdaycompar_change" />
                                                    <asp:CheckBoxList ID="chklstdaycompar" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkklstdaycompar_selectIndex">
                                                        <asp:ListItem Value="0">Monday</asp:ListItem>
                                                        <asp:ListItem Value="1">Tuesday</asp:ListItem>
                                                        <asp:ListItem Value="2">Wednesday</asp:ListItem>
                                                        <asp:ListItem Value="3">Thursday</asp:ListItem>
                                                        <asp:ListItem Value="4">Friday</asp:ListItem>
                                                        <asp:ListItem Value="5">Saturday</asp:ListItem>
                                                        <asp:ListItem Value="6">Sunday</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtdaycompar"
                                                    PopupControlID="p00" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo1" Text="Go" Visible="false" runat="server" Style="top: 10px;
                                            left: 505px; font-family: 'Book Antiqua'" CssClass="textbox btn" OnClick="btngo1_Click" />
                                    </td>
                                    <td>
                                        <%--                        <asp:Button ID="btnaddnew" Text="Add New" runat="server" Style="top: 10px; left: 696px;
                            font-family: 'Book Antiqua'" CssClass="textbox btn1" OnClick="btnaddnew_Click" />--%>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <%--                    <div style="width: 800px; height: 350px; font-family: Book Antiqua; border-bottom-color: Black;
                        border-collapse: collapse; border: 1px solid black">--%>
                            <fieldset style="height: 210px; border-radius: 10px">
                                <fieldset style="height: 50px; float: left; border-radius: 10px">
                                    <legend>Edit Menu Total Strength</legend>
                                    <asp:CheckBoxList ID="cbltypeoftotal" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="H">H</asp:ListItem>
                                        <asp:ListItem Value="D">D</asp:ListItem>
                                        <asp:ListItem Value="HS">HS</asp:ListItem>
                                        <asp:ListItem Value="S">S</asp:ListItem>
                                        <asp:ListItem Value="G">G</asp:ListItem>
                                        <asp:ListItem Value="O">Oth</asp:ListItem>
                                    </asp:CheckBoxList>
                                </fieldset>
                                <fieldset style="height: 50px; width: 505px; float: left; border-radius: 10px">
                                    <legend>Use Only Student Attendance Hour</legend>
                                    <asp:CheckBox ID="cbAllstudentAttendance" runat="server" Text="All Student" />
                                    <asp:CheckBox ID="cbstudentAttendancehour" runat="server" Text="Use Only Student Attendance Hour"
                                        AutoPostBack="true" OnCheckedChanged="cbstudentAttendancehour_Change" />
                                    <span style="font-size: medium; font-weight: bold;">Hour</span>
                                    <asp:DropDownList ID="ddl_hour" runat="server" Enabled="false" Height="30px" Width="65px"
                                        CssClass="textbox textbox1">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txt_Attendancehour" Enabled="false" Visible="false" runat="server"
                                        Height="20px" CssClass="Dropdown_Txt_Box" ReadOnly="true" Width="135px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" HorizontalAlign="Left" CssClass="multxtpanel"
                                        Style="height: 250px; width: 150px; top: 50px; left: 110px; position: absolute;
                                        display: none;">
                                        <asp:CheckBox ID="cbAttendancehour" runat="server" Visible="false" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbAttendancehour_CheckedChanged"
                                            Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblAttendancehour" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblAttendancehour_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                        <%-- Height="200px"--%>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_Attendancehour"
                                        DynamicServicePath="" PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </fieldset>
                                <fieldset style="height: 50px; float: left; border-radius: 10px">
                                    <legend>Use Only Staff Bio-Matric Attendance</legend>
                                    <asp:CheckBox ID="cbstaffbiomarric" runat="server" Text="Use Only Staff Bio-Matric Attendance" />
                                </fieldset>
                                <fieldset style="height: 50px; float: left; border: 1px solid white;">
                                    <br />
                                    <asp:Button ID="btnhostel_save" runat="server" OnClick="btnhostel_Click" Visible="false"
                                        Style="float: left;" Text="Save" CssClass="textbox btn1" />
                                </fieldset>
                                <fieldset style="height: 50px; width: 434px; float: left; border-radius: 10px;">
                                    <legend>Attendance Strength wise Status Settings</legend>
                                    <asp:CheckBoxList ID="daily_consumption" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem>H</asp:ListItem>
                                        <asp:ListItem>D</asp:ListItem>
                                        <asp:ListItem>S</asp:ListItem>
                                        <asp:ListItem>G</asp:ListItem>
                                    </asp:CheckBoxList>
                                </fieldset>
                                <fieldset style="height: 50px; width: 283px; float: left; border-radius: 10px;">
                                    <legend>Mess Attendance Settings</legend>
                                    <asp:CheckBoxList ID="cbl_messattendance" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem>H</asp:ListItem>
                                        <asp:ListItem>D</asp:ListItem>
                                        <asp:ListItem>S</asp:ListItem>
                                    </asp:CheckBoxList>
                                </fieldset>
                                <fieldset style="height: 50px; width: 283px; float: left; border-radius: 10px;">
                                    <legend>Menu purpose Catagory</legend>
                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" />
                                    <asp:DropDownList ID="ddl_group" runat="server" CssClass="textbox  ddlheight3">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btnminus_Click" />
                                </fieldset>
                            </fieldset>
                        </div>
                        <br />
                        <br />
                        <center>
                            <div>
                                <asp:Button ID="btn_update" runat="server" OnClick="btn_update_Click" Text="Update"
                                    CssClass="textbox btn1" />
                                <asp:Button ID="btn_delete" runat="server" OnClick="btn_delete_Click" Text="Delete"
                                    CssClass="textbox btn1" />
                                <asp:Button ID="btnsave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="textbox btn1" />
                                <asp:Button ID="btnExit" runat="server" Text="Exit" OnClick="btnexit1_Click" CssClass="textbox btn1" />
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                <center>
                    <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                        height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <table style="line-height: 30px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_addgroup" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txt_addgroup" runat="server" Width="200px" CssClass="textbox textbox1"
                                        onkeypress="display1()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="line-height: 35px">
                                    <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                    <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                        CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label2" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
