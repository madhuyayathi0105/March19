<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="OptionalSubjectsPage.aspx.cs" Inherits="OptionalSubjectsPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Optional Subject Page</title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="lbl" runat="server" Text="Optional Subject Creation" Font-Bold="true"
            Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
    </center>
    <center>
        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblBatch" Text="Batch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" Text="Degree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" Text="Branch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Sem" Text="Sem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSec" Text="Sec" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSec" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSubject" Text="Subject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSubject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbpassedout" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Passed Out" Width="110px" />
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                </td>
                <td>
                    <asp:Button ID="btnview" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="View" OnClick="btnview_Click" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpStudent" AutoPostBack="False" runat="server" Visible="false"
            BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
            OnUpdateCommand="FpStudent_Command" OnButtonCommand="FpStudent_ButtonCommand">
            <%--OnButtonCommand="FpStudent_ButtonCommand"--%>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" Text="Delete" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </center>
    <div id="errdiv" runat="server" visible="false" style="height: 150em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0px;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-radius: 10px; margin-top: 200px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_popuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                        OnClick="btn_errorclose_Click" Text="Ok" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
</asp:Content>
