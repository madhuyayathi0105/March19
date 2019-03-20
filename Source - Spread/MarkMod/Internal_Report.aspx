<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Internal_Report.aspx.cs" Inherits="Internal_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblxlerr').innerHTML = "";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-top: 10px;
            margin-bottom: 15px;">CAM R11-Branchwise Subject Analysis</span>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div>
                    <table class="maintablestyle" style="margin: 0px; margin-top: 10px; margin-bottom: 15px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch" Width="100px" Font-Bold="True"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbatch" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" ScrollBars="Vertical" Style="margin-left: 12px;">
                                    <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkbatch_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddebatch" runat="server" DropDownControlID="pbatch" DynamicServicePath=""
                                    Enabled="true" TargetControlID="txtbatch">
                                </asp:DropDownExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" ScrollBars="Vertical" Style="margin-left: 12px;">
                                    <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkdegree_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddedegree" runat="server" DropDownControlID="pdegree" DynamicServicePath=""
                                    Enabled="true" TargetControlID="txtdegree">
                                </asp:DropDownExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Width="90px" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pbranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" ScrollBars="Auto" Width="350px" Style="font-family: 'Book Antiqua';
                                    margin-left: 166px;">
                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddebranch" runat="server" DropDownControlID="pbranch" DynamicServicePath=""
                                    Enabled="true" TargetControlID="txtbranch">
                                </asp:DropDownExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsection" runat="server" Text="Section" Width="100px" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsection" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psection" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" ScrollBars="Auto" Width="110px" Style="font-family: 'Book Antiqua';
                                    margin-left: 8px;">
                                    <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chksection_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        OnSelectedIndexChanged="chklstsection_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddesection" runat="server" DropDownControlID="psection"
                                    DynamicServicePath="" Enabled="true" TargetControlID="txtsection">
                                </asp:DropDownExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Width="100px" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsubject" runat="server" Height="20px" ReadOnly="true" Width="150px"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="psubject" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" Height="300px" ScrollBars="Auto" Width="350px" Style="font-family: 'Book Antiqua';
                                    margin-left: 195px;">
                                    <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chksubject_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Width="350px" OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddesubject" runat="server" DropDownControlID="psubject"
                                    DynamicServicePath="" Enabled="true" TargetControlID="txtsubject">
                                </asp:DropDownExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbltest" runat="server" Text="Test" Width="100px" Font-Bold="True"
                                    ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltest" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin-left: -31px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Style="margin: 0px; margin-top: 10px; margin-bottom: 15px;"></asp:Label>
                    </center>
                    <center>
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
                        <br />
                        <center style="margin: 0px; margin-top: 10px; margin-bottom: 15px;">
                            <asp:Label ID="lblxlerr" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
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
                            <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnmasterprint_Click" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            <br />
                        </center>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnmasterprint" />
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
    </center>
</asp:Content>
