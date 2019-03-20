<%@ Page Title="Tabulated Mark/Results Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true"
    CodeFile="TabulatedMarkResults.aspx.cs" Inherits="TabulatedMarkResults" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Tabulated Mark/Results Report</title>
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        body
        {
            font-family: Book Antiqua;
            height: auto;
            background-color: #ffffff;
            color: Black;
        }
        .Chartdiv
        {
            background-color: #ffffff;
            margin: 0px;
            color: #000000;
            position: relative;
            font-family: Book Antiqua;
            height: auto;
            width: 100%;
        }
    </style>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        @media print
        {
            #divMainContent
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
            #FpSpreadTabMarks, FpSpreadTabMarks_viewport
            {
                display: block;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%#divMainContent.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1100');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body>');
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
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="Div1" class="noprint" runat="server">
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green">Tabulated Mark/Results Report</span>
            </center>
            <br />
            <center>
                <fieldset style="width: 970px; height: auto; border-color: silver;
                    border-radius: 10px;">
                    <table class="maintablestyle" width="970px">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                    AutoPostBack="true" Width="50px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblReportFormat" runat="server" Text="Report Type" Font-Bold="True"
                                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlReportFormat" runat="server" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" AutoPostBack="true" Width="100px" style="margin-left:-38px" OnSelectedIndexChanged="ddlReportFormat_Selectchanged">
                                                <asp:ListItem Selected="True" Text="Format-1" Value="0"></asp:ListItem>
                                                <asp:ListItem Selected="False" Text="Format-2" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="position: relative;">
                                                <asp:UpdatePanel ID="UpnlSubjects" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtSubjects" Width=" 139px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlSubjects" runat="server" CssClass="multxtpanel" Height="200px"
                                                            Width="250px">
                                                            <asp:CheckBox ID="chkSubjects" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubjects_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblSubjects" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSubjects_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popubExtSubjects" runat="server" TargetControlID="txtSubjects"
                                                            PopupControlID="pnlSubjects" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblExamYear" runat="server" Text="Exam Year" Font-Bold="True" ForeColor="Black"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlExamYear" runat="server" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddlExamYear_Selectchanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month" Font-Bold="True" ForeColor="Black"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlExamMonth" runat="server" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddlExamMonth_Selectchanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Width="59px" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                        </td>
                                        </tr><tr>
                                        <td>
                                        
                                        <asp:CheckBox ID="cbsubcode" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Text="Subject Code" Width="134px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div>
                                <br />
                                <center>
                                    <asp:Panel ID="pnlHeaderFilter" runat="server" BackColor="#0ca6ca" CssClass="table2"
                                        Height="22px" Width="850px" Style="margin-top: -0.1%;">
                                        <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                        <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
                                    </asp:Panel>
                                </center>
                                <br />
                            </div>
                            <center>
                                <asp:Panel ID="pnlColumnOrder" runat="server" BackColor="#0ca6ca" CssClass="table2"
                                    Width="850px">
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
                                                    <asp:ListItem Value="1" Selected="True">Register No.</asp:ListItem>
                                                    <asp:ListItem Value="2" Selected="True">Student Name</asp:ListItem>
                                                    <asp:ListItem Value="3" Selected="True">External Valuation-I</asp:ListItem>
                                                    <asp:ListItem Value="4" Selected="True">External Valuation-II</asp:ListItem>
                                                    <asp:ListItem Value="5" Selected="True">External Valuation-III</asp:ListItem>
                                                    <asp:ListItem Value="6" Selected="True">External Total</asp:ListItem>
                                                    <asp:ListItem Value="12" Selected="True">External Average</asp:ListItem>
                                                    <asp:ListItem Value="7" Selected="True">External Mark</asp:ListItem>
                                                    <asp:ListItem Value="8" Selected="True">External Round Off</asp:ListItem>
                                                    <asp:ListItem Value="9" Selected="True">Internal Mark</asp:ListItem>
                                                    <asp:ListItem Value="10" Selected="True">Total</asp:ListItem>
                                                    <asp:ListItem Value="11" Selected="True">Overall Total</asp:ListItem>
                                                    <asp:ListItem Value="13" Selected="True">Batch</asp:ListItem>
                                                  
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
                            <br />
                            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <br />
                            <center>
                                <div id="divMainContent" runat="server" visible="false">
                                    <FarPoint:FpSpread ID="FpSpreadTabMarks" AutoPostBack="false" Width="1050px" runat="server"
                                        Visible="true" BorderStyle="Solid" BorderWidth="1px" CssClass="spreadborder"
                                        ShowHeaderSelection="false" Style="width: 100%; height: auto; display: block;">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <center>
                                    <div id="rptprint1" class="noprint" runat="server" visible="false" style="margin-top: 10px;
                                        margin-bottom: 10px;">
                                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                            Height="35px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                            CssClass="textbox textbox1" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                        <asp:Button ID="btnPrint" runat="server" Visible="false" Text="Direct Print" OnClientClick="return PrintPanel();"
                                            Font-Names="Book Antiqua" OnClick="btnPrint_Click" Font-Size="Medium" Font-Bold="true"
                                            Height="35px" CssClass="textbox textbox1" />
                                    </div>
                                </center>
                            </center>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnPrint" />
                            <asp:PostBackTrigger ControlID="btnExcel1" />
                            <asp:PostBackTrigger ControlID="btnprintmaster1" />
                        </Triggers>
                    </asp:UpdatePanel>
                </fieldset>
            </center>
        </div>
        <div id="popupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
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
    </body>
    </html>
</asp:Content>
