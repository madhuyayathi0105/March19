<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="UniversityPassPercentageReport.aspx.cs" Inherits="UniversityPassPercentageReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .defaultWidthHeight
        {
            width: auto;
            height: auto;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">University
            Pass Percentage Report</span>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
            position: relative;">
            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                        </asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlCollege" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCollege" Visible="true" Width="100px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlCollege" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                        height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkCollege" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkCollege_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblCollege" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtCollege" runat="server" TargetControlID="txtCollege"
                                        PopupControlID="pnlCollege" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlCollege" Visible="false" runat="server" CssClass="dropdown commonHeaderFont"
                                        Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblStream" runat="server" Text="Stream" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlStream" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtStream" Visible="false" Width="75px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlStream" Visible="false" runat="server" CssClass="multxtpanel" Style="width: 130px;
                                        height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkStream" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkStream_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblStream" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblStream_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtStream" runat="server" TargetControlID="txtStream"
                                        PopupControlID="pnlStream" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlStream" Visible="true" Width="60px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlStream_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblEduLevel" runat="server" Text="Edu Level" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlEduLevel" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtEduLevel" Visible="false" Width="95px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlEduLevel" Visible="false" runat="server" CssClass="multxtpanel"
                                        Style="width: 130px; height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkEduLevel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkEduLevel_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblEduLevel" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblEduLevel_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtEduLevel" runat="server" TargetControlID="txtEduLevel"
                                        PopupControlID="pnlEduLevel" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlEduLevel" Visible="true" Width="60px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlEduLevel_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont">
                        </asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="false" Width="85px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="false" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                        height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkBatch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlBatch" Visible="true" runat="server" CssClass="commonHeaderFont"
                                        OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" AutoPostBack="True" Width="80px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree">
                        </asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="85px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                        height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlDegree" Visible="false" runat="server" CssClass="commonHeaderFont"
                                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" AutoPostBack="True" Width="80px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch">
                        </asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="85px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                        height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlBranch" Visible="false" runat="server" CssClass="commonHeaderFont"
                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="True" Width="150px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Semester">
                        </asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlSem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSem" Visible="false" Width="85px" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlSem" Visible="false" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                        height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkSem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblSem" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtSem" runat="server" TargetControlID="txtSem"
                                        PopupControlID="pnlSem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlSem" Visible="true" runat="server" CssClass="commonHeaderFont"
                                        OnSelectedIndexChanged="ddlSem_SelectedIndexChanged" AutoPostBack="True" Width="40px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblExamYear" runat="server" CssClass="commonHeaderFont" Text="ExamYear">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upnlExamYear" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"
                                    AutoPostBack="True" Width="80px">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblExamMonth" runat="server" CssClass="commonHeaderFont" Text="ExamMonth">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upnlExamMonth" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlExamMonth" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlExamMonth_SelectedIndexChanged"
                                    AutoPostBack="True" Width="80px">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblReport" runat="server" CssClass="commonHeaderFont" Text="Report">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upnlReport" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlReport" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged"
                                    AutoPostBack="True" Width="100px">
                                    <asp:ListItem Value="0" Text="Subject Wise"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="SubjectType"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnllSubjectType" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblSubjectType" runat="server" CssClass="commonHeaderFont" Text="Subject Type">
                                            </asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlSubjectType" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlSubjectType" runat="server" CssClass="commonHeaderFont"
                                                OnSelectedIndexChanged="ddlSubjectType_SelectedIndexChanged" AutoPostBack="True"
                                                Width="100px">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="textbox textbox1 commonHeaderFont"
                                        OnClick="btnGo_Click" Style="width: auto; height: auto;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display: none;">
                </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px;"></asp:Label>
            <div id="ShowReport" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <FarPoint:FpSpread ID="spreadDet" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
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
                                <div id="print" runat="server" visible="false">
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                        Height="32px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                        Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
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
                                        <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
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
