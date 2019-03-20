<%@ Page Title="Performance Report ICSE XI" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="PerformanceReportCardICSE XI.aspx.cs" Inherits="MarkMod_PerformanceReportCardICSE_XI" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

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
        #printCommonPdf
        {
        }
    </style>
    <%--<script type="text/javascript">
        function display1() {
            document.getElementById('<%#lblExcelErr.ClientID %>').innerHTML = "";
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
           
            <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Academic
                Performance Report Card ICSE I To III</span>
        </div>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;">
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
                        <asp:Label ID="lblSec" runat="server" Text="Section" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlSec"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="txtTest"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTest" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlTest" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
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
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto;" />
                    </td>
                    <td>
                        <asp:Button ID="btnMarkTypeSettings" CssClass="textbox textbox1 commonHeaderFont"
                            runat="server" OnClick="btnMarkTypeSettings_Click" Text="Settings" Style="width: auto;
                            height: auto;" />
                    </td>
                     <td colspan="3">
                        <asp:CheckBox ID="chkManualAttendance" Checked="false" runat="server" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua" Text="Include Attendance" />
                    </td>
                    <%-- <td>
                    <asp:Label ID="lblSubject" AssociatedControlID="ddlSubject" runat="server" Text="Subject"
                        CssClass="commonHeaderFont"></asp:Label>
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:UpdatePanel ID="UpnlSubject" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSubject" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                    ReadOnly="true">-- Select --</asp:TextBox>
                                <asp:Panel ID="pnlSubject" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                    Width="280px">
                                    <asp:CheckBox ID="chkSubject" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                        AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                    <asp:CheckBoxList ID="cblSubject" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                    PopupControlID="pnlSubject" Position="Bottom">
                                </asp:PopupControlExtender>
                                <asp:DropDownList ID="ddlSubject" Width="152px" Visible="false" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged" CssClass="commonHeaderFont">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkConvertedTo" Checked="false" runat="server" CssClass="commonHeaderFont"
                        Text="Include Converted Mark" AutoPostBack="true" OnCheckedChanged="chkConvertedTo_CheckedChanged" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtConvertedMaxMark" runat="server" CssClass="commonHeaderFont"
                        Text="" Width="50px"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="filterConvert" runat="server" TargetControlID="txtConvertedMaxMark"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                </td>--%>
                </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 15px; margin-top: 10px;"></asp:Label>
            <center>
                <div id="divMainContents" runat="server" visible="false" style="margin-bottom: 10px;
                    margin-top: 10px; position: relative;">
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnReportCard" runat="server" CssClass="textbox textbox1 commonHeaderFont"
                                    Style="width: auto; height: auto;" Text="Report Card" OnClick="btnReportCard_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpStudentList" autopostback="false" Width="1000px" runat="server"
                                    Visible="true" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" OnButtonCommand="FpStudentList_Command"
                                    ShowHeaderSelection="false" Style="width: 100%; height: auto;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
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
    <%-- Subject Mark Type Setting --%>
    <center>
        <div id="divSubjectSetting" runat="server" visible="false" style="height: 150em;
            z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0; left: 0px;">
            <center>
                <div id="divSetting" runat="server" class="table" style="background-color: White;
                    height: auto; width: 68%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 15%; right: 15%; top: 8%; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%; padding: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rblSubjectOrSubjectType" AutoPostBack="true" CssClass="commonHeaderFont" runat="server"
                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rblSubjectOrSubjectType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Subject" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Subject Type" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <FarPoint:FpSpread ID="FpSubjectList" AutoPostBack="False" runat="server" Visible="false"
                                        BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
                                        OnUpdateCommand="FpSubjectList_Command">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnSave" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnSave_Click" Text="Save" runat="server" />
                                        <asp:Button ID="btnExit" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnExit_Click" Text="Exit" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
