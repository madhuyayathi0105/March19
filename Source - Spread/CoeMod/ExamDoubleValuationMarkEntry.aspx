<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamDoubleValuationMarkEntry.aspx.cs" Inherits="CoeMod_ExamDoubleValuationMarkEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div class="maindivstyle">
            <span style="color: Green; font-size: large;" class=" fontstyleheader">Exam Mark Subject
                Wise Report</span>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Year And Month"></asp:Label>
                        <asp:DropDownList ID="ddlYear1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="textbox ddlheight" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddltype" runat="server" Width="128px" CssClass="textbox ddlheight"
                            AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddldegree1" runat="server" CssClass="textbox ddlheight" Width="100px"
                            OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Branch"></asp:Label>
                        <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="textbox ddlheight" Width="160px"
                            OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Sem"></asp:Label>
                        <asp:DropDownList ID="ddlsem1" runat="server" CssClass="textbox ddlheight" Width="90px"
                            OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlsubtype" runat="server" AutoPostBack="true" CssClass="textbox ddlheight"
                            Width="150px" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlSubject" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                            CssClass="textbox ddlheight" Width="330px">
                        </asp:DropDownList>
                        <asp:Label ID="lblSubSubject" runat="server" Text="Sub-Subject" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlSubSub" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSubSub_SelectedIndexChanged"
                            CssClass="textbox ddlheight" Width="120px">
                        </asp:DropDownList>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="True"></asp:Label>
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox ddlheight" Width="80px">
                        </asp:DropDownList>
                        <asp:Button ID="btnviewre" runat="server" Text="Go" OnClick="btnviewre_Click" CssClass="textbox btn"
                            Width="100px" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false"></asp:Label>
            <br />
            <table width="950px">
                <tr>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnsave1" runat="server" OnClick="btnsavel1_click" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Save" />
                        <asp:Button ID="btnreset" runat="server" OnClick="btnreset_print" CssClass="textbox btn"
                            Width="60px" Visible="false" Text="Reset" />
                    </td>
                </tr>
            </table>
            <center>
                <asp:Label ID="lblaane" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Maroon" Text="Note:Please Enter If  AB: Absent, NR: Not Registered, NE:Not Entered, M: Mal Practice, LT: Discontinue, W: Course WidthDraw"></asp:Label>
            </center>
            <br />
            <FarPoint:FpSpread ID="fpmarkimport" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                CssClass="stylefp">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <FarPoint:FpSpread ID="fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                EnableClientScript="true" BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Never"
                HorizontalScrollBarPolicy="Never" CssClass="stylefp">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                        SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 200%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: auto;
                            width: 507px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: auto; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <br />
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn" Width="40px" OnClick="btn_errorclose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <br />
                        </div>
                    </center>
                </div>
            </center>
        </div>
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
        <center>
            <asp:Button ID="btnprintt" runat="server" OnClick="btnprintt_print" CssClass="textbox btn"
                Width="60px" Visible="false" Text="Print" />
            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
        </center>
        <%--<tr>
            <td>
                <asp:CheckBox ID="chksubwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="True" AutoPostBack="true" OnCheckedChanged="chksubwise_CheckedChanged" />
                <asp:CheckBox ID="ChkBundlewise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="BundleNo" AutoPostBack="true" OnCheckedChanged="chkBundleNo_CheckedChanged" />
            </td>
        </tr>--%>
</asp:Content>
