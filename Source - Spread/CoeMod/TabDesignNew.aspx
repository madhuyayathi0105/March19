<%@ Page Title="Tabulated Report" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TabDesignNew.aspx.cs" Inherits="TabDesignNew"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%#divTabSpread.ClientID %>");
            var printWindow = window.open('', '', 'height=800,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body style="width:100%;">');
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
    <style tyle="text/css">
        @media print
        {
            #divTabSpread
            {
                display: block;
            }
            .noprint
            {
                display: none;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divHead" class="noprint" runat="server">
        <center>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-top: 10px;
                margin-bottom: 15px;">Tabulated Report</span>
        </center>
        <center>
            <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;
                margin: 0px; margin-top: 10px; margin-bottom: 15px;">
                <table class="maintablestyle" id="tblsearch" runat="server" style="width: auto; height: auto;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="120px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblExamYear" runat="server" Text="Exam Year" Font-Bold="true" Font-Names="Book Antiqua"
                                ForeColor="Black" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamYear" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month" Font-Bold="true" Font-Names="Book Antiqua"
                                ForeColor="Black" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlExamMonth" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Enabled="true" Font-Size="Medium" Width="90px" CssClass="arrow" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                            Width="85px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            CssClass="multicheckbox" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                            <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="85px">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlDegree" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                            Width="85px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                            <asp:CheckBox ID="cbDegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbDegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popubExtDegree" runat="server" TargetControlID="txtDegree"
                                            PopupControlID="pnlDegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="85px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBranch" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                            Width="85px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlBranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                            <asp:CheckBox ID="cbBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbBranch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblBranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Height="58px" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popubExtBranch" runat="server" TargetControlID="txtBranch"
                                            PopupControlID="pnlBranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Height="30px" Text="Go" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Width="59px" CssClass="textbox btn2" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-top: 10px; margin-bottom: 15px;"></asp:Label>
        </center>
    </div>
    <center>
        <div id="divTabSpread" runat="server" visible="false" style="width: 100%;">
            <style type="text/css" media="print">
                @page
                {
                    size: A4 portrait;
                    margin: 0.5cm;
                }
                @media print
                {
                    thead
                    {
                        top: 0%;
                        left: 0px;
                        height: 20%;
                        display: table-header-group;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    tfoot
                    {
                        height: 20%;
                        bottom: 20%;
                        left: 0px;
                        display: table-footer-group;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    tbody
                    {
                        display: table-row-group;
                        height: 50%;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    #header
                    {
                        position: fixed;
                        top: 10%;
                        left: 0px;
                        height: 20%;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    #footer
                    {
                        position: fixed;
                        bottom: 20%;
                        height: 20%;
                        left: 0px;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    #tblClgHeader
                    {
                        position: relative;
                        bottom: 20%;
                        top: 20%;
                        height: 55%;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                }
                @media screen
                {
                    thead
                    {
                        display: block;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                    tfoot
                    {
                        display: block;
                        border-top: 1px solid #000000;
                        border-bottom: 1px solid #000000;
                    }
                }
            </style>
            <table id="tblClgHeader" cellspacing="0px" style="width: 980px; font-family: Book Antiqua;
                font-weight: bold; height: auto; border: 1px solid #000000; margin: 0px;">
                <thead style="position: relative; margin-top: 0px; width: 980px;">
                    <tr style="border: 1px thin #000000;">
                        <th style="display: table; width: 100%">
                            <div style="display: table-row; width: auto;">
                                <div style="display: table-cell; width: 60%;">
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span id="clgname" runat="server" style="font-family: Book Antiqua; font-weight: bold;
                                            font-size: 15px; display: table-cell;"></span>
                                    </div>
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span style="font-family: Book Antiqua; font-weight: bold; font-size: 15px;">Code: <span
                                            id="lblClgCode" runat="server" style="font-family: Book Antiqua; display: table-cell;
                                            font-weight: bold; font-size: 15px;"></span></span>
                                    </div>
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span id="lblAddr" runat="server" style="font-family: Book Antiqua; display: table-cell;
                                            font-weight: bold; font-size: 15px;"></span>
                                    </div>
                                </div>
                                <div style="display: table-cell; width: 40%; margin-left: 10px;">
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span style="font-family: Book Antiqua; font-weight: bold; font-size: 15px; display: table-cell;">
                                            Tabulated Register </span>
                                    </div>
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span id="lblDegreeCourse" runat="server" style="font-family: Book Antiqua; display: table-cell;
                                            font-weight: bold; font-size: 15px;"></span>
                                    </div>
                                    <div style="display: table-row; width: auto; margin: 0px; text-align: left;">
                                        <span id="lblYrofAdmission" runat="server" style="font-family: Book Antiqua; font-weight: bold;
                                            font-size: 15px;"></span>
                                    </div>
                                </div>
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div>
                                <table cellspacing="0px" cellpadding="0px" style="width: 980px; border-color: Black;
                                    text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: 10px;
                                    border-style: solid; border-width: 1px;">
                                    <tr style="background-color: #00aff0; color: #000000; border: 1px thin #000000; margin: 0px;
                                        padding: 0px;">
                                        <td rowspan="3" style="width: 25px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            S.No
                                        </td>
                                        <td style="width: 78px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Reg.No
                                        </td>
                                        <td rowspan="3" style="width: 185px; text-align: left; background-color: #00aff0;
                                            color: #000000; border: 1px thin #000000; margin: 0px; padding: 0px;">
                                            Name of the Candidate in English and in Tamil with Initial
                                        </td>
                                        <td rowspan="3" style="width: 230px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Optional Subjects
                                        </td>
                                        <td rowspan="3" style="width: 75px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Theory<br />
                                            Class
                                        </td>
                                        <td rowspan="3" style="width: 75px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Practical<br />
                                            Class
                                        </td>
                                        <td rowspan="3" style="width: 100px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Month and<br />
                                            Year of<br />
                                            Passing
                                        </td>
                                        <td rowspan="3" style="width: 80px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Photos
                                        </td>
                                        <td rowspan="3" style="width: 114px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Medium of Instruction
                                        </td>
                                    </tr>
                                    <tr style="background-color: #00aff0; color: #000000; border: 1px thin #000000; margin: 0px;
                                        padding: 0px;">
                                        <td style="width: 78px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            D.O.B
                                        </td>
                                    </tr>
                                    <tr style="background-color: #00aff0; color: #000000; border: 1px thin #000000; margin: 0px;
                                        padding: 0px;">
                                        <td style="width: 78px; background-color: #00aff0; color: #000000; border: 1px thin #000000;
                                            margin: 0px; padding: 0px;">
                                            Community
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </thead>
                <tbody style="width: 980px; margin: 0px; padding: 0px; margin-left: 0px;">
                    <tr align="left" style="width: 100%; margin: 0px; padding: 0px;">
                        <td colspan="3" align="left" style="width: 100%; margin: 0px; padding: 0px;">
                            <FarPoint:FpSpread ID="FpTabSpread" autopostback="false" Width="1000px" runat="server"
                                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                                ShowHeaderSelection="false" Style="width: 100%; height: auto; margin: 0px; padding: 0px;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                </tbody>
                <tfoot style="width: 980px; margin: 0px; padding: 0px;">
                    <tr>
                        <td>
                            <table style="width: auto; margin-top: 3px;">
                                <tr style="font-family: Book Antiqua; font-weight: bold; font-size: 8px;">
                                    <td style="text-align: left; font-weight: bold;">
                                    </td>
                                    <td style="text-align: left; font-weight: bold;">
                                        First Class With Distinction : I-D
                                    </td>
                                    <td style="text-align: left; font-weight: bold;">
                                        First Class : I
                                    </td>
                                    <td style="text-align: left; font-weight: bold;">
                                        Second Class : II
                                    </td>
                                    <td style="text-align: left; font-weight: bold;">
                                        Third Class : III
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 980px; margin: 0px; margin-top: 20px; margin-bottom: 0px;">
                                <tr style="font-family: Book Antiqua; font-weight: bold; font-size: 12px;">
                                    <td style="text-align: left;">
                                        CONTROLLER OF EXAMINATIONS
                                    </td>
                                    <td style="text-align: right;">
                                        PRINCIPAL
                                    </td>
                                </tr>
                                <tr style="font-family: Book Antiqua; font-weight: bold; font-size: 12px; margin: 0px;
                                    padding: 0px;">
                                    <td colspan="2" style="text-align: right; font-size: 9px;">
                                        (College Seal)
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </center>
    <center>
        <div id="rptprint1" class="noprint" runat="server" visible="false" style="margin: 0px;
            margin-top: 10px; margin-bottom: 15px;">
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
            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
        </div>
    </center>
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
                                <asp:Label ID="lblpoperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
</asp:Content>
