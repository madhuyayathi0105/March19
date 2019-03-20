<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DepartmentWiseInternaltestAnalysis.aspx.cs"
    Inherits="DepartmentWiseInternaltestAnalysis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
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
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 165px;
            position: absolute;
            font-weight: bold;
            width: 950px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 950px;
            position: absolute;
            height: 50px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            ForeColor="Green" Font-Size="Large" Text="Department Wise Internal Exam Result Analysis"></asp:Label></center>
    <body>
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div>
                        <table style="width: 900px; height: 60px; background-color: #0CA6CA;">
                            <tr>
                                <td>
                                    <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                CssClass="multxtpanel" Width="114px" Font-Size="Medium" BackColor="White" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="1px">
                                                <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                                <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                                Font-Bold="true" runat="server" ReadOnly="true" Width="100px" Font-Size="Medium">--Select--</asp:TextBox>
                                            <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" Font-Bold="true"
                                                Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                                <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                                                    Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                                                <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="pdegree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                                runat="server" ReadOnly="true" Width="100px" Font-Size="Medium">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="150px">
                                                <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                                    runat="server" OnSelectedIndexChanged="chklst_branchselected" AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlsem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                                Font-Size="Medium" Width="60px" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblTestname" runat="server" Text="Test Name " font-name="Book Antiqua"
                                        Font-Size="Medium" Width="100px" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddltest" AutoPostBack="true" OnSelectedIndexChanged="ddltest_SelectedIndexChanged"
                                        runat="server" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        Width="125px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                                                Font-Size="Medium" Font-Bold="true" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
        <br />
        <%-- <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                    Width="959px">
                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                        Font-Bold="True" Font-Names="Book Antiqua" />
                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                        ImageAlign="Right" />
                </asp:Panel>
                <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                    <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                        OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                        RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                        <asp:ListItem Text="S.No"></asp:ListItem>
                        <asp:ListItem Text="Boys"></asp:ListItem>
                        <asp:ListItem Text="Girls"></asp:ListItem>
                        <asp:ListItem Text="Total"></asp:ListItem>
                        <asp:ListItem Text="No of Students"></asp:ListItem>
                        <asp:ListItem Text="No of Girl Students"></asp:ListItem>
                        <asp:ListItem Text="No of Boys Students"></asp:ListItem>
                        <asp:ListItem Text="No of Girl Hostel Students"></asp:ListItem>
                        <asp:ListItem Text="No of Boys Hostel Students"></asp:ListItem>
                        <asp:ListItem Text="No of Girl Day Scholar Students"></asp:ListItem>
                        <asp:ListItem Text="No of Boys Day Scholar Students"></asp:ListItem>
                        <asp:ListItem Text="Appear"></asp:ListItem>
                        <asp:ListItem Text="Pass"></asp:ListItem>
                        <asp:ListItem Text="Fail"></asp:ListItem>
                        <asp:ListItem Text="Pass %"></asp:ListItem>
                    </asp:CheckBoxList>
                </asp:Panel>
                <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                    CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                    TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                    ExpandedImage="../images/down.jpeg">
                </asp:CollapsiblePanelExtender>--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <center>
                    <br />
                    <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                    <br />
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
                        <asp:GridView ID="gridview1" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                            OnRowDataBound="gridview1_OnRowDataBound">
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+|\}{][':;?><,./">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnmasterprint_Click" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
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
</asp:Content>
