<%@ Page Title="Student On Duty Details" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentOndutydetails.aspx.cs" Inherits="AttendanceMOD_StudentOnDutyEntryDetailsNew"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function frelig() {
            document.getElementById('<%=btnReasonSet.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnReasonDel.ClientID%>').style.display = 'block';
        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
        function displayReason() {
            document.getElementById('<%=lblReasonErr.ClientID %>').innerHTML = "";
        }

        function SelLedgers(chkSelAll) {

            var tbl = document.getElementById("<%=gviewstudetails.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length - 1); i++) {
                var chkall = document.getElementById('MainContent_gviewstudetails_check_0');
                var chkSelectid = document.getElementById('MainContent_gviewstudetails_check_' + i.toString());
                if (chkall.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }

        }
    </script>
    <script type="text/javascript">
        function tdate() {
            var cun = 0;
            var daycun = 0;
            var id = document.getElementById("<%=txtFromDateOD.ClientID %>");
            var from = id.value;
            var end = document.getElementById("<%=txtToDateOD.ClientID %>");
            var to = end.value;
            var sp = from.split("/");
            from = sp[1] + '/' + sp[0] + '/' + sp[2];
            var sp1 = to.split("/");
            to = sp1[1] + '/' + sp1[0] + '/' + sp1[2];
            var varDate = new Date(from);
            var ToDate = new Date(to);
            var d = new Date(from);
            var month = (d.getMonth() + 1);
            var day = d.getDate();
            var year = d.getFullYear();
            var m = month.length;
            if (month < 10) month = '0' + month;
            if (day < 10) day = '0' + day;
            var dto = new Date(to);
            var monthto = (dto.getMonth() + 1);
            var dayto = dto.getDate();
            var yearto = dto.getFullYear();
            if (monthto < 10) monthto = '0' + monthto;
            if (dayto < 10) dayto = '0' + dayto;
            var fndate = month + '/' + day + '/' + year;
            var tndate = monthto + '/' + dayto + '/' + yearto;
            if (new Date(fndate).getTime() > new Date(tndate).getTime()) {
                alert("The Start Date must be Less to End date");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function Selectall(id) {
            //alert('hi');
            var checkboxall = document.getElementById(id);
            var e = document.getElementById(id);
            var array = id.split('_');
            var pos = array[2].toString();
            var position = pos.replace('chkhour', '');
            var grid = document.getElementById('<%=gviewOD.ClientID%>');

            var chkall = document.getElementById('MainContent_gviewOD_' + pos + '_0');

            if (chkall.checked) {
                for (var jk = 1; jk < grid.rows.length - 1; jk++) {
                    var chk = document.getElementById('MainContent_gviewOD_chkhour' + position + '_' + jk.toString());
                    chk.checked = true;
                }
            }
            else {
                for (var jk = 1; jk < grid.rows.length - 1; jk++) {
                    var chk = document.getElementById('MainContent_gviewOD_chkhour' + position + '_' + jk.toString());
                    chk.checked = false;
                }
            }
        }
    </script>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
            margin-bottom: 15px; margin-top: 10px;">Student On Duty Details </span>
        <asp:UpdatePanel ID="Upanel1" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: auto; height: auto; background-color: #0CA6CA;
                    padding: 8px; margin: 0px; margin-bottom: 15px; margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="arrow" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="271px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="41px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpnlSec" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSec" Width="69px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                    <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel" Height="100px" Width="110px">
                                                        <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtSec" runat="server" TargetControlID="txtSec"
                                                        PopupControlID="pnlSec" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <asp:DropDownList ID="ddlSec" Width="52px" Visible="false" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="true" Width="80px"></asp:TextBox>
                                        <asp:CalendarExtender ID="calExtFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" Font-Bold="true" Width="80px"></asp:TextBox>
                                        <asp:CalendarExtender ID="calExtToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UPGo" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                                    Text="Go" OnClick="btnGo_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UPAdd" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAdd" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                                    Text="Add" OnClick="btnAdd_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="Upanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 15px; margin-top: 10px;"></asp:Label>
            <center>
                <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 5px;
                    margin-top: 10px;">
                    <center>
                        <div id="divSpread" style="margin: 0px; margin-bottom: 5px; margin-top: 10px;">
                            <table>
                                <tr>
                                    <td colspan="6" align="center">
                                        <%--OnRowDataBound="ODDetails_OnRowDataBound"--%>
                                        <asp:GridView ID="gviewODDetails" Font-Names="Book Antique" AutoGenerateColumns="true"
                                            runat="server" Visible="false" Width="900" ShowHeader="false">
                                            <Columns>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="gview" runat="server" OnRowCreated="gview_OnRowCreated" AutoGenerateColumns="true"
                                            Width="900" OnRowDataBound="gview_OnRowDataBound" ShowHeader="false">
                                            <%--OnSelectedIndexChanged="gview_OnSelectedIndexChanged" --%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="TagNote" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsnotag" runat="server" Visible="false" Text='<%#Eval("SNotag") %>' />
                                                        <asp:Label ID="lblsnonote" runat="server" Visible="false" Text='<%#Eval("SNonote") %>' />
                                                        <asp:Label ID="lblrolltag" runat="server" Visible="false" Text='<%#Eval("Rolltag") %>' />
                                                        <asp:Label ID="lblrollnote" runat="server" Visible="false" Text='<%#Eval("Rollnote") %>' />
                                                        <asp:Label ID="lblouttmetag" runat="server" Visible="false" Text='<%#Eval("outtmetag") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:CheckBox ID="checkall" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkall_Indexchanged" />
                                                            <asp:CheckBox ID="chck" runat="server" AutoPostBack="true" OnCheckedChanged="chk_Indexchanged" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <table>
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
                                                    <asp:UpdatePanel ID="UPExcel" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnExportExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                                OnClick="btnExportExcel_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                                                Text="Export To Excel" CssClass="textbox textbox1" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnExportExcel" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UPPrint" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnPrintPDF" runat="server" Text="Print" OnClick="btnPrintPDF_Click"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                                                height: auto;" CssClass="textbox textbox1" />
                                                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2" align="right">
                                        <div id="divODUpdateDelete" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div id="divUpdateOD" runat="server" visible="false">
                                                                    <asp:Button ID="btnOnDutyUpdate" Visible="true" CssClass="textbox textbox1" runat="server"
                                                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                                                        height: auto;" Text="Update" OnClick="btnOnDutyUpdate_Click" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:UpdatePanel ID="Upanel7" runat="server">
                                                            <ContentTemplate>
                                                                <div id="divDeleteOD" runat="server" visible="false">
                                                                    <asp:Button ID="btnOnDutyDelete" Visible="true" CssClass="textbox textbox1" runat="server"
                                                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                                                        height: auto;" Text="Delete" OnClick="btnOnDutyDelete_Click" />
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="Upanel3" runat="server">
            <ContentTemplate>
                <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
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
                                                <asp:UpdatePanel ID="UPpopclose" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                            Text="Ok" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="Upanel8" runat="server">
            <ContentTemplate>
                <div id="divConfirmShow" runat="server" visible="false" style="height: 300em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divConfirmationShow" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblConfirmMsgShow" runat="server" Text="Do You Want To Save marks?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="lblSaveorDeleteShow" Visible="false" runat="server" Text="1" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="popUp" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnConfirmShowYes" CssClass=" textbox btn1 textbox1" Style="height: auto;
                                                            width: auto;" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnConfirmShowYes_Click"
                                                            Text="Yes" runat="server" />
                                                        <asp:Button ID="btnConfirmShowNo" Font-Names="Book Antiqua" CssClass=" textbox btn1 textbox1"
                                                            Style="height: auto; width: auto;" Font-Bold="true" Font-Size="Medium" OnClick="btnConfirmShowNo_Click"
                                                            Text="No" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="upnlODStudDetails" runat="server">
        <ContentTemplate>
            <center>
                <asp:UpdateProgress ID="upgODDetails" runat="server" AssociatedUpdatePanelID="upnlODStudDetails">
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
                <asp:ModalPopupExtender ID="ModalPopupExtender7" runat="server" TargetControlID="upgODDetails"
                    PopupControlID="upgODDetails">
                </asp:ModalPopupExtender>
            </center>
            <asp:ModalPopupExtender ID="mPopExtODDetails" runat="server" TargetControlID="upgODDetails"
                PopupControlID="upgODDetails">
            </asp:ModalPopupExtender>
            <center>
                <div id="divODEntryDetails" runat="server" visible="false" style="height: 70em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0px;">
                    <center>
                        <%--left: 15%; right: 15%; position: absolute;--%>
                        <div id="divODEntry" runat="server" class="table" style="background-color: White;
                            border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-left: auto;
                            margin-right: auto; width: 970px; height: auto; z-index: 1000; border-radius: 5px;">
                            <asp:Label ID="lbl_clgT" runat="server" Visible="false"></asp:Label>
                            <center>
                                <span style="top: 20px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Student's On Duty Entry </span>
                            </center>
                            <table style="margin: 0px; margin-bottom: 10px; margin-top: 25px; position: relative;
                                width: auto;">
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:CheckBox ID="chkStudentWise" runat="server" Text="Student Wise" AutoPostBack="true"
                                            OnCheckedChanged="chkStudentWise_CheckedChanged" CssClass="font" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="left">
                                        <div id="divAddStudents" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <div runat="server" visible="false" id="divSearchBy">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblSearch" runat="server" Text="Option" CssClass="font"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"
                                                                            Font-Bold="true">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStudentOptions" runat="server" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStudent" runat="server" CssClass="font" Text=""></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="filterStudent" runat="server" TargetControlID="txtStudent"
                                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" -/">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddStudent" CssClass="textbox textbox1" Font-Bold="True" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" runat="server" OnClick="btnAddStudent_Click" Text="Add"
                                                            Style="width: auto; height: auto;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divSearchAllStudents" runat="server" visible="true">
                                <table style="margin: 0px; margin-bottom: 10px; margin-top: 20px; position: relative;
                                    width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCollegeOD" runat="server" Text="Batch" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCollegeOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlCollegeOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="80px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBatchOD" runat="server" Text="Batch" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBatchOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlBatchOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="80px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDegreeOD" runat="server" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDegreeOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlDegreeOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="80px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranchOD" runat="server" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBranchOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlBranchOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSemOD" runat="server" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSemOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlSemOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="40px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSecOD" runat="server" Text="sec" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSecOD" runat="server" CssClass="font" OnSelectedIndexChanged="ddlSecOD_SelectedIndexChanged"
                                                AutoPostBack="True" Width="40px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <center>
                                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;
                                    width: 100%;">
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Label ID="lblsquare" runat="server" BackColor="Tan" Width="10px" Height="10px"></asp:Label>
                                                <asp:Label ID="lblColorText" runat="server" ForeColor="Black" Text="OD Limit Exceeded"></asp:Label>
                                            </center>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <%--<asp:UpdatePanel ID="upd2" runat="server">
                                                <ContentTemplate>--%>
                                            <center>
                                                <div id="gviewdiv" runat="server" style="width: 500px; height: 300px; overflow: auto;">
                                                    <asp:GridView ID="gviewstudetails" runat="server" AutoGenerateColumns="false" OnRowDataBound="gviewstudetails_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblsno1" runat="server" Text='<%#Eval("sno") %>' /></center>
                                                                    <asp:Label ID="lblsnotag" Visible="false" runat="server" Text='<%#Eval("snotag") %>' />
                                                                    <asp:Label ID="lblsnonote" Visible="false" runat="server" Text='<%#Eval("snonote") %>' />
                                                                    <asp:Label ID="lblsnovalue" Visible="false" runat="server" Text='<%#Eval("snovalue") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Roll No">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblroll" runat="server" Text='<%#Eval("Roll No") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblrolltag" Visible="false" runat="server" Text='<%#Eval("Rolltag") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblrollnote" Visible="false" runat="server" Text='<%#Eval("Rollnote") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblrollvalue" Visible="false" runat="server" Text='<%#Eval("Rollvalue") %>' /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reg No">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblreg" runat="server" Text='<%#Eval("Reg No") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblregtag" Visible="false" runat="server" Text='<%#Eval("Regtag") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblregnote" Visible="false" runat="server" Text='<%#Eval("Regnote") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblregvalue" Visible="false" runat="server" Text='<%#Eval("Regvalue") %>' /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Admission No">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lbladmno" runat="server" Text='<%#Eval("Admission No") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lbladmnotag" Visible="false" runat="server" Text='<%#Eval("Admissiontag") %>' />
                                                                        <asp:Label ID="lbladmnobote" Visible="false" runat="server" Text='<%#Eval("Admissionnote") %>' />
                                                                        <asp:Label ID="lbladmnovalue" Visible="false" runat="server" Text='<%#Eval("Admissionvalue") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Student Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstunme" runat="server" Text='<%#Eval("Student Name") %>' />
                                                                    <asp:Label ID="lblstunmetag" Visible="false" runat="server" Text='<%#Eval("Studenttag") %>' />
                                                                    <asp:Label ID="lblstunmenote" Visible="false" runat="server" Text='<%#Eval("Studentnote") %>' />
                                                                    <asp:Label ID="lblstunmevalue" Visible="false" runat="server" Text='<%#Eval("Studentvalue") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Semester">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblsem" runat="server" Text='<%#Eval("Semester") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblsemtag" Visible="false" runat="server" Text='<%#Eval("Semestertag") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblsemnote" Visible="false" runat="server" Text='<%#Eval("Semesternote") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblsemvalue" Visible="false" runat="server" Text='<%#Eval("Semestervalue") %>' /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="OD Count">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lblodcon" runat="server" Text='<%#Eval("OD Count") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblodtag" Visible="false" runat="server" Text='<%#Eval("ODtag") %>' /></center>
                                                                    <center>
                                                                        <asp:Label ID="lblodnote" Visible="false" runat="server" Text='<%#Eval("ODnote") %>' /></center>
                                                                    <%--<asp:Label ID="lblReferDay" Visible="false" runat="server" Text='<%#Eval("ReferDay") %>' />--%>
                                                                    <center>
                                                                        <asp:Label ID="lblvalue" Visible="false" runat="server" Text='<%#Eval("ODvalue") %>' /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:CheckBox ID="check" runat="server" /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" />
                                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                        <RowStyle ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                    </asp:GridView>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;
                                    width: 100%; height: auto;">
                                    <td align="center">
                                        <asp:Button ID="btnRemoveOdStudents" CssClass="textbox textbox1" Style="width: auto;
                                            height: auto;" runat="server" Text="Remove Selected Students" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnRemoveOdStudents_Click"
                                            Visible="false" />
                                    </td>
                                </table>
                            </center>
                            <div id="divODStuff" style="margin: 0px; padding: 2px; text-align: left;">
                                <table style="text-align: left; margin-bottom: 10px; margin-top: 10px; position: relative;">
                                    <tr>
                                        <td style="padding-left: 10px; padding-right: 10px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPurpose" runat="server" Text="Purpose" CssClass="font"></asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnReasonSet" runat="server" Text="+" Font-Bold="True" CssClass="textbox textbox1"
                                                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnReasonSet_Click" Style="display: none;
                                                            width: auto; height: auto;" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPurpose" runat="server" CssClass="font" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlPurpose_SelectedIndexChanged" Width="200px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnReasonDel" runat="server" CssClass="textbox textbox1" Text="-"
                                                            OnClick="btnReasonDel_Click" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            Style="display: none; width: auto; height: auto;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAttendanceOption" runat="server" Text="Attendance Option" CssClass="font"></asp:Label>
                                            <asp:DropDownList ID="ddlAttendanceOption" runat="server" AutoPostBack="true" CssClass="font">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 10px; padding-right: 10px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFromDateOD" runat="server" Text="From Date" CssClass="font"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDateOD" runat="server" AutoPostBack="true" CssClass="font"
                                                            Width="84px" OnTextChanged="txtFromDateOD_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="calExtFromDateOD" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDateOD">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-right: 5px; margin-left: 5px; margin-right: 5px;">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDateOD" runat="server" CssClass="font" Text="To Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToDateOD" runat="server" AutoPostBack="true" OnTextChanged="txtToDateOD_TextChanged"
                                                            CssClass="font" Width="80px"></asp:TextBox>
                                                        <%--onchange="tdate()" --%>
                                                        <asp:CalendarExtender ID="calExtToDateOD" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDateOD">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnOdhour" runat="server" Text="Go" CssClass="textbox textbox1" Font-Bold="True"
                                                            Font-Size="Medium" OnClick="btnOdhour_Click" Font-Names="Book Antiqua" Style="width: auto;
                                                            height: auto;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 10px; padding-right: 10px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblOutTime" runat="server" Text="Out Time" CssClass="font"></asp:Label>
                                        </td>
                                        <td align="left" colspan="2">
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlOutTimeHr" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlOutTimeMM" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlOutTimeSess" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                                <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;&nbsp;
                                            <asp:Label ID="lblInTime" runat="server" Text="In Time" CssClass="font"></asp:Label>
                                            &nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlInTimeHr" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlInTimeMM" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlInTimeSess" runat="server" AutoPostBack="true" CssClass="font"
                                                Width="45px">
                                                <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 10px; padding-right: 10px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                    </tr>
                                </table>
                                <div style="margin-top: 10px; margin-left: 23px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="height: 300px; width: 315px; overflow: auto;">
                                                    <asp:GridView ID="gviewOD" runat="server" AutoGenerateColumns="false" OnRowDataBound="gviewOD_OnRowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblODdate" runat="server" Text='<%#Eval("OdDate") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour1" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour2" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour3" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour4" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour5" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour6" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour7" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour8" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour9" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour10" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour11" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour12" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour13" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="14" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour14" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="15" HeaderStyle-HorizontalAlign="Center" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkhour15" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                                            Font-Names="Book Antiqua" />
                                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                        <RowStyle ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table style="margin-bottom: 10px; margin-top: 10px; position: relative;">
                                    <tr>
                                        <td style="padding-left: 5px; padding-right: 5px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbFullDay" runat="server" AutoPostBack="True" CssClass="font"
                                                GroupName="Days" Text="Full Day" Visible="false" OnCheckedChanged="rbFullDay_CheckedChanged" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbHalfDay" runat="server" AutoPostBack="True" CssClass="font"
                                                            GroupName="Days" Visible="false" OnCheckedChanged="rbHalfDay_CheckedChanged"
                                                            Text="Half Day" />
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <div id="divHalfHr" runat="server" style="border: 1px solid #000000;">
                                                            <asp:RadioButton ID="rbAM" runat="server" Visible="false" AutoPostBack="True" GroupName="Hours"
                                                                OnCheckedChanged="rbAM_CheckedChanged" Text="F.N" />
                                                            <asp:RadioButton ID="rbPM" runat="server" Visible="false" AutoPostBack="True" GroupName="Hours"
                                                                OnCheckedChanged="rbPM_CheckedChanged" Text="A.N" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rbHourWise" runat="server" AutoPostBack="True" CssClass="font"
                                                GroupName="Days" Visible="false" OnCheckedChanged="rbHourWise_CheckedChanged"
                                                Text="Hour Wise" />
                                        </td>
                                        <td style="padding-left: 5px; padding-right: 5px; margin-left: 5px; margin-right: 5px;">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNoOfHours" runat="server" Visible="false" CssClass="font" Text="Select Hours"></asp:Label>
                                        </td>
                                        <td>
                                            <div style="position: relative;">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtNoOfHours" runat="server" Style="width: 75px;" CssClass="font"
                                                            OnTextChanged="txtNoOfHours_TextChanged" Visible="false">
                                                        </asp:TextBox>
                                                        <asp:Panel ID="pnlHours" runat="server" Style="background-color: White; border-color: Black;
                                                            border-width: 0px; border-style: solid; visibility: visible; z-index: 1000; width: 76px;">
                                                            <asp:CheckBoxList ID="cblHours" Visible="false" runat="server" Font-Size="Small"
                                                                AutoPostBack="True" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                                                Width="60px" OnSelectedIndexChanged="cblHours_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popExtHours" runat="server" TargetControlID="txtNoOfHours"
                                                            PopupControlID="pnlHours" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <center>
                                    <asp:CheckBox ID="chkIncludeSplHrs" runat="server" CssClass="font" Text="Include Special Hour Marking Attendance" />
                                    <table style="margin-bottom: 10px; margin-top: 10px; position: relative;">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPopDeleteOD" runat="server" Text="Delete" CssClass="textbox textbox1"
                                                    Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                                    height: auto;" Visible="false" OnClick="btnPopDeleteOD_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPopSaveOD" runat="server" Text="Save" CssClass="textbox textbox1"
                                                    Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Style="width: auto;
                                                    height: auto;" OnClick="btnPopSaveOD_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPopExitOD" runat="server" Text="Exit" CssClass="textbox textbox1"
                                                    Font-Names="Book Antiqua" Font-Bold="True" Font-Size="Medium" Style="width: auto;
                                                    height: auto;" OnClick="btnPopExitOD_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <table style="margin-bottom: 10px; margin-top: 10px; position: relative;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPopODErr" runat="server" Text=" " CssClass="font" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </center>
                </div>
            </center>
       
    <center>
        <div id="divPopODAlert" runat="server" visible="false" style="height: 130em; z-index: 20000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divODAlert" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblODAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnODPopAlertClose" runat="server" CssClass=" textbox btn1 comm"
                                            Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px;
                                            width: 65px;" OnClick="btnODPopAlertClose_Click" Text="Ok" />
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
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="Updatepane1" runat="server">
        <ContentTemplate>
            <center>
                <div id="divConfirm" runat="server" visible="false" style="height: 130em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divConfirmation" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Save marks?" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="lblSaveorDelete" Visible="false" runat="server" Text="1" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnConfirmYes" CssClass=" textbox btn1 textbox1" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="height: auto; width: auto;"
                                                    OnClick="btnConfirmYes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnConfirmNo" CssClass=" textbox btn1 textbox1" Font-Bold="true"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" Style="height: auto; width: auto;"
                                                    OnClick="btnConfirmNo_Click" Text="No" runat="server" />
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
            <asp:PostBackTrigger ControlID="btnConfirmYes" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlreasondetails" runat="server">
        <ContentTemplate>
            <center>
                <asp:UpdateProgress ID="upginfrac" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upnlreasondetails">
                    <ProgressTemplate>
                        <center>
                            <div class="centerpb" style="height: 40px; width: 40px; text-align: center">
                                <img alt="" src="../images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </center>
            <asp:ModalPopupExtender ID="mpopextinfrac" runat="server" TargetControlID="upginfrac"
                PopupControlID="upginfrac">
            </asp:ModalPopupExtender>
            <div id="divShowInFraction" runat="server" visible="false" style="height: 82em; z-index: 1000;
                width: 100%; position: absolute; top: 0%; left: 0%; background-color: rgba(54, 25, 25, .2);">
                <center>
                    <%--left: 25%; right: 25%; top: 25%; position: absolute;--%>
                    <div id="divInFration" runat="server" class="table" style="background-color: White;
                        border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        margin-left: auto; margin-right: auto; width: 690px; height: auto; z-index: 3000;
                        border-radius: 5px;">
                        <center>
                            <span style="margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;
                                font-size: large; color: Green; font-weight: bold; text-align: center;">Infraction
                                Type </span>
                        </center>
                        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" Width="600px" Height="35px" TextMode="MultiLine"
                                        Font-Bold="True" onkeypress="displayReason()" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Style="resize: none;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddReason" CssClass="textbox textbox1" runat="server" Text="Add"
                                                    OnClick="btnAddReason_Click" Style="height: auto; width: auto" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnExitReason" CssClass="textbox textbox1" runat="server" Text="Exit"
                                                    Style="height: auto; width: auto" OnClick="btnExitReason_Click" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblReasonErr" runat="server" Text="" Style="margin: 0px; margin-bottom: 20px;
                                        margin-top: 10px; position: relative; font-size: medium; font-weight: bold; text-align: center;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
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
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UPAdd">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UPPrint">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UPExcel">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="Upanel6">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="Upanel7">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress7" runat="server" AssociatedUpdatePanelID="popUp">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender8" runat="server" TargetControlID="UpdateProgress7"
            PopupControlID="UpdateProgress7">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress8" runat="server" AssociatedUpdatePanelID="UPpopclose">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender9" runat="server" TargetControlID="UpdateProgress8"
            PopupControlID="UpdateProgress8">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
