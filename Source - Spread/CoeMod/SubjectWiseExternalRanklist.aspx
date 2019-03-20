<%@ Page Title="Subject Wise Extrenal Mark Rank List" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectWiseExternalRanklist.aspx.cs" Inherits="SubjectWiseExternalRanklist" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
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
            height: 140px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .fontb
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = document.getElementById("<%=pnlContents.ClientID %>");
            var panelheader = document.getElementById("<%=panelheader.ClientID %>");
            var panelfooter = document.getElementById("<%=panelfooter.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=595');
            printWindow.document.write('<html><head><title>DIV Contents</title>');
            printWindow.document.write('</head><body style="width:auto; margin:0px;">');
            printWindow.document.write(panelheader.innerHTML);
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(panelfooter.innerHTML);
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
    <center>
        <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
            ForeColor="Green" Font-Size="Large" Text="Subject Wise Extrenal Mark Rank List"
            Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
        <asp:UpdatePanel ID="updpan_batch" runat="server">
            <ContentTemplate>
                <table style="width: auto; height: auto; padding: 5px; background-color: #0CA6CA;
                    margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                    <tr>
                        <td>
                            <asp:Label ID="lblstream" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                runat="server" Text="Stearm"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstream" runat="server" Width="80px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlstream_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Bold="true" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbledu" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                runat="server" Text="Education"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative">
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtedu" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Font-Bold="true" runat="server" ReadOnly="true" Width="50px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Pedu" runat="server" Font-Names="Book Antiqua" Font-Bold="true" CssClass="MultipleSelectionDDL"
                                            Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                            <asp:CheckBox ID="chkedu" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chkedu_batchchanged" />
                                            <asp:CheckBoxList ID="chklsedu" Font-Bold="true" Font-Size="Medium" runat="server"
                                                AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsedu_batchselected">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtedu"
                                            PopupControlID="Pedu" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            CssClass="MultipleSelectionDDL" Width="114px" Font-Size="Medium" BackColor="White"
                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                            Height="150px">
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
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="Medium" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                            Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" Font-Bold="true"
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
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                Font-Size="Medium" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                            runat="server" ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
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
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsem" runat="server" Text="Sem" font-name="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtsem" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua" Font-Bold="true"
                                                        runat="server" ReadOnly="true" Width="75px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="PSem" runat="server" CssClass="MultipleSelectionDDL" Font-Bold="true"
                                                        Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                                        <asp:CheckBox ID="chksem" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chksem_CheckedChanged" />
                                                        <asp:CheckBoxList ID="chklssem" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssem_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsem"
                                                        PopupControlID="PSem" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubtype" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" Text="Subject Type" Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtsubtype" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="135px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="psubtype" runat="server" CssClass="MultipleSelectionDDL" Font-Bold="true"
                                                        Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="1px" ScrollBars="Vertical" Height="150px">
                                                        <asp:CheckBox ID="chksubtype" Font-Bold="true" runat="server" Font-Size="Medium"
                                                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chksubtype_CheckedChanged" />
                                                        <asp:CheckBoxList ID="chklssubtype" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubtype_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubtype"
                                                        PopupControlID="psubtype" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" Text="Subject"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtsubject" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                                        Font-Bold="true" runat="server" ReadOnly="true" Width="126px">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Psubject" runat="server" CssClass="MultipleSelectionDDL" Font-Bold="true"
                                                        Font-Size="Medium" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="1px" ScrollBars="Vertical" Height="450px">
                                                        <asp:CheckBox ID="chksubject" Font-Bold="true" runat="server" Font-Size="Medium"
                                                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="chksubject_CheckedChanged" />
                                                        <asp:CheckBoxList ID="chklssubject" Font-Bold="true" Font-Size="Medium" runat="server"
                                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubject_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsubject"
                                                        PopupControlID="Psubject" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbwoarrear" runat="server" Text="With Arrear" AutoPostBack="true"
                                            OnCheckedChanged="Radiochange" GroupName="Arrear" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true" Width="120px" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbwarrear" runat="server" Text="With Out Arrear" AutoPostBack="true"
                                            OnCheckedChanged="Radiochange" GroupName="Arrear" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true" Width="150px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" Text="Percentage From" Width="140px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtminimunpercent" runat="server" Height="20px" Width="50px" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtminimunpercent"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                            Font-Size="Medium" Text="Top"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_top" runat="server" Height="20px" Width="50px" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtminimunpercent"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo1"
                                            Font-Size="Medium" Font-Bold="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                    Font-Names="Book Antiqua" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                    position: relative;"></asp:Label>
                <asp:Panel ID="pnlContents" runat="server" Style="width: 100%; margin: 0px; padding: 0px;">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="false" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                        position: relative;" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                        CssClass="stylefp">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </asp:Panel>
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
                <asp:Button ID="btnPrint" runat="server" CssClass="fontb" Text="Direct Print" OnClientClick="return PrintPanel();" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btngo" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnmasterprint" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <div style="height: 1px; width: 1px; overflow: auto; position: absolute;">
        <asp:Panel ID="panelheader" runat="server" Style="width: 100%; height: auto; margin: 0px;
            padding: 0px;">
            <div id="header" runat="server" style="width: 100%; height: auto; margin: 0px; padding: 0px;">
                <table style="width:885px; height: auto; margin: 0px; margin-top: 10px; margin-bottom: 10px;">
                    <tr>
                        <td rowspan="4" align="left" style="width: 100px; height: 110px;">
                            <img id="img_leftlogo" alt="" src="college/Left_Logo.jpeg" style="height: 100px;" />
                        </td>
                        <td colspan="3" align="center" style="width: 67%;">
                            <center>
                                <asp:Label ID="lblcolname" Style="font-family: book antiqua; font-size: 20px; font-weight: bold;"
                                    runat="server">THE NEW COLLEGE (AUTONOMOUS)</asp:Label>
                            </center>
                        </td>
                        <td rowspan="4" align="right" style="width: 100px; height: 110px;">
                            <%-- <img id="img1" alt="" src="college/Right_Logo.jpeg" style="  height: 100px;"/>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <center>
                                <asp:Label ID="lbl_Affliated" Style="font-family: book antiqua; font-size: 12px;
                                    font-weight: normal;" runat="server">(Affiliated to the University of Madras and Accredited by NAAC with "A" Grade)</asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <center>
                                <asp:Label ID="lbl_con" Style="font-family: book antiqua; font-size: 16px; font-weight: bold;"
                                    runat="server">OFFICE OF THE CONTROLLER OF EXAMINATIONS</asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <center>
                                <asp:Label ID="lbl_tit" Style="font-family: book antiqua; font-size: 17px; font-weight: bold;"
                                    runat="server">RANK LIST</asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table style="float: left; margin: 0px; width: 50%;">
                                <tr>
                                    <td>
                                        <span id="span_0" runat="server" class="fontb">Batch Year </span>
                                    </td>
                                    <td>
                                        <span id="co1" runat="server" class="fontb">:</span>
                                    </td>
                                    <td>
                                        <span id="Span1" runat="server" class="fontb"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span2" runat="server" class="fontb">Branch</span>
                                    </td>
                                    <td>
                                        <span id="co2" runat="server" class="fontb">:</span>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span8" runat="server" class="fontb movetop">Branch</span>
                                    </td>
                                    <td>
                                        <span id="co3" runat="server" class="fontb movetop">:</span>
                                    </td>
                                    <td style="width: 226px;">
                                        <span id="Span3" runat="server" class="fontb movetop"></span>
                                    </td>
                                </tr>
                            </table>
                            <table style="float: right; margin: 0px; width: 45%;">
                                <tr>
                                    <td>
                                        <span id="Span4" runat="server" class="fontb">Semester</span>
                                    </td>
                                    <td>
                                        <span id="co4" runat="server" class="fontb">:</span>
                                    </td>
                                    <td>
                                        <span id="Span5" runat="server" class="fontb"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span6" runat="server" class="fontb movetop">Month & Year</span>
                                    </td>
                                    <td>
                                        <span id="Span10" runat="server" class="fontb movetop">:</span>
                                    </td>
                                    <td>
                                        <span id="Span11" runat="server" style="text-transform: uppercase;" class="fontb movetop">
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <span id="Span9" runat="server" style="font-family: book antiqua; text-align: center;
                                bottom: 15px; font-size: 18px; font-weight: bold;"></span>
                        </td>
                    </tr>
                </table>
                <style>
                    .movetop
                    {
                        margin-top: 15px;
                        position: relative;
                        top: 10px;
                    }
                </style>
            </div>
        </asp:Panel>
        <asp:Panel ID="panelfooter" runat="server" Style="height: auto; margin-top: 595px;
            position: relative;">
            <div id="Div1" runat="server" style="height: auto;">
                <table style="width: 885px; height: auto;">
                    <tr>
                        <td style="text-align: right; margin-top: 290px;">
                            <span id="span7" runat="server" class="fontb">
                            <br />
                            <br />
                            <br />
                            <br />
                            CONTROLLER OF EXAMINATIONS </span>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
