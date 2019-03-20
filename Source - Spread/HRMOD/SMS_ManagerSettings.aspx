<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="SMS_ManagerSettings.aspx.cs" Inherits="SMS_ManagerSettings" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function smsfocusin() {
            $('.errorsms').html("");
        }
        function emailfocusin() {
            $('.errormail').html("");
        }
        function smsfocusout() {
            var str = $('#<%=txtmobilno.ClientID %>').val();
            var validMobNum = [];
            var invalidmobnum = [];
            var strarray = str.split(',');
            var validcount = 0;
            var invalidcount = 0;
            if ($('#<%=txtmobilno.ClientID %>').val() != "") {
                for (var i = 0; i < strarray.length; i++) {
                    if (validatePhone(strarray[i])) {
                        validMobNum[validcount] = strarray[i];
                        validcount++;
                    }
                    else {
                        invalidmobnum[invalidcount] = strarray[i];
                        invalidcount++;
                    }
                }
                $('#<%=txtmobilno.ClientID %>').val(validMobNum);
                if (invalidmobnum.length > 0 && invalidcount != 0) {
                    $('.errorsms').css('display', 'block');
                    $('.errorsms').html("Please Enter Valid 10 - Digits Mobile Numbers Seperated By Comma!!! Invalid Mobile Numbers Are " + invalidmobnum.join(','));
                }
                else {
                    $('.errorsms').html("");
                }
            }
            else {
                $('#<%=txtmobilno.ClientID %>').val("");
                $('.errorsms').html("");
            }
        }
        function emailfocusout() {
            var stremailid = $('#<%=txtsendmail.ClientID %>').val();
            var validemailid = [];
            var invalidemailid = [];
            var stremailarray = stremailid.split(',');
            var validemailcount = 0;
            var invalidemailcount = 0;
            if ($('#<%=txtsendmail.ClientID %>').val() != "") {
                for (var i = 0; i < stremailarray.length; i++) {
                    if (validateEmail(stremailarray[i])) {
                        validemailid[validemailcount] = stremailarray[i];
                        validemailcount++;
                        //                        alert(strarray[i] + 'is Valid.' + validcount)
                    }
                    else {
                        invalidemailid[invalidemailcount] = stremailarray[i];
                        invalidemailcount++;
                        //                        alert(strarray[i] + 'Is Invalid.' + invalidcount)
                    }
                }
                $('#<%=txtsendmail.ClientID %>').val(validemailid);
                //                $('#invalidMobnums').text(invalidmobnum);
                if (invalidemailid.length > 0 && invalidemailcount != 0) {
                    $('.errormail').css('display', 'block');
                    $('.errormail').html("Please Enter Valid Email Ids Seperated By Comma!!! Invalid Email Ids Are " + invalidemailid.join(','));
                }
                else {
                    $('.errormail').html("");
                }
            }
            else {
                $('#<%=txtsendmail.ClientID %>').val("");
                $('.errormail').html("");
            }
        }
        function smskeypress(e) {
            if (e.which == 44) { }
            else {
                if (e.which != 8 && e.which != 0 && e.which != 13 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    $('.errorsms').css('color', 'red');
                    $('.errorsms').css('display', 'block');
                    $('.errorsms').html("Its Allows Numbers And Comma Only").show().fadeOut("slow");
                    return false;
                }
            }
        }
        function InitEvents() {
            $('#<%=txtmobilno.ClientID %>').focusin(smsfocusin);
            $('#<%=txtsendmail.ClientID %>').focusin(emailfocusin);
            $('#<%=txtmobilno.ClientID %>').focusout(smsfocusout);
            $('#<%=txtsendmail.ClientID %>').focusout(emailfocusout);
            $('#<%=txtmobilno.ClientID %>').keypress(smskeypress);
        }
        function validatePhone(phoneText) {
            var filter = /^[0-9]{10}$/;
            if (filter.test(phoneText)) {
                return true;
            }
            else {
                return false;
            }
        }
        function validateEmail(sEmail) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(sEmail)) {
                return true;
            }
            else {
                return false;
            }
        }
        $(document).ready(InitEvents);
    </script>
    <style>
        body
        {
            font-family: Book Antiqua;
            font-size: 14px;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Automatic SMS Settings</span></div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <fieldset id="mainfld" runat="server" class="spreadborder" style="height: 475px;
                        background-color: #F0F0F0; border-color: transparent; width: 900px;">
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <table id="Table1" class="maintablestyle" runat="server" width="366px">
                                    <tr>
                                        <td>
                                            College Name
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                                                AutoPostBack="true" CssClass="textbox1 ddlheight4" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <table cellpadding="10px">
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdb_Sms" runat="server" Style="margin-left: 100px" Text="Sms"
                                                Visible="true" GroupName="grdo" AutoPostBack="true" OnCheckedChanged="rdb_Sms_Check"
                                                Checked="true" />
                                            <asp:RadioButton ID="rdb_Mail" runat="server" Text="Mail" GroupName="grdo" Visible="true"
                                                AutoPostBack="true" OnCheckedChanged="rdb_Mail_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Type
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1 ddlheight3" Width="350px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddltype_change">
                                                            <asp:ListItem Selected="True" Text="Birthday Wishes" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Student Attendance" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="CAM Marks" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Staff Attendance" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Block Box" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Student Attendance Shortage" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Hostel Student Attendance" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="Student/Staff Cumulative Attendance" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="Automatic Download And Mark Time Attendance Settings" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="Finance Settings" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="Driving License Renewal Settings" Value="10"></asp:ListItem>
                                                            <asp:ListItem Text="FC Reminder Settings" Value="11"></asp:ListItem>
                                                            <asp:ListItem Text="Insurance Renewal Settings" Value="12"></asp:ListItem>
                                                            <asp:ListItem Text="Student Cummulative Attendance" Value="13"></asp:ListItem>
                                                            <asp:ListItem Text="Receipt Cancel" Value="14"></asp:ListItem>
                                                            <asp:ListItem Text="Student Due Date Automatic Sms" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="Student Home Work" Value="16"></asp:ListItem>
                                                            <asp:ListItem Text="Student Absent" Value="17"></asp:ListItem>
                                                            <asp:ListItem Text="Hostel Student Absent" Value="18"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddltype1" runat="server" CssClass="textbox1 ddlheight3" Width="350px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddltype1_change" Visible="false">
                                                            <%--sarany(31.10.2017)--%>
                                                            <asp:ListItem Text="Hostel Absent List" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Attendance Summary Hostel Wise" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Item Stock Report" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Black Box Report" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Over All Attendance Report For Particular Day" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Absentees Report" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Finance BillNoWise Paid Report" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="Finance InstitutionWise Paid Report" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="Financial Student Paymode Collection Report" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="Staff Attendance Report" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="Department Wise Staff Attendance Report" Value="10"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:Label ID="lbltestname" runat="server" Visible="false" Text="Test Name"></asp:Label>
                                                        <asp:DropDownList ID="ddltest" runat="server" Visible="false" CssClass="textbox1 ddlheight3"
                                                            Width="230px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbldevname" runat="server" Visible="false" Text="Device Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="upddevname" runat="server" Visible="false">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtdevname" runat="server" Width="140px" CssClass="textbox textbox1"
                                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                                <asp:Panel ID="pnldevname" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                                    <asp:CheckBox ID="cbdevname" runat="server" AutoPostBack="true" OnCheckedChanged="cbdevname_CheckedChanged"
                                                                        Text="Select All" />
                                                                    <asp:CheckBoxList ID="cbldevname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbldevname_selectedchanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="popdevname" runat="server" PopupControlID="pnldevname"
                                                                    TargetControlID="txtdevname" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="send" runat="server">
                                            Send To
                                        </td>
                                        <td>
                                            <fieldset id="fldresource" runat="server" style="height: 30px; width: 369px;">
                                                <table id="tblOld" runat="server" visible="false">
                                                    <%--cellpadding="6px"--%>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkstudent" runat="server" Text="Student" OnCheckedChanged="chkstudent_change"
                                                                AutoPostBack="true" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkstaff" runat="server" OnCheckedChanged="chkstaff_change" AutoPostBack="true"
                                                                Text="Staff" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkhod" runat="server" Enabled="false" AutoPostBack="true" Text="HOD"
                                                                OnCheckedChanged="chkhodoff_Change" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkhigheroff" runat="server" Enabled="false" AutoPostBack="true"
                                                                OnCheckedChanged="chkhigheroff_Change" Text="Higher Officials" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="tblSms" runat="server" visible="false">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkstudsms" runat="server" Text="Student" />
                                                            <asp:CheckBox ID="chkfatsms" runat="server" Text="Father" />
                                                            <asp:CheckBox ID="chkmotsms" runat="server" Text="Mother" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsendwish" runat="server" Visible="false" Text="Send to mobile <br /> for student wish"></asp:Label><br />
                                            <asp:Label ID="lblhostelname" runat="server" Visible="false" Text="Hostel Name"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblcount" runat="server" Visible="false" Text="Send to Hof for <br /> student wish Or staff wish"></asp:Label>
                                        </td>
                                        <td>
                                            <fieldset id="fldsendwish" runat="server" style="width: 85px;" visible="false">
                                                <asp:CheckBox ID="chkstudwish" runat="server" Text="Student" /><br />
                                                <asp:CheckBox ID="chkfatwish" runat="server" Text="Father" /><br />
                                                <asp:CheckBox ID="chkmotwish" runat="server" Text="Mother" />
                                            </fieldset>
                                            <fieldset id="fldcount" runat="server" style="width: 85px;" visible="false">
                                                <asp:CheckBox ID="chkstudcount" runat="server" Text="Student" /><br />
                                                <asp:CheckBox ID="chkstafcount" runat="server" Text="Staff" /><br />
                                            </fieldset>
                                            <asp:UpdatePanel ID="updhosname" runat="server" Visible="false">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txthosname" runat="server" Width="140px" CssClass="textbox textbox1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlhosname" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                        <asp:CheckBox ID="cbhosname" runat="server" AutoPostBack="true" OnCheckedChanged="cbhosname_CheckedChanged"
                                                            Text="Select All" />
                                                        <asp:CheckBoxList ID="cblhosname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblhosname_selectedchanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pophosname" runat="server" PopupControlID="pnlhosname"
                                                        TargetControlID="txthosname" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdldate" runat="server" visible="false">
                                            Sending Date
                                        </td>
                                        <td colspan="7" id="tdltime" runat="server" visible="false">
                                            <asp:TextBox ID="txtsenddt" runat="server" Enabled="false" CssClass="textbox txtheight2"
                                                Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="calsenddt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                Format="dd/MM/yyyy" TargetControlID="txtsenddt">
                                            </asp:CalendarExtender>
                                            Sending Time
                                            <asp:DropDownList ID="ddlhr" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="12" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmin" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmer" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="AM" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="lblsession" runat="server" Visible="false" Text="Session"></asp:Label>
                                            <asp:DropDownList ID="ddlsession" runat="server" Visible="false" CssClass="textbox1 ddlheight3">
                                                <asp:ListItem Selected="True" Text="Evening" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Morning" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnaddtime" runat="server" Visible="false" Text="Add" CssClass="textbox textbox1 btn2"
                                                OnClick="btnaddtime_click" />
                                        </td>
                                    </tr>
                                    <tr id="tblaltrow1" runat="server" visible="false">
                                        <td>
                                        </td>
                                        <td colspan="6">
                                            &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblalt1" runat="server" Text="Alternate Time1"></asp:Label>
                                            <asp:DropDownList ID="ddlhr1" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="12" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmin1" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmer1" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="AM" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="tblaltrow2" runat="server" visible="false">
                                        <td>
                                        </td>
                                        <td colspan="6">
                                            &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblalt2" runat="server" Text="Alternate Time2"></asp:Label>
                                            <asp:DropDownList ID="ddlhr2" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="12" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmin2" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlmer2" runat="server" CssClass="textbox1 ddlheight" Width="60px">
                                                <asp:ListItem Selected="True" Text="AM" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--added by saranya on17-11-2017--%>
                                    <tr id="tblemail" runat="server" visible="false">
                                        <td>
                                            Higher Off Email ID(s)
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtsendmail" runat="server" Enabled="false" CssClass="textbox txtheight3"
                                                Width="570px" autocomplete="off"></asp:TextBox>
                                            <asp:Label ID="spnEmailError" runat="server" class="errormail" Style="font-family: Book Antiqua;
                                                font-size: smaller; font-weight: bold; height: auto; width: 80%; position: relative;
                                                color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tblMobileno" runat="server" visible="false">
                                        <td>
                                            Higher Off Mobile No(s)
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtmobilno" runat="server" Enabled="false" CssClass="textbox txtheight3"
                                                Width="385px" autocomplete="off"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtermble" runat="server" FilterType="Custom,Numbers"
                                                FilterMode="ValidChars" ValidChars="," TargetControlID="txtmobilno">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CheckBox ID="chksendsms" runat="server" Text="Send SMS" Checked="true" />
                                            <asp:Label ID="spnSmsError" runat="server" class="errorsms" Visible="true" Style="font-family: Book Antiqua;
                                                font-size: smaller; font-weight: bold; position: relative; width: 80%; height: auto;
                                                color: Red;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="tbldays" runat="server" visible="false">
                                        <td>
                                            Day Once
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdays" runat="server" CssClass="textbox txtheight3" Width="50px"></asp:TextBox>
                                            <asp:CheckBox ID="cksendsms" runat="server" Text="Send SMS" />
                                        </td>
                                    </tr>
                                    <tr id="tblrow3" runat="server" visible="false">
                                        <td colspan="7">
                                            <%--</td>
                                <td>--%>
                                            <fieldset id="fldgrphos" runat="server" visible="false" style="width: 240px;">
                                                <asp:CheckBox ID="chkgrphos" runat="server" Text="Group By Hosteler / Day Scholar" />
                                            </fieldset>
                                            <fieldset id="fldfrmtodt" runat="server" visible="false" style="width: 675px;">
                                                From Date
                                                <asp:TextBox ID="txtfrmdt" runat="server" Width="75px" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_frmdt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                    Format="dd/MM/yyyy" TargetControlID="txtfrmdt">
                                                </asp:CalendarExtender>
                                                To Date
                                                <asp:TextBox ID="txttodt" runat="server" Width="75px" CssClass="textbox textbox1"></asp:TextBox>
                                                <asp:CalendarExtender ID="cal_todt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                    Format="dd/MM/yyyy" TargetControlID="txttodt">
                                                </asp:CalendarExtender>
                                                <table cellpadding="5px" style="width: 340px; margin-left: 310px; margin-top: -30px;">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkmorabs" runat="server" Text="Mark Absent for <br /> Mor UnRegistered Staff" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkeveabs" runat="server" Text="Mark Absent for <br /> Eve UnRegistered Staff" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr id="tblrow4" runat="server" visible="false">
                                        <td colspan="7">
                                            <asp:CheckBox ID="chkinclongabs" runat="server" Visible="false" Text="Include Long Absentees" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblgreater" runat="server" Visible="false" Text="Greater"></asp:Label>
                                            <asp:TextBox ID="txtgreater" runat="server" Visible="false" CssClass="textbox txtheight2"
                                                Width="50px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtergrt" runat="server" FilterType="Numbers" TargetControlID="txtgreater">
                                            </asp:FilteredTextBoxExtender>
                                            <fieldset id="fldstftypcat" runat="server" visible="false" style="width: 675px; margin-top: -32px;">
                                                <asp:DropDownList ID="ddlstftypcat" runat="server" CssClass="textbox1 ddlheight3"
                                                    OnSelectedIndexChanged="ddlstftypcat_change" AutoPostBack="true">
                                                    <asp:ListItem Selected="True" Text="Staff Type" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Staff Category" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlstftypedown" runat="server" Visible="false" CssClass="textbox1 ddlheight3">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlstfcatdown" runat="server" Visible="false" CssClass="textbox1 ddlheight3">
                                                </asp:DropDownList>
                                                Department
                                                <asp:DropDownList ID="ddldeptdown" runat="server" CssClass="textbox1 ddlheight3">
                                                </asp:DropDownList>
                                                Shift
                                                <asp:DropDownList ID="ddlshiftdown" runat="server" CssClass="textbox1 ddlheight3">
                                                </asp:DropDownList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <div id="fldwithlev" runat="server" visible="false">
                                                <asp:CheckBox ID="chkwithlev" runat="server" Text="With LeaveType" />
                                                <asp:CheckBox ID="chkgrpby" runat="server" Text="Group By" OnCheckedChanged="chkgrpby_Change"
                                                    AutoPostBack="true" />
                                                <div id="divstftyp" runat="server" visible="false" style="margin-left: 235px; margin-top: -18px;
                                                    background-color: #F0F0F0; border-color: transparent; width: 215px; height: 25px;"
                                                    class="spreadborder">
                                                    <asp:RadioButton ID="rdb_stftype" runat="server" Checked="true" Text="Staff Type"
                                                        OnCheckedChanged="rdb_stftype_change" AutoPostBack="true" GroupName="stftype" />
                                                    <asp:RadioButton ID="rdb_stfcat" runat="server" Text="Staff Category" OnCheckedChanged="rdb_stfcat_change"
                                                        AutoPostBack="true" GroupName="stftype" />
                                                </div>
                                                <div id="divsellst" runat="server" visible="false" style="margin-left: 468px; margin-top: -24px;">
                                                    <asp:CheckBox ID="chksellst" runat="server" OnCheckedChanged="chksellst_change" AutoPostBack="true"
                                                        Text="Select List" />
                                                    <asp:UpdatePanel ID="updstftyp" runat="server" Visible="false">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtstftype" runat="server" Style="float: right; margin-top: -24px;"
                                                                Width="140px" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panelstftyp" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="cbstftype" runat="server" AutoPostBack="true" OnCheckedChanged="cbstftype_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cblstftype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstftype_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popstftyp" runat="server" PopupControlID="panelstftyp"
                                                                TargetControlID="txtstftype" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdatePanel ID="updstfcat" runat="server" Visible="false">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtstfcat" runat="server" Style="float: right; margin-top: -24px;"
                                                                Width="140px" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panelstfcat" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="cbstfcat" runat="server" AutoPostBack="true" OnCheckedChanged="cbstfcat_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cblstfcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblstfcat_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="popstfcat" runat="server" PopupControlID="panelstfcat"
                                                                TargetControlID="txtstfcat" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="tblrow5" runat="server" visible="false">
                                        <td>
                                            Days Before Remind
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdaysRemind" runat="server" MaxLength="2" CssClass="textbox txtheight2"
                                                Width="50px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filtertxtdaysRemind" runat="server" FilterType="Numbers"
                                                TargetControlID="txtdaysRemind">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr id="trstdhrday" runat="server" visible="false">
                                        <td id="myvis2" runat="server">
                                            <asp:RadioButton ID="rdohour" runat="server" Text="Hour Wise" AutoPostBack="True"
                                                Font-Size="Medium" OnCheckedChanged="rdohour_CheckedChanged" Checked="True" Font-Names="Book Antiqua"
                                                Height="16px" GroupName="a" />
                                        </td>
                                        <td id="myvis1" runat="server">
                                            <asp:RadioButton ID="rdodaily" runat="server" Text="Day Wise" OnCheckedChanged="rdodaily_CheckedChanged"
                                                AutoPostBack="true" Width="95px" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="20px" GroupName="a" />
                                        </td>
                                        <td id="hourwise" runat="server">
                                            <fieldset id="Fielset1" runat="server" style="height: 30px; width: 235px; margin-left: -298px;
                                                margin-top: 3px;">
                                                <asp:Label ID="lbl_hour" Text="Hour" runat="server" Style="margin-left: 4px; margin-top: -16px;"></asp:Label>
                                                <asp:UpdatePanel ID="Upp1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_hour" runat="server" Style="margin-left: 50px; margin-top: -16px;"
                                                            CssClass="textbox txtheight1 textbox1" ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                        <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                            BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                            <asp:CheckBox ID="cb_hour" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_hour_checkedchange" />
                                                            <asp:CheckBoxList ID="cbl_hour" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hour_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hour"
                                                            PopupControlID="p1" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </fieldset>
                                        </td>
                                        <td colspan="3" id="daywise" runat="server" visible="false">
                                            <fieldset style="width: 222px; height: 30px; margin-left: -298px; margin-top: 3px;">
                                                <asp:RadioButtonList ID="rbldayType" runat="server" RepeatDirection="Horizontal"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rbldayType_Selected">
                                                    <asp:ListItem Text="Morning" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Evening" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Both" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="Checkhrdaysend" runat="server" Checked="true" Text="Send SMS" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                    <br />
                    <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" CssClass="textbox1 btn2" />
                    <asp:Button ID="btnexit" runat="server" Text="Exit" OnClick="btnexit_click" CssClass="textbox1 btn2" />
                    <br />
                    <br />
                    <div id="alertpopwindow" runat="server" class="popupstyle popupheight1" visible="false"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 280px;
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
                                                        Text="Ok" runat="server" OnClick="btnerrclose_Click" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
