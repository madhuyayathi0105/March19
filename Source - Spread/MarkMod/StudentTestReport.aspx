<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentTestReport.aspx.cs" Inherits="StudentTestReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
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
    <asp:ScriptManager ID="scriptvel" runat="server">
    </asp:ScriptManager>
    <body>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">CAM R10-Student's Overall CAM Report</span></center>
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div style="width: 1072px">
                    <center>
                        <asp:Panel ID="pnl" class="maintablestyle" runat="server" Style="background-color: #0CA6CA;
                            width: 954px; border: 1px solid black;" Height="66px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Height="25px" Width="69px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Height="25px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Width="380px"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" Width="41px">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table style="position: absolute">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Width="164px"
                                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td style="margin: 0 0 0 150px;" class="style1">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Width="100px" Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="left: 63px;
                                                    position: absolute; top: 7px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="psubject" runat="server" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" OnCheckedChanged="chksubject_CheckedChanged"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtsubject"
                                                    PopupControlID="psubject" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltest" runat="server" Text="Test Name" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="82px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txttest" runat="server" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                                    Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="position: relative;
                                                    left: 2px; top: 1px;">---Select---</asp:TextBox>
                                                <asp:Panel ID="paneltest" runat="server" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="chktest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnCheckedChanged="chktest_CheckedChanged" Text="Select All"
                                                        AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cbltest_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttest"
                                                    PopupControlID="paneltest" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="24px" Width="85px" AutoPostBack="True" OnTextChanged="txtfromdate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="24px" Width="92px" AutoPostBack="True" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblConvert_Value" runat="server" Text="Conversion Mark" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConvert_Value" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="17px" Width="58px" AutoPostBack="true" OnTextChanged="txtConvert_Value_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtConvert_Value"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <br />
                        <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                            margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;
                            overflow: auto">
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
                                HeaderStyle-BackColor="#0CA6CA" Font-Names="book antiqua" togeneratecolumns="true"
                                OnRowDataBound="Showgrid_OnRowDataBound">
                            </asp:GridView>
                        </div>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text=""></asp:Label>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    </center>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
                <asp:PostBackTrigger ControlID="btnPrint" />
            </Triggers>
        </asp:UpdatePanel>
        <%--progressBar for go--%>
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
        <%--progressBar for Sem--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel_sem">
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
        </center>
    </body>
</asp:Content>
