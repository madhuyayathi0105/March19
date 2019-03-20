<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newstaf.aspx.cs" Inherits="newstaf" EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function reason() {
            document.getElementById('<%=btnaddreason.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnremovereason.ClientID%>').style.display = 'block';
        }
    </script>
    <style type="text/css">
        .floats
        {
            float: right;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .cur
        {
            cursor: pointer;
        }
        .label
        {
            font-family: Book Antiqua;
            font-size: 15px;
            font-weight: bold;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox")
                var o = window.event.srcElement;
            {
                __doPostBack("", "");
            }
        }
        function callme() {
            var i = val.lastIndexOf("\\");
            return val.substring(i + 1);
        }
        function callme(oFile) {
            document.getElementById("TextBox1").value = oFile.value;
        }
    </script>
    <style type="text/css" media="screen">
        .floats
        {
            height: 26px;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label1" runat="server" Text="Attendance" class="fontstyleheader" Font-Bold="True"
            Font-Names="Book Antiqua" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px;
            position: relative;"></asp:Label>
    </center>
    <asp:UpdatePanel ID="updattendance" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="updattendance">
                <ProgressTemplate>
                    <div class="CenterPB" style="height: 40px; width: 40px;">
                        <img src="../images/progress2.gif" height="180px" width="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <center>
                <table class="maintablestyle" style="margin:0px; margin-top: 10px; margin-bottom: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="scodelbl" runat="server" Text="Staff Code" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="scodetxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="26px" AutoPostBack="True" OnSelectedIndexChanged="scodetxt_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="26px" AutoPostBack="True" OnSelectedIndexChanged="ddlstaffname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Labelfdate" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbfdate" runat="server" OnTextChanged="tbfdate_TextChanged" Height="19px"
                                Width="91px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbfdate" Format="d-MM-yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" runat="server"
                            ControlToValidate="tbfdate" ErrorMessage="Select From Date" ForeColor="Red" Width="110px"></asp:RequiredFieldValidator>
                        <td>
                            <asp:Label ID="Labeltodate" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbtodate" runat="server" OnTextChanged="tbtodate_TextChanged" Height="18px"
                                Width="80px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d-MM-yyyy" TargetControlID="tbtodate"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server"
                            ControlToValidate="tbtodate" ErrorMessage="Select From Date" ForeColor="Red"
                            Width="110px"></asp:RequiredFieldValidator>
                        <td>
                            <asp:Button ID="Buttongo" runat="server" Text="Go" OnClick="Buttongo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:CheckBox ID="ck_append" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Text="Append Periods" />
                            <asp:CheckBox ID="chkis_studavailable" runat="server" Visible="false" Text="" />
                        </td>
                        <td>
                            <asp:Button ID="btnsliplist" runat="server" Text="Slip List" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" OnClick="btnsliplist_Click" />
                        </td>
                    </tr>
                </table>
                <table style="margin: 0px; margin-bottom: 10px; position: relative;">
                    <tr>
                        <td>
                            <asp:Label ID="snamelbl1" runat="server" Text="Staff Name:" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="snamelbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
            <asp:Label ID="Labelstaf" runat="server" ForeColor="Red" Text="There is no class for the staff between the given date"
                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-top: 20px; margin-bottom: 15px; position: relative;"></asp:Label>
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                CssClass="cur" BorderWidth="1px" Height="200" Width="400" OnPreRender="FpSpread1_SelectedIndexChanged"
                OnCellClick="FpSpread1_CellClick" ShowHeaderSelection="false" Style="margin: 0px;
                margin-top: 20px; margin-bottom: 15px; position: relative;">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <asp:Label ID="lbl_alert" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                ForeColor="Red" Text="You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator"
                Visible="False"></asp:Label>
            <asp:Panel ID="Panel2" Visible="false" runat="server">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                    Visible="false" Height="19px" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem Value="1" Text="Rollno"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Regno"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Admission no"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RadioButtonList ID="option" RepeatDirection="Horizontal" runat="server" Height="19px"
                    Width="191px" Visible="False">
                    <asp:ListItem Value="1" Text="General"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Individual"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel ID="Panel4" Visible="false" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblselected" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For the Selected Student" Width="182px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmark" runat="server" OnSelectedIndexChanged="ddlmark_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr">
                            </asp:DropDownList>
                            <asp:Label ID="lblmarkabs" runat="server" Text="Select" Visible="false" ForeColor="Red"
                                Style="font-weight: 400"></asp:Label><asp:Label ID="Label10" runat="server" Text="Should not be same as Rest of the students"
                                    Visible="false" ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblreg" runat="server" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Text="Enter The Roll No" Width="180px" Visible="false"></asp:Label>
                            <asp:Label ID="lblroll" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="Enter The Reg No" Visible="false" Width="180px" CssClass="style109"></asp:Label>
                            <asp:Label ID="lblad" runat="server" Visible="false" Style="font-family: 'Baskerville Old Face'"
                                Text="Enter Admission No" Width="180px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <br />
                            <br />
                            <br />
                            <asp:TextBox ID="txtregno" runat="server" Height="21px" Width="97px" CssClass="style109"
                                onKeyPress="return alpha(event)" AutoPostBack="True" OnTextChanged="txtregno_TextChanged"></asp:TextBox>
                            <asp:TextBox ID="txtrunning" runat="server" Height="21px" onKeyPress="return alpha1(event)"
                                Visible="false" Width="335px" CssClass="style109" AutoPostBack="True" OnTextChanged="txtrunning_TextChanged"></asp:TextBox>
                            &nbsp;<asp:Label ID="lblstate" runat="server" ForeColor="#996633" Style="font-weight: 700"
                                Text="Static Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblrun" runat="server" ForeColor="#996633"
                                Style="font-weight: 700" Text="Running Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblrunerror" runat="server" ForeColor="Red" Text="Enter Running Part"
                                Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblinvalidreg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <br />
                            &nbsp;<asp:Label ID="lblregno" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btngoindividual" runat="server" CssClass="cursorptr" Height="29px"
                                OnClick="btngoindividual_Click" Text="GO" Width="59px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style110">
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblindisave" runat="server" Text="Saved Successfully" Visible="false"
                                ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblrest" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For Rest of the Students" Width="181px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmarkothers" runat="server" OnSelectedIndexChanged="ddlmarkothers_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr">
                            </asp:DropDownList>
                            <asp:Label ID="markdiff" runat="server" Text="Should not be same as Selected students"
                                Visible="false" ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pHeaderatendence" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="Labelatend" runat="server" Text="Mark Attendance" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="ImageSel" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodyatendence" runat="server" CssClass="cpBody">
                <asp:Label ID="lbldayorder" runat="server" Text="day" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Large" ForeColor="#3333CC" Visible="False"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbgraphics" runat="server" Text="Graphical Display" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="attendance" AutoPostBack="true"
                                OnCheckedChanged="rbgraphics_checkchange" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbappenses" runat="server" Text="Absent Entry" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="attendance" AutoPostBack="true"
                                OnCheckedChanged="rbappenses_checkchange" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblreason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnaddreason" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" OnClick="btnaddreason_Click" Style="display: none;" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="25px" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnremovereason" runat="server" Text="-" OnClick="btnremovereason_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="panel1" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Height="125px" Width="690px">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Add Reason
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblatreason" runat="server" Text="Add Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtreason" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnreasonnew" runat="server" Text="Add" OnClick="btnreasonnew_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:Button ID="btnreasonexit" runat="server" Text="Exit" OnClick="btnreasonexit_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <br />
                <div style="margin-left: 0px">
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" CssClass="cur"
                                    BorderStyle="Solid" OnUpdateCommand="FpSpread2_UpdateCommand" BorderWidth="1px"
                                    Width="400" Visible="False" VerticalScrollBarPolicy="AsNeeded" HorizontalScrollBarPolicy="AsNeeded"
                                    ShowHeaderSelection="false" Style="height: auto;">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
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
                <div id="divatt" runat="server" style="margin-left: 450px">
                    <table id="tableat" style="text-align: center">
                        <tr>
                            <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@PRABHA On 13/6/12@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                            <td>
                                <asp:Label ID="lblmanysubject" runat="server" Text="Select Class For Below Details"
                                    Font-Size="Medium" Width="225px" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlselectmanysub" runat="server" AutoPostBack="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="23px" OnSelectedIndexChanged="ddlselectmanysub_SelectedIndexChanged"
                                    Width="166px">
                                </asp:DropDownList>
                            </td>
                            <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                            <td>
                                <asp:CheckBox ID="check_attendance" runat="server" Text="Copy Attendance" Font-Size="small"
                                    Font-Names="Book Antiqua" Font-Bold="true" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonselectall" Visible="false" CssClass="floats" Font-Bold="true"
                                    runat="server" Text="Select All" OnClick="Buttonselectall_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Buttondeselect" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="De-Select All" OnClick="Buttondeselect_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonsave" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="Save" OnClick="Buttonsave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonupdate" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="Update" OnClick="Buttonupdate_Click" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonexit" runat="server" Font-Bold="true" Visible="false" CssClass="floats"
                                    Text="Exit" OnClick="Buttonexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <fieldset id="fieldat" runat="server" style="width: 300px; height: 430px">
                    <table style="text-align: right;">
                        <tr>
                            <td>
                                <asp:Label ID="lblatdate" runat="server" Text="Date :" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcurdate" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhour" Text="Hour(s) :" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhrvalue" Text="" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblattend" runat="server" Text="Selected Students :" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlattend" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnaddrow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Add Row" OnClick="btnaddrow_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="fpattendanceentry" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Height="100" Width="600" Enabled="False" ShowHeaderSelection="false">
                                    <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" Visible="true">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblreststudent" runat="server" Text="For The Rest Of Students" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreststudent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblerrmsg" runat="server" Font-Bold="true" ForeColor="Red" CssClass="floats"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnaddattendance" runat="server" Text="Save" OnClick="btnaddattendance_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 550px;
                                    position: absolute;" />
                    </table>
                    </td></tr>
                </fieldset>
                </div>
                <asp:CollapsiblePanelExtender ID="cpeatend" runat="server" TargetControlID="pBodyatendence"
                    CollapseControlID="pHeaderatendence" ExpandControlID="pHeaderatendence" Collapsed="true"
                    TextLabelID="Labelatend" CollapsedSize="0" ImageControlID="ImageSel" CollapsedImage="../images/right.jpeg"
                    ExpandedImage="../images/down.jpeg">
                </asp:CollapsiblePanelExtender>
            </asp:Panel>
            <br />
            <asp:Panel ID="pHeaderlesson" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="Labellesson" runat="server" Text="Daily Entry" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodylesson" runat="server" CssClass="cpBody">
                <asp:Label ID="Labellvalid" Visible="False" runat="server" Text="Label" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panelcomplete" Visible="false" runat="server" Height="338px" Width="312px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Labelc" runat="server" Text="Topics Completed" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvcomplete" ForeColor="Blue" runat="server" BorderWidth="0px" SelectedNodeStyle-ForeColor="Red"
                                    ShowCheckBoxes="Leaf" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <HoverNodeStyle ForeColor="Black" />
                                    <Nodes>
                                        <asp:TreeNode Expanded="True" SelectAction="Expand"></asp:TreeNode>
                                    </Nodes>
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="Panelyet" Visible="false" runat="server" Height="335px" Width="312px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Label2" runat="server" Text="Topics Yet To Complete" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvyet" runat="server" ShowCheckBoxes="Leaf" BorderWidth="0px" OnTreeNodeCheckChanged="OnTreeNodeCheckChanged"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnTreeNodeExpanded="OnTreeNodeCheckChanged"
                                    OnSelectedNodeChanged="tvyet_SelectedNodeChanged1">
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="Plessionalter" Visible="true" runat="server" Height="335px" Width="300px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Label5" runat="server" Text="Previous Days Lession Plan Topics" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvalterlession" runat="server" ShowCheckBoxes="Leaf" BorderWidth="0px"
                                    OnTreeNodeCheckChanged="OnTreeNodeCheckChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnTreeNodeExpanded="OnTreeNodeCheckChanged" OnSelectedNodeChanged="tvyet_SelectedNodeChanged1">
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="850px">
                    <tr>
                        <td>
                            <asp:Button ID="btndailyentrydelete" runat="server" Visible="true" Text="Delete"
                                Font-Bold="true" OnClick="btndailyentrydelete_Click" />
                            <asp:Button ID="Buttonexitlesson" runat="server" Visible="false" CssClass="floats"
                                Text="Exit" Font-Bold="true" OnClick="Buttonexitlesson_Click" />
                            <asp:Button ID="Buttonsavelesson" Visible="false" runat="server" CssClass="floats"
                                Font-Bold="true" Text="Save" OnClick="Buttonsavelesson_Click" />
                            <asp:CheckBox ID="chkalterlession" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Previous Days Lession Plan Topics" Font-Bold="true"
                                OnCheckedChanged="chkalterlession_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <asp:CollapsiblePanelExtender ID="cpelesson" runat="server" TargetControlID="pBodylesson"
                    CollapseControlID="pHeaderlesson" ExpandControlID="pHeaderlesson" Collapsed="true"
                    TextLabelID="Labellesson" CollapsedSize="0" ImageControlID="Image1" CollapsedImage="../images/right.jpeg"
                    ExpandedImage="../images/down.jpeg">
                </asp:CollapsiblePanelExtender>
            </asp:Panel>
            <br />
            <asp:Panel ID="headerpanelnotes" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblnotes" runat="server" Text="Notes" Font-Size="Medium" Font-Names="Book Antiqua" />
                <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodynotes" runat="server" CssClass="cpBody">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Font-Bold="true" Text="Add Notes" CssClass="floats"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnSave_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnnotesdelete" runat="server" Font-Bold="true" Text="Delete" CssClass="floats"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnnotesdelete_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblerror" runat="server" Text="" Visible="false" ForeColor="Red" CssClass="floats"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="fpspread3panel">
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    OnPreRender="fpspread3prerender" OnCellClick="fpspread3_click" BorderWidth="1px"
                                    Height="200" Width="540" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="Never"
                                    ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpenotes" runat="server" TargetControlID="pBodynotes"
                CollapseControlID="headerpanelnotes" ExpandControlID="headerpanelnotes" Collapsed="true"
                TextLabelID="lblnotes" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <br />
            <asp:Panel ID="headerADDQuestion" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblADDquestion" runat="server" Text="Add Question" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodyaddquestion" runat="server" CssClass="cpBody">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblunits" runat="server" Text="Units" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlunits" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblquestion1" runat="server" Text="Question" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtquestion1" runat="server" Font-Bold="True" Width="400px" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblMarks" runat="server" Text="Marks" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlgivemarks" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="40px">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnaddquestion" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnaddquestion_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnupdatequetion" runat="server" Text="Update" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnupdatequetion_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btndeleteatndqtn" runat="server" Text="Delete" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btndeleteatndqtn_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblerrorquestionadd_att" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="spreadttqtnadd">
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="spreadatt_qtnadd" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    OnCellClick="spreadatt_qtnadd_cellclick" OnPreRender="spreadatt_qtnadd_SelectedChange"
                                    BorderWidth="1px" Height="200" Width="600" HorizontalScrollBarPolicy="Never"
                                    VerticalScrollBarPolicy="Never" ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pBodyaddquestion"
                CollapseControlID="headerADDQuestion" ExpandControlID="headerADDQuestion" Collapsed="true"
                TextLabelID="lblADDquestion" CollapsedSize="0" ImageControlID="Image3" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <br />
            <asp:Panel ID="headerquestionaddition" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblquestionaddition" runat="server" Text="Objective Type" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                &nbsp; &nbsp;
                <asp:RadioButton ID="RadioSubject" runat="server" GroupName="Question" AutoPostBack="true"
                    Text="Subject" CssClass="label" OnCheckedChanged="RadioSubject_CheckedChanged"
                    Checked="true" />
                <asp:RadioButton ID="RadioGeneral" runat="server" GroupName="Question" AutoPostBack="true"
                    Text="General" CssClass="label" OnCheckedChanged="RadioGeneral_CheckedChanged" />
            </asp:Panel>
            <asp:Panel ID="pBodyquestionaddition" runat="server" CssClass="cpBody" BorderColor="Gray"
                BackImageUrl="~/StudentImage/Box.jpg" BorderWidth="2px" Height="300px" Width="1000px">
                <asp:Panel ID="paneltoaddquestion" runat="server">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblnoofanswers" runat="server" Text="No of Answers" CssClass="label"></asp:Label>
                                <asp:DropDownList ID="ddlnoofanswers" runat="server" CssClass="font" AutoPostBack="true "
                                    Width="40px" OnSelectedIndexChanged="ddlnoofanswers_SelectedIndexchanged">
                                    <asp:ListItem Value="A" Text=""></asp:ListItem>
                                    <asp:ListItem Value="B">2</asp:ListItem>
                                    <asp:ListItem Value="C">3</asp:ListItem>
                                    <asp:ListItem Value="D">4</asp:ListItem>
                                    <asp:ListItem Value="E">5</asp:ListItem>
                                    <asp:ListItem Value="F">6</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblqtnName" runat="server" CssClass="label" Text="Question Name"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtqtnname" runat="server" CssClass="font" Width="900px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAnswers" runat="server" CssClass="label" Text="Answers"></asp:Label>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                &nbsp; &nbsp;
                                <asp:Label ID="lblcorrectans" runat="server" CssClass="label" Text="Check Correct Answers(any one)"></asp:Label>
                                <asp:Label ID="lblcompulsarymark" runat="server" ForeColor="Red" CssClass="label"
                                    Text="*"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="sprdnoofchoices" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Height="100px" Width="420px" OnUpdateCommand="sprdnoofchoices_UpdateCommand"
                                    ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbltoughness" runat="server" Text="Toughness" CssClass="label"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="radiotough1" runat="server" GroupName="tough" CssClass="label"
                                    Text="Easy" Font-Bold="True" Checked="true" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough2" runat="server" GroupName="tough" CssClass="label"
                                    Text="Medium" Font-Bold="True" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough3" runat="server" GroupName="tough" CssClass="label"
                                    Text="Difficult" Font-Bold="True" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough4" runat="server" GroupName="tough" CssClass="label"
                                    Text="Very Difficult" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelsavecommands" runat="server">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <table id="Table3" runat="server" align="center">
                        <tr>
                            <td>
                                <asp:Button ID="btnqtnsave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtnsave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnqtnupdate" runat="server" Text="Update" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtnupdate_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnNew" runat="server" Text="New" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnNew_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnqtndelete" runat="server" Text="Delete" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtndelete_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lblnorec" runat="server" Text="No records Found" Visible="false" CssClass="label"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="sprdviewdatapanel" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <FarPoint:FpSpread ID="sprdviewdata" Width="600" Height="200" BorderWidth="0px" runat="server"
                                        OnCellClick="sprdviewdata_cellclick" OnPreRender="sprdviewdata_call" ShowHeaderSelection="false">
                                        <CommandBar ButtonShadowColor="ControlDark" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            BackColor="Control" ButtonType="PushButton">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pBodyquestionaddition"
                CollapseControlID="headerquestionaddition" ExpandControlID="headerquestionaddition"
                Collapsed="true" TextLabelID="lblquestionaddition" CollapsedSize="0" ImageControlID="Image4"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <div>
                <asp:Panel ID="pnl_sliplist" runat="server" BackColor="AliceBlue" Height="200" Width="670"
                    Style="top: 492px; left: 190px; position: absolute; height: 200px; width: 600"
                    BorderColor="Black" BorderStyle="Double">
                    <table>
                        <tr>
                            <td class="style4" colspan="2">
                                <center>
                                    <asp:Label ID="headlbl_sl" runat="server" Text="Pending Slip List" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style2">
                                <center>
                                    <FarPoint:FpSpread ID="spread_sliplist" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Height="200" Width="600" CommandBar-Visible="false" ShowHeaderSelection="false">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread></center>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                            </td>
                            <td>
                                <asp:Button ID="exit_sliplist" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="exit_sliplist_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <%-- Confirmation --%>
            <center>
                <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divConfirm" runat="server" class="table" style="background-color: White;
                            height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: auto; width: 100%; padding: 3px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnYes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnNo_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%-- Alert Box --%>
            <center>
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="btnnotesdelete" />
            <asp:PostBackTrigger ControlID="FpSpread3" />
            <asp:PostBackTrigger ControlID="tbtodate" />
            <asp:PostBackTrigger ControlID="tbfdate" />
            <asp:PostBackTrigger ControlID="Buttongo" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel ID="pnotesuploadadd" runat="server" BorderColor="Black" BackColor="AliceBlue"
        BorderWidth="2px" Style="left: 150px; top: 350px; position: absolute;" Height="200px"
        Width="691px">
        <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: Book Antiqua;
            font-size: medium; font-weight: bold">
            <br />
            <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                left: 200px">
                Notes Upload
            </caption>
            <br />
            <br />
            <table style="text-align: left">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Select Class For Below Details" Font-Size="Medium"
                            Width="225px" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlclassnotes" runat="server" AutoPostBack="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Height="23px" OnSelectedIndexChanged="ddlselectmanysub_SelectedIndexChanged"
                            Width="166px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:FileUpload ID="fileupload" runat="server" onchange="callme(this)" />
                    </td>
                    <td>
                        <asp:Button ID="btnaddnotes" runat="server" Font-Bold="true" Text="Save" CssClass="floats"
                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnaddnotes_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnclosenotes" runat="server" Font-Bold="true" Text="Exit" CssClass="floats"
                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnclosenotes_Click" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text=" " CssClass="font" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
