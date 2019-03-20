<%@ Page Title="TMR Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TMR_Report2.aspx.cs" Inherits="TMR_Report2"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .hb
        {
            display: none;
        }
        tfoot
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>TMR Report</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin: 0px; margin-bottom: 10px; width: auto; height: auto; position: relative;">
        <center>
            <asp:Label ID="Label1" runat="server" Text="TMR Report" Font-Bold="True" CssClass="fontstyleheader"
                ForeColor="Green" Style="margin-bottom: 20px; position: relative;"></asp:Label>
            <table class="maintablestyle" style="height: auto; width: auto; background-color: #0CA6CA;
                margin-top: 15px;">
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        CausesValidation="True" Height="21px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CausesValidation="True" Width="81px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                        Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"> </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CausesValidation="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblpages" runat="server" Text="Page" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                        CssClass="style40">
                                    </asp:DropDownList>
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
                                    <asp:Label ID="lblExamMonth" runat="server" Text="Month" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamYear" runat="server" Text="Year" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsubjtype" runat="server" Font-Bold="True" AutoPostBack="true" Font-Names="Book Antiqua"
                                        Text="Subject Type" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:TextBox ID="txtsubjtype" runat="server" Font-Bold="True" AutoPostBack="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="19px" OnTextChanged="txtsubjtype_TextChanged"
                                            Width="105px"></asp:TextBox>
                                        <asp:Panel ID="pnlsubjtype" runat="server" Style="width: 101px; height: 55px;" BackColor="White"
                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:CheckBoxList ID="chksubjtype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="chksubjtype_SelectedIndexChanged"
                                                Width="99px" Height="20px" Style="top: 5px; left: 4px; position: absolute">
                                                <asp:ListItem Value="0">Regular</asp:ListItem>
                                                <asp:ListItem Value="1">Arrear</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <Ajax:DropDownExtender ID="ddesubjtype" runat="server" TargetControlID="txtsubjtype"
                                            DropDownControlID="pnlsubjtype">
                                        </Ajax:DropDownExtender>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblarrear_sem" runat="server" Font-Bold="True" AutoPostBack="true"
                                        Font-Names="Book Antiqua" Text="Arrear Sem" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <div style="position: relative;">
                                        <asp:TextBox ID="txtarrear_sem" runat="server" Font-Bold="True" AutoPostBack="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" Width="106px" OnTextChanged="txtarrear_sem_TextChanged"></asp:TextBox>
                                        <asp:Panel ID="pnlarrear_Sem" runat="server" BorderStyle="Inset" BackColor="White"
                                            Style="width: 94px;">
                                            <asp:CheckBoxList ID="chkarrear_Sem" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="chkarrear_Sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <Ajax:DropDownExtender ID="ddearrear_sem" runat="server" TargetControlID="txtarrear_sem"
                                            DropDownControlID="pnlarrear_sem">
                                        </Ajax:DropDownExtender>
                                    </div>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="True" Height="24px" Width="75px"></asp:TextBox>
                                    <Ajax:CalendarExtender ID="calndr2" runat="server" TargetControlID="txtDate" Format="d/MM/yyyy">
                                    </Ajax:CalendarExtender>
                                </td>
                                <td style="border: 1px solid #008080; color: #000000; padding: 5px;">
                                    <asp:CheckBox ID="chkIncludePrivate" runat="server" Text="For Private" Font-Names="Book Antiqua"
                                        Checked="false" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td style="border: 1px solid #008080; color: #ffffff; padding: 5px;">
                                    <asp:CheckBox ID="chkIncludePassedOut" runat="server" Text="For Passout" AutoPostBack="true"
                                        OnCheckedChanged="chkIncludePassedOut_CheckedChanged" Font-Names="Book Antiqua"
                                        Checked="false" Font-Size="Medium" Font-Bold="True" />
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
                                    <asp:RadioButton ID="rdotmr" runat="server" GroupName="tmr" Text="Semester" Font-Bold="true"
                                        Font-Names="Book Antiqua" Checked="True" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdotmr1" runat="server" GroupName="tmr" Text="Consolidate" Font-Bold="true"
                                        Font-Names="Book Antiqua" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkfailshow" runat="server" Text="Result Showing Fail" Font-Bold="true"
                                        AutoPostBack="true" OnCheckedChanged="chkfailshow_CheckedChanged" Font-Names="Book Antiqua" />
                                </td>
                                <td colspan="2">
                                    <div id="divFailValue" runat="server" visible="false">
                                        <asp:TextBox ID="txtFailValue" runat="server" Font-Bold="true" Text="" Width="30px"></asp:TextBox>
                                    </div>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb1" Text="Format 1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        GroupName="Formate" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb2" Text="Format 2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        GroupName="Formate" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_subjectwisegrade" runat="server" Text="2015 Regulation" Font-Names="Book Antiqua"
                                        AutoPostBack="true" OnCheckedChanged="chk_subjectwisegrade_CheckedChanged" Font-Size="Medium"
                                        Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkgrade" runat="server" Text="Include GPA" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chkgrade_CheckedChanged" />
                                </td>
                                <td>
                                    <div id="divRedo" visible="false" runat="server">
                                        <asp:CheckBox ID="chkRedo" runat="server" Text="Include Redo" Font-Names="Book Antiqua"
                                            Checked="false" Font-Size="Medium" Font-Bold="True" />
                                    </div>
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
                                    <asp:CheckBox ID="chkshowsub_name" runat="server" Text="Subject Name" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chkshowsub_name_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkgender" runat="server" Text="Gender" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chkgender_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowValuationMarks" runat="server" Text="Show Valuation Marks"
                                        Font-Names="Book Antiqua" Checked="false" AutoPostBack="true" OnCheckedChanged="chkShowValuationMarks_CheckedChanged"
                                        Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <div id="divOfficeDeptCopy" runat="server" style="margin: 0px; border: 1px solid #000000;
                                        position: relative;">
                                        <asp:RadioButtonList ID="rblOfficeDeptCopy" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Table" Font-Bold="true" ToolTip="Choose Office Copy or Department Copy">
                                            <asp:ListItem Text="Office Copy" Value="0" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="Department Copy" Value="1" Selected="False"></asp:ListItem>
                                            <asp:ListItem Text="None" Value="2" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeDiscontinue" runat="server" Text="Include Discontinue"
                                        Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
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
                                    <asp:Label ID="lblCollegeHeaderName" runat="server" Text="College Header Name" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCollegeHeader" Text="" Font-Bold="true" Width="200px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowsSectionWise" runat="server" Text="Show By Section Wise"
                                        Font-Names="Book Antiqua" Checked="false" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowNoteDescription" runat="server" Text="Show Note Description"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrderby" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px">
                                        <asp:ListItem>Order by Arrear Subject</asp:ListItem>
                                        <asp:ListItem>Order by Regular Subject</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="12" style="margin: 0px; border: 0px; padding: 0px; width: auto;">
                        <table style="margin: 0px; border: 0px; padding: 0px; width: auto;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkIncludeNotRegistered" runat="server" Text="Include Not Registered"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeRedoSuspended" runat="server" Text="Include Redo/Suspended"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeFeesNotPaid" runat="server" Text="Include Fees Not Paid"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeMalPractice" runat="server" Text="Include Malpractice"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIncludeLackOfAttendance" runat="server" Text="Include Lack of Attendance"
                                        Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
        <div style="margin-top: 10px; width: auto; height: auto; position: relative;">
            <center>
                <asp:Panel ID="pnlHeaderFilter" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                    Height="22px" Width="850px" Style="margin-top: 20px; position: relative;">
                    <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                    <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
                </asp:Panel>
            </center>
            <center>
                <asp:Panel ID="pnlColumnOrder" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                    CssClass="table2" Width="850px" Style="margin-top: 5px; margin-bottom: 25px;
                    position: relative;">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkColumnOrderAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkColumnOrderAll_CheckedChanged" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnRemoveAll" runat="server" Font-Size="X-Small" Height="16px"
                                    Style="font-family: 'Book Antiqua'; color: #ffffff; font-weight: 700; font-size: small;
                                    margin-left: -599px;" Visible="false" Width="111px" OnClick="lbtnRemoveAll_Click">Remove All</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                                <asp:TextBox ID="txtOrder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="noresize"
                                    AutoPostBack="true" runat="server" Enabled="false">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblColumnOrder" runat="server" Height="43px" AutoPostBack="true"
                                    Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                    RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblColumnOrder_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">S.No</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">Roll No</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="True">Register No</asp:ListItem>
                                    <asp:ListItem Value="3" Selected="True">Student Name</asp:ListItem>
                                    <asp:ListItem Value="4" Selected="True">Student Type</asp:ListItem>
                                    <asp:ListItem Value="5" Selected="False" Enabled="false">Gender</asp:ListItem>
                                    <asp:ListItem Value="6" Selected="True">Subject Code</asp:ListItem>
                                    <asp:ListItem Value="7" Selected="True">Internal Mark</asp:ListItem>
                                    <asp:ListItem Value="8" Selected="True">External Mark</asp:ListItem>
                                    <asp:ListItem Value="9" Selected="True">Total</asp:ListItem>
                                    <asp:ListItem Value="10" Selected="False" Enabled="false">Grade</asp:ListItem>
                                    <asp:ListItem Value="11" Selected="True">Result</asp:ListItem>
                                    <asp:ListItem Value="12" Selected="True">Year of Passing</asp:ListItem>
                                    <asp:ListItem Value="13" Selected="False" Enabled="false">GPA</asp:ListItem>
                                    <asp:ListItem Value="14" Selected="False" Enabled="false">CGPA</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlColumnOrder"
                CollapseControlID="pnlHeaderFilter" ExpandControlID="pnlHeaderFilter" Collapsed="true"
                TextLabelID="lblFilter" CollapsedSize="0" ImageControlID="imgFilter" CollapsedImage="~/images/right.jpeg"
                ExpandedImage="~/images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <asp:Panel ID="pnlrecordcount" runat="server" Style="margin-top: 5px; margin-bottom: 25px;
                position: relative;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="24px" Width="58px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                                Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="16px" Width="32px"></asp:TextBox>
                            <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <asp:Label ID="lblstudselect" runat="server" Font-Bold="True" Visible="False" Width="449px"
                                Font-Names="Book Antiqua" ForeColor="#FF3300" Text="Please Select Atleast One Student To Print The GradeSheet"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="There is no record found" Visible="False"></asp:Label>
            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
            <br />
            <center>
                <asp:Panel ID="pnlContents" runat="server" Style="width: 100%;">
                    <style type="text/css" media="print">
                        @page
                        {
                            size: A3 portrait;
                            margin: 0.5cm;
                        }
                        
                        @media print
                        {
                            thead
                            {
                                top: 10%;
                                left: 0%;
                                height: 20%;
                                display: table-header-group;
                            }
                            tfoot
                            {
                                height: 20%;
                                bottom: 20%;
                                left: 0px;
                                display: table-footer-group;
                            }
                            tbody
                            {
                                display: table-row-group;
                                height: 50%;
                            }
                            #header
                            {
                                position: fixed;
                                top: 10%;
                                left: 0%;
                                height: 20%;
                            }
                            #footer
                            {
                                position: fixed;
                                bottom: 20%;
                                height: 20%;
                                left: 0%;
                            }
                            #printable
                            {
                                position: relative;
                                bottom: 20%;
                                top: 20%;
                                height: 50%;
                            }
                        
                        }
                        @media screen
                        {
                            thead
                            {
                                display: block;
                            }
                            tfoot
                            {
                                display: block;
                            }
                            tbody
                            {
                                display: table-row-group;
                            }
                        }
                    </style>
                    <div id="printable" style="width: 100%; height: auto;">
                        <table style="width: 100%;">
                            <thead style="width: 100%; padding: 0px;">
                                <tr class="hb">
                                    <td rowspan="4" style="margin: 0px; padding: 0px; width: 70px;">
                                        <img id="imgLeftLogo" runat="server" alt="" style="width: 60px; height: 60px; margin: 0px;
                                            padding: 0px;" src="~/college/Left_Logo.jpeg" />
                                    </td>
                                    <td colspan="5" align="center">
                                        <span id="spnCollegeHeader" runat="server" style="font-weight: bold; font-size: large;">
                                        </span>
                                    </td>
                                </tr>
                                <tr class="hb">
                                    <td colspan="5" align="center">
                                        <span id="spnOfficeController" runat="server" style="font-weight: bold; font-size: medium;">
                                            Office of the Controller of Examinations </span>
                                    </td>
                                </tr>
                                <tr class="hb">
                                    <td colspan="5" align="center">
                                        <span id="spnExamYearMonth" runat="server" style="font-weight: bold; font-size: medium;">
                                        </span>
                                    </td>
                                </tr>
                                <tr class="hb">
                                    <td colspan="5" align="center">
                                        <span id="spnDegreeDetails" runat="server" style="font-weight: bold; font-size: medium;">
                                        </span>
                                    </td>
                                </tr>
                                <tr class="hb">
                                    <td colspan="5" align="left">
                                        <span id="spnSemester" runat="server" style="font-weight: bold; font-size: medium;">
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <FarPoint:FpSpread ID="FpExternalHeader" runat="server" Style="border: 1px solid black;
                                            font-family: Book Antiqua; font-size: large; font-weight: bold; width: 100%;"
                                            BorderWidth="3px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
                                            <CommandBar ShowPDFButton="false" ButtonType="PushButton" Visible="true">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="White" BackColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </thead>
                            <tbody style="width: 100%; margin: 0px; padding: 0px;">
                                <tr>
                                    <td colspan="6" align="center">
                                        <FarPoint:FpSpread ID="FpExternal" runat="server" Style="border: 3px solid black;
                                            border-bottom-color: transparent; margin: 0px; margin-top: -9px; width: 100%;"
                                            HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
                                            <CommandBar ShowPDFButton="false" ButtonType="PushButton" Visible="true">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" AllowSort="true" GridLineColor="White" BackColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </tbody>
                            <tfoot style="width: 100%; margin: 0px; padding: 0px;">
                                <tr>
                                    <td colspan="6" align="center">
                                        <div id="divFooterResult" runat="server" style="margin: 0px; padding: 0px; height: auto;
                                            width: 100%; font-weight: bold;">
                                        </div>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlSpread" runat="server" Visible="false" Style="margin-top: 5px;
                    margin-bottom: 10px;">
                    <table>
                        <tr>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnExcel_Click" Visible="false" Width="120px" Text="Export to Excel" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                <asp:Button ID="btnPrint" runat="server" CssClass="fontblack" Text="Direct Print"
                                    OnClientClick="return PrintPanel();" />
                                <style>
                                    .fontblack
                                    {
                                        font-family: Book Antiqua;
                                        font-size: medium;
                                        font-weight: bold;
                                        color: Black;
                                    }
                                </style>
                                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
        </div>
    </div>
</asp:Content>
