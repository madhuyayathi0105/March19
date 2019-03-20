<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ITCalCulationSettings.aspx.cs" Inherits="HRMOD_ITCalCulationSettings" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <br />
                <div>
                    <span class="fontstyleheader" style="color: Green;">Income Tax Calculation</span></div>
            </center>
            <fieldset id="maindiv" runat="server" style="width: 978px; height: 1100px; border-color: silver;
                border-radius: 10px;">
                <fieldset style="height: 60px; width: 960px; border: 1px solid #0ca6ca; border-radius: 10px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College Name : " Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="356px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_scode" runat="server" Text="Staff Code" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_scode" runat="server" CssClass="textbox textbox1" MaxLength="10"
                                    Style="font-weight: bold; width: 100px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_sname" runat="server" CssClass="textbox textbox1" MaxLength="50"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <td colspan="6">
                            <%--<asp:LinkButton ID="lnkitsetting" runat="server" Text="IT Calculation Settings" OnClick="lnkitsetting_click"
                                        Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkotherallowance" runat="server" Text="Other Allowance and Deduction"
                                        OnClick="lnkotherallowance_click" Font-Bold="true" Font-Size="Large" Font-Names="Book Antiqua"></asp:LinkButton>--%>
                            <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" CssClass="textbox textbox1 btn2"
                                Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                            <asp:Button ID="btnIncomeHead" runat="server" Text="Other Income" OnClick="btnIncomeHead_Click"
                                CssClass="textbox textbox1 btn2" Width="120px" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                            <asp:Button ID="btnDeductionHead" runat="server" Text="Other Deduction" OnClick="btnDeductionHead_Click"
                                CssClass="textbox textbox1 btn2" Width="140px" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                            <asp:Button ID="btnHouseRentpaid" runat="server" Text="House Rent Amount" OnClick="btnHouseRentpaid_Click"
                                CssClass="textbox textbox1 btn2" Width="170px" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />

                                   <asp:Button ID="btnreinvestment" runat="server" Text="Reimbursement" OnClick="btnReinvestment_Click"
                                CssClass="textbox textbox1 btn2" Width="170px" Style="font-weight: bold; font-family: book antiqua;
                                font-size: medium;" />
                        </td>
                    </table>
                    <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                        box-shadow: 0 0 8px #999999; height: 45px; margin-left: 0px; margin-top: 8px;
                        padding: 1em; margin-left: 0px; width: 924px; display: none;">
                        <table style="margin-top: -14px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_desig" runat="server" Text="Designation" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P2" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_desig" runat="server" Text="Select All" OnCheckedChanged="cb_desig_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_desig" runat="server" OnSelectedIndexChanged="cbl_desig_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_desig"
                                                PopupControlID="P2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P3" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                                <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                                PopupControlID="P3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_stype" runat="server" Text="Staff Type" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P4" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_stype" runat="server" Text="Select All" OnCheckedChanged="cb_stype_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_stype" runat="server" OnSelectedIndexChanged="cbl_stype_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_stype"
                                                PopupControlID="P4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_stat" runat="server" Text="Staff Status" Style="font-weight: bold;
                                        font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stat" runat="server" ReadOnly="true" CssClass="textbox textbox1 txtheight1"
                                                Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                            <asp:Panel ID="P5" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_stat" runat="server" Text="Select All" OnCheckedChanged="cb_stat_CheckedChange"
                                                    AutoPostBack="true" />
                                                <asp:CheckBoxList ID="cbl_stat" runat="server" OnSelectedIndexChanged="cbl_stat_SelectedIndexChange"
                                                    AutoPostBack="true">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_stat"
                                                PopupControlID="P5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    <asp:Button ID="btnsetOtherAllow" runat="server" Text="Set Other Allowance" OnClick="btnsetOtherAllow_Click"
                                        CssClass="textbox textbox1 btn2" Width="104px" Style="font-weight: bold; font-family: book antiqua;
                                        font-size: medium;" />
                                </td>
                                <td>
                                    <asp:Button ID="btnsetotherDeduction" runat="server" Text="Set Other Deduction" OnClick="btnsetotherDeduction_Click"
                                        CssClass="textbox textbox1 btn2" Width="104px" Style="font-weight: bold; font-family: book antiqua;
                                        font-size: medium;" />
                                </td>
                            </tr>--%>
                        </table>
                    </fieldset>
                </fieldset>
                </br>
                <center>
                    <asp:Label ID="lbl_alert" runat="server" Visible="false" Style="color: red; font-weight: bold;
                        font-family: book antiqua; font-size: medium;"></asp:Label>
                </center>
                <br />
                <div id="sp_div" runat="server">
                    <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Width="980px" Height="800px" Style="margin-left: 2px;"
                        class="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <br />
                <center>
                    <div id="rprint" runat="server" visible="false">
                        <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                            Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel"
                            OnClick="btnexcel_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                            Width="100px" />
                        <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
                    </div>
                </center>
                <br />
            </fieldset>
            <div id="DivAddIncomeHead" runat="server" visible="false" style="height: 43em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="imgyear" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 10px; margin-left: 435px;"
                    OnClick="imgyear_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 900px;
                    height: 620px;" align="center">
                    <div id="addinc_div" runat="server" style="border: 2px solid indigo; border-radius: 10px;
                        height: 440px; width: 880px;">
                        <br />
                        <center>
                            <asp:Label ID="lbl_addincome" runat="server" Text="Other Income Head" Style="font-weight: bold;
                                font-family: book antiqua; font-size: large; color: green;"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <%--poomalar--%>
                                        <asp:Label ID="lblHead" runat="server" Text="" Visible="true"></asp:Label>
                                        <%--Income Head--%>
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddlIncomeHead" CssClass="textbox1 ddlheight2" Width="300px"
                                            runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblStaffCode" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblIncomeType" runat="server" Visible="false"></asp:Label>
                                         <asp:CheckBox ID="Cb_otherallowance" runat="server"  Text="Include In Other Allowance" Checked="false"/>

                                    </td>
                                    <td colspan="2">
                                   
                                    
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtamount" runat="server" MaxLength="15" onkeyup="chkamnt(this);"
                                            CssClass="textbox textbox1" Width="135px"></asp:TextBox>
                                        <span style="color: Red;">*</span>
                                        <asp:FilteredTextBoxExtender ID="flx" runat="server" TargetControlID="txtamount"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        Doc Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdocdate" runat="server" CssClass="textbox textbox1" Width="119px"></asp:TextBox>
                                        <asp:CalendarExtender ID="cal_docdate" runat="server" TargetControlID="txtdocdate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        Doc No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdocno" runat="server" MaxLength="15" Width="135px" CssClass="textbox textbox1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdocno"
                                            FilterType="Numbers,Custom,UppercaseLetters,LowercaseLetters" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cheque/DD No
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtchqno" runat="server" MaxLength="15" CssClass="textbox textbox1"
                                            Width="107px"></asp:TextBox>
                                    </td>
                                    <td>
                                        Cheque / Challan / DD Date
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtchqdt" runat="server" CssClass="textbox textbox1" Width="112px"></asp:TextBox>
                                        <asp:CalendarExtender ID="cal_chqdt" runat="server" TargetControlID="txtchqdt" Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        Bank Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankCode" runat="server" CssClass="textbox textbox1" Width="107px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        IT Month & Year
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlMonth" CssClass="textbox1 ddlheight2" Width="80px" runat="server">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlYear" CssClass="textbox1 ddlheight2" Width="80px" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
                                        Account For Month & Year
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlAccMonth" CssClass="textbox1 ddlheight2" Width="80px" runat="server">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlAccYear" CssClass="textbox1 ddlheight2" Width="80px" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Challan No/Transfer voucher
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtchallonNoTransferVoucher" runat="server" CssClass="textbox textbox1"
                                            Width="107px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Remarks
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_Remarks" runat="server" CssClass="textbox textbox1" Height="30px"
                                            TextMode="MultiLine" Width="400px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_allowalert" Visible="false" runat="server" Text="" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium; color: Red;"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="cb_splallow" runat="server" AutoPostBack="true" Text="Common Deduction" OnCheckedChanged="rb_allow_CheckedChanged"/>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlotherAllowance" runat="server" CssClass="textbox1 ddlheight3"
                                            Width="250px">
                                        </asp:DropDownList>
                                        <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtOtherallowance" runat="server" Style="height: 20px; width: 255px;"
                                                    ReadOnly="true"></asp:TextBox>
                                                <asp:Panel ID="pl" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 350px;
                                                    height: 180px;">
                                                    <asp:CheckBox ID="cbOtherallowance" runat="server" Width="100px" Text="Select All"
                                                        AutoPostBack="True" OnCheckedChanged="cbOtherallowanceOnCheckedChanged" />
                                                    <asp:CheckBoxList ID="cblOtherallowance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblOtherallowanceOnSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txtOtherallowance"
                                                    PopupControlID="pl" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="cbincludedpercentage" runat="server" AutoPostBack="true"  Text="Percentage" Checked="false" OnCheckedChanged="cbincludedpercentage_CheckedChanged"/>

                                         <asp:TextBox ID="txtpercent" runat="server" Enabled="false" CssClass="textbox textbox1" AutoPostBack="true" OnTextChanged="txt_change" Width="75px" MaxLength="3">
                                    
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender149" runat="server"   TargetControlID="txtpercent"
                                    FilterType="Numbers" ValidChars="">
                                </asp:FilteredTextBoxExtender>
                                    
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btnSaveIncome" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnSaveIncome_Click" Text="Save" CssClass="textbox1 textbox btn2" />
                            <asp:Button ID="btnNewIncome" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnNewIncome_Click" Text="New" CssClass="textbox1 textbox btn2" />
                            <asp:Button ID="btnDeleteIncome" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnDeleteIncome_Click" Text="Delete" CssClass="textbox1 textbox btn2" />
                            <asp:Button ID="btnExitIncome" runat="server" OnClick="btnExitIncome_Click" Font-Bold="true"
                                Font-Names="Book Antiqua" Text="Exit" CssClass="textbox1 textbox btn2" />
                        </center>
                    </div>
                    <br />
                    <br />
                    <div>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </div>
            </div>
        </div>
    </center>
</asp:Content>
