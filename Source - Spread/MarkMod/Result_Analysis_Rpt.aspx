<%@ Page Title="CAM R12-Branchwise Result Analysis" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Result_Analysis_Rpt.aspx.cs" EnableEventValidation="false"
    Inherits="Result_Analysis_Rpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerr').innerHTML = "";
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
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 1px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
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
        
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style58
        {
            width: 132px;
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
        <asp:Label ID="Label1" CssClass="fontstyleheader" runat="server" Style="color: Green;
            margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;" Text="CAM R12-Branchwise Result Analysis"></asp:Label>
    </center>
    <center>
        <div>
            <table class="maintablestyle" style="height: 70px; width: auto; background-color: #0CA6CA;
                margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative; padding: 3px;">
                <tr>
                    <td>
                        <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                            OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" CausesValidation="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                            Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua"> </asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                    OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CausesValidation="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="false">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            Visible="false" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RadioButton ID="rbappear" runat="server" Text="Appear" AutoPostBack="true" OnCheckedChanged="RadioButton_CheckedChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" GroupName="Pass"
                            Width="80px" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rbstrength" runat="server" Text="Strength" AutoPostBack="true"
                            Font-Bold="True" Font-Names="Book Antiqua" OnCheckedChanged="RadioButton_CheckedChanged"
                            Font-Size="Medium" GroupName="Pass" Width="90px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdinternal" runat="server" Text="InternalReport" AutoPostBack="true"
                                        Font-Bold="True" Width="150px" Font-Names="Book Antiqua" Font-Size="Medium" GroupName="Report"
                                        OnCheckedChanged="rdinternal_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdexternal" runat="server" Text="ExternalReport" AutoPostBack="true"
                                        Font-Bold="True" Width="140px" Font-Names="Book Antiqua" Font-Size="Medium" GroupName="Report"
                                        OnCheckedChanged="rdexternal_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblstaff" runat="server" Text="View Staff Type" Font-Bold="True" ForeColor="Black"
                                        Font-Size="Medium" Font-Names="Book Antiqua" Style=""></asp:Label>
                                    <asp:TextBox ID="txtstaff" runat="server" Height="20px" CssClass="Dropdown_Txt_staff"
                                        ReadOnly="true" Width="100px" Style="height: 15px; margin-left: 2px; font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pStaff" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chkstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkstaff_CheckedChanged" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstaff_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                            <asp:ListItem>Staff Name</asp:ListItem>
                                            <asp:ListItem>Staff Code</asp:ListItem>
                                            <asp:ListItem>Staff Code And Staff Name</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtstaff"
                                        PopupControlID="pStaff" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="False" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                        Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                                        Width="160px" />
                                </td>
                                <td>
                                    <asp:Label ID="lblTest" runat="server" Text="Test" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTest" runat="server" Width="100px" Font-Bold="True" AutoPostBack="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlTest_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_Gender" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Gender" Visible="false"> </asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelGender" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextGender" Height="20px" Width="100px" runat="server" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                            <asp:Panel ID="PGender" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="CheckGender" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="CheckGender_CheckedChanged" />
                                                <asp:CheckBoxList ID="CkLGender" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="CkLGender_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="TextGender"
                                                PopupControlID="PGender" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnGo_Click" Text="Go" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkincludepastout" runat="server" Text="Include PassedOut" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="includepastout_CheckedChanged"
                                        AutoPostBack="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRegulation" runat="server" Text="Regulation" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegulation" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblGetDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGetDegree" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblGetDept" runat="server" Text="Department" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDepartment" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblChkCourse" runat="server" Text="CourseCode" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="Chkbxcou" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                            Visible="False" OnCheckedChanged="Chkbxcou_CheckedChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lblCOE" runat="server" Text="COE Enrollment No" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCOE" runat="server" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblOutgone" runat="server" Text="OutGone" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkOutgone" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" Visible="False" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Text="No records found" Visible="False"></asp:Label>
            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Visible="False"></asp:Label>
        </div>
    </center>
    <asp:Panel ID="pnlrecordcount" runat="server" Style="margin: 0px; margin-bottom: 5px;
        margin-top: 5px; position: relative;">
        <table>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                        Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="24px" Width="58px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                        AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
                        Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                        OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="16px" Width="32px"></asp:TextBox>
                    <Ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                        FilterType="Numbers" />
                </td>
                <td>
                    <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <center>
        <div id="Columnorder" runat="server" visible="false">
            <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                Width="959px">
                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                    Font-Bold="True" Font-Names="Book Antiqua" />
                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                    ImageAlign="Right" />
            </asp:Panel>
            <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                <asp:CheckBox ID="Cbcolumn" runat="server" Checked="true" AutoPostBack="true" Font-Bold="True"
                    Width="164px" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="Cbcolumn_CheckedChanged"
                    Text="Select All" />
                <asp:CheckBoxList ID="cblsearch" runat="server" Height="43px" Width="850px" AutoPostBack="true"
                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                    RepeatDirection="Horizontal" OnSelectedIndexChanged="cblsearch_SelectedIndexChanged">
                </asp:CheckBoxList>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </div>
    </center>
    <br />
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
                HeaderStyle-BackColor="#0CA6CA" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
            </asp:GridView>
        </div>
        <div>
            <center style="margin: 0px; margin-bottom: 10px; margin-top: 8px; position: relative;">
                <asp:Label ID="lblerr" runat="server" Text="" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="false"></asp:Label>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <Ajax:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </Ajax:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnmasterprint_Click" />
                <asp:Button ID="btn_dirtprint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
            </center>
        </div>
    </center>
    <%--    </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btn_dirtprint" />
                <asp:PostBackTrigger ControlID="btnGo" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </center>
    <%--progressBar for Go--%>
    <%--  <center>
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
    </center>--%>
    <%--progressBar for Sem--%>
    <%--<center>
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
    </center>--%>
</asp:Content>
