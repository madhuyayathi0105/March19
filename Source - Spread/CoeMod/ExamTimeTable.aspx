<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="ExamTimeTable.aspx.cs" Inherits="ExamTimeTable" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
    </style><br />
<center>
    <asp:Label ID="Label7" runat="server" Text="Exam Time Table Generation" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
 <br />
    <asp:Label ID="errmsg" runat="server" CssClass="font" ForeColor="Red"></asp:Label>
<center>
    <asp:Panel ID="pnlvisible" runat="server" style="width:900px; height:350px; background-color:#0CA6CA;">
        <fieldset style="width: 870px; height: 122px">
            <legend id="Legend1" class="font" runat="server" style="height: 10px">Exam Time Table
                Generate Setting</legend>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblMonthOfExam" runat="server" Text="Month" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server" CssClass="font" Width="88px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label13" runat="server" Text="Year" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="font" Width="85px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
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
                        <asp:Label ID="Label1" runat="server" Text="No Of Students" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtnoofstudent" runat="server" CssClass="font" Width="60px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblExamStartDate" runat="server" Text="Start Date" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtexamstartdate" runat="server" CssClass="font" AutoPostBack="True"
                            OnTextChanged="txtExamFinishDate_TextChanged" Width="83px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblExamEndDate" runat="server" Text="Finish Date" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtExamFinishDate" runat="server" CssClass="font" Width="83px" AutoPostBack="True"
                            OnTextChanged="txtExamFinishDate_TextChanged"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblHolidays" runat="server" Text="Holidays" CssClass="font"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtHolidays" runat="server" CssClass="font" Width="83px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnsetting" runat="server" OnClick="btnsettingclick" Text="Day and Session Setting"
                            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                    </td>
                    <td>
                        <asp:Button ID="btnsubject" runat="server" OnClick="btnsubjectclick" Text="Subject Priority"
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
    <asp:Panel ID="pnlHolidays" runat="server" BackColor="White" BorderWidth="1px" BorderColor="Black">
        <asp:CheckBoxList ID="cblHolidays" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblHolidays_SelectedIndexChanged">
        </asp:CheckBoxList>
    </asp:Panel>
    <asp:CalendarExtender ID="cetxtexamstartdate" runat="server" TargetControlID="txtexamstartdate"
        Format="dd-MM-yyyy">
    </asp:CalendarExtender>
    <asp:CalendarExtender ID="cetxtExamFinishDate" runat="server" TargetControlID="txtExamFinishDate"
        Format="dd-MM-yyyy">
    </asp:CalendarExtender>
    <asp:DropDownExtender ID="detxtHolidays" runat="server" TargetControlID="txtHolidays"
        DropDownControlID="pnlHolidays">
    </asp:DropDownExtender>
    <table>
        <tr>
            <td align="center" rowspan="2" style="width: 800px;">
                <asp:Label ID="lblerror" runat="server" ForeColor="Red" CssClass="font"></asp:Label>
            </td>
        </tr>
        <tr>
        </tr>
    </table>
    <asp:Panel ID="panel6" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
        BorderWidth="2px" Style="left: 129px; top: 90px; position: absolute;">
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
                            <asp:Button ID="Button2" runat="server" Text="Close" ForeColor="Black" OnClick="btn_click"
                                Style="top: 0px; left: 628px; position: absolute; height: 26px; border-width: 0;
                                background-color: AliceBlue;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                Font-Size="Medium" />
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
    <asp:Panel ID="panel4" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
        BorderWidth="2px" Style="left: 223px; top: 146px; height: 424px; width: 600px;
        position: absolute;">
        <asp:Button ID="Button5" runat="server" Text="Close" ForeColor="Black" OnClick="btnc_click"
            Style="top: 0px; left: 524px; position: absolute; height: 26px; border-width: 0;
            background-color: AliceBlue;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
            Font-Size="Medium" />
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
            </tr>
            <tr>
                <td>
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
    <asp:Panel ID="BatchSetPanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
        Visible="false" BorderWidth="2px" Style="left: 223px; top: 146px;
        width: 600px; position: absolute;">
        <asp:Button ID="btnbatchsetclose" runat="server" Text="Close" ForeColor="Black" OnClick="btnbatchsetclose_click"
            Style="top: 0px; left: 524px; position: absolute; height: 26px; border-width: 0;
            background-color: AliceBlue;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
            Font-Size="Medium" />
        <br />
        <asp:Label ID="Label14" runat="server" Text="Education" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:Label>
        <asp:DropDownList ID="ddlbatchedu" runat="server" Width="100px" AutoPostBack="true"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbatchedu_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <FarPoint:FpSpread ID="FpBatchSetting" runat="server" BorderColor="Black" BorderStyle="Solid"
            BorderWidth="0.5" AutoPostBack="true" OnButtonCommand="FpBatchSetting_command">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <asp:Button ID="btnbatchsetsave" runat="server" Text="Save" ForeColor="Black" OnClick="btnbatchsetsave_click"
            Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
        <br />
        <asp:Label ID="lblbatcherror" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red"></asp:Label>
    </asp:Panel>
</asp:Content>

