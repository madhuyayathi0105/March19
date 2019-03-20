<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DegreePriority.aspx.cs" Inherits="StudentMod_DegreePriority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="s" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <asp:Label ID="lblpopheader" runat="server" Text="Degree Priority" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
            </center>
            <br />
            <table id="Table1" class="maintablestyle" runat="server">
                <tr>
                    <td>
                        College
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcolhouse" runat="server" Width="250px" CssClass="textbox1 ddlheight4"
                            OnSelectedIndexChanged="ddlcolhouse_Change" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Degree
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updhouse" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDegree" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true"></asp:TextBox>
                                <asp:Panel ID="phouse" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                    <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_degree_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="phousepop" runat="server" TargetControlID="txtDegree"
                                    PopupControlID="phouse" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                            OnClick="btngo_click" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <center>
                <div id="spreadDiv" runat="server" visible="false">
                    <FarPoint:FpSpread ID="Fpspreadpophouse" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="810px" Height="300px" OnCellClick="Cellpophouse_Click"
                        OnPreRender="Fpspreadpophouse_render" OnButtonCommand="Fpspreadpophouse_buttoncommand"
                        CssClass="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <asp:Button ID="btnsetpriority" runat="server" Text="Set Priority" CssClass="textbox textbox1 btn2"
                        OnClick="btnsetpriority_click" />
                    <asp:Button ID="btnresetpriority" runat="server" Text="Reset" CssClass="textbox textbox1 btn2"
                        OnClick="btnresetpriority_click" />
                </div>
            </center>
            <center>
                <div id="alertpop" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                    OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
