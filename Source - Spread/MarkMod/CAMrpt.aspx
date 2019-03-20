<%@ Page Title="CAM R5-CAM REPORT" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CAMrpt.aspx.cs" Inherits="CAMrpt" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style43
        {
            top: 250px;
            left: 20px;
            position: absolute;
            height: 19px;
            width: 168px;
        }
        .style44
        {
            top: 251px;
            left: 310px;
            position: absolute;
            height: 21px;
            width: 126px;
        }
        .style45
        {
            top: 250px;
            left: 449px;
            position: absolute;
            height: 22px;
            width: 55px;
        }
        .style46
        {
            top: 250px;
            left: 516px;
            position: absolute;
        }
        .style47
        {
            top: 250px;
            left: 570px;
            position: absolute;
            height: 21px;
        }
        .style48
        {
            top: 250px;
            left: 672px;
            position: absolute;
            width: 34px;
        }
        .style49
        {
            top: 228px;
            left: 553px;
            position: absolute;
            height: 21px;
            width: 303px;
        }
        .style50
        {
            top: 283px;
            left: 20px;
            position: absolute;
            height: 21px;
            width: 329px;
        }
        .style51
        {
            top: 230px;
            left: -4px;
            position: absolute;
            width: 1169px;
        }
        .style52
        {
            height: 73px;
            width: 1017px;
        }
        .style53
        {
            width: 10px;
        }
        .style54
        {
            width: 179px;
            height: 21px;
            position: absolute;
            left: 790px;
            top: 204px;
        }
        .style55
        {
            margin-left: -341px;
            margin-top: -39px;
            position: absolute;
            height: 21px;
            width: 76px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">CAM R2-CAM REPORT</span>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            padding: 5px;" width="950px">
            <tr>
                <td>
                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Height="21px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                        Width="71px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 56px">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="93px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 56px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style="width: 288px;"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 48px;" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Style="height: 21px; width: 26px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                        Style="width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 15px; width: 46px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            ReadOnly="true" Font-Size="Medium" OnTextChanged="TextBox1_TextChanged" CssClass="Dropdown_Txt_Box"
                                            Style="height: auto; width: 125px;"></asp:TextBox>
                                        <asp:Panel ID="pnlCustomers" runat="server" CssClass="multxtpanel" Height="400px">
                                            <asp:CheckBox ID="SelectAll" runat="server" AutoPostBack="True" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="SelectAll_CheckedChanged"
                                                Text="Select All" />
                                            <asp:CheckBoxList ID="ddlreport" runat="server" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged"
                                                AutoPostBack="true" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                                <asp:ListItem Value="0">Rank</asp:ListItem>
                                                <asp:ListItem Value="1">Medium</asp:ListItem>
                                                <asp:ListItem Value="2">12th Info</asp:ListItem>
                                                <asp:ListItem Value="3">CGPA</asp:ListItem>
                                                <asp:ListItem Value="4">Class Strength</asp:ListItem>
                                                <asp:ListItem Value="5">Students Appeared</asp:ListItem>
                                                <asp:ListItem Value="6">Students Absent</asp:ListItem>
                                                <asp:ListItem Value="7">Students Passed</asp:ListItem>
                                                <asp:ListItem Value="8">Students Failed</asp:ListItem>
                                                <asp:ListItem Value="9">Average(&lt;50)</asp:ListItem>
                                                <asp:ListItem Value="10">Average(50to65)</asp:ListItem>
                                                <asp:ListItem Value="11">Average(&gt;60)</asp:ListItem>
                                                <asp:ListItem Value="12">Class Average</asp:ListItem>
                                                <asp:ListItem Value="13">Class Max-Min Mark</asp:ListItem>
                                                <asp:ListItem Value="14">Pass Percentage</asp:ListItem>
                                                <asp:ListItem Value="15">Staff Name</asp:ListItem>
                                                <asp:ListItem Value="16">DayScholarPass</asp:ListItem>
                                                <asp:ListItem Value="17">HostlerPass</asp:ListItem>
                                                <asp:ListItem Value="18">TamMediumPass</asp:ListItem>
                                                <asp:ListItem Value="19">EngMediumPass</asp:ListItem>
                                                <asp:ListItem Value="20">Gender</asp:ListItem>
                                                <asp:ListItem Value="21">GirlsPass</asp:ListItem>
                                                <asp:ListItem Value="22">BoysPass</asp:ListItem>
                                                <asp:ListItem Value="23">Quota</asp:ListItem>
                                                <asp:ListItem Value="24">NFPS</asp:ListItem>
                                                <asp:ListItem Value="25">NoOfHrAttended</asp:ListItem>
                                                <asp:ListItem Value="26">Attendance %</asp:ListItem>
                                                <asp:ListItem Value="27">Average(&gt;=75) %</asp:ListItem>
                                                <asp:ListItem Value="28">Average(60to74)</asp:ListItem>
                                                <asp:ListItem Value="29">Average(50to59)</asp:ListItem>
                                                <asp:ListItem Value="30">Average(30to49)</asp:ListItem>
                                                <asp:ListItem Value="31">Average(20to29)</asp:ListItem>
                                                <asp:ListItem Value="32">Average(&lt;=19)</asp:ListItem>
                                                <asp:ListItem Value="33">Maxmark rollno</asp:ListItem>
                                                <asp:ListItem Value="34">Exam Date</asp:ListItem>
                                                <asp:ListItem Value="35">Conducted Hours</asp:ListItem>
                                                <asp:ListItem Value="36">Subjects Failed</asp:ListItem>
                                                <asp:ListItem Value="37">Average(&gt;65)</asp:ListItem>
                                                <asp:ListItem Value="38">Average(&gt;80)</asp:ListItem>
                                                <asp:ListItem Value="39">No of all Cleared</asp:ListItem>
                                                <asp:ListItem Value="40">% of all Cleared</asp:ListItem>
                                                <asp:ListItem Value="41">Grade</asp:ListItem>
                                                <asp:ListItem Value="42">No of Failures</asp:ListItem>
                                                <asp:ListItem Value="43">No of Sub Ab</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="TextBox1"
                                            PopupControlID="pnlCustomers" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="75px" Style="height: 17px;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="80px" Style="height: 17px;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text=" Test" Style="width: 31px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged1"
                                    Style="width: 90px; height: auto;" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIncludeAbsent" Checked="false" runat="server" Text="Include Absent in Pass Pecentage"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbladminpass" runat="server" Text="Optional Min Pass Mark" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="height: 15px;" Width="185px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtoptiminpassmark" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="3" Style="height: 15px; width: 45px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtoptiminpassmark"
                                    FilterType="Numbers" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chart" runat="server" Text="Chart" Width="80px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" />
                            </td>
                            <td>
                                <asp:RadioButtonList ID="RadioBtnlist_sub" runat="server" CellSpacing="0" AutoPostBack="false"
                                    RepeatDirection="Horizontal" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-weight: bold;
                                    float: left;" Font-Bold="True">
                                    <asp:ListItem Value="1">Subject code</asp:ListItem>
                                    <asp:ListItem Value="2">Subject Name</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="width: auto; height: auto;"
                                    Text="Go" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" />
                            </td>
                            <td colspan="4">
                                <fieldset style="border: 1px solid #000; height: auto; width: auto; margin: 0px;
                                    padding: 0px;">
                                    <legend>Criteria For Mark</legend>
                                    <table style="height: auto; width: auto; margin: 0px; padding: 0px;">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_pass" runat="server" Text="Pass" AutoPostBack="true" OnCheckedChanged="RadioButtonList3_SelectedIndexChanged"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_fail" runat="server" Text="Fail" AutoPostBack="true" OnCheckedChanged="RadioButtonList3_SelectedIndexChanged"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chk_abst" runat="server" Text="Absent" AutoPostBack="true" OnCheckedChanged="RadioButtonList3_SelectedIndexChanged"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                            <td>
                            <asp:CheckBox ID="cbincludepassout" runat="server" Text="Include Passed Out" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="cbincludepassout_OnCheckedChanged" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td>
                    <asp:RadioButton ID="RadioHeader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in All Pages" />
                    <asp:RadioButton ID="Radiowithoutheader" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header" Text="Header in 1st Page" />
                    <asp:Label ID="lbltesterr" runat="server" Font-Bold="True" Font-Size="Medium" Visible="false"
                        Font-Names="Book Antiqua" ForeColor="Red" CssClass="style54">Please Select The Test</asp:Label>
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
                    <asp:Label ID="lblcharterr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="#FF3300" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
            CssClass="style43">
        </asp:Label>
        <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44"></asp:Label>
        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
            Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
            CssClass="style45">
        </asp:DropDownList>
        <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
            AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style46"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
            FilterType="Numbers" />
        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
            Width="96px" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style47"></asp:Label>
        <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
            OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="17px" CssClass="style48"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
            FilterType="Numbers" />
        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style49"></asp:Label>
    </center>
    <div style="width: 100%; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
    
    <asp:GridView ID="gview" runat="server" AutoGenerateColumns="true" Font-Names="Book Antique" ShowFooter="false" ShowHeader="false"
    BorderStyle="Double" OnRowDataBound="gviewOnRowDataBound" CssClass="grid-view" GridLines="Both">
    <Columns>
    </Columns>
    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
    <FooterStyle BackColor="White" ForeColor="#333333" />
    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
    <RowStyle ForeColor="#333333" />
    <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
    </asp:GridView>
    </div>
    <center>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
             <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </center>
    <center>
        <div id="dvconsolidated" runat="server" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; text-align: center;">
            <center>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <tr>
                        <td align="center">
                            <asp:GridView ID="GridViewselectedfield" runat="server" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px;" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Chart ID="Chart1" runat="server" Width="800px" Visible="false" Font-Names="Book Antiqua"
                                EnableViewState="true" Font-Size="Medium" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px;">
                                <Series>
                                </Series>
                                <Legends>
                                    <asp:Legend Title="Staff Performance" ShadowOffset="3" Font="Book Antiqua">
                                    </asp:Legend>
                                </Legends>
                                <Titles>
                                    <asp:Title Docking="Bottom" Text="Marks">
                                    </asp:Title>
                                    <asp:Title Docking="Left" Text="PASS %">
                                    </asp:Title>
                                </Titles>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                        <AxisY LineColor="White">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisY>
                                        <AxisX LineColor="White">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                            <MajorGrid LineColor="#e6e6e6" />
                                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:GridView ID="GridViewchart" runat="server" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px;" Width="645px" Visible="false" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Button ID="btnExcelchart" runat="server" Text="Export Excel" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" OnClick="btnExcelchart_Click" Visible="false" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrintchart" runat="server" Text="Print" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" OnClick="btnPrintchart_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </center>
</asp:Content>
