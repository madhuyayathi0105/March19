<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="cumm_issued_report.aspx.cs" Inherits="LibraryMod_cumm_issued_report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Cummulative Issued Report</span></div>
        </center>
    </div>
    <div>
        <center>
            <div id="maindiv" runat="server" class="maindivstyle" style="font-family: Book Antiqua;
                font-weight: bold; width: 1000px; height: auto">
                <div>
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                        margin-bottom: 10px; padding: 6px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblreporttype" runat="server" Text="Report Type" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreporttype" runat="server" CssClass="dropdown commonHeaderFont"
                                    Width="150px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtlib" Width="100px" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnl_lib" runat="server" CssClass="multxtpanel" Style="width: 330px;
                                                height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                                <asp:CheckBox ID="chklib" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                    margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chklib_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstlib" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    OnSelectedIndexChanged="chklstlib_SelectedIndexChanged" runat="server" AutoPostBack="True"
                                                    Style="width: 100%; height: auto; padding: 0px; border: 0px;">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="ddllibrary" runat="server" TargetControlID="txtlib"
                                                PopupControlID="pnl_lib" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="deptpnl" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtdep" Width="120px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Style="width: 330px;
                                                height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                                <asp:CheckBox ID="chksdept" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="True" Style="height: auto; margin: 0px;
                                                    padding: 0px; border: 0px;" OnCheckedChanged="chksdept_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstdept" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" Style="width: 50%; height: auto; margin: 0px;
                                                    padding: 0px; border: 0px;" OnSelectedIndexChanged="chklstdept_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="department" runat="server" TargetControlID="txtdep"
                                                PopupControlID="pnldept" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <%---  <tr>
                            <td>
                                <asp:Label ID="lblselectfor" runat="server" Text="Select for " CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlselectfor" runat="server" CssClass="dropdown commonHeaderFont"
                                    Width="100px" AutoPostBack="True" >
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblrollno" runat="server" Text="Roll No:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rolllno" runat="server" Style="height: 20px; width: 80px;">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbcumlative" runat="server" AutoPostBack="true" Text="Cumlative"
                                     />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbnotreturn" runat="server" AutoPostBack="true" Text="Not Return"
                                    />
                            </td>
                            <td colspan="1">
                            <asp:CheckBox ID="cbbatch" runat="server" Visible="false" AutoPostBack="true" />
                                <asp:Label ID="lblbatch" runat="server" Visible="false" Text="Batch Year" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Visible="false"  CssClass="dropdown commonHeaderFont"
                                    Width="100px" AutoPostBack="True" >
                                </asp:DropDownList>
                            </td>
                        </tr>---%>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rbllostbooks" runat="server" Visible="false" RepeatDirection="Horizontal"
                                    AutoPostBack="true">
                                    <asp:ListItem>ReplacebyNewBook</asp:ListItem>
                                    <asp:ListItem>WithFine</asp:ListItem>
                                    <asp:ListItem>All</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <fieldset style="width: 300px; height: 20px;">
                                    <asp:CheckBox ID="cbfrom" runat="server" AutoPostBack="true" OnCheckedChanged="cbfrom_OnCheckedChanged" />
                                    <asp:Label ID="lbl_fromdate1" runat="server" Text="From:"></asp:Label>
                                    <asp:TextBox ID="txt_fromdate1" runat="server" Enabled="false" Style="height: 20px;
                                        width: 75px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_fromdate1" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lbl_todate1" runat="server" Text="To:" Style="margin-left: 4px;"></asp:Label>
                                    <asp:TextBox ID="txt_todate1" runat="server" Enabled="false" Style="height: 20px;
                                        width: 75px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate1" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </fieldset>
                            </td>
                            <td colspan="3">
                                <fieldset style="width: 350px; height: 20px;">
                                    <asp:Label ID="lbl_select" runat="server" Text="Type" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddl_type" runat="server" CssClass="dropdown commonHeaderFont"
                                        Width="100px" AutoPostBack="True">
                                    </asp:DropDownList>
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/LibImages/Go.jpg" Style="margin-top: 10px;"
                                    OnClick="btn_go_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <br />
        <center>
            <div id="showreport2" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:GridView ID="grdCumIssReport" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                Font-Names="Book Antiqua" toGenerateColumns="false" OnRowDataBound="grdCumIssReport_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="black" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <td>
                        <center>
                            <div id="print" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="Red" Text="" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                             <%--   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>--%>
                                <asp:ImageButton ID="btnExcel1" runat="server" ImageUrl="~/LibImages/export to excel.jpg"
                                    OnClick="btnExcel1_Click" />
                                <asp:ImageButton ID="btnprintmaster1" runat="server" ImageUrl="~/LibImages/Print White.jpg"
                                    OnClick="btnprintmaster1_Click" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </div>
                        </center>
                    </td>
                </table>
            </div>
        </center>
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
    </div>
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
</asp:Content>
