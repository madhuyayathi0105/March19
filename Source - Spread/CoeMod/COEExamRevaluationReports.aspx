<%@ Page Title="Exam Revaluation Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="COEExamRevaluationReports.aspx.cs" Inherits="CoeMod_COEExamRevaluationReports"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
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
        function display1() {
            document.getElementById('<%#lblExcelErr.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Exam Revaluation Report</title>');
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
    <center>
        <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
            margin-bottom: 15px; margin-top: 10px; position: relative;">Exam Revaluation Report
        </span>
        <table class="maintablestyle" style="width: auto; height: auto; background-color: #0CA6CA;
            padding: 5px; margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <div id="divCollege" style="position: relative;">
                                    <asp:UpdatePanel ID="upnlCollege" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtCollege" Visible="false" Width="90px" runat="server" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlCollege" Visible="false" runat="server" CssClass="multxtpanel"
                                                Style="width: 330px; height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                                <asp:CheckBox ID="chkCollege" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                    margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkCollege_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblCollege" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                    padding: 0px; border: 0px;" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popExtCollege" runat="server" TargetControlID="txtCollege"
                                                PopupControlID="pnlCollege" Position="Bottom">
                                            </asp:PopupControlExtender>
                                            <asp:DropDownList ID="ddlCollege" Visible="true" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged" Width="255px" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
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
                                <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"> </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
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
                                <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <div id="divSec" style="position: relative;">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSec" Visible="true" Width="70px" runat="server" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlSec" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                                <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                    margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSec_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                    padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popExtSec" runat="server" TargetControlID="txtSec"
                                                PopupControlID="pnlSec" Position="Bottom">
                                            </asp:PopupControlExtender>
                                            <asp:DropDownList ID="ddlSec" Visible="false" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExamYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlExamMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblDate" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Width="76px"></asp:TextBox>
                                <asp:CalendarExtender ID="calExtDate" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowsSectionWise" runat="server" Text="Show By Section Wise"
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
                                <asp:CheckBox ID="chkShowFailResults" runat="server" Text="Result Showing Fail Also"
                                    Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkShowFailResults_CheckedChanged"
                                    Font-Names="Book Antiqua" />
                            </td>
                            <td>
                                <div id="divFailValue" runat="server" visible="false" style="margin: 0px; border: 0px;
                                    padding-right: 6px; padding-left: 5px;">
                                    <asp:TextBox ID="txtFailValue" runat="server" Font-Bold="true" Text="" Width="35px"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div id="divOfficeDeptCopy" runat="server" style="margin: 0px; border: 1px solid #000000;
                                    position: relative; padding-left: 5px; padding-right: 6px;">
                                    <asp:RadioButtonList ID="rblOfficeDeptCopy" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Table" Font-Bold="true" ToolTip="Choose Office Copy or Department Copy">
                                        <asp:ListItem Text="None" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Office Copy" Value="1" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Department Copy" Value="2" Selected="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOrderby" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="80px">
                                    <asp:ListItem Selected="True" Text="Order by Arrear Subject" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Order by Regular Subject" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                            <asp:RadioButtonList ID="rblformat" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Font-Bold="true" AutoPostBack="true"  OnSelectedIndexChanged="rblformat_SelectedIndexChanged">
                            <asp:ListItem Text="Format1" Value="0" Selected="True"></asp:ListItem>
                             <asp:ListItem Text="Format2" Value="1" Selected="False"></asp:ListItem>
                            </asp:RadioButtonList>
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
                                <asp:CheckBox ID="chkShowNoteDescription" runat="server" Text="Show Note Description"
                                    Font-Names="Book Antiqua" Checked="true" Font-Size="Medium" Font-Bold="True" />
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Panel ID="pnlHeaderFilter" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Height="22px" Width="850px" Style="margin-top: 20px; position: relative;" Visible="false">
            <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                Font-Names="Book Antiqua" Style="margin-left: 0%;" Visible="false" />
            <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
        </asp:Panel>
        <asp:Panel ID="pnlColumnOrder" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            CssClass="table2" Width="850px" Style="margin-top: 5px; margin-bottom: 25px;
            position: relative;" Visible="false">
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
                            <asp:ListItem Value="5" Selected="True">Gender</asp:ListItem>
                            <asp:ListItem Value="6" Selected="True">Subject Code</asp:ListItem>
                            <asp:ListItem Value="7" Selected="True">Internal Mark</asp:ListItem>
                            <asp:ListItem Value="8" Selected="True">Actual External Mark</asp:ListItem>
                            <asp:ListItem Value="9" Selected="True">Revaluation_1</asp:ListItem>
                            <asp:ListItem Value="10" Selected="True">Revaluation_2</asp:ListItem>
                            <asp:ListItem Value="11" Selected="True">Revaluation_3</asp:ListItem>
                            <asp:ListItem Value="12" Selected="True">Final External Mark</asp:ListItem>
                            <asp:ListItem Value="13" Selected="True">Total</asp:ListItem>
                            <asp:ListItem Value="14" Selected="True">Grade</asp:ListItem>
                            <asp:ListItem Value="15" Selected="True">Result</asp:ListItem>
                            <asp:ListItem Value="16" Selected="True">Year of Passing</asp:ListItem>
                            <%--<asp:ListItem Value="13" Selected="True">GPA</asp:ListItem>
                            <asp:ListItem Value="14" Selected="True">CGPA</asp:ListItem>--%>
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
    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 15px; margin-top: 10px; position: relative;"></asp:Label>
    <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 15px;
        margin-top: 10px; position: relative;">
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
                                <td rowspan="3" style="margin: 0px; padding: 0px; width: 70px;">
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
                            <%-- <tr class="hb">
                                <td colspan="5" align="center">
                                    <span id="spnDegreeDetails" runat="server" style="font-weight: bold; font-size: medium;">
                                    </span>
                                </td>
                            </tr>--%>
                            <tr class="hb">
                                <td colspan="5" align="center">
                                    <span id="spnDegreeDetails" runat="server" style="font-weight: bold; font-size: medium;">
                                    </span>
                                </td>
                               <%-- <td colspan="3" align="right">
                                    <span id="spnSemester" runat="server" style="font-weight: bold; font-size: medium;">
                                    </span>
                                </td>--%>
                            </tr>
                            <tr class="hb">
                             <td colspan="3" align="left">
                                    <span id="spnSemester" runat="server" style="font-weight: bold; font-size: medium;">
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <FarPoint:FpSpread ID="FpRevaluationHeader" Visible="false" runat="server" Style="border: 1px solid black;
                                        font-family: Book Antiqua; font-size: large; font-weight: bold; width: auto;
                                        height: auto;" BorderWidth="3px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
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
                                    <FarPoint:FpSpread ID="FpRevaluation" runat="server" Style="border: 3px solid black;
                                        border-bottom-color: transparent; margin: 0px; margin-top: -9px; width: 100%;"
                                        HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
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
            <div id="divPrint1" runat="server" style="margin: 0px; margin-top: 20px;">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblExcelErr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblExcelReportName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExcelName" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtExcelName"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\@#$%^&*()-=+!~`<>?|:;'">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnExportExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnExportExcel_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                Text="Export To Excel" CssClass="textbox textbox1" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrintPDF" runat="server" Text="Print" OnClick="btnPrintPDF_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                height: auto;" CssClass="textbox textbox1" />
                        </td>
                        <td>
                            <asp:Button ID="btnDirectPrint" CssClass="fontblack" runat="server"
                                 Text="Direct Print" OnClientClick="return PrintPanel();" />
                                 <style>
                                    .fontblack
                                    {
                                        font-family: Book Antiqua;
                                        font-size: medium;
                                        font-weight: bold;
                                        color: Black;
                                    }
                                </style>
                                  <Insproplus:printmaster runat="server" ID="printCommonPdf" Visible="false" />
                        </td>

                       
                              

                    </tr>
                </table>
            </div>
        </center>
    </div>
   <%-- <Insproplus:printmaster runat="server" ID="printCommonPdf" Visible="false" />--%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
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
