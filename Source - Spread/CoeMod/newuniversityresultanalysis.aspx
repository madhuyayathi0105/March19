<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newuniversityresultanalysis.aspx.cs" Inherits="newuniversityresultanalysis" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <asp:Label ID="Label2" runat="server" Text="Semester Grade Sheet and Result Analysis"
            CssClass="fontstyleheader" ForeColor="Green"></asp:Label></center>
    <br />
    <center>
        <table style="width: 700px; height: 70px; background-color: #0CA6CA;" class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddl_college" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged"
                        Width="218px">
                    </asp:DropDownList>
                </td>
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
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                        Width="180px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" CausesValidation="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                                <br />
                            </td>
                            <td>
                                <asp:Label ID="lblExamMonth" runat="server" Text="Exam Month" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                            </td>
                            <td>
                                <asp:Label ID="lblExamYear" runat="server" Text="Exam Year" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_frmdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfrm_date" runat="server" Font-Bold="True" Height="17px" Width="84px"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtfrm_date" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtfrm_date"
                                    FilterType="Custom,Numbers" ValidChars="/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtend_date" runat="server" Height="17px" Width="84px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtend_date"
                                    FilterType="Custom,Numbers" ValidChars="/">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtend_date" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
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
                                <asp:RadioButtonList ID="rbbeforeandafterrevaluation" runat="server" Font-Bold="True"
                                    Font-Names="Book Antiqua" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbbeforeandafterrevaluation_selectedindexchanged"
                                    Font-Size="Medium" AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="1">Before Revaluation </asp:ListItem>
                                    <asp:ListItem Value="2">After Revaluation </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkonlyrevaluation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Revaluation Status" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_subjectwisegrade" runat="server" Text="2015 Regulation" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Width="150px" />

                                   <asp:CheckBox ID="CheckBox1" runat="server" Text="For Arrear GPA" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Width="150px" />
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <br />
    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red" Text=" " Visible="False"></asp:Label>
    <FarPoint:FpSpread ID="FpExternal" runat="server" BorderColor="Black" BorderStyle="Solid"
        BorderWidth="1px" Width="900px" HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="AsNeeded">
        <CommandBar ShowPDFButton="false" ButtonType="PushButton" Visible="true">
        </CommandBar>
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AllowSort="false" GridLineColor="Black" BackColor="White">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <center>
        <asp:Label ID="lblerrormsg" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
            Style="position: absolute; left: 50px;"></asp:Label>
        <div id="lastdiv" runat="server">
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:Printcontrol runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </center>
</asp:Content>
