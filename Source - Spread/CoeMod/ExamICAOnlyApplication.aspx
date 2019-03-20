<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ExamICAOnlyApplication.aspx.cs" Inherits="ExamICAOnlyApplication" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <asp:Label ID="lbl_Header" CssClass="fontstyleheader" runat="server" ForeColor="Green"
            Style="margin: 0px; margin-top: 10px; margin-bottom: 10px; position: relative;"
            Text="Exam ICA Application"></asp:Label>
    </center>
    <center>
        <table style="width: 700px; height: 70px; background-color: #0CA6CA; margin: 0px;
            margin-top: 10px; margin-bottom: 10px; position: relative;">
            <tr>
                <td>
                    <asp:Label ID="lblmonthYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Year And Month" Width="125px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlYear1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        OnSelectedIndexChanged="ddlYear1_SelectedIndexChanged" Font-Size="Medium" Width="60px"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMonth1" runat="server" CssClass="font" OnSelectedIndexChanged="ddlMonth1_SelectedIndexChanged"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltype" Text="Stream" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" Width="128px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree1" runat="server" CssClass="font" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="101px" OnSelectedIndexChanged="ddldegree1_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Branch"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbranch1" runat="server" CssClass="font" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="160px" OnSelectedIndexChanged="ddlbranch1_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Sem"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsem1" runat="server" CssClass="font" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="90px" OnSelectedIndexChanged="ddlsem1_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubtype" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubtype" AutoPostBack="true" Width="150px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubject" AutoPostBack="true" Width="160px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="9">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chksubwise" runat="server" Text="Subject Wise" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True" Style="padding-right: 10px;" AutoPostBack="true"
                                    OnCheckedChanged="chksubwise_CheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lblicatype" runat="server" Text="ICA Type" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlicatype" AutoPostBack="true" Width="130px" runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlicatype_SelectedIndexChanged"
                                    Font-Size="Medium">
                                    <asp:ListItem Text="ICA Repeat" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ICA Retake" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnviewre" runat="server" Text="View Report" OnClick="btnviewre_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerr1" runat="server" Font-Bold="True" ForeColor="Red" Font-Names="Book Antiqua"
        Font-Size="Medium" Visible="false" Style="margin: 0px; margin-top: 10px; margin-bottom: 10px;
        position: relative;"></asp:Label>
    <center>
        <table style="margin: 0px; margin-top: 10px; margin-bottom: 10px; position: relative;
            text-align: center; width:auto; height:auto;">
            <tr>
                <td align="right">
                    <asp:Button ID="btnsave1" runat="server" Font-Bold="true" OnClick="btnsavel1_click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Text="Save" Style="margin: 0px;
                        margin-top: 10px; margin-bottom: 10px; position: relative;" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <FarPoint:FpSpread ID="fpspread" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                        currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                        EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                        OnUpdateCommand="fpspread_UpdateCommand" Style="margin: 0px; margin-top: 10px;
                        margin-bottom: 10px; position: relative;">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
