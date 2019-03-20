<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Cam_Performance_Report.aspx.cs" Inherits="Cam_Performance_Report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style3
        {
            width: 383px;
        }
        .style6
        {
            width: 103px;
        }
        .style8
        {
            width: 86px;
        }
        .style9
        {
            width: 417px;
        }
        .style10
        {
            width: 20px;
        }
        .style11
        {
            width: 138px;
        }
        .style12
        {
            width: 212px;
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
        function display() {

            document.getElementById('MainContent_errmsg').innerHTML = "";

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
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">CAM R12-Subjectwise Performance</span>
        </center>
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td class="style8">
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" Style=""></asp:Label>
                                </td>
                                <td class="style11">
                                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="100px" Style="height: 15px; width: 100px; font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td class="style9">
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" Style=""></asp:Label>
                                </td>
                                <td class="style10">
                                    <asp:TextBox ID="txtdegree" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td class="style3">
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td class="style6">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="180px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbranch" runat="server" Width="350px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                                PopupControlID="pbranch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsemester" runat="server" Text="Semester" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="75px" Style="">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style8">
                                    <asp:Label ID="lblsection" runat="server" Text="Section" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td class="style11">
                                    <asp:TextBox ID="txtsection" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="psection" runat="server" Width="110px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsection"
                                        PopupControlID="psection" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td class="style9">
                                    <asp:Label ID="lblcriteria" runat="server" Text="Criteria" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td class="style10">
                                    <asp:TextBox ID="txtcriteria" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pcriteria" runat="server" Width="250px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkcriteria" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkcriteria_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstcriteria" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstcriteria_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                            <asp:ListItem Value="0">Pass Count</asp:ListItem>
                                            <asp:ListItem Value="1">Pass Percentage</asp:ListItem>
                                            <asp:ListItem Value="2">Fail Count</asp:ListItem>
                                            <asp:ListItem Value="3">Fail Percentage</asp:ListItem>
                                            <asp:ListItem Value="4">Absent Count</asp:ListItem>
                                            <asp:ListItem Value="5">Absent Percentage</asp:ListItem>
                                            <asp:ListItem Value="6">Expected No of Pass</asp:ListItem>
                                            <asp:ListItem Value="7">Expected Pass Percentage</asp:ListItem>
                                            <asp:ListItem Value="8">Expected No of Failures</asp:ListItem>
                                            <asp:ListItem Value="9">Expected Fail Percentage</asp:ListItem>
                                            <asp:ListItem Value="10">Total Strength</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtcriteria"
                                        PopupControlID="pcriteria" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td class="style3">
                                    <asp:Label ID="lblstaff" runat="server" Text="Staff" Width="90px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td class="style6">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtstaff" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="180px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pstaff" runat="server" Width="273px" Height="273px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chksatff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chksatff_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklststaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chklststaff_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtstaff"
                                                PopupControlID="pstaff" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="style3">
                                    <asp:Label ID="lbltest" runat="server" Text="Test" Width="90px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                </td>
                                <td class="style6">
                                    <asp:UpdatePanel ID="UpdatePanel1text" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txttest" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="118px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="Panel3test" runat="server" Width="165px" Height="165px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="chktest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="chktest_CheckedChanged" Text="Select All"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chkltest" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    OnSelectedIndexChanged="chkltest_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                                    Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txttest"
                                                PopupControlID="Panel3test" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="style12">
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnPrintMaster" runat="server" Font-Bold="True" Text="Print Master Setting"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style44" OnClick="btnPrintMaster_Click"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <asp:Label ID="testerrmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <br />
                        <br />
                        </center>
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
                                HeaderStyle-BackColor="#0CA6CA" Font-Names="book antiqua" togeneratecolumns="true"
                                OnRowDataBound="Showgrid_OnRowDataBound">
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            <br />
                        </center>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnxl" />
                    <asp:PostBackTrigger ControlID="btnPrintMaster" />
                </Triggers>
            </asp:UpdatePanel>
            <%--progressBar for Go--%>
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
