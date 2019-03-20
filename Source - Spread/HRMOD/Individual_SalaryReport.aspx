<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Individual_SalaryReport.aspx.cs" Inherits="Individual_SalaryReport" %>

<%@ Register Src="~/Usercontrols/GridPrintMaster.ascx" TagName="GridPrintMaster"
    TagPrefix="InsproplusGrid" %>
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
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById("<%=lblsmserror.ClientID %>").innerHTML = "";
            }

            function checkFloatValue(el) {
                var ex = /^[0-9]+\.?[0-9]*$/;
                if (ex.test(el.value) == false) {
                    el.value = "";
                }
            }

            function pgeval() {
                var val = document.getElementById("<%=txt_pagecount.ClientID %>").value;
                if (val.trim() != "") {
                    if (parseFloat(val) <= 0) {
                        document.getElementById("<%=txt_pagecount.ClientID %>").value = "";
                    }
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Overall / Individual Salary Report</span></div>
                    </center>
                </div>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" width="400px">
                                <tr align="center">
                                    <td>
                                        <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight3" Width="250px"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_Change">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table class="maintablestyle">
                                <tr>
                                    <td colspan="12">
                                        Month & Year
                                        <asp:DropDownList ID="ddl_mon" runat="server" OnSelectedIndexChanged="ddl_mon_Change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight1">
                                            <asp:ListItem Value="1">Jan</asp:ListItem>
                                            <asp:ListItem Value="2">Feb</asp:ListItem>
                                            <asp:ListItem Value="3">Mar</asp:ListItem>
                                            <asp:ListItem Value="4">Apr</asp:ListItem>
                                            <asp:ListItem Value="5">May</asp:ListItem>
                                            <asp:ListItem Value="6">June</asp:ListItem>
                                            <asp:ListItem Value="7">July</asp:ListItem>
                                            <asp:ListItem Value="8">Aug</asp:ListItem>
                                            <asp:ListItem Value="9">Sep</asp:ListItem>
                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                            <asp:ListItem Value="12">Dec</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox1 ddlheight1">
                                        </asp:DropDownList>
                                        Staff Code
                                        <asp:TextBox ID="txtstaffcode" runat="server" OnTextChanged="txtstaff_txtchanged"
                                            AutoPostBack="true" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaffcode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                        Staff Name
                                        <asp:TextBox ID="txtstaffname" runat="server" OnTextChanged="txtname_txtchanged"
                                            AutoPostBack="true" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaffname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <%--<tr>
                                <td colspan="12">
                                    <fieldset style="border-color: Black; border-radius: 5px;">
                                        <asp:CheckBox ID="cb_basicpay" runat="server" Text="Basic Pay" />
                                        <asp:CheckBox ID="cb_gradepay" runat="server" Text="Grade Pay" />
                                        <asp:CheckBox ID="cb_grosspay" runat="server" Text="Gross Pay" />
                                        <asp:CheckBox ID="cb_totded" runat="server" Text="Total Deductions" />
                                        <asp:CheckBox ID="cb_netpay" runat="server" Text="Net Pay" />
                                        <asp:CheckBox ID="cb_pfcont" runat="server" Text="PF Contribution" />
                                        <asp:CheckBox ID="cb_esicont" runat="server" Text="ESI Contribution" />
                                    </fieldset>
                                </td>
                            </tr>--%>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_all" runat="server" Text="Allowances" AutoPostBack="true" OnCheckedChanged="cb_all_checkedchanged" />
                                        <%--<asp:Label ID="lbl_all" runat="server" Text="Allowances" />--%>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upall" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_allow" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Enabled="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="panelall" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                    <asp:CheckBox ID="cb_allow" runat="server" AutoPostBack="true" OnCheckedChanged="cb_allow_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_allow" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_allow_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popall" runat="server" PopupControlID="panelall" TargetControlID="txt_allow"
                                                    Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_deduct" runat="server" Text="Deductions" AutoPostBack="true"
                                            OnCheckedChanged="cb_deduct_checkedchanged" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upded" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_ded" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Enabled="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlded" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                    <asp:CheckBox ID="cb_ded" runat="server" AutoPostBack="true" OnCheckedChanged="cb_ded_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_ded" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_ded_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popded" runat="server" PopupControlID="pnlded" TargetControlID="txt_ded"
                                                    Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbleavecat" runat="server" Text="Leave Category" AutoPostBack="true"
                                            OnCheckedChanged="cbleavecat_checkedchanged" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updlevcat" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtleavecat" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                                    Enabled="false">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnllevcat" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                    <asp:CheckBox ID="chklev" runat="server" AutoPostBack="true" OnCheckedChanged="chklev_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="chklstlev" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstlev_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="poplevcat" runat="server" PopupControlID="pnllevcat"
                                                    TargetControlID="txtleavecat" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
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
                                        Designation
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updesi" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_desig" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnldes" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                    <asp:CheckBox ID="cb_desig" runat="server" AutoPostBack="true" OnCheckedChanged="cb_desig_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_desig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_desig_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="pnldes"
                                                    TargetControlID="txt_desig" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Staff Category
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upstaffcat" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_staffcat" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnl_staffcat" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="200px">
                                                    <asp:CheckBox ID="cb_staffcat" runat="server" AutoPostBack="true" OnCheckedChanged="cb_staffcat_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_staffcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffcat_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="pnl_staffcat"
                                                    TargetControlID="txt_staffcat" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--<td>
                                </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        Staff Type
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updstafftype" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_stafftyp" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlstafftyp" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="200px">
                                                    <asp:CheckBox ID="cb_stafftyp" runat="server" AutoPostBack="true" OnCheckedChanged="cb_stafftyp_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_stafftyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftyp_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" PopupControlID="pnlstafftyp"
                                                    TargetControlID="txt_stafftyp" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        Pay Mode
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updpaymode" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtpaymode" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlpaymode" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                                    <asp:CheckBox ID="cb_paymode" runat="server" AutoPostBack="true" OnCheckedChanged="cb_paymode_CheckedChanged"
                                                        Text="Select All" />
                                                    <asp:CheckBoxList ID="cbl_paymode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_paymode_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popuppaymode" runat="server" PopupControlID="pnlpaymode"
                                                    TargetControlID="txtpaymode" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td colspan="7">
                                        <asp:LinkButton ID="lb_header" runat="server" Font-Size="Large" Text="Header Settings"
                                            OnClick="lb_header_Click"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lb_footer" runat="server" Font-Size="Large" Text="Footer Settings"
                                            OnClick="lb_footer_Click"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lb_print" runat="server" Font-Size="Large" Text="Print Settings"
                                            OnClick="lb_print_Click"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                    </td>
                                    <%--<td>
                                    <asp:TextBox ID="Txtentryfrom" runat="server" Style="margin-bottom: 0px" Height="20px"
                                        Width="75px" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="True" OnTextChanged="Txtentryfrom_TextChanged"
                                        Font-Size="Medium" Visible="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="Txtentryfrom"
                                        FilterType="Custom, Numbers" ValidChars="/" />
                                    <asp:CalendarExtender ID="Txtentryfrom_CalendarExtender" runat="server" TargetControlID="Txtentryfrom"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="To" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtentryto" runat="server" Height="20px" Width="75px" Font-Bold="True"
                                        Font-Names="Book Antiqua" OnTextChanged="Txtentryto_TextChanged" Font-Size="Medium"
                                        Visible="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txtentryto"
                                        FilterType="Custom, Numbers" ValidChars="/" />
                                    <asp:CalendarExtender ID="Txtentryto_CalendarExtender" runat="server" TargetControlID="Txtentryto"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>--%>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        Page Name
                                        <asp:Button ID="btnplus" runat="server" Text="+" CssClass="textbox textbox1 btn"
                                            OnClick="btnplus_click" />
                                        <asp:DropDownList ID="ddladdreason" runat="server" OnSelectedIndexChanged="ddladdreason_Change"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight3">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnminus" runat="server" Text="-" CssClass="textbox textbox1 btn"
                                            OnClick="btnminus_click" />
                                        &nbsp;
                                        <asp:CheckBox ID="chk_amnt" runat="server" Text="Include Zero Amount" />
                                        &nbsp;
                                        <asp:CheckBox ID="chk_loandet" runat="server" Text="Include Loan Detail" />
                                        &nbsp;
                                        <asp:CheckBox ID="chkAccNo" runat="server" Checked="false" Font-Names="Book Antiqua"
                                            Text="OrderBy Acc No" />
                                        &nbsp;
                                        <asp:CheckBox ID="chkShowPF" runat="server" Visible="false" Checked="false" Font-Names="Book Antiqua"
                                            Text="Show PF Salary" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_collBank" runat="server" Text="College Bank" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_colbank" runat="server" Width="200px" 
                                            Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        <asp:LinkButton ID="lnk_btn_print" runat="server" Text="Salary Certificate Print Setting"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="lnk_btn_print_click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:CheckBox ID="chkIncLopAmnt" runat="server" Text="Include LOP Amount" Checked="false"
                                            Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        &nbsp;
                                        <asp:CheckBox ID="chksms" runat="server" Text="SMS" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        &nbsp;
                                        <asp:CheckBox ID="chkmail" runat="server" Text="E-Mail" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                        &nbsp;
                                        <asp:CheckBox ID="cb_partimestaff" runat="server" Text="Part Time Staff" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="cb_parttimestaff_checkedchange" />
                                        &nbsp; &nbsp;
                                        <asp:CheckBox ID="cb_relived" runat="server" Text="Relieving Staff" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="cb_relived_checkedchange" />
                                        &nbsp;
                                        <asp:CheckBox ID="cb_hold" runat="server" Text="Include Salary Hold Staff" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="SalaryHold_checkedchange" />
                                        &nbsp;
                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_click" CssClass="textbox textbox1 btn1" />
                                        &nbsp;
                                        <asp:Button ID="btnpayslip" runat="server" Text="Pay Slip" BackColor="LightGreen"
                                            OnClick="btnpayslip_click" CssClass="textbox textbox1 btn2" Width="115px" />
                                        &nbsp;
                                        <asp:Button ID="btnsalcer" runat="server" Text="Salary Certificate" BackColor="LightGreen"
                                            OnClick="btnsalcer_click" CssClass="textbox textbox1 btn2" Width="115px" />
                                        <asp:Label ID="lblorder" runat="server" Text="Order by"></asp:Label>
                                        <asp:DropDownList ID="ddlorder" runat="server" CssClass="textbox1 ddlheight2">
                                            <asp:ListItem Text="Dept & Staff Code"></asp:ListItem>
                                            <asp:ListItem Text="Priority"></asp:ListItem>
                                            <asp:ListItem Text="Print Priority-1"></asp:ListItem>
                                            <asp:ListItem Text="Print Priority-2"></asp:ListItem>
                                            <asp:ListItem Text="Account No"></asp:ListItem>
                                            <asp:ListItem Text="Staff Wise Priority"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <asp:UpdatePanel ID="updcolumn" runat="server">
                        <ContentTemplate>
                            <div>
                                <br />
                                <center>
                                    <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                        Width="920px" Style="margin-top: -0.1%;">
                                        <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                        <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                    </asp:Panel>
                                </center>
                                <br />
                            </div>
                            <center>
                                <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="920px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                    Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:TextBox ID="tborder" Visible="true" ReadOnly="true" Width="891px" TextMode="MultiLine"
                                                    CssClass="style1" AutoPostBack="true" runat="server" Enabled="true">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                                    Width="920px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                    RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">SELECT</asp:ListItem>
                                                    <asp:ListItem Value="1">STAFF CODE</asp:ListItem>
                                                    <asp:ListItem Value="2">NAME</asp:ListItem>
                                                    <asp:ListItem Value="3">DESIGNATION</asp:ListItem>
                                                    <asp:ListItem Value="4">DEPT</asp:ListItem>
                                                    <asp:ListItem Value="5">DEPT ACR</asp:ListItem>
                                                    <asp:ListItem Value="6">DESIGN ACR</asp:ListItem>
                                                    <asp:ListItem Value="7">DATE OF BIRTH</asp:ListItem>
                                                    <asp:ListItem Value="8">DATE OF APPOINTED</asp:ListItem>
                                                    <asp:ListItem Value="9">DATE OF JOINING</asp:ListItem>
                                                    <asp:ListItem Value="10">DATE OF RETIREMENT</asp:ListItem>
                                                    <asp:ListItem Value="11">DATE OF INCREMENT</asp:ListItem>
                                                    <asp:ListItem Value="12">LOAN DETAILS</asp:ListItem>
                                                    <asp:ListItem Value="13">CATEGORY</asp:ListItem>
                                                    <asp:ListItem Value="14">STAFF TYPE</asp:ListItem>
                                                    <asp:ListItem Value="15">PAY MODE</asp:ListItem>
                                                    <asp:ListItem Value="16">BANK NAME</asp:ListItem>
                                                    <asp:ListItem Value="17">BRANCH NAME</asp:ListItem>
                                                    <asp:ListItem Value="18">BANK ACCTYPE</asp:ListItem>
                                                    <asp:ListItem Value="19">IFSC CODE</asp:ListItem>
                                                    <asp:ListItem Value="20">BANK ACCOUNT NO</asp:ListItem>
                                                    <asp:ListItem Value="21">PF NO</asp:ListItem>
                                                    <asp:ListItem Value="22">ESI NO</asp:ListItem>
                                                    <asp:ListItem Value="23">PAN NO</asp:ListItem>
                                                    <asp:ListItem Value="24">LIC NO</asp:ListItem>
                                                    <asp:ListItem Value="25">AADHAR NO</asp:ListItem>
                                                    <asp:ListItem Value="26">LOAN NO</asp:ListItem>
                                                    <asp:ListItem Value="27">GPF NO</asp:ListItem>
                                                    <asp:ListItem Value="28">UAN NO</asp:ListItem>
                                                    <asp:ListItem Value="29">LOP DAYS</asp:ListItem>
                                                    <asp:ListItem Value="30">LOP DATES</asp:ListItem>
                                                    <asp:ListItem Value="31">WORKING DAYS</asp:ListItem>
                                                    <asp:ListItem Value="32">ABSENT DAYS</asp:ListItem>
                                                    <asp:ListItem Value="33">PRESENT DAYS</asp:ListItem>
                                                    <asp:ListItem Value="34">NO. OF INCREMENT</asp:ListItem>
                                                    <asp:ListItem Value="35">LOP AMOUNT</asp:ListItem>
                                                    <asp:ListItem Value="36">PF Salary</asp:ListItem>
                                                    <asp:ListItem Value="37">ESI Salary</asp:ListItem>
                                                    <asp:ListItem Value="38">ADV RS.</asp:ListItem>
                                                    <asp:ListItem Value="39">COLLECTED AMT</asp:ListItem>
                                                    <asp:ListItem Value="40">DA %</asp:ListItem>
                                                    <%--<asp:ListItem Value="30">Basic</asp:ListItem>--%>
                                                    <asp:ListItem Value="41">AGP</asp:ListItem>
                                                    <asp:ListItem Value="42">INCREMENT IN RS.</asp:ListItem>
                                                    <asp:ListItem Value="43">ACTUAL BASIC</asp:ListItem>
                                                    <asp:ListItem Value="44">BASIC PAY Rs.</asp:ListItem>
                                                    <asp:ListItem Value="45">PAY BAND</asp:ListItem>
                                                    <asp:ListItem Value="46">ACTUAL GRADE</asp:ListItem>
                                                    <asp:ListItem Value="47">GRADE PAY</asp:ListItem>
                                                    <asp:ListItem Value="48">TOT DED Rs.</asp:ListItem>
                                                    <asp:ListItem Value="49">GROSS PAY Rs.</asp:ListItem>
                                                    <asp:ListItem Value="50">ACTUAL GROSS SALARY</asp:ListItem>
                                                    <asp:ListItem Value="51">PAY SCALE</asp:ListItem>
                                                    <asp:ListItem Value="52">TITLE</asp:ListItem>
                                                    <%--<asp:ListItem Value="41">Department With Pay Scale</asp:ListItem>--%>
                                                    <asp:ListItem Value="53">NET PAY</asp:ListItem>
                                                    <asp:ListItem Value="54">BANK FORMAT</asp:ListItem>
                                                    <asp:ListItem Value="55">SIGNATURE</asp:ListItem>
                                                    <%--delsi--%>
                                                    <asp:ListItem Value="56">One Day Salary</asp:ListItem>
                                                    <asp:ListItem Value="57">COLLEGE BANK</asp:ListItem>
                                                   
                                                    <asp:ListItem Value="58" Enabled="false">HOUR AMT</asp:ListItem>
                                                    <asp:ListItem Value="59" Enabled="false">CONVENES EXP</asp:ListItem>
                                                    <asp:ListItem Value="60" Enabled="false">LUNCH EXP</asp:ListItem>
                                                    <asp:ListItem Value="61">GRATUITY</asp:ListItem>
                                                    <asp:ListItem Value="62">YEAR OF EXP</asp:ListItem>
                                                </asp:CheckBoxList>
                                                <asp:ListBox ID="lstcolorder" runat="server" Visible="false"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </center>
                            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                                CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                                TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                                ExpandedImage="../images/down.jpeg">
                            </asp:CollapsiblePanelExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        ForeColor="Red" Visible="False"></asp:Label>
                    <br />
                    <br />
                    <center>
                        <div id="div1" runat="server" visible="false" style="border-radius: 10px; overflow: auto;">
                            <FarPoint:FpSpread ID="fpsalary" runat="server" OnCellClick="Cell_Click" Width="850px"
                                CssClass="spreadborder" OnButtonCommand="fpsalary_ButtonCommand" ShowHeaderSelection="false">
                                <%--OnPreRender="fpsalary_render" --%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                        BackColor="White">
                                        <%--AutoPostBack="true"--%>
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <div id="rprint" runat="server">
                        <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                            Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="False"></asp:Label>
                        <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel" OnClick="btnexcel_Click" />
                         <asp:Button ID="btnPrintNew" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Text="Print New" OnClick="btnPrintNew_Click" CssClass="textbox textbox1 btn2"
                            Width="100px" />
                        <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                            Width="100px" />
                        <asp:Button ID="btnprintset" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Text="printsett" OnClick="btnprintset_click" CssClass="textbox textbox1 btn2"
                            Width="100px" />
                        <asp:Button ID="btn_finalsettlement" runat="server" Font-Bold="true" Visible="false"
                            Font-Names="Book Antiqua" Text="Final Settlement" OnClick="btnFinalSettlement_click"
                            CssClass="textbox textbox1 btn2" Width="115px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                               <InsproplusGrid:GridPrintMaster runat="server" ID="GridPrintmaster" Visible="false" />
                    </div>
                    <br />
                    <br />
                    <asp:UpdatePanel ID="updsms" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsmstype" Text="SMS Type" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsmstype" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="True" AutoPostBack="true" OnSelectedIndexChanged="ddlsmstype_SelectedIndexChanged">
                                            <asp:ListItem Text="Automatic"></asp:ListItem>
                                            <asp:ListItem Text="Template With Salary"></asp:ListItem>
                                            <asp:ListItem Text="Template With Out Salary"></asp:ListItem>
                                            <asp:ListItem Text="Template With LOP"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblpurpose1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Purpose"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlpurpose" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="300px" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="left">
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" Height="250px" Width="1000px" OnCellClick="FpSpread2_CellClick"
                                            OnPreRender="FpSpread2_SelectedIndexChanged">
                                            <%--<CommandBar BackColor="Control" ButtonType="PushButton">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>--%>
                                            <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" />
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" SelectionBackColor="LightGreen"
                                                    SelectionForeColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="templatepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                Visible="false" BorderWidth="2px" Height="390px" Width="690px">
                                <div class="PopupHeaderrstud2" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                    font-size: Small; font-weight: bold">
                                    <table>
                                        <caption>
                                            <br />
                                            <br />
                                            <br />
                                            <caption>
                                                Message Template</caption>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblpurpose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" ForeColor="Black" Text="Purpose" Width="100px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnsum" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnsum_Click" Text=" + " />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlpurposemsg" runat="server" AutoPostBack="True" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btndiff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btndiff_Click" Text=" - " />
                                                </td>
                                            </tr>
                                        </caption>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtpurposemsg" runat="server" TextMode="MultiLine" Height="200px"
                                                    Width="680px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnTextChanged="txtpurposemsg_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnsavepur" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnClick="btnsavepur_Click" Height=" 26px" Width=" 88px" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnexitpur" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Height=" 26px" Width=" 88px" OnClick="btnexitpur_Click" />
                                            </td>
                                        </tr>
                                        <caption>
                                            <br />
                                            <br />
                                            <br />
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblerrorpur" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" ForeColor="Red" Style="top: 340px; left: 5px; position: absolute;
                                                        height: 21px" Width="676px"></asp:Label>
                                                </td>
                                            </tr>
                                        </caption>
                                    </table>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="purposepanel" runat="server" BorderColor="Black" BackColor="AliceBlue"
                                Visible="false" BorderWidth="2px" Height="100px" Width="300px">
                                <div class="panelinfraction" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                                    font-size: Small; font-weight: bold">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpurtype" runat="server" Text="Purpose Type" Style="text-align: center;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpurposecaption" runat="server" Text="Purpose" Style="font-size: medium;
                                                    font-weight: bold; height: 22px; font-family: 'Book Antiqua'; position: absolute;
                                                    top: 21px; left: 10px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpurposecaption" runat="server" Style="font-size: medium; font-weight: bold;
                                                    height: 22px; font-family: 'Book Antiqua';"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnpurposeadd" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                    height: 26px;" OnClick="btnpurposeadd_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnpurposeexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                                    height: 26px; width: 88px;" OnClick="btnpurposeexit_Click" />
                                            </td>
                                        </tr>
                                    </table>
                            </asp:Panel>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnaddtemplate" runat="server" Text="Add Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnaddtemplate_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btndeletetemplate" runat="server" Text="Delete Template" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btndeletetemplate_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:TextBox ID="txtsms" runat="server" TextMode="MultiLine" Text="" Font-Names="Book Antiqua"
                                Width="500px" Height="100px" Font-Size="Medium" Font-Bold="true" Visible="false"
                                MaxLength="1000">
                            </asp:TextBox>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnsendsms" runat="server" Text="Send" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" OnClick="btnsendsms_Click" Visible="false" />
                    <br />
                    <br />
                    <asp:Panel ID="emailpanel" runat="server" Visible="false">
                        <table id="Tablenote" runat="server">
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lblnotification" Text="E-Mail" Font-Size="Large" Font-Names="Book Antiqua"
                                        runat="server" Font-Bold="true" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px;" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsubject" runat="server" Style="display: inline-block; color: Black;
                                        font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 500px;"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnote" Text="Content" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px;" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbodycontant" TextMode="MultiLine" runat="server" MaxLength="4000"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold; width: 500px; height: 300px;" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <br />
                    <%--<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button2"
                    CancelControlID="Button1" PopupControlID="Panel4" PopupDragHandleControlID="PopupHeader"
                    Drag="true" BackgroundCssClass="ModalPopupBG">
                </asp:ModalPopupExtender>--%>
                    <asp:Panel ID="Panel4" runat="server" Width="1000px" Height="550px" ScrollBars="Auto"
                        BorderColor="Black" BorderStyle="Double" Style="display: none;">
                        <div class="HellowWorldPopup">
                            <div class="PopupHeader" id="Div4" style="text-align: center; color: Blue; font-family: Book Antiqua;
                                font-size: xx-large; font-weight: bold">
                            </div>
                            <div class="PopupBody">
                            </div>
                            <div class="Controls">
                                <center>
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Height="245px" Width="950px" Visible="False" VerticalScrollBarPolicy="Never"
                                        ActiveSheetViewIndex="0" Style="background-color: White;">
                                        <%--<CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton">
                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                    </CommandBar>--%>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                                EditTemplateColumnCount="2" GridLineColor="white" GroupBarText="Drag a column to group by that column."
                                                SelectionBackColor="#EAECF5" AutoPostBack="True">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                            VerticalAlign="NotSet" />
                                    </FarPoint:FpSpread>
                                    <center>
                                        <asp:Button ID="Button1" runat="server" Text="Close" />
                                        <br />
                    </asp:Panel>
                    <br />
                </div>
            </center>
            <asp:HiddenField ID="hidden2" runat="server" />
            <asp:ModalPopupExtender ID="modalpopupex1" runat="server" TargetControlID="hidden2"
                PopupControlID="jairam">
            </asp:ModalPopupExtender>
            <asp:Panel ID="jairam" runat="server" CssClass="pop" Style="display: none; height: 500;
                width: 250;" DefaultButton="btnsmsok">
                <table width="350">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="td2">
                            <asp:Label ID="lblconformationmsg" runat="server" Font-Bold="True" Text="Confirmation"
                                Font-Names="Book Antiqua" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="img2" runat="server" ImageUrl="~/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblsmstechmsg" runat="server" Font-Bold="True" Text="" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsmsok" runat="server" Text="Yes" OnClick="btnsmsok_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 170px; position: absolute;
                                top: 81px;" />
                            <asp:Button ID="btnsmscancel" runat="server" Text="No" OnClick="btnsmscancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 232px;
                                position: absolute; top: 81px;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField ID="hidden3" runat="server" />
            <asp:ModalPopupExtender ID="modalpoppupemail" runat="server" TargetControlID="hidden3"
                PopupControlID="panelsuper">
            </asp:ModalPopupExtender>
            <asp:Panel ID="panelsuper" runat="server" CssClass="pop" Style="display: none; height: 500;
                width: 250;" DefaultButton="btnemailok">
                <table width="350">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="td1">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblemailalart" runat="server" Font-Bold="True" Text="" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnemailok" runat="server" Text="Yes" OnClick="btnemailok_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 170px;
                                position: absolute; top: 81px;" />
                            <asp:Button ID="btnemailcancel" runat="server" Text="No" OnClick="btnemailcancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 232px;
                                position: absolute; top: 81px;" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lopdatehidden" Text="" runat="server"></asp:Label>
            </asp:Panel>
            <center>
                <div id="popheader" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 307px;"
                        OnClick="ImageButton2_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 650px;
                        height: 315px;" align="center">
                        <br />
                        <br />
                        <br />
                        <center>
                            <table style="width: 540px;" runat="server" id="collinfo">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <asp:CheckBox ID="chkselall" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chkselall_CheckedChanged" />
                                            <asp:CheckBoxList ID="chkcollege" runat="server" Height="43px" AutoPostBack="true"
                                                RepeatColumns="3" RepeatDirection="Vertical">
                                                <asp:ListItem Value="0" Text="College Name"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="University"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Affliated By"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Address"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="City"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="District & State & Pincode"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Phone No & Fax"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Email & Web Site"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Right Logo"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Left Logo"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnsavehead" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                OnClick="btnsavehead_Click" />
                            <asp:Button ID="btnexithead" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                OnClick="btnexithead_Click" />
                        </center>
                    </div>
                </div>
                <div id="popfooter" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 243px;"
                        OnClick="ImageButton1_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 525px;
                        height: 315px;" align="center">
                        <br />
                        <br />
                        <br />
                        <center>
                            <table style="width: 330px;" runat="server" id="Table1">
                                <tr>
                                    <td>
                                        Footer1
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot0" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer2
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot1" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer3
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot2" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer4
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot3" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Footer5
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtfoot4" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnsavefoot" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                OnClick="btnsavefoot_Click" />
                            <asp:Button ID="btnexitfoot" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                OnClick="btnexitfoot_Click" />
                        </center>
                    </div>
                </div>
                <div id="popprint" runat="server" visible="false" style="height: 50em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 33px; margin-left: 243px;"
                        OnClick="ImageButton3_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; overflow: auto; width: 525px;
                        height: 450px;" align="center">
                        <br />
                        <br />
                        <center>
                            <table style="width: 330px;" runat="server" id="Table2">
                                <tr>
                                    <td>
                                        Page Size
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pagesize" runat="server" CssClass="textbox1 ddlheight">
                                            <asp:ListItem Value="0" Text="A4"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="A3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Page Row Count
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pagecount" runat="server" MaxLength="2" onkeyup="pgeval();"
                                            onblur="pgeval();" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="" TargetControlID="txt_pagecount">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Set Cell Padding
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsetpadd" runat="server" MaxLength="2" onkeyup="pgeval();" onblur="pgeval();"
                                            CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="" TargetControlID="txtsetpadd">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbasic" runat="server" Text="Basic Pay (>=)" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbasic" runat="server" Visible="false" placeholder="0.00" onkeyup="checkFloatValue(this)"
                                            CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterbas" runat="server" FilterMode="ValidChars"
                                            FilterType="Custom,Numbers" ValidChars="." TargetControlID="txtbasic">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td>
                                    Page Name
                                </td>--%>
                                    <td>
                                        <asp:TextBox ID="txt_pagename" runat="server" Visible="false" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk_pfrepo" runat="server" Text="If PF Report" AutoPostBack="true"
                                            OnCheckedChanged="chk_pfrepo_checked" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <fieldset>
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_pageno" runat="server" Text="Include Page No" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_showheader" runat="server" Text="Show Header All Pages" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_showfooter" runat="server" Text="Show Footer All Pages" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chk_grandtot" runat="server" Text="Begins With Total" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="cbincbaslop" runat="server" Text="Include Basic+LOP With Staff Name" />
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="chk_grandtotall" runat="server" Text="Grand Total All Pages" />
                                                </td>
                                            </tr>--%>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnprintsave" runat="server" CssClass="textbox textbox1 btn2" Text="Save"
                                OnClick="btnprintsave_Click" />
                            <asp:Button ID="btnprintexit" runat="server" CssClass="textbox textbox1 btn2" Text="Exit"
                                OnClick="btnprintexit_Click" />
                        </center>
                    </div>
                </div>
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
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <div id="popdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="addreason" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblpagename" runat="server" Text="Add Page Name" Style="color: Green;"
                                                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txtaddpage" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnaddreason" CssClass="textbox textbox1 btn1" OnClick="btnaddreason_Click"
                                                    Text="Add" runat="server" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                                                <asp:Button ID="btnexitreason" CssClass="textbox textbox1 btn1" OnClick="btnexitreason_Click"
                                                    Text="Exit" runat="server" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <center>
                    <div id="printpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div5" runat="server" class="table" style="background-color: White; height: 147px;
                                width: 555px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: auto; width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_print" runat="server" Text="Footer Name" Style="color: Black;
                                                    width: 165px;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_print" runat="server" Width="425px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:Button ID="btn_printSave" CssClass=" textbox1 btn2" OnClick="btnsavePrint_Click"
                                        Text="Save" runat="server" />
                                    <asp:Button ID="btn_printexit" CssClass=" textbox1 btn2" OnClick="btnexitPrint_Click"
                                        Text="Exit" runat="server" />
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="img_div1" runat="server" visible="false" style="height: 150em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="alertdiv" runat="server" class="table" style="background-color: White; height: auto;
                                width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: auto; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblsavealert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass="textbox textbox1 btn2" Width="50px" OnClick="btnerrclose_Click"
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
                <div id="popconfirm" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="confirmdiv" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="width: 100%; height: 100px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalertconfirm" runat="server" Text="" Style="color: Red;" Font-Names="Book Antiqua"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnyes" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnno" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnno_Click" Text="No" runat="server" />
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
    </body>
    </html>
</asp:Content>
