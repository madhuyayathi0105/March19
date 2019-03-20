<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FeesStructureReport.aspx.cs" Inherits="FeesStructureReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }       
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Fees Structure Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_str1" runat="server" Text="Stream"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstream" runat="server" Enabled="false" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlstream_OnSelectedIndexChanged" CssClass="textbox  ddlheight"
                                Style="width: 108px;">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Batch
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_batch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="panel_batch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UP_degree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="panel_degree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Up_dept" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 171px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                        PopupControlID="panel_dept" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel10" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="Panel10" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                        </td>
                        <td>
                            <%-- <asp:DropDownList ID="ddlheader" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            OnSelectedIndexChanged="ddlheader_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>--%>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txthedg" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlhedg" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 248px;
                                        height: auto;">
                                        <asp:CheckBox ID="cbhedg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbhedg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblhedg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblhedg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txthedg"
                                        PopupControlID="pnlhedg" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtledg" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                        Style="width: 248px; height: auto;">
                                        <asp:CheckBox ID="cbledg" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cbledg_OnCheckedChanged" />
                                        <asp:CheckBoxList ID="cblledg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblledg_OnSelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txtledg"
                                        PopupControlID="pnl_studled" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2" id="tdhd" runat="server" visible="false">
                            <asp:RadioButton ID="rbheader" runat="server" Visible="false" Text="HeaderWise" AutoPostBack="true"
                                GroupName="header" OnCheckedChanged="rbheader_Changed" />
                            <asp:RadioButton ID="rbledger" runat="server" AutoPostBack="true" Visible="false"
                                Text="LedgerWise" GroupName="header" OnCheckedChanged="rbledger_Changed" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_seat" runat="server" Text="Seat Type"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_seat" runat="server" Height="15px" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                        height: auto;">
                                        <asp:CheckBox ID="cb_seat" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_seat_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_seat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_seat_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_seat"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtfyear" Style="height: 20px; width: 174px;" CssClass="Dropdown_Txt_Box"
                                        runat="server" ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Pfyear" runat="server" CssClass="multxtpanel" Width="178px" Style="height: atuo;">
                                        <asp:CheckBox ID="chkfyear" runat="server" Text="Select All" OnCheckedChanged="chkfyear_changed"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsfyear" runat="server" OnSelectedIndexChanged="chklsfyear_selected"
                                            AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtfyear"
                                        PopupControlID="Pfyear" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="rblMode_Selected">
                                <asp:ListItem Text="Header" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Ledger"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
            </center>
            <center>
                <%-- <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">--%>
                <table>
                    <tr>
                        <td>
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                                BorderWidth="0px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                    CssClass="textbox textbox1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                            </div>
                        </td>
                    </tr>
                </table>
                <%--  </div>--%>
            </center>
        </div>
        <center>
            <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
