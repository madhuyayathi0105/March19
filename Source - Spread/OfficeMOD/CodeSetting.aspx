<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CodeSetting.aspx.cs" Inherits="OfficeMOD_CodeSetting" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Code Setting</span>
            </div>
        </center>
    </div>
    <br />
    <br />
    <div>
        <center>
            <table class="maintablestyle" style="width: 750px; height: 40px;">
                <tr>
                    <td>
                        <asp:Label ID="lblcol" runat="server" Text="College Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcol_OnSelectedIndexChanged"
                            CssClass="textbox textbox1 ddlheight4">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_frmdate" Text="From Date" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox textbox1" OnTextChanged="txt_frmdate_OnTextChanged"
                            AutoPostBack="true" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="calfrmdate" TargetControlID="txt_frmdate" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lbl_prvdate" Text="Previous Date" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_prvdate" runat="server" CssClass="textbox textbox1" Width="80px"
                            Enabled="false" Visible="false"></asp:TextBox>
                        <asp:CalendarExtender ID="calvacatedate" TargetControlID="txt_prvdate" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:DropDownList ID="ddl_PrevDate" runat="server" CssClass=" textbox  ddlheight2"
                            OnSelectedIndexChanged="ddl_PrevDate_OnSelectedIndexChange" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="txtdateerr" runat="server" Style="color: Red;" Visible="false"></asp:Label>
                    </td>
                </tr>
                <br />
                <tr>
                    <td>
                        Department
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updept" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                    <asp:CheckBox ID="cb_dept" runat="server" AutoPostBack="true" OnCheckedChanged="cb_dept_CheckedChanged"
                                        Text="Select All" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dept_selectedchanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pnlextnder" runat="server" PopupControlID="pnldept"
                                    TargetControlID="txt_dept" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                            OnClick="btnGo_OnClick" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div id="divtable" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <center>
                                <FarPoint:FpSpread ID="Fpload1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                    Style="overflow: auto; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                    box-shadow: 0px 0px 8px #999999;" OnUpdateCommand="Fpload1_UpdateCommand" class="spreadborder">
                                    <%--   OnCellClick="Cell_Click1" OnButtonCommand="Fpload_OnButtonCommand" OnPreRender="Fpspread_render" AutoPostBack="false"--%>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btnSave" Text="Save" Visible="false" runat="server" CssClass="textbox textbox1 btn2"
                            OnClick="btnSave_Click" OnClientClick="return checkvalue1()" />
                        <asp:Button ID="btn_update" Text="Update" Visible="false" OnClick="btnupdate_Click" CssClass="textbox btn2"
                            OnClientClick="return gym()" runat="server" />
                        <asp:Button ID="btn_delete" Text="Delete" Visible="false" OnClick="btndelete_Click" CssClass="textbox btn2"
                            OnClientClick="return gym()" runat="server" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <center>
        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
        <div id="Div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label3" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                            OnClick="Button1_Click" Text="ok" runat="server" />
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
        <div id="Divdelete" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label4" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button2" CssClass=" textbox textbox1 btn1" Style="height: 28px; width: 65px;"
                                            OnClick="Button2_Click" Text="ok" runat="server" />
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
        <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_sure" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_yes" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                        <asp:Button ID="btn_no" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btn_sureno_Click" Text="no" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>
