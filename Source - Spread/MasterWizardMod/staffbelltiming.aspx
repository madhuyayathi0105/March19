<%@ Page Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master" AutoEventWireup="true"
    CodeFile="staffbelltiming.aspx.cs" Inherits="staffbelltiming" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PrintMaster.ascx" TagName="printmaster" TagPrefix="InsproPlus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Bell Timing</span>
            </div>
        </center>
        <center>
            <asp:UpdatePanel ID="Upmain" runat="server">
                <ContentTemplate>
                    <div class="maindivstyle" style="width: 1000px; height: 600px;">
                        <br />
                        <div>
                            <fieldset style="height: 15px; width: 255px; position: absolute; margin-top: 2px;
                                margin-left: 328px;">
                                <table width="300px">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdbstudent" runat="server" Text="Student" AutoPostBack="true"
                                                Checked="True" OnCheckedChanged="rdbstudent_changed" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbstaff" runat="server" Text="Staff" AutoPostBack="true" Checked="False"
                                                OnCheckedChanged="rdbstaff_changed" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdbhostel" runat="server" Text="Hostel" AutoPostBack="true"
                                                Checked="False" OnCheckedChanged="rdbhostel_changed" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        </br> </br> </br>
                        <table class="maintablestyle" width="auto">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcol" runat="server" Font-Bold="true" Style="font-family: 'Book Antiqua'"
                                        Text="College Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcol" runat="server" Width="242px" CssClass="textbox ddlheight4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcol_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlstafcatmain" runat="server" Visible="false" Width="135px"
                                        CssClass="textbox ddlheight4" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlhostel" runat="server" Visible="false" Width="135px" CssClass="textbox ddlheight4"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_go" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                                OnClick="btn_go_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_addnew" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                                OnClick="btn_addnew_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblstubatchyear" runat="server" Font-Bold="true" Style="font-family: 'Book Antiqua'"
                                        Text="Batch Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstubatchyear" runat="server" Visible="true" Width="80px"
                                        CssClass="textbox ddlheight4" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblstudegreecode" runat="server" Font-Bold="true" Style="font-family: 'Book Antiqua'"
                                        Text="Course"></asp:Label>
                                    <asp:DropDownList ID="ddlstudegreecode" runat="server" Visible="true" Width="200px"
                                        CssClass="textbox ddlheight4" AutoPostBack="true" OnSelectedIndexChanged="ddlstudegreecode_changed">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblstusem" runat="server" Font-Bold="true" Style="font-family: 'Book Antiqua'"
                                        Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstusem" runat="server" Visible="true" Width="77px" CssClass="textbox ddlheight4"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        </br>
                        <asp:UpdatePanel ID="upgo" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <asp:GridView ID="grdstaff" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" toGenerateColumns="false"
                                    ShowHeader="false" OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChanged">
                                    <%--OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChanged"--%>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Uphos" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="SelectedGridCellIndexhos" runat="server" Value="-1" />
                                <asp:GridView ID="grdhost" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" toGenerateColumns="false"
                                    ShowHeader="false" OnRowCreated="OnRowCreatedgrdhost" OnRowDataBound="grdhost_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChangedgrdhost">
                                    <%--OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChanged"--%>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="Upstud" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="SelectedGridCellIndexstud" runat="server" Value="-1" />
                                <asp:GridView ID="grdstud" Width="800px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                    HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" toGenerateColumns="false"
                                    ShowHeader="false" OnRowCreated="OnRowCreatedgrdstud" OnRowDataBound="grdstud_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChangedgrdstud">
                                    <%--OnRowCreated="OnRowCreated" OnRowDataBound="grdstaff_RowDataBound"
                                    OnSelectedIndexChanged="SelectedIndexChanged"--%>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="popper1" runat="server" visible="false" style="height: 70em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 18px; margin-left: 523px;"
                                OnClick="imagebtnpopclose1_Click" />
                            <br />
                            <div style="height: 900px; width: 1090px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <br />
                                <div>
                                    <fieldset style="height: 15px; width: 315px; position: absolute; margin-top: 2px;
                                        margin-left: 34px;">
                                        <asp:CheckBox ID="chkotherpermission" runat="server" Checked="false" Text="Other Permission"
                                            Style="position: absolute; margin-top: 1px; margin-left: -152px;" />
                                    </fieldset>
                                    <center>
                                        <span style="color: Green; font-size: large;">Staff Bell Timing</span>
                                    </center>
                                </div>
                                <br />
                                <div>
                                    <fieldset style="height: 115px; width: 315px; position: absolute; margin-top: -2px;
                                        margin-left: 34px;">
                                        <asp:Label ID="lblmonth" runat="server" Text="Month" Style="position: absolute; margin-top: 25px;
                                            margin-left: -140px;"></asp:Label>
                                        <asp:DropDownList ID="ddlmonth" CssClass="textbox ddlheight2" runat="server" Style="position: absolute;
                                            margin-top: 20px; margin-left: -85px; width: 85px;">
                                            <asp:ListItem Value="1">January</asp:ListItem>
                                            <asp:ListItem Value="2">Febrarury</asp:ListItem>
                                            <asp:ListItem Value="3">March</asp:ListItem>
                                            <asp:ListItem Value="4">April</asp:ListItem>
                                            <asp:ListItem Value="5">May</asp:ListItem>
                                            <asp:ListItem Value="6">June</asp:ListItem>
                                            <asp:ListItem Value="7">July</asp:ListItem>
                                            <asp:ListItem Value="8">Auguest</asp:ListItem>
                                            <asp:ListItem Value="9">September</asp:ListItem>
                                            <asp:ListItem Value="10">Octobar</asp:ListItem>
                                            <asp:ListItem Value="11">November</asp:ListItem>
                                            <asp:ListItem Value="12">December</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblyear" runat="server" Text="Year" Style="position: absolute; margin-top: 25px;
                                            margin-left: 15px;"></asp:Label>
                                        <asp:DropDownList ID="ddlyear" CssClass="textbox ddlheight2" runat="server" Style="position: absolute;
                                            margin-top: 20px; margin-left: 55px; width: 65px;">
                                        </asp:DropDownList>
                                        <asp:Label ID="lbldate" runat="server" Text="Date" Style="position: absolute; margin-top: 58px;
                                            margin-left: -47px;"></asp:Label>
                                        <asp:TextBox ID="Txtentryto" runat="server" Height="20px" Width="75px" Visible="true"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute; margin-top: 55px;
                                            margin-left: 0px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                                            FilterType="Custom, Numbers" ValidChars="/" />
                                        <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </fieldset>
                                    <fieldset style="height: 115px; width: 152px; margin-top: -2px; margin-left: -94px;">
                                        <asp:RadioButton ID="rdballdays" runat="server" Text="All Days" AutoPostBack="true"
                                            Checked="true" OnCheckedChanged="rdballdays_changed" Style="position: absolute;
                                            margin-top: -2px; margin-left: -68px;" />
                                        <asp:RadioButton ID="rdbselecteddate" runat="server" Text="Selected Date" AutoPostBack="true"
                                            Checked="false" OnCheckedChanged="rdbselecteddate_changed" Style="position: absolute;
                                            margin-top: 27px; margin-left: -68px;" />
                                        <asp:RadioButton ID="rdbseelctedday" runat="server" Text="Selected Day" AutoPostBack="true"
                                            Checked="false" OnCheckedChanged="rdbseelctedday_changed" Style="position: absolute;
                                            margin-top: 55px; margin-left: -68px;" />
                                        <asp:DropDownList ID="ddlsingleday" runat="server" CssClass="textbox ddlheight2"
                                            Visible="false" Style="position: absolute; margin-top: 90px; margin-left: -45px;">
                                            <asp:ListItem Value="0">Monday </asp:ListItem>
                                            <asp:ListItem Value="1">Tuesday </asp:ListItem>
                                            <asp:ListItem Value="2">Wednesday </asp:ListItem>
                                            <asp:ListItem Value="3">Thursday</asp:ListItem>
                                            <asp:ListItem Value="4">Friday </asp:ListItem>
                                            <asp:ListItem Value="5">Saturday </asp:ListItem>
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset style="height: 30px; width: 395px; position: absolute; margin-top: -129px;
                                        margin-left: 614px;">
                                        <asp:RadioButton ID="rdbstaffcategory" runat="server" Text="By Category" AutoPostBack="true"
                                            Checked="true" OnCheckedChanged="rdbstaffcategory_changed" />
                                        <asp:RadioButton ID="rdbstafftype" runat="server" Text="By Staff Type" AutoPostBack="true"
                                            Checked="false" OnCheckedChanged="rdbstafftype_changed" />
                                    </fieldset>
                                    <fieldset style="height: 300px; width: 445px; position: absolute; margin-top: -77px;
                                        margin-left: 614px;">
                                        <legend>Morning</legend>
                                        <asp:Label ID="lablstaff" runat="server" Text="Staff Category" Style="position: absolute;
                                            margin-top: 9px; margin-left: -95px"></asp:Label>
                                        <asp:DropDownList ID="ddlstaffcategorytype" runat="server" Visible="true" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 9px; margin-left: 40px">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="chk_NightShift" runat="server" Text="Night Shift" Style="position: absolute; margin-top: 9px; margin-left: 215px" />
                                        <asp:Label ID="lblshift" runat="server" Text="Shift:" Style="position: absolute;
                                            margin-top: 50px; margin-left: -95px"></asp:Label>
                                        <asp:Button ID="btn_shift" Text="+" runat="server" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_shift_OnClick" Style="margin-top: 55px; margin-left: -28px;" />
                                        <asp:DropDownList ID="ddlshift" runat="server" Visible="true" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 55px; margin-left: 23px">
                                        </asp:DropDownList>
                                        <asp:Button ID="btn_shiftminus" Text="-" runat="server" CssClass="textbox textbox1 btn1"
                                            OnClick="btn_shiftminus_OnClick" Style="margin-top: 35px; margin-left: 200px;" />
                                        
                                        <asp:Label ID="lblintime" runat="server" Text="In Time:" Style="position: absolute;
                                            margin-top: 110px; margin-left: -349px"></asp:Label>
                                        <cc1:TimeSelector ID="intime" runat="server" Style="position: absolute; margin-top: 110px;
                                            margin-left: -169px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                        <asp:Label ID="lblgracetime" runat="server" Text="Grace Time:" Style="position: absolute;
                                            margin-top: 70px; margin-left: -219px"></asp:Label>
                                        <cc1:TimeSelector ID="gracetime" runat="server" Style="position: absolute; margin-top: 70px;
                                            margin-left: -39px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                        <asp:Label ID="lblexgracetime" runat="server" Text="Extend Grace Time:" Style="position: absolute;
                                            margin-top: 85px; margin-left: -219px"></asp:Label>
                                        <cc1:TimeSelector ID="exgracetime" runat="server" Style="position: absolute; margin-top: 85px;
                                            margin-left: -39px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                        <asp:Label ID="lbllatetime" runat="server" Text="Late Time:" Style="position: absolute;
                                            margin-top: 100px; margin-left: -219px"></asp:Label>
                                        <cc1:TimeSelector ID="latetime" runat="server" Style="position: absolute; margin-top: 100px;
                                            margin-left: -39px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                        <asp:Label ID="lblouttime" runat="server" Text="Out Time:" Style="position: absolute;
                                            margin-top: 115px; margin-left: -219px"></asp:Label>
                                        <cc1:TimeSelector ID="outtimebell" runat="server" Style="position: absolute; margin-top: 115px;
                                            margin-left: -39px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                    </fieldset>
                                    <fieldset style="height: 80px; width: 353px; position: absolute; margin-top: 252px;
                                        margin-left: 614px;">
                                        <legend>Lunch Time</legend>
                                        <asp:Label ID="lblstarttime" runat="server" Text="Start Time" Style="position: absolute;
                                            margin-top: 9px; margin-left: -175px"></asp:Label>
                                        <cc1:TimeSelector ID="starttime" runat="server" Style="position: absolute; margin-top: 9px;
                                            margin-left: 10px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lblendtime" runat="server" Text="End Time" Style="position: absolute;
                                            margin-top: 35px; margin-left: -175px"></asp:Label>
                                        <cc1:TimeSelector ID="endtime" runat="server" Style="position: absolute; margin-top: 35px;
                                            margin-left: 10px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                    </fieldset>
                                    <fieldset style="height: 80px; width: 360px; position: absolute; margin-top: 360px;
                                        margin-left: 614px;">
                                        <legend>Evening</legend>
                                        <asp:Label ID="lblpermission" runat="server" Text="Permission Time" Style="position: absolute;
                                            margin-top: 9px; margin-left: -175px"></asp:Label>
                                        <cc1:TimeSelector ID="permission" runat="server" Style="position: absolute; margin-top: 9px;
                                            margin-left: 10px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lbleveningout" runat="server" Text="Out Time" Style="position: absolute;
                                            margin-top: 35px; margin-left: -175px"></asp:Label>
                                        <cc1:TimeSelector ID="eveningout" runat="server" Style="position: absolute; margin-top: 35px;
                                            margin-left: 10px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        </br>
                                    </fieldset>
                                    <fieldset style="height: 130px; width: 528px; position: absolute; margin-top: 38px;
                                        margin-left: 34px;">
                                        <legend>Default Settings</legend>
                                        <asp:Label ID="lbldefaul1" runat="server" Text="Morning Official OD Time --> From Grace Time to Extend Grace Time"
                                            Style="position: absolute; margin-top: 10px; margin-left: -260px;"></asp:Label>
                                        <asp:Label ID="lbldefaul2" runat="server" Text="Morning Permission Time --> From Extend Grace Time To Late Time"
                                            Style="position: absolute; margin-top: 35px; margin-left: -260px;"></asp:Label>
                                        <asp:Label ID="lbldefaul3" runat="server" Text="Morning Late Time --> From Late Time To Lunch Start Time"
                                            Style="position: absolute; margin-top: 60px; margin-left: -260px;"></asp:Label>
                                        <asp:Label ID="lbldefaul4" runat="server" Text="Evening Permission Time --> From Permission Time To Out Time"
                                            Style="position: absolute; margin-top: 85px; margin-left: -260px;"></asp:Label>
                                    </fieldset>
                                    <fieldset style="height: 100px; width: 528px; position: absolute; margin-top: 195px;
                                        margin-left: 34px;">
                                        <legend>
                                            <asp:CheckBox ID="cbmanuelsetting" runat="server" Checked="false" AutoPostBack="true"
                                                OnCheckedChanged="cbmanuelsetting_checkedchange" />Manual Settings</legend>
                                        <asp:Label ID="lblmrnglate" runat="server" Text="Morning Late Time:" Style="position: absolute;
                                            margin-top: 10px; margin-left: -260px;"></asp:Label>
                                        <asp:DropDownList ID="ddl1" runat="server" Enabled="false" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 10px; margin-left: -65px;">
                                            <asp:ListItem Value="0">Late Time </asp:ListItem>
                                            <asp:ListItem Value="1">Grace Time </asp:ListItem>
                                            <asp:ListItem Value="2">Extend Grace Time </asp:ListItem>
                                            <asp:ListItem Value="3">Late Time </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl2" runat="server" Enabled="false" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 10px; margin-left: 105px;">
                                            <asp:ListItem Value="0">Lunch Start Time </asp:ListItem>
                                            <asp:ListItem Value="1">Grace Time </asp:ListItem>
                                            <asp:ListItem Value="2">Extend Grace Time </asp:ListItem>
                                            <asp:ListItem Value="3">Late Time </asp:ListItem>
                                            <asp:ListItem Value="4">Lunch Start Time </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblmrngpertime" runat="server" Text="Morning Permission Time:" Style="position: absolute;
                                            margin-top: 45px; margin-left: -260px;"></asp:Label>
                                        <asp:DropDownList ID="ddl3" runat="server" Enabled="false" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 45px; margin-left: -65px;">
                                            <asp:ListItem Value="0">Extend Grace Time </asp:ListItem>
                                            <asp:ListItem Value="1">Grace Time </asp:ListItem>
                                            <asp:ListItem Value="3">Extend Grace Time</asp:ListItem>
                                            <asp:ListItem Value="4">Late Time</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl4" runat="server" Enabled="false" CssClass="textbox ddlheight4"
                                            Style="position: absolute; margin-top: 45px; margin-left: 105px;">
                                            <asp:ListItem Value="0">Extend Grace Time </asp:ListItem>
                                            <asp:ListItem Value="1">Grace Time </asp:ListItem>
                                            <asp:ListItem Value="3">Extend Grace Time</asp:ListItem>
                                            <asp:ListItem Value="4">Late Time</asp:ListItem>
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset style="height: 270px; width: 528px; position: absolute; margin-top: 325px;
                                        margin-left: 34px;">
                                        <legend>Per Month</legend>
                                        <asp:Label ID="lblnooflate" runat="server" Text="No Of Late Allowed:" Style="position: absolute;
                                            margin-top: 10px; margin-left: -260px;"></asp:Label>
                                        <asp:TextBox ID="txtnooflate" runat="server" CssClass="textbox ddlheight3" Style="position: absolute;
                                            margin-top: 10px; margin-left: -115px; width: 45px; height: 20px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtnooflate"
                                            FilterType="Numbers" />
                                        <asp:Label ID="lblnoofperallowed" runat="server" Text="No Of Permission Allowed:"
                                            Style="position: absolute; margin-top: 10px; margin-left: -40px;"></asp:Label>
                                        <asp:TextBox ID="txtnoofper" runat="server" CssClass="textbox ddlheight3" Style="position: absolute;
                                            margin-top: 10px; margin-left: 147px; width: 45px; height: 20px;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtnoofper"
                                            FilterType="Numbers" />
                                        <asp:Label ID="lblmarkedas" runat="server" Text="Marked as:" Style="position: absolute;
                                            margin-top: 50px; margin-left: -260px;"></asp:Label>
                                        <asp:DropDownList ID="ddlleavecat" runat="server" CssClass="textbox ddlheight4" Style="position: absolute;
                                            margin-top: 50px; margin-left: -115px; width: 55px; height: 30px;">
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="cbpayprocess" runat="server" Text="Pay Process Date " Checked="false"
                                            Style="position: absolute; margin-top: 50px; margin-left: -40px;" />
                                        <asp:Label ID="lblunregisteredstaff" runat="server" Text="Unregistered Staff Mark As"
                                            Style="position: absolute; margin-top: 95px; margin-left: -260px;"></asp:Label>
                                        <asp:CheckBox ID="cbunregisteredstaff" runat="server" AutoPostBack="true" OnCheckedChanged="cbunregistered_checkedchange"
                                            Checked="false" Style="position: absolute; margin-top: 95px; margin-left: -40px;" />
                                        <asp:DropDownList ID="ddlunregistered" runat="server" Enabled="false" CssClass="textbox ddlheight3"
                                            Style="position: absolute; margin-top: 92px; margin-left: -16px;">
                                        </asp:DropDownList>
                                        <%--      <asp:Label ID="lblleavesettings" runat="server" Text="Leave Settings" Style="position: absolute;
                                            margin-top: 95px; margin-left: 132px;"></asp:Label>--%>
                                        <fieldset style="height: 80px; width: 485px; position: absolute; margin-top: 145px;
                                            margin-left: 0px;">
                                            <asp:Label ID="lblcomworking" runat="server" Text="Compulsory Working" Style="position: absolute;
                                                margin-top: 10px; margin-left: -245px;"></asp:Label>
                                            <asp:CheckBox ID="cbcompworking" runat="server" Checked="false" AutoPostBack="true"
                                                OnCheckedChanged="cbcompworking_checkedchange" Style="position: absolute; margin-top: 10px;
                                                margin-left: -100px;" />
                                            <fieldset style="height: 60px; width: 145px; position: absolute; margin-top: 3px;
                                                margin-left: 175px;">
                                                <asp:RadioButton ID="rdbselecteddatecomp" runat="server" Enabled="false" Text="Selected Date"
                                                    AutoPostBack="true" Checked="false" OnCheckedChanged="rdbselecteddatecomp_changed"
                                                    Style="position: absolute; margin-top: 2px; margin-left: -68px;" />
                                                <asp:RadioButton ID="rdbselecteddaycomp" runat="server" Enabled="false" Text="Selected Day"
                                                    AutoPostBack="true" Checked="false" OnCheckedChanged="rdbselecteddaycomp_changed"
                                                    Style="position: absolute; margin-top: 30px; margin-left: -68px;" />
                                            </fieldset>
                                            <asp:DropDownList ID="ddlcompworking" runat="server" CssClass="textbox ddlheight3"
                                                Style="position: absolute; margin-top: 8px; margin-left: 115px;">
                                                <asp:ListItem Value="0">Select </asp:ListItem>
                                                <asp:ListItem Value="1">Monday </asp:ListItem>
                                                <asp:ListItem Value="2">Tuesday </asp:ListItem>
                                                <asp:ListItem Value="3">Wednesday </asp:ListItem>
                                                <asp:ListItem Value="4">Thursday</asp:ListItem>
                                                <asp:ListItem Value="5">Friday </asp:ListItem>
                                                <asp:ListItem Value="6">Saturday </asp:ListItem>
                                            </asp:DropDownList>
                                        </fieldset>
                                    </fieldset>
                                </div>
                                <asp:UpdatePanel ID="Upsave" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_click" CssClass="textbox textbox1 btn2"
                                            Style="position: absolute; margin-top: 500px; margin-left: 130px;" />
                                        <asp:Button ID="btndel" runat="server" Text="Delete" Visible="false" OnClick="btndel_click"
                                            CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: 500px;
                                            margin-left: 130px;" />
                                        <asp:Button ID="btnupdate" runat="server" Text="Update" Visible="false" OnClick="btnupdate_click"
                                            CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: 500px;
                                            margin-left: 47px;" />
                                        <asp:Button ID="btnexit" runat="server" Text="Exit" OnClick="btnexit_click" CssClass="textbox textbox1 btn2"
                                            Style="position: absolute; margin-top: 500px; margin-left: 230px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="poppe2" runat="server" visible="false" style="height: 70em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 1px; margin-left: 405px;"
                                OnClick="imagebtnpopclose2_Click" />
                            <div style="height: 430px; width: 855px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <div>
                                    <center>
                                        <span style="color: Green; font-size: large;">Hostel Bell Timing</span>
                                    </center>
                                </div>
                                <div>
                                    <fieldset style="height: 300px; width: 320px; position: absolute; margin-top: 40px;
                                        margin-left: 34px;">
                                        <asp:CheckBox ID="cbregisted_single" Visible="false" runat="server" Checked="false"
                                            OnCheckedChanged="cbregisted_single_checkedchange" AutoPostBack="true" Text="Register Single Time"
                                            Style="position: absolute; margin-top: 1px; margin-left: -152px;" />
                                        <asp:Label ID="lblcollegename" runat="server" Text="College Name" Style="position: absolute;
                                            margin-top: 20px; margin-left: -145px;"></asp:Label>
                                        <asp:DropDownList ID="ddlcollegename" CssClass="textbox ddlheight2" runat="server"
                                            Style="position: absolute; margin-top: 20px; margin-left: -40px; width: 200px;">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblhostelname" runat="server" Text="Hostel Name" Style="position: absolute;
                                            margin-top: 59px; margin-left: -145px;"></asp:Label>
                                        <asp:DropDownList ID="ddlhostaelname" CssClass="textbox ddlheight2" OnSelectedIndexChanged="ddlhostaelname_SelectedIndexChanged"
                                            AutoPostBack="true" runat="server" Style="position: absolute; margin-top: 59px;
                                            margin-left: -40px; width: 200px;">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblfromoutTime" runat="server" Text="From Out Time" Style="position: absolute;
                                            margin-top: 97px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hostelfromouttime" runat="server" Style="position: absolute;
                                            margin-top: 97px; margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                            MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lbltoouttime" runat="server" Text="To Out Time" Style="position: absolute;
                                            margin-top: 127px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hosteltoouttime" runat="server" Style="position: absolute;
                                            margin-top: 127px; margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                            MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lblmrnglatetime" runat="server" Text="Morning Late Time" Style="position: absolute;
                                            margin-top: 157px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hostelmrnglatetime" runat="server" Style="position: absolute;
                                            margin-top: 157px; margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                            MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lblfromintime" runat="server" Text="From in Time" Style="position: absolute;
                                            margin-top: 187px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hostelfromintime" runat="server" Style="position: absolute;
                                            margin-top: 187px; margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                            MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lbltointime" runat="server" Text="To in Time" Style="position: absolute;
                                            margin-top: 217px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hosteltointime" runat="server" Style="position: absolute; margin-top: 217px;
                                            margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true" MinuteIncrement="1"
                                            SecondIncrement="1">
                                        </cc1:TimeSelector>
                                        <asp:Label ID="lblevnglatetime" runat="server" Text="Evening Late Time" Style="position: absolute;
                                            margin-top: 247px; margin-left: -145px;"></asp:Label>
                                        <cc1:TimeSelector ID="hosteleveninglate" runat="server" Style="position: absolute;
                                            margin-top: 247px; margin-left: 6px" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                            MinuteIncrement="1" SecondIncrement="1">
                                        </cc1:TimeSelector>
                                    </fieldset>
                                    <fieldset style="height: 100px; width: 400px; position: absolute; margin-top: 31px;
                                        margin-left: 400px;">
                                        <legend>Per Month</legend>
                                        <asp:Label ID="lblnooflatehostel" runat="server" Text="No Of Late allowed" Style="position: absolute;
                                            margin-top: 8px; margin-left: -145px;"></asp:Label>
                                        <asp:TextBox ID="txthostellateallowed" runat="server" CssClass="textbox ddlheight4"
                                            Height="20px" Width="55px" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                                            Style="position: absolute; margin-top: 1px; margin-left: 52px;"></asp:TextBox>
                                        <asp:Label ID="lblnoofperhostel" runat="server" Text="No Of Permission allowed" Visible="false"
                                            Style="position: absolute; margin-top: 32px; margin-left: -145px;"></asp:Label>
                                        <asp:TextBox ID="txthostelperallowed" runat="server" Visible="false" CssClass="textbox ddlheight4"
                                            Height="20px" Width="55px" Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute;
                                            margin-top: 32px; margin-left: 52px;"></asp:TextBox>
                                    </fieldset>
                                    <%--      <fieldset  style="height: 100px; width: 400px; position: absolute; margin-top: 157px;
                                        margin-left: 400px;">--%>
                                    <%--  <legend>--%>
                                    <asp:CheckBox ID="cbhostelManuelsetting" Visible="false" runat="server" Checked="false"
                                        AutoPostBack="true" OnCheckedChanged="cbhostelManuelsetting_checkedchange" />
                                    <%-- Manual Settings</legend>--%>
                                    <asp:Label ID="lblmrnglatehostel" runat="server" Visible="false" Text="Morning Late Time"
                                        Style="position: absolute; margin-top: 8px; margin-left: -208px;"></asp:Label>
                                    <asp:DropDownList ID="ddlmrnglate1" runat="server" Visible="false" CssClass="textbox ddlheight3"
                                        Style="position: absolute; margin-top: 8px; margin-left: -70px;">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlmrnglate2" runat="server" Visible="false" CssClass="textbox ddlheight3"
                                        Style="position: absolute; margin-top: 8px; margin-left: 72px;">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblmrngperhost" runat="server" Visible="false" Text="Morning Permission Time"
                                        Style="position: absolute; margin-top: 50px; margin-left: -208px;"></asp:Label>
                                    <asp:DropDownList ID="ddlper1" runat="server" Visible="false" CssClass="textbox ddlheight3"
                                        Style="position: absolute; margin-top: 50px; margin-left: -70px;">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddlper2" runat="server" Visible="false" CssClass="textbox ddlheight3"
                                        Style="position: absolute; margin-top: 50px; margin-left: 72px;">
                                    </asp:DropDownList>
                                    <%--   </fieldset>--%>
                                    <asp:UpdatePanel ID="Uphostel" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btn_hostelsave" runat="server" Text="Save" OnClick="btn_hostelsave_click"
                                                CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: 300px;
                                                margin-left: 75px;" />
                                            <asp:Button ID="btn_hostelupdate" runat="server" Text="Update" Visible="false" OnClick="btn_hostelupdate_click"
                                                CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: 300px;
                                                margin-left: 75px;" />
                                            <asp:Button ID="btn_hostelexit" runat="server" Text="Exit" OnClick="btn_hostelexit_click"
                                                CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: 300px;
                                                margin-left: 170px;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div id="poppe3" runat="server" visible="false" style="height: 70em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="Image3" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                                width: 30px; position: absolute; margin-top: 18px; margin-left: 329px;" OnClick="Image3_Click" />
                            <br />
                            <div style="height: 400px; width: 710px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <br />
                                <div>
                                    <center>
                                        <span style="color: Green; font-size: large;">Student Bell Timing</span>
                                    </center>
                                </div>
                                <table style="height: 339px;">
                                    <tr>
                                        <td>
                                            <div>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            College
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_popclg1" runat="server" CssClass="textbox1 ddlstyle ddlheight3"
                                                                OnSelectedIndexChanged="ddl_popclg1_change" Width="183px" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbbatchyear" runat="server" Text="Batch Year"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UP_batch" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                        <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                                            OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                                        PopupControlID="panel_batch" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                                        <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                            OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                                        PopupControlID="panel_degree" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                        Width="125px" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="height: 150px;
                                                                        width: 175px">
                                                                        <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                            OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                                        PopupControlID="panel_dept" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Updp_sem" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight1" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="height: 150px;">
                                                                        <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                                            OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                                                        PopupControlID="panel_sem" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td colspan="2">
                                                            Period
                                                            <asp:DropDownList ID="ddlperiod" runat="server" Visible="true" CssClass="textbox ddlheight3"
                                                                Width="82">
                                                            </asp:DropDownList>
                                                            <asp:CheckBox ID="Cbperiod" Visible="true" runat="server" Text="Is Break" Checked="false"
                                                                AutoPostBack="true" OnCheckedChanged="Cbperiod_checkedchange" />
                                                            <asp:Label ID="lblnoofbreak" runat="server" Text="No of Break" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtnoofbreak" runat="server" Width="50px" Visible="false" CssClass="textbox textbox1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <div id="div1" runat="server">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblstarttimestudent" runat="server" Text="Start Time"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <cc1:TimeSelector ID="stustarttime" runat="server" RepeatDirection="Horizondal" AllowSecondEditing="true"
                                                                                MinuteIncrement="1" SecondIncrement="1">
                                                                            </cc1:TimeSelector>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblendtimestud" runat="server" Text="End Time"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <cc1:TimeSelector ID="stuendtime" runat="server" SRepeatDirection="Horizondal" AllowSecondEditing="true"
                                                                                MinuteIncrement="1" SecondIncrement="1">
                                                                            </cc1:TimeSelector>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblstuddesc" runat="server" Text="Description"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtstuddesc" runat="server" Width="200px" CssClass="textbox textbox1"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="btnstudsave" runat="server" Text="Save" OnClick="btnstudsave_click"
                                    CssClass="textbox textbox1
                    btn2" Style="position: absolute; margin-top: -48px; margin-left: -175px;" />
                                <asp:Button ID="btnstudupdate" runat="server" Text="Update" OnClick="btnstudupdate_click"
                                    CssClass="textbox
                    textbox1 btn2" Visible="false" Style="position: absolute; margin-top: -48px; margin-left: -175px;" />
                                <asp:Button ID="bthndeletestud" runat="server" Text="Delete" OnClick="btndeletestud_click"
                                    CssClass="textbox textbox1 btn2" Visible="false" Style="position: absolute; margin-top: -48px;
                                    margin-left: -89px;" />
                                <asp:Button ID="btnstudexit" runat="server" Text="Exit" OnClick="btnstudexit_click"
                                    CssClass="textbox textbox1 btn2" Style="position: absolute; margin-top: -48px;
                                    margin-left: -3px;" />
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdatePanel ID="uppopup" runat="server">
                <ContentTemplate>
                    <div id="Plusapt" runat="server" visible="false" class="popupstyle popupheight1"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                        <center>
                            <div id="Div112" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="line-height: 30px">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="headerapt" runat="server" Text="" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txt_addstream" runat="server" MaxLength="25" CssClass="textbox txtheight2"
                                                    Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="line-height: 35px">
                                                <asp:Button ID="btn_plusAdd" Text=" Add " Visible="false" runat="server" OnClick="btn_plusAdd_OnClick"
                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; border-radius: 6px;" />
                                                <asp:Button ID="btn_Plusexit" Text=" Exit " runat="server" OnClick="btn_Plusexit_OnClick"
                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; border-radius: 6px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <center>
            <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <center>
                        <div style="height: 40px; width: 150px;">
                            <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="ModalPopupExtender12" runat="server" TargetControlID="UpdateProgress12"
                PopupControlID="UpdateProgress12">
            </asp:ModalPopupExtender>
        </center>
    </body>
    </html>
</asp:Content>
