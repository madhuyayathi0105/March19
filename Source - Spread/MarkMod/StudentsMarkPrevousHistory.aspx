<%@ Page Title="Student's Previous CAM Mark Report" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentsMarkPrevousHistory.aspx.cs" Inherits="MarkMod_StudentsMarkPrevousHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
        
        .style1
        {
            width: 122px;
        }
        
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #divMainContents
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
            document.getElementById('<%#lblExcelErr.ClientID %>').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=divMainContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
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
    <div>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <center>
                    <div>
                        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Student's
                            Previous CAM Mark Report</span>
                    </div>
                    <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
                        height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
                        position: relative;">
                        <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                            margin-bottom: 10px; padding: 6px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                                        AssociatedControlID="ddlCollege"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                                        Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSearchBy" runat="server" Text="Search By" CssClass="commonHeaderFont"
                                        AssociatedControlID="ddlSearchBy"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="dropdown commonHeaderFont"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSearchStudent" AssociatedControlID="txtSearchStudent" runat="server"
                                        Text="Roll No" CssClass="commonHeaderFont"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchStudent" runat="server" CssClass="textbox textbox1 commonHeaderFont "
                                        Text=""></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterStudent" runat="server" TargetControlID="txtSearchStudent"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" -/">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnSearchStudent" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                                                OnClick="btnSearchStudent_Click" Text="Search Student" Style="width: auto; height: auto;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <div id="divStudentDetail" runat="server" visible="true">
                            <table style="border: 1px solid #000000; margin: 0px; margin-bottom: 5px; margin-top: 5px;
                                padding: 5px;">
                                <tr>
                                    <td class="fontStyle">
                                        <span>Student Name</span>
                                    </td>
                                    <td class="fontStyle">
                                        <span>:</span>
                                    </td>
                                    <td class="fontStyle1">
                                        <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="fontStyle">
                                        <span>Roll No</span>
                                    </td>
                                    <td class="fontStyle">
                                        <span>:</span>
                                    </td>
                                    <td class="fontStyle1">
                                        <asp:Label ID="lblStudentRollNo" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="fontStyle">
                                        <span>Admission No</span>
                                    </td>
                                    <td class="fontStyle">
                                        <span>:</span>
                                    </td>
                                    <td class="fontStyle1">
                                        <asp:Label ID="lblAdmissionNo" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="fontStyle">
                                        <span>Reg. No</span>
                                    </td>
                                    <td class="fontStyle">
                                        <span>:</span>
                                    </td>
                                    <td class="fontStyle1">
                                        <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="fontStyle">
                                        <span>Class</span>
                                    </td>
                                    <td class="fontStyle">
                                        <span>:</span>
                                    </td>
                                    <td class="fontStyle1">
                                        <asp:Label ID="lblClassName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td colspan="4" class="fontStyle1">
                                        <asp:Label ID="lblAppNo" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table class="maindivstyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                                margin-bottom: 20px; padding: 5px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPrevCollege" runat="server" Text="College" AssociatedControlID="ddlPrevCollege"
                                            CssClass="commonHeaderFont"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPrevCollege" runat="server" CssClass="dropdown commonHeaderFont"
                                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlPrevCollege_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                                            AssociatedControlID="ddlBatch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                            AutoPostBack="True" Width="80px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                                            AssociatedControlID="ddlDegree"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                                            AutoPostBack="True" Width="80px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                                            AssociatedControlID="ddlBranch"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                            AutoPostBack="True" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem" AssociatedControlID="ddlSem"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                                            AutoPostBack="True" Width="40px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSec" runat="server" Text="Sec" CssClass="commonHeaderFont" AssociatedControlID="ddlSec"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSec" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                            AutoPostBack="True" Width="40px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="txtTest"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtTest" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlTest" runat="server" CssClass="multxtpanel" Height="200px" Width="280px">
                                                        <asp:CheckBox ID="chkTest" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chkTest_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblTest" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblTest_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtTest" runat="server" TargetControlID="txtTest"
                                                        PopupControlID="pnlTest" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <asp:DropDownList ID="ddlTest" runat="server" Visible="false" CssClass="commonHeaderFont"
                                                        OnSelectedIndexChanged="ddlTest_SelectedIndexChanged" AutoPostBack="True" Width="80px">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubject" AssociatedControlID="txtSubject" runat="server" Text="Subject"
                                            CssClass="commonHeaderFont"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpnlSubject" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSubject" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                                        ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Height="200px" Width="280px">
                                                        <asp:CheckBox ID="chkSubject" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                            AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSubject" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                                        PopupControlID="pnlSubject" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <asp:DropDownList ID="ddlSubject" Width="52px" Visible="false" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged" CssClass="commonHeaderFont">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGetMarks" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                                            OnClick="btnGetMarks_Click" Text="Get Marks" Style="width: auto; height: auto;" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divPrint1" runat="server" visible="false" style="margin: 0px; margin-top: 20px;">
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
                                            <asp:Button ID="btnDirectPrint" Visible="true" CssClass="textbox textbox1" runat="server"
                                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                                height: auto;" Text="Direct Print" OnClientClick="return PrintPanel();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 15px;
                                margin-top: 10px; position: relative;">
                                <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                                    <tr>
                                        <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                            <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                Width="100px" Height="100px" />
                                        </td>
                                        <th class="marginSet" align="center" colspan="6">
                                            <span id="spCollegeName" class="headerDisp" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="marginSet" align="center" colspan="6">
                                            <span id="spAddr" class="headerDisp1" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="marginSet" align="center" colspan="6">
                                            <span id="spReportName" class="headerDisp1" runat="server"></span>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="marginSet" colspan="3" align="center">
                                            <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                                        </td>
                                        <td class="marginSet" colspan="3" align="right">
                                            <span id="spSem" class="headerDisp1" runat="server"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="marginSet" colspan="3" align="left">
                                            <span id="spProgremme" class="headerDisp1" runat="server"></span>
                                        </td>
                                        <td class="marginSet" colspan="3" align="right">
                                            <span id="spSection" class="headerDisp1" runat="server"></span>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="Showgrid" Style="height: auto;" runat="server" Visible="false"
                                    HeaderStyle-ForeColor="Black" font-name="Book Antiqua" HeaderStyle-BackColor="#0CA6CA"
                                    AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                                </asp:GridView>
                            </div>
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </div>
                    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                        margin-bottom: 15px; margin-top: 10px;"></asp:Label>
                </center>
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
                <asp:PostBackTrigger ControlID="btnExportExcel" />
                <asp:PostBackTrigger ControlID="btnPrintPDF" />
                <asp:PostBackTrigger ControlID="btnDirectPrint" />
                <asp:PostBackTrigger ControlID="btnGetMarks" />
            </Triggers>
        </asp:UpdatePanel>
        <%--progressBar for Upbook_go--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
                <ProgressTemplate>
                    <center>
                        <div style="height: 40px; width: 150px;">
                            <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
        </center>
    </div>
</asp:Content>
