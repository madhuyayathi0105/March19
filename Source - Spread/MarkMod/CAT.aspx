<%@ Page Title="CAM R6-CAT Report" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAT.aspx.cs" Inherits="CAT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerr').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">CAM R3-CAT Report</span>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td>
                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px;"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                        Style="height: 24px; width: 56px;" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 56px">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Style="margin-left: 0px;"
                        Width="93px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 56px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style="width: 288px;"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 20px; width: 33px"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 48px;" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                        Style="width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblCriteria" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text=" Criteria" Style="width: 31px; height: 23px;">
                    </asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtdropdownlist" runat="server" Style="height: 19px; width: 140px"
                        OnTextChanged="txtdropdownlist_TextChanged"></asp:TextBox>
                    <asp:Panel ID="pnlChk" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="2px" Style="font-family: 'Book Antiqua'; width: 155px; height: 395px;">
                        <asp:CheckBox ID="chkselectall" runat="server" OnCheckedChanged="chkselectall_CheckedChanged"
                            Text="Select All" Style="overflow: auto; font-family: 'Book Antiqua'; width: 96px;
                            height: 20px; background-color: White" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklist" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Small" AutoPostBack="True" OnSelectedIndexChanged="chklist_SelectedIndexChanged1">
                            <asp:ListItem Value="0"> Rank</asp:ListItem>
                            <asp:ListItem Value="1">NoofHrAttended</asp:ListItem>
                            <asp:ListItem Value="2">Attendance %</asp:ListItem>
                            <asp:ListItem Value="3">Present</asp:ListItem>
                            <asp:ListItem Value="4">Absent</asp:ListItem>
                            <asp:ListItem Value="5">Pass</asp:ListItem>
                            <asp:ListItem Value="6">Fail</asp:ListItem>
                            <asp:ListItem Value="7">Average</asp:ListItem>
                            <asp:ListItem Value="8">Highest Mark</asp:ListItem>
                            <asp:ListItem Value="9">Lowest Mark</asp:ListItem>
                            <asp:ListItem Value="10">Pass Percentage</asp:ListItem>
                            <asp:ListItem Value="11">Staff Signature</asp:ListItem>
                            <asp:ListItem Value="12">Date Of Exam</asp:ListItem>
                            <asp:ListItem Value="13">Date Of Submission</asp:ListItem>
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:DropDownExtender ID="DropDownExtender1" runat="server" TargetControlID="txtdropdownlist"
                        DropDownControlID="pnlChk">
                    </asp:DropDownExtender>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text=" Test" Style="width: 31px; height: 23px;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                    Style="width: 186px; height: 21px;" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                                <asp:Label ID="lblEtest" runat="server" ForeColor="Red" Text="Select Test" Visible="False"
                                    Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 82px; right: 909px;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="20px" Width="75px" Style="" OnTextChanged="txtFromDate_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 76px">
                                </asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="80px" OnTextChanged="txtToDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblMrkconversion" runat="server" Text="MarkConversion Value" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 20px; width: 175px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMarkconversion" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 18px; width: 43px;">
                                </asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="markfilter" runat="server" FilterType="Numbers"
                                    TargetControlID="txtMarkconversion">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lblEmrkconversion" runat="server" ForeColor="Red" Visible="False"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Style="text-align: center;" Text="Go" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" OnClick="btnGo_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIncludeAbsent" Checked="false" runat="server" Text="Include Absent in Pass Pecentage"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerroe" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="#FF3300" Text="" Style="margin: 0px; margin-bottom: 10px;
        margin-top: 10px;" Visible="False"></asp:Label>
    <center>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <asp:Panel ID="pnlforrecord" runat="server">
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Style="height: 18px; width: 180px">
                        </asp:Label>
                        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px; width: 126px"></asp:Label>
                        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="24px" Width="58px" Style="">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Width="34px" AutoPostBack="True"
                            OnTextChanged="TextBoxother_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 14px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                            FilterType="Numbers" />
                        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px"></asp:Label>
                        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="width: 34px; height: 14px;"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                            FilterType="Numbers" />
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 21px; width: 303px"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" GroupName="header"
                        Text="Header in All Pages" OnCheckedChanged="RadioHeader_CheckedChanged" />
                    <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header"
                        Text="Header in 1st Page" OnCheckedChanged="Radiowithoutheader_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <center>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <FarPoint:FpSpread ID="FpEntry" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Width="1000px">
                                    <CommandBar>
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" AutoPostBack="false" GridLineColor="White">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </center>
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <center>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                        <asp:Button ID="Button1" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </td>
            </tr>
            <tr>
                <td style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <asp:Label ID="lblerr" runat="server" Text="" ForeColor="Red" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
