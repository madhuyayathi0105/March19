<%@ Page Title="Semester Exam Pass Percentage Analysis Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SemesterExamPassPercentageReport.aspx.cs" Inherits="CoeMod_SemesterExamPassPercentageNew"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                #divSpread
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
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=divSpread.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head><title>Semester Exam Pass Percentage Analysis Report</title>');
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
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Semester Exam Pass Percentage Analysis Report</span>
        </div>
        <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <table class="maintablestyle" style="width: auto; height: auto;">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Edulevel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Width="90px" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEdulevel" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlEdulevel_SelectedIndexChanged"
                            AutoPostBack="true" Width="50px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="width: 90px;">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua';
                                        overflow-y: scroll;">
                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="chklsbatch" />
                                </Triggers>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="chkbatch" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree" runat="server" ReadOnly="true" CssClass="dropdown" Width="99px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                        <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Branch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch" runat="server" CssClass="dropdown" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" Height="300px" Style="font-family: 'Book Antiqua'; overflow-y: scroll;">
                                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblExamYr" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Exam Year"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExamyr" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlExamyr_SelectedIndexChanged"
                            AutoPostBack="true" Width="50px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblExamMonth" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            runat="server" Text="Exam Month"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlExamMonth" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                            AutoPostBack="true" Width="50px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Go" CssClass="textbox btn2" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        
        <div id="divSpread" runat="server" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <FarPoint:FpSpread ID="FpSpread1" AutoPostBack="false" Width="1050px" runat="server"
                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                ShowHeaderSelection="false" Style="width: 100%; height: auto; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div id="rptprint1" class="noprint" runat="server" visible="false" style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px;">
            <table>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                    </td>
                    <td>
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="divpopupErr" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lblpopuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                            OnClick="btn_errorclose_Click" Text="Ok" />
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
