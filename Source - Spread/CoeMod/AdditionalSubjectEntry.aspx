<%@ Page Title="Additional/Exempted Subject Entry" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AdditionalSubjectEntry.aspx.cs" Inherits="OptionalSubjectsPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Additional/Exempted Subject Entry</title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lblHeading" runat="server" Text="Additional/Exempted Subject Entry"
            CssClass="fontstyleheader" Font-Bold="true" Style="color: Green; margin: 0px;
            margin-bottom: 10px; margin-top: 10px;"></asp:Label>
    </center>
    <center>
        <table style="width: auto; height: auto; padding: 10px; background-color: #0CA6CA;
            border-radius: 5px; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td>
                    <asp:Label ID="lblCollege" Text="Batch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCollege" runat="server" Width="190px" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
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
                    <asp:DropDownList ID="ddlDegree" Width="90px" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" Text="Branch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" Width="130px" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSem" Text="Sem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
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
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSubjectType" Text="Subject Type" runat="server" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSubjectType" runat="server" Width="180px" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjectType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblSubject" Text="Subject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSubject" runat="server" Width="180px" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                </td>
                <td>
                    <asp:Button ID="btnview" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="View" OnClick="btnview_Click" />
                </td>
                <td>
                    <asp:Button ID="btnSettings" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Settings" OnClick="btnSettings_Click" />
                </td>
                <td>
                    <asp:CheckBox ID="cbpassedout" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Passed Out" />
                </td>
            </tr>
        </table>
    </center>
    <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px;"></asp:Label>
    <center>
        <div id="divMainContent" runat="server" visible="false">
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
            <FarPoint:FpSpread ID="FpStudent" AutoPostBack="False" runat="server" Visible="false"
                BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
                OnUpdateCommand="FpStudent_Command" OnButtonCommand="FpStudent_ButtonCommand">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
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
                                            OnClick="btnYes_Click" Text="No" runat="server" />
                                        <asp:Button ID="btnCancel" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnCancel_Click" Text="Cancel" runat="server" />
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
        <div id="divGradeSetting" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divGrade" runat="server" class="table" style="background-color: White; width: 60%;
                    height: 75%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 20%;
                    right: 20%; top: 10%; position: fixed; border-radius: 10px; overflow: auto;">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTotalGrade" Visible="false" AssociatedControlID="txtTotalGrade"
                                        runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="No. of Grades"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalGrade" Visible="false" runat="server" Width="80px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text=""></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filterExtTotalGrade" runat="server" TargetControlID="txtTotalGrade"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass=" textbox btn1 textbox1" Font-Bold="True" Style="height: auto;
                                        width: auto;" OnClick="btnAdd_Click" Text="Add New Row" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <table style="height: auto; width: 100%; padding: 3px; margin: 0px; margin-bottom: 20px;
                            margin-top: 15px;">
                            <tr>
                                <td colspan="4" align="center">
                                    <FarPoint:FpSpread ID="FpGradeSetting" Height="300px" AutoPostBack="False" runat="server"
                                        Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                                        ShowHeaderSelection="false" Style="width: 50%; margin: 0px; margin-bottom: 20px;
                                        margin-top: 15px;">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center" style="margin: 0px; margin-bottom: 20px; margin-top: 15px;">
                                    <asp:Button ID="btnSaveSetting" Font-Bold="True" CssClass=" textbox btn1 textbox1"
                                        Style="height: auto; width: auto;" OnClick="btnSaveSetting_Click" Text="Save"
                                        runat="server" />
                                    <asp:Button ID="btnCloseSetting" Font-Bold="True" CssClass=" textbox btn1 textbox1"
                                        Style="height: auto; width: auto;" OnClick="btnCloseSetting_Click" Text="Exit"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
