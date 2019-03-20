<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="InternalMarkReport.aspx.cs" Inherits="MarkMod_InternalMarkReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">

        function columnOrderCbl() {
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblColumnOrder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            if (cball.checked == true) {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = false;
                }
            }
        }

        function columnOrderCb() {
            var count = 0;
            var cball = document.getElementById('<%=cb_column.ClientID%>');
            var cblall = document.getElementById('<%=cblColumnOrder.ClientID%>');
            var tagname = cblall.getElementsByTagName("input");
            for (var i = 0; i < tagname.length; i++) {
                if (tagname[i].checked == true) {
                    count += 1;
                }
            }
            if (tagname.length == count) {
                cball.checked = true;
            }
            else {
                cball.checked = false;
            }

        }

        function display1() {
            document.getElementById('<%=lblnorec.ClientID %>').innerHTML = "";
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
    <style type="text/css">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Internal Mark Report</span>
    </center>
    <center>
        <div>
            <center>
                <div>
                    <input runat="server" type="hidden" id="hdnDescTotal" value="0" />
                    <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                        padding: 5px;" width="950px">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:DropDownList ID="ddlCollege" runat="server" Height="26px" Width="193px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:DropDownList ID="ddlBatch" runat="server" Height="26px" Width="76px" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 21px; width: 56px">
                                </asp:Label>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="26px"
                                    OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Width="143px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 21px; width: 56px"></asp:Label>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="26px"
                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style="width: 196px;"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                            </td>
                            <td style="padding-left: 7px;">
                                <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="26px"
                                    OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="width: 58px;" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" Style="height: 21px; width: 26px"></asp:Label>
                                        </td>
                                        <td style="padding-left: 7px;">
                                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="26px" Style="width: 58px;"
                                                OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 7px;">
                                            <asp:Label ID="Label3" runat="server" Text="Test" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" Style="height: 21px; width: 26px"></asp:Label>
                                        </td>
                                        <td style="padding-left: 7px;">
                                            <asp:DropDownList ID="ddlTest" runat="server" AutoPostBack="true" Height="26px" Style="width: 175px;"
                                                OnSelectedIndexChanged="ddlTest_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left: 7px;">
                                            <asp:Label ID="Label2" runat="server" Text="Subject" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua" Style="height: 21px; width: 32px"></asp:Label>
                                        </td>
                                        <td style="padding-left: 7px;">
                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSub" runat="server" Style="height: 26px; width: 219px;" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlSub" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 219px;
                                                        height: 160px;">
                                                        <asp:CheckBox ID="cbSub" runat="server" Width="136px" Text="Select All" AutoPostBack="True"
                                                            OnCheckedChanged="cbSub_OnCheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSub" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSub_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtSub"
                                                        PopupControlID="pnlSub" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <%-- <td style="padding-left: 7px;">
                                    <asp:CheckBox ID="cbIsLab" runat="server" AutoPostBack="true" Text="Laboratory" OnCheckedChanged="cbIsLab_OnCheckedChanged" />
                                </td>--%>
                                        <td style="padding-left: 45px;">
                                            <asp:Button ID="btnGo" runat="server" Text="GO" Font-Bold="true" Width="57px" Height="30px"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnGo_OnClick" />
                                        </td>
                                        <td style="padding-left: 32px;">
                                            <asp:LinkButton ID="lnkBtnColOrder" runat="server" Text="Column Order" OnClick="lnkBtnColOrder_OnClick"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <br />
            <span runat="server" id="lblNote" style="width: 404px; float: right; color: Red">Note:Absent
                - AAA</span>
            <br />
            <center>
                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
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
                    <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                        HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                        OnRowDataBound="Showgrid_OnRowDataBound">
                    </asp:GridView>
                </div>
            </center>
            <br />
            <center>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
                <table>
                    <tr runat="server" id="tr_printReport">
                        <td>
                            <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtxl" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnExcel" runat="server" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                OnClick="btnExcel_OnClick" />
                        </td>
                        <td>
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmasterr_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        </td>
                        <td>
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
            </center>
            <center>
                <div id="divColOrder" runat="server" style="height: 100%; display: none; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 324px;
                            width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 100px;
                            border-radius: 10px;">
                            <center>
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblcolr" runat="server" Text="Column Order" Style="font-family: Book Antiqua;
                                                font-size: 20px; font-weight: bold; color: Green;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="cb_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" onchange="return columnOrderCbl()" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblColumnOrder" runat="server" Height="43px" Width="600px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal" onclick="return columnOrderCb()">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                            <center>
                                                <asp:Button ID="btnColOrderOK" OnClientClick="return divhide();" CssClass=" textbox btn1 comm"
                                                    Style="height: 28px; width: 65px;" Text="OK" runat="server" OnClick="btnColOrderOK_OnClick" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
