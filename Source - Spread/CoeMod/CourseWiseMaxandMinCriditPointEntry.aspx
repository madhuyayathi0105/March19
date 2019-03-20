<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CourseWiseMaxandMinCriditPointEntry.aspx.cs"
    Inherits="CoeMod_CourseWiseMaxandMinCriditPointEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lbl" runat="server" Text="Course Wise Max and Min Credit Entry" Font-Bold="true"
            Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="lblCollege" Text="College" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBatch" Text="Batch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" Enabled="false" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" Text="Degree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" Width="100px" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" Text="Branch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" Width="200px" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="cbBatchWise" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="BatchWise" Width="110px" AutoPostBack="true" OnCheckedChanged="cbBatchWise_Change" />
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
    <center>
        <FarPoint:FpSpread ID="FpStudent" AutoPostBack="False" Width="900px" runat="server"
            Visible="false" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
            ShowHeaderSelection="false">
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
