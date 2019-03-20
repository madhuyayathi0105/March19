<%@ Page Title="" Language="C#" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Hoster_transport_report.aspx.cs" Inherits="Hoster_transport_report"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <center>
                    <center>
                        <br />
                        <div>
                            <span class="fontstyleheader" style="color: #008000;">Admission Status - Hostler and
                                Transport Report</span>
                        </div>
                    </center>
                    <div class="maindivstyle" style="height: auto; width: 1000px;">
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="Institution Name" runat="server" CssClass="txtheight"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1  ddlheight5"
                                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblbatch" Text="Batch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox1  ddlheight" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="180px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                            <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="250px" Height="200px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="p4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="checkdicon" runat="server" Text="Student Catagory" Font-Names="Book Antiqua"
                                        Font-Size="Medium" />
                                    <%--AutoPostBack="true" OnCheckedChanged="checkdicon_Changed"--%>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtinclude" Enabled="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                Style="height: 20px; width: 81px;" CssClass="Dropdown_Txt_Box" runat="server"
                                                ReadOnly="true" Width="145px">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlinclude" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="200px" Style="height: auto;">
                                                <asp:CheckBox ID="cbinclude" runat="server" Text="Select All" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" OnCheckedChanged="cbinclude_OnCheckedChanged" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="cblinclude" runat="server" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    OnSelectedIndexChanged="cblinclude_OnSelectedIndexChanged" AutoPostBack="True">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txtinclude"
                                                PopupControlID="pnlinclude" Position="Bottom">
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
                                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
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
                                <%----  <td colspan="3">
                            <asp:RadioButtonList ID="rblMode" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                              >
                                <asp:ListItem Text="General" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Hostel"></asp:ListItem>
                                <asp:ListItem Text="Transport"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>---%>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            <br />
                        </div>
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" Visible="false" runat="server" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" CssClass="spreadborder" OnCellClick="FpSpread1_CellClick"
                            OnPreRender="FpSpread1_SelectedIndexChanged">
                            <%--Width="966px" Height="500px"--%>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                        <br />
                        <asp:UpdatePanel ID="up_spd1" runat="server">
                            <ContentTemplate>
                                <center>
                                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="1px"
                                        BorderStyle="Solid" BorderColor="Black" CssClass="spreadborder" ActiveSheetViewIndex="0"
                                        Style="margin-left: -5px">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                    <br />
                                    <div id="rptprint1" runat="server" visible="false">
                                        <asp:Label ID="lblvalidation2" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblrptname1" runat="server" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname1" CssClass="textbox textbox1" runat="server" Height="20px"
                                            Width="180px" onkeypress="display()"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txtexcelname1"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel1_Click" CssClass="textbox btn1"
                                            Text="Export To Excel" Width="127px" />
                                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                            CssClass="textbox btn1" />
                                        <Insproplus:printmaster runat="server" ID="Printmaster" Visible="false" />
                                    </div>
                                    <br />
                                </center>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <br />
                </center>
            </div>
        </div>
    </body>
    </html>
</asp:Content>
