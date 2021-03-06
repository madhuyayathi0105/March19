﻿<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Scheme_Admission_Report.aspx.cs" Inherits="Scheme_Admission_Report"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        body
        {
            font-family: Book Antiqua;
            font-size: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function ClearPrint1() {
            document.getElementById('<%=lbl_validation1.ClientID %>').innerHTML = "";
        }
    </script>
    <div>
        <center>
            <div>
                
                <span class="fontstyleheader" style="color: Green;">Scheme Admission Report</span></div>
            <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight5 textbox1" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_batch" Width="80px" runat="server" Text="Batch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_batch" CssClass="ddlheight textbox1" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="ddlheight4 textbox1" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_degree_Selectedindexchange">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="p4" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_branch_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="p4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" runat="server" Width="70px" CssClass="textbox textbox1 txtheight"
                                        ReadOnly="true">-- Select--</asp:TextBox>
                                    <asp:Panel ID="Panel8" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                        PopupControlID="Panel8" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblScheme" Text="Scheme Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtScheme" runat="server" Width="150px" CssClass="textbox textbox1 txtheight"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cb_Scheme" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_Scheme_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_Scheme" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Scheme_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtScheme"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="textbox1 btn1" OnClick="btnGO_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMainErr" runat="server" Visible="false" Font-Names="Book Antiqua"
                    Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                <br />
                <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" ActiveSheetViewIndex="0"
                    ShowHeaderSelection="false" CssClass="spreadborder">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="div_report" runat="server" visible="false">
                    <asp:Label ID="lbl_validation1" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight5"
                        onkeypress="return ClearPrint1()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                        AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" Font-Bold="true" />
                    <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                        CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click"
                        Font-Bold="true" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
        </center>
    </div>
</asp:Content>
