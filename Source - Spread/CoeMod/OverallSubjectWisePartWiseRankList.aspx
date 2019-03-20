<%@ Page Title="Overall Consolidate/Subject/Part Wise Rank List" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="OverallSubjectWisePartWiseRankList.aspx.cs"
    Inherits="CoeMod_SubjectsPartWiseRankList" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="upProgMain" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upnlMain">
                <ProgressTemplate>
                    <div style="height: 1000px; width: 100%; position: fixed; top: 0%; left: 0%; right: 0%;
                        bottom: 0%; background-color: White;">
                        <center>
                            <img src="../images/progress2.gif" alt="Processing Please Wait..." style="margin-top: 100px;
                                height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; color: Gray;">Processing
                                Please Wait...</span>
                        </center>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="mpopExtMain" runat="server" TargetControlID="upProgMain"
                PopupControlID="upProgMain">
            </asp:ModalPopupExtender>
            <center>
                <span class="fontstyleheader" style="color: Green; font-weight: bold; margin: 0px;
                    margin-bottom: 15px; margin-top: 10px; position: relative;">Overall Consolidate/Subject/Part
                    Wise Rank List</span>
                <table class="maintablestyle" style="width: 70%; height: auto; background-color: #0CA6CA;
                    padding: 5px; margin: 0px; margin-bottom: 15px; margin-top: 10px; position: relative;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlCollege" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCollege" Width="90px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel" Style="width: 330px;
                                            height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkCollege" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblCollege" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtCollege" runat="server" TargetControlID="txtCollege"
                                            PopupControlID="pnlCollege" Position="Bottom">
                                        </asp:PopupControlExtender>
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
                                        <asp:TextBox ID="txtStream" Width="75px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlStream" runat="server" CssClass="multxtpanel" Style="width: auto;
                                            height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkStream" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkStream_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblStream" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblStream_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtStream" runat="server" TargetControlID="txtStream"
                                            PopupControlID="pnlStream" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblEduLevel" runat="server" Text="Edu Level" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEduLevel" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlEduLevel_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBatch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBatch" Visible="false" Width="70px" runat="server" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlBatch" Visible="false" runat="server" CssClass="multxtpanel" Style="width: 90px;
                                            height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkBatch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblBatch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: auto; margin: 0px; padding: 0px;
                                                border: 0px; height: auto;" OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                            PopupControlID="pnlBatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <asp:DropDownList ID="ddlBatch" Visible="true" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlDegree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDegree" Width="75px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                            margin: 0px; padding: 0px; height: 200px; overflow: auto;">
                                            <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                            PopupControlID="pnlDegree" Position="Bottom">
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
                                        <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtBranch" Width="100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Style="width: auto;
                                                        height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                                        <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                        PopupControlID="pnlBranch" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReportType" runat="server" Text="Report Type" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReportType" Width="100px" runat="server" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="Semester Wise" Value="1"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Subject Wise" Value="2"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Consolidate Report" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%-- <td colspan="2">
                            <div runat="server" id="divSem"></div>
                            <table>
                            
                            </table>
                            </td>--%>
                                    <td>
                                        <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlSem" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSem" Width="70px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlSem" runat="server" CssClass="multxtpanel" Style="width: 100px;
                                                        height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                                        <asp:CheckBox ID="chkSem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSem_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSem" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtSem" runat="server" TargetControlID="txtSem"
                                                        PopupControlID="pnlSem" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                    <asp:DropDownList ID="ddlSem" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblPartType" runat="server" Text="Part Type" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlPart" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlPartType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlPartType_SelectedIndexChanged">
                                                        <%--<asp:ListItem Selected="True" Text="All Part" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Part-I" Value="1"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Part-II" Value="2"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Part-III" Value="3"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Part-IV" Value="4"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Part-V" Value="5"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlSubject" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtSubject" Width="85px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                        height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                                        <asp:CheckBox ID="chkSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                        <asp:CheckBoxList ID="cblSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            runat="server" AutoPostBack="True" Style="width: auto; height: auto; margin: 0px;
                                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                                        PopupControlID="pnlSubject" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
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
                                        <asp:Label ID="lblRankBy" runat="server" Text="Rank By" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRankBy" Visible="true" runat="server" Width="100px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow">
                                            <asp:ListItem Selected="True" Text="CGPA" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="CWAM" Value="1"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Average" Value="2"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Over All Total" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlRankBySubject" Visible="false" runat="server" Width="100px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow">
                                            <asp:ListItem Selected="True" Text="Total" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Average" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- AutoPostBack="true" OnSelectedIndexChanged="ddlRankBy_SelectedIndexChanged"--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSubReportType" runat="server" Text="Format" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSubReportType" Visible="false" runat="server" Width="100px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow">
                                            <asp:ListItem Selected="True" Text="Individual Subject Total" Value="0"></asp:ListItem>
                                            <asp:ListItem Selected="False" Text="Cumulative Subject Total" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTop" runat="server" Text="Top Rank" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTop" Visible="true" Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua"
                                            Width=" 45px" runat="server" Text="" CssClass="textbox  txtheight2" MaxLength="2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtTop"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDepartmentWise" Checked="false" Text="Rank List Partition" runat="server"
                                            AutoPostBack="true" OnCheckedChanged="chkDepartmentWise_CheckedChanged" />
                                    </td>
                                    <td>
                                        <div runat="server" id="divGroupBy" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblGroupBy" runat="server" Text="Rank Group By" Font-Bold="true" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div id="divAll" runat="server" visible="false" style="position: relative;">
                                                            <asp:UpdatePanel ID="upnlGroupBy" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtGroupBy" Width="85px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="pnlGroupBy" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                        height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                                                        <asp:CheckBox ID="chkGroupBy" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkGroupBy_CheckedChanged" />
                                                                        <asp:CheckBoxList ID="cblGroupBy" runat="server" AutoPostBack="true" Font-Bold="true"
                                                                            Font-Size="Medium" RepeatLayout="Table" RepeatDirection="Vertical" OnSelectedIndexChanged="cblGroupBy_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="False" Text="College Wise" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Stream Wise" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="EduLevel Wise" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Batch Wise" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Department Wise" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Degree Wise" Value="5"></asp:ListItem>
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="popExtGroupBy" runat="server" TargetControlID="txtGroupBy"
                                                                        PopupControlID="pnlGroupBy" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div id="divSubjectWise" runat="server" visible="false" style="position: relative;">
                                                            <asp:UpdatePanel ID="upnlSubGroupBy" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtSubGroupBy" Width="85px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="pnlSubGroupBy" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                                        height: 200px; overflow: auto; margin: 0px; padding: 0px;">
                                                                        <asp:CheckBox ID="chkSubGroupBy" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                            runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubGroupBy_CheckedChanged" />
                                                                        <asp:CheckBoxList ID="cblSubGroupBy" runat="server" AutoPostBack="true" Font-Bold="true"
                                                                            Font-Size="Medium" RepeatLayout="Table" RepeatDirection="Vertical" OnSelectedIndexChanged="cblSubGroupBy_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="False" Text="College Wise" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Stream Wise" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="EduLevel Wise" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Batch Wise" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Department Wise" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Degree Wise" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Subject Name Wise" Value="6"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Subject Code Wise" Value="7"></asp:ListItem>
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="popExtSubGroupBy" runat="server" TargetControlID="txtSubGroupBy"
                                                                        PopupControlID="pnlSubGroupBy" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                            Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                            Text="Go" OnClick="btnGo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
            <div>
                <center>
                    <asp:Panel ID="pnlHeaderFilter" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                        CssClass="table2" Height="22px" Width="850px" Style="margin-top: -0.1%; margin-bottom: 10px;">
                        <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                        <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
                    </asp:Panel>
                </center>
            </div>
            <center>
                <asp:Panel ID="pnlColumnOrder" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                    CssClass="table2" Width="850px">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkColumnOrderAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkColumnOrderAll_CheckedChanged" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnRemoveAll" runat="server" Font-Size="X-Small" Height="16px"
                                    Style="font-family: 'Book Antiqua'; color: #ffffff; font-weight: 700; font-size: small;
                                    margin-left: -599px;" Visible="false" Width="111px" OnClick="lbtnRemoveAll_Click">Remove All</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtOrder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="noresize"
                                    AutoPostBack="true" runat="server" Enabled="false">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblColumnOrder" runat="server" Height="43px" AutoPostBack="true"
                                    Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                    RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblColumnOrder_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="SNo" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="College Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Batch Year" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Stream" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Edulevel" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Course Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Department Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Degree Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="Semester" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="Register No" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="Roll No" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="Admission No" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="Student Name" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="13" Text="Total Secured Marks" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="14" Text="Average" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="15" Text="CGPA" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="16" Text="CWAM" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="17" Text="Grade" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="18" Enabled="false" Text="Rank" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="19" Text="Signature Of the HOD" Selected="True"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlColumnOrder"
                CollapseControlID="pnlHeaderFilter" ExpandControlID="pnlHeaderFilter" Collapsed="true"
                TextLabelID="lblFilter" CollapsedSize="0" ImageControlID="imgFilter" CollapsedImage="~/images/right.jpeg"
                ExpandedImage="~/images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px; position: relative;" Font-Size="Medium"></asp:Label>
            <center>
                <asp:UpdatePanel ID="Updp_Degree" runat="server">
                    <ContentTemplate>
                        <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 5px;
                            margin-top: 10px; text-align: left;">
                            <center>
                                <div id="divSpread" style="margin: 0px; margin-bottom: 5px; margin-top: 10px;
                                    text-align: left; left: 0%;">
                                    <div id="divPrint1" runat="server" style="margin: 0px; margin-top: 20px; text-align: center;">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
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
                                                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                            OnClick="btnExcel1_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                                            Text="Export To Excel" CssClass="textbox textbox1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                                            height: auto;" CssClass="textbox textbox1" />
                                                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                                                    </td>
                                                    <%--<td>
                                    <asp:Button ID="btnCalculate" Visible="true" CssClass="textbox textbox1" runat="server"
                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto;
                                        height: auto;" Text="Calculate GPA And CGPA" OnClick="btnCalculate_Click" />
                                </td>--%>
                                                </tr>
                                            </table>
                                            <FarPoint:FpSpread ID="FpStudentsRankList" autopostback="false" Width="1000px" runat="server"
                                                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                                                ShowHeaderSelection="false" Style="width: auto; height: auto; left: 0%; margin: 0px;
                                                margin-bottom: 15px; margin-top: 10px; padding: 0px;">
                                                <Sheets>
                                                    <FarPoint:SheetView DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                                        AllowPage="False" PageSize="100" SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </center>
                                    </div>
                                </div>
                            </center>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExcel1" />
                        <asp:PostBackTrigger ControlID="btnprintmaster1" />
                    </Triggers>
                </asp:UpdatePanel>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel1" />
            <asp:PostBackTrigger ControlID="btnprintmaster1" />
            <asp:PostBackTrigger ControlID="btnGo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
