<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_Belltime_Settings.aspx.cs" Inherits="Staff_Belltime_Settings"
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
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green;">Staff Mandatory BellTime Settings</span>
            <br />
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbl_college" runat="server" Text="College Name" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                    width: 250px;">
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
                                <asp:TextBox ID="txt_desig" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P2" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;
                                    width: 250px;">
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
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_staffc" runat="server" Text="Staff Category" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 154px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                <asp:TextBox ID="txt_stype" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="P4" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                    <td>
                        <asp:Label ID="lblMonYear" runat="server" Text="Month & Year" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMon" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlMon_Change"
                            AutoPostBack="true">
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
                        <asp:DropDownList ID="ddlYear" runat="server" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;" CssClass="textbox1 ddlheight" OnSelectedIndexChanged="ddlYear_Change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFrmDt" runat="server" Text="From Date" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;"></asp:Label>
                    </td>
                    <td colspan="8">
                        <asp:TextBox ID="txtFrmDt" runat="server" Enabled="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" CssClass="textbox textbox1 txtheight1"
                            OnTextChanged="txtFrmDt_Change" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="calFrmDt" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFrmDt">
                        </asp:CalendarExtender>
                        <asp:Label ID="lblToDt" runat="server" Text="To Date" Style="font-weight: bold; font-family: book antiqua;
                            font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" Enabled="false" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" CssClass="textbox textbox1 txtheight1"
                            OnTextChanged="txtToDt_Change" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="calToDt" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDt">
                        </asp:CalendarExtender>
                        <asp:Label ID="lbl_scode" runat="server" Text="Staff Code" Style="font-weight: bold;
                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                            MaxLength="10" Style="font-weight: bold; width: 123px; font-family: book antiqua;
                            font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                        <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                            margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                            MaxLength="50" Style="font-weight: bold; font-family: book antiqua; margin-left: 0px;
                            font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                        <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="textbox1 btn2" Style="font-weight: bold;
                            font-family: book antiqua; font-size: medium;" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <fieldset id="fldTime" runat="server" visible="false" style="width: 520px; border: 1px solid #999999;
                background-color: #F0F0F0; box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;
                -webkit-box-shadow: 0px 0px 10px #999999; border: 3px solid #D9D9D9; border-radius: 15px;">
                <asp:Label ID="lblSetTime" runat="server" Text="Set Time" Style="font-weight: bold;
                    font-family: book antiqua; font-size: medium;"></asp:Label>
                <asp:DropDownList ID="ddlHr" runat="server" Style="font-weight: bold; font-family: book antiqua;
                    font-size: medium;" CssClass="textbox1 ddlheight" Width="60px">
                    <asp:ListItem Selected="True" Text="12" Value="0"></asp:ListItem>
                    <asp:ListItem Text="01" Value="1"></asp:ListItem>
                    <asp:ListItem Text="02" Value="2"></asp:ListItem>
                    <asp:ListItem Text="03" Value="3"></asp:ListItem>
                    <asp:ListItem Text="04" Value="4"></asp:ListItem>
                    <asp:ListItem Text="05" Value="5"></asp:ListItem>
                    <asp:ListItem Text="06" Value="6"></asp:ListItem>
                    <asp:ListItem Text="07" Value="7"></asp:ListItem>
                    <asp:ListItem Text="08" Value="8"></asp:ListItem>
                    <asp:ListItem Text="09" Value="9"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlMin" runat="server" Style="font-weight: bold; font-family: book antiqua;
                    font-size: medium;" CssClass="textbox1 ddlheight" Width="60px">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlMer" runat="server" Style="font-weight: bold; font-family: book antiqua;
                    font-size: medium;" CssClass="textbox1 ddlheight" Width="60px">
                    <asp:ListItem Selected="True" Text="AM" Value="0"></asp:ListItem>
                    <asp:ListItem Text="PM" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnSetTime" runat="server" Text="Set Time" CssClass="textbox1 btn2"
                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: LightGreen;"
                    OnClick="btnSetTime_Click" />
                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="textbox1 btn2"
                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: LightGreen;"
                    OnClick="btnRemove_Click" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="textbox1 btn2" Style="font-weight: bold;
                    font-family: book antiqua; font-size: medium; background-color: LightGreen;"
                    OnClick="btnSave_Click" />
            </fieldset>
            <br />
            <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" ForeColor="Red"
                Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                BorderStyle="Solid" BorderWidth="1px" Width="980px" Height="800px" Style="margin-left: 2px;"
                class="spreadborder" OnButtonCommand="FpSpread_Command" ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <br />
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
                    Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="140px" Text="Export Excel"
                    OnClick="btnexcel_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2"
                    Width="100px" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </div>
            <br />
            <div id="alertpopwindow" runat="server" class="popupstyle popupheight1" visible="false"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 280px;
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
                                                Text="Ok" runat="server" OnClick="btnerrclose_Click" />
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
