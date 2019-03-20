<%@ Page Title="Report Card For Class-I & Class-II" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ReportCard_I_To_II.aspx.cs" Inherits="ReportCard_I_To_II"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Report Card For Class-I & Class-II</title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green">Report Card For Class-I & Class-II</span>
        </div>
        <div style="width: 978px; height: 26px; padding-left: auto; padding-right: auto;
            background-color: Teal; text-align: right; margin-top: 10px;">
        </div>
        <div id="divSearch" runat="server" visible="true" style="width: 978px; height: auto;
            padding: 0px; background-color: #219DA5; border-color: #219DA5; line-height: 27px;">
            <table id="tblsearch" runat="server" style="height: auto; margin: 0px; width: auto;
                background-color: #219DA5; border-color: #219DA5; line-height: 27px;">
                <%--//class="maintablestyle" --%>
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                            AutoPostBack="true" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            AutoPostBack="true" Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDept" runat="server" Style="font-family: 'Book Antiqua';" Text="Department"
                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDept" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlDept_SelectedIndexChanged"
                            AutoPostBack="true" Width="90px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Semester"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlSem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Width="70px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlsem" runat="server" CssClass="multxtpanel" Style="width: auto;
                                        margin: 0px;">
                                        <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_sem_OnCheckedChanged" Font-Bold="True" ForeColor="Black"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; width: auto;" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged"
                                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Style="display: -moz-inline-box; width: 100%;">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_sem" runat="server" TargetControlID="txtSem" PopupControlID="pnlsem"
                                        Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                            AutoPostBack="true" Width="55px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Test" Width="80px" Style="margin-left: 5px;"></asp:Label>
                    </td>
                    <td colspan="11">
                        <table>
                            <tr>
                                <td>
                                    <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlTest" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_test" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Width="70px" runat="server" Font-Size="Medium" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_test" runat="server" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="Cb_test" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        Text="Select All" AutoPostBack="True" OnCheckedChanged="Cb_test_CheckedChanged" />
                                                    <asp:CheckBoxList ID="Cbl_test" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" AutoPostBack="True" OnSelectedIndexChanged="Cbl_test_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_test"
                                                    PopupControlID="pnl_test" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkManualAttendance" Checked="false" runat="server" Font-Bold="True"
                                        Font-Size="Medium" Font-Names="Book Antiqua" Text="Calculate Manual Attendance" />
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Width="59px" CssClass="textbox btn2" Text="Go" OnClick="btnGo_Click" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnlbtnHeaderSettings" runat="server" OnClick="lnlbtnHeaderSettings_Click"
                                        Text="Set Header Settings" Style="font-weight: bold; margin-left: 20px;"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin:0px; margin-top: 10px;
        margin-bottom: 10px; position: relative;"></asp:Label>
    <center>
        <div id="divViewSpread" runat="server" visible="false" style="margin:0px; margin-top: 10px; margin-bottom: 10px;
            position: relative;">
            <table>
                <tr>
                    <td>
                        <FarPoint:FpSpread ID="FpViewSpread" AutoPostBack="false" Width="1000px" runat="server"
                            Visible="true" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" OnButtonCommand="FpViewSpread_Command"
                            ShowHeaderSelection="false" Style="width: 100%; height: auto;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSpreadErr" runat="server" Text="" Visible="false" ForeColor="Red"
                            Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnrpt" runat="server" Height="27px" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Black" Style="background-color: #e6e6e6; box-shadow: 1px 11px 10px -11px;
                            color: darkslategrey; border: 2px solid teal;" Text="Report Card" OnClick="btnrpt_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="divHeaderSettings" runat="server" visible="false" style="height: 100em;
            z-index: 1; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
            top: 0; left: 0px;">
            <center>
                <div id="divHeaderContents" runat="server" class="table" style="background-color: White;
                    height: auto; width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 75px; margin-bottom: 10px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; margin:0px; margin-top: 10px;
        margin-bottom: 10px;">
                            <tr>
                                <td colspan="2" align="center">
                                    <center>
                                        <b style="font-size: 20px; color: Red;">Header Settings </b>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">
                                    <asp:CheckBox ID="chkHeaderBased" runat="Server" Text="Report Header Based on Header Settings"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:GridView ID="gvHeaderSettings" runat="server" Height="200px" Width="400px" AutoGenerateColumns="false"
                                        OnDataBound="gvHeaderSettings_OnDataBound" Style="margin:0px; margin-top: 10px;
        margin-bottom: 10px; position: relative;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHeaderName" runat="server" Text='<%#Eval("Header_Name")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelectHeader" runat="server"></asp:CheckBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <asp:Button ID="btnSaveHeader" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnSaveHeader_Click" Text="Save" runat="server" />
                                        <asp:Button ID="btnResetHeader" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnResetHeader_Click" Text="Reset" runat="server" />
                                        <asp:Button ID="btnCloseHeader" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btnCloseHeader_Click" Text="Close" runat="server" />
                                        <asp:CheckBox ID="chkSelectAllHeader" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="chkSelectAllHeader_OnCheckChange" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div id="popupdiv" runat="server" visible="false" style="height: 100%; z-index: 2;
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
                                <asp:Label ID="lblpoperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
