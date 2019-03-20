<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HRMOD/HRSubSiteMaster.master"
    CodeFile="StaffPaySettings.aspx.cs" Inherits="StaffPaySettings" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <title></title>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblsmserror.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Staff Pay Settings</span></div>
                </center>
                <fieldset id="maindiv" runat="server" style="width: 978px; margin-left: 0px; height: 1100px;
                    border-color: silver; border-radius: 10px;">
                    <fieldset style="height: 116px; width: 960px; border: 1px solid #0ca6ca; border-radius: 10px;">
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
                                        margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                                        MaxLength="10" Style="font-weight: bold; width: 100px; margin-left: 10px; font-family: book antiqua;
                                        font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sname" runat="server" Text="Staff Name" Style="font-weight: bold;
                                        margin-left: 0px; font-family: book antiqua; font-size: medium;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                                        MaxLength="50" Style="font-weight: bold; font-family: book antiqua; margin-left: 0px;
                                        font-size: medium;"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="txtsearchpan">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                        </table>
                        <fieldset style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                            box-shadow: 0 0 8px #999999; height: 44px; margin-left: 0px; margin-top: 8px;
                            padding: 1em; margin-left: 0px; width: 735px;">
                            <table style="margin-top: -14px;">
                                <tr>
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
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
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
                                                <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                    Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
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
                                </tr>
                                <tr>
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
                                    <td colspan="2">
                                        <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                            font-family: book antiqua; font-size: medium;" />
                                        <asp:Button ID="btn_setallow" runat="server" Text="Set Allowance" OnClick="btn_setallow_Click"
                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium;" />
                                    </td>
                                </tr>
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
                            class="spreadborder" OnButtonCommand="FpSpread_Command" ShowHeaderSelection="false">
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
                                Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                </fieldset>
                <div id="poperrjs" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 372px;"
                        OnClick="imagebtnpopcloseadd_Click" />
                    <br />
                    <br />
                    <div class="subdivstyle" style="background-color: White; margin-left: 85px; overflow: auto;
                        width: 700px; height: 620px;" align="center">
                        <br />
                        <br />
                        <div align="left" style="overflow: auto; width: 620px; height: 540px; border-radius: 10px;
                            border: 1px solid Gray;">
                            <br />
                            <center>
                                <span class="fontstyleheader" style="color: Green;" runat="server">Set Allowance</span>
                                <br />
                                <br />
                                <table class="maintablestyle" runat="server">
                                    <tr>
                                        <td>
                                            Allowances
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updallow" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtallow" runat="server" ReadOnly="true" CssClass="textbox txtheight1"
                                                        Style="font-weight: bold; width: 120px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
                                                    <asp:Panel ID="pnlallow" runat="server" CssClass="multxtpanel" Height="200px">
                                                        <asp:CheckBox ID="chkallow" runat="server" Text="SelectAll" OnCheckedChanged="chkallow_changed"
                                                            AutoPostBack="true" />
                                                        <asp:CheckBoxList ID="chklstallow" runat="server" OnSelectedIndexChanged="chklstallow_onselectedchanged"
                                                            AutoPostBack="true">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="popallow" runat="server" PopupControlID="pnlallow"
                                                        TargetControlID="txtallow" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkbasic" runat="server" Text="Basic Amount" AutoPostBack="true"
                                                OnCheckedChanged="chkbasic_change" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbasic" runat="server" PlaceHolder="Percent" MaxLength="2" CssClass="textbox textbox1"
                                                Visible="false"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterbasic" runat="server" FilterType="Numbers"
                                                TargetControlID="txtbasic">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnpopgo" runat="server" Text="GO" CssClass="textbox textbox1 btn1"
                                                OnClick="btnpopgo_click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label ID="poperr" runat="server" Text="" Font-Bold="true" Font-Size="Large"
                                    ForeColor="Red" Visible="false"></asp:Label>
                                <br />
                                <br />
                                <FarPoint:FpSpread ID="Fpspreadpop" runat="server" Visible="false" BorderColor="Black"
                                    BorderStyle="Solid" BorderWidth="1px" Width="470px" Height="300px" Style="margin-left: 2px;"
                                    class="spreadborder" OnButtonCommand="Fpspreadpop_Command" ShowHeaderSelection="false">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <br />
                                <asp:Button ID="btnsavepop" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                    OnClick="btnsavepop_click" />
                                <asp:Button ID="btnexitpop" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                    OnClick="btnexitpop_click" />
                                <br />
                            </center>
                        </div>
                    </div>
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
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
            </div>
        </center>
    </body>
    </html>
</asp:Content>
