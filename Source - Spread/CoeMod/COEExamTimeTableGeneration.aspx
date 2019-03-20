<%@ Page Title="Exam Time Table Generation - A" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COEExamTimeTableGeneration.aspx.cs" Inherits="CoeMod_COEExamTimeTableGeneration"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ss" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Exam Time Table Generation - A</span>
        <div class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative; width: 850px; padding: 15px;">
            <center>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblExamType" runat="server" Text="Exam Type" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamType" runat="server" CssClass="font">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExmType" runat="server" CssClass="font">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnView" runat="server" Text="View" CssClass="font" OnClick="btnView_Click" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <asp:Label ID="errmsg" runat="server" CssClass="font" ForeColor="Red" Style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
        <asp:Panel ID="pnlvisible" runat="server">
            <fieldset style="width: 870px; height: auto;">
                <legend id="Legend1" class="font" runat="server" style="height: 10px;">Exam</legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblExamStartDate" runat="server" Text="Start Date" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtexamstartdate" runat="server" CssClass="font" AutoPostBack="True"
                                    OnTextChanged="txtExamFinishDate_TextChanged" Width="83px"></asp:TextBox>
                                <asp:CalendarExtender ID="cetxtexamstartdate" runat="server" TargetControlID="txtexamstartdate"
                                    Format="dd-MM-yyyy">
                                </asp:CalendarExtender>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblExamEndDate" runat="server" Text="Finish Date" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtExamFinishDate" runat="server" CssClass="font" Width="83px" AutoPostBack="True"
                                    OnTextChanged="txtExamFinishDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="cetxtExamFinishDate" runat="server" TargetControlID="txtExamFinishDate"
                                    Format="dd-MM-yyyy">
                                </asp:CalendarExtender>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblHolidays" runat="server" Text="Holidays" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtHolidays" runat="server" CssClass="font" Width="83px"></asp:TextBox>
                                <asp:Panel ID="pnlHolidays" runat="server" BackColor="White" BorderWidth="1px" BorderColor="Black">
                                    <asp:CheckBoxList ID="cblHolidays" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblHolidays_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="detxtHolidays" runat="server" TargetControlID="txtHolidays"
                                    DropDownControlID="pnlHolidays">
                                </asp:DropDownExtender>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="No Of Students" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtnoofstudent" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Mode" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlexammode" runat="server" CssClass="font">
                                <asp:ListItem Text="ODD"></asp:ListItem>
                                <asp:ListItem Text="EVEN"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblStartsWith" runat="server" Text="Exam starts with" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamstartswith" runat="server" CssClass="font" Width="85px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlExamstartswith_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamStartType" runat="server" CssClass="font">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSession" runat="server" Text="Session" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:TextBox ID="txtSession" runat="server" CssClass="font" Width="83px"></asp:TextBox>
                                <asp:Panel ID="pnlSession" runat="server" CssClass="font" Height="135px" ScrollBars="Auto"
                                    BackColor="White" BorderColor="Black" BorderWidth="1px">
                                    <asp:CheckBoxList ID="clSession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="clSession_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="detxtSession" runat="server" TargetControlID="txtSession"
                                    DropDownControlID="pnlSession">
                                </asp:DropDownExtender>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMonthOfExam" runat="server" Text="Month and Year" CssClass="font"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Width="88px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Width="85px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="0">
                                        <asp:Label ID="lblSubType" runat="server" CssClass="commonHeaderFont" Text="Exclude Subject Type in Time Table"
                                            AssociatedControlID="txtSubType"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSubType" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlSubType" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                                        Width="280px" ScrollBars="Auto">
                                                        <asp:CheckBox ID="chkSubtype" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chkSubtype_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSubtype" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblSubtype_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtSubtype" runat="server" TargetControlID="txtSubType"
                                                        PopupControlID="pnlSubtype" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                     <td colspan="14">
                                         <asp:CheckBox ID="settbase" runat="server" Text="settings based" AutoPostBack="true" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnsetting" runat="server" OnClick="btnsettingclick" Text="Setting"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnsubject" runat="server" OnClick="btnsubjectclick" Text="Subject"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnbatchset" runat="server" OnClick="btnbatchset_Click" Text="Batch Year Setting"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btngen" runat="server" OnClick="btngenerateclick" Text="Generate"
                                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset id="Fieldset1" runat="server" style="width: 870px; height: 80px">
                <legend class="font" style="height: 10px">Theory </legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblStartTimeam" runat="server" Text="Exam Start Time (Am)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltheoryStartTimeamHrs" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltheoryStartTimeamHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltheoryStartTimeamMin" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltheoryStartTimeamMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblExamEndTime" runat="server" Text="Exam End Time (Am/Pm)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltheoryendtimeamHrs" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltheoryendtimeamHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltheoryendtimeamMin" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltheoryendtimeamMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltheoryendtimeampm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddltheoryendtimeampm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTheoryDurationam" runat="server" Text="Duration" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTheoryDurationam" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTheoryStartTimepm" runat="server" Text="Exam Start Time (Pm)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTheoryStartTimeHrsPm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTheoryStartTimeHrsPm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTheoryStartTimeMinPm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTheoryStartTimeMinPm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTheoryEndTimepm" runat="server" Text="Exam End Time (Pm)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTheoryEndTimeHrsPm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTheoryEndTimeHrsPm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTheoryEndTimeMinPm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTheoryEndTimeMinPm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTheoryendtimePmam" runat="server" CssClass="font" Width="50px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTheoryDurationpm" runat="server" Text="Duration" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTheoryDurationpm" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset style="width: 870px; height: 80px">
                <legend class="font" style="height: 10px">Practical </legend>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblpracexamtimestartam" runat="server" Text="Exam Start Time (Am)"
                                CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracstarttimeamHrs" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracstarttimeamHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracstarttimeamMin" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracstarttimeamMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblpracexamtimeendam" runat="server" Text="Exam End Time (Am/Pm)"
                                CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimeamHrs" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracendtimeamHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimeamMin" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracendtimeamMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimeAmPm" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracendtimeAmPm_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblpracdurationam" runat="server" Text="Duration" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpracdurationam" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblpracstarttimepm" runat="server" Text="Exam Start Time (Pm)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracstarttimePmHrs" runat="server" CssClass="font" AutoPostBack="true"
                                Width="50px" OnSelectedIndexChanged="ddlpracstarttimePmHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracstarttimePmMin" runat="server" CssClass="font" AutoPostBack="true"
                                Width="50px" OnSelectedIndexChanged="ddlpracstarttimePmMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblpracendtimepm" runat="server" Text="Exam End Time (Pm)" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimePmHrs" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracendtimePmHrs_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimePmMin" runat="server" CssClass="font" Width="50px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlpracendtimePmMin_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlpracendtimePmAm" runat="server" CssClass="font" Width="50px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblpracdurationpm" runat="server" Text="Duration" CssClass="font"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpracdurationpm" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9" align="center">
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </center>
    <table>
        <tr>
            <td align="center" rowspan="2" style="width: 800px;">
                <asp:Label ID="lblerror" runat="server" ForeColor="Red" CssClass="font"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="font" OnClick="btnGenerate_Click" />
            </td>
        </tr>
    </table>
    <%--    </ContentTemplate>
    </asp:UpdatePanel>--%>
    <center>
        <div id="panel6" runat="server" visible="false" style="height: 100em; z-index: 100;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <asp:Panel ID="pnlTTSettings" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="true" BorderWidth="2px" Style="margin-top: 90px; width: 850px;">
                    <asp:UpdatePanel ID="upd1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltype" runat="server" Text="Mode" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddltype" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                            <asp:ListItem Value="0">ODD</asp:ListItem>
                                            <asp:ListItem Value="1">EVEN</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="Label5" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddltp" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="X" ForeColor="Black" OnClick="btn_click"
                                            Style="height: 26px; border-width: 0; background-color: AliceBlue; width: 25px;"
                                            Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="0.5" AutoPostBack="true" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" SelectionBackColor="CadetBlue">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                                <tr colspan="3" align="center">
                                    <td>
                                        <asp:Button ID="Butsave" runat="server" OnClick="btnsaveclick" Text="Save" Font-Bold="true"
                                            Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                </tr>
                            </table>
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="LightYellow"
                                            Visible="false" BorderWidth="1px" Height="108px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Width="74px"></asp:Label>
                                                        <asp:DropDownList ID="ddlyears" runat="server" Width="100px" AutoPostBack="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="arriercheck" runat="server" Text="Arrears" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Education" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:DropDownList ID="ddledu" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddledusel">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label10" runat="server" Text="Copy From" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:DropDownList ID="ddlcpfrom" runat="server" Width="100px" AutoPostBack="true"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label11" runat="server" Text="Session" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:DropDownList ID="ddlsess" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">
                                                            <asp:ListItem Value="1">FN</asp:ListItem>
                                                            <asp:ListItem Value="2">AN</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="Label12" runat="server" Text="Copy To" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                        <asp:TextBox ID="txt_copy" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                            Width="140px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                                        <asp:Panel ID="phostel" runat="server" CssClass="MultipleSelectionDDL" Height="150px">
                                                            <asp:CheckBox ID="checkcopyto" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="checkcopyto_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cheklist_copyto" runat="server" Font-Size="Medium" Font-Bold="True"
                                                                Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="cheklist_copyto_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_copy"
                                                            PopupControlID="phostel" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="Button6" runat="server" OnClick="btnremove" Text="Remove" Font-Bold="true"
                                                            Font-Size="Medium" Font-Names="Book Antiqua" />
                                                        <asp:Button ID="btnk" runat="server" OnClick="btnkclick" Text="Ok" Font-Bold="true"
                                                            Font-Size="Medium" Font-Names="Book Antiqua" />
                                                        <asp:TextBox ID="txtvl" runat="server" ReadOnly="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                                                        <asp:Button ID="Button7" runat="server" OnClick="btnCopyclick" Text="Copy" Font-Bold="true"
                                                            Font-Size="Medium" Font-Names="Book Antiqua" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </center>
        </div>
    </center>
    <center>
        <div id="panel4" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <asp:Panel ID="pnlSubTypePriority" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="true" BorderWidth="2px" Style="height: 424px; width: 579px; margin-top: 146px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Mode" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddlmd" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlmd_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:Label ID="Label8" runat="server" Text="Education" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddleduc" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddleduselectvl">
                                </asp:DropDownList>
                                <asp:Label ID="Label6" runat="server" Text="Year" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="36px"></asp:Label>
                                <asp:DropDownList ID="dddyr" runat="server" Width="100px" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                                <asp:Button ID="Button1" runat="server" OnClick="btnokclick" Text="Ok" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" />
                            </td>
                            <td>
                                <asp:Button ID="Button5" runat="server" Text="X" ForeColor="Black" OnClick="btnc_click"
                                    Style="height: 26px; border-width: 0; background-color: AliceBlue; width: 25px;"
                                    Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="0.5" AutoPostBack="true" OnButtonCommand="FpSpread2_command" Visible="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Visible="false" Font-Size="Medium" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr align="right">
                            <td>
                                <asp:Button ID="Button3" runat="server" OnClick="btnreset" Text="Reset" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" Visible="false" />
                                <asp:Button ID="Button4" runat="server" OnClick="btnset" Text="Set" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
        </div>
    </center>
    <center>
        <div id="BatchSetPanel" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <asp:Panel ID="pnlBatchSettings" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="true" BorderWidth="2px" Style="margin-top: 146px; width: 546px; height: auto;">
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button ID="btnbatchsetclose" runat="server" Text="Close" ForeColor="Black" OnClick="btnbatchsetclose_click"
                                    Style="border-width: 0; background-color: AliceBlue;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                    Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Education" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlbatchedu" runat="server" Width="100px" AutoPostBack="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbatchedu_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <FarPoint:FpSpread ID="FpBatchSetting" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="0.5" AutoPostBack="true" OnButtonCommand="FpBatchSetting_command">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button ID="btnbatchsetsave" runat="server" Text="Save" ForeColor="Black" OnClick="btnbatchsetsave_click"
                                    Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblbatcherror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
        </div>
    </center>
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
</asp:Content>
