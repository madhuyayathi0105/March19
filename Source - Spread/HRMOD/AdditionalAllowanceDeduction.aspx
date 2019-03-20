<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="AdditionalAllowanceDeduction.aspx.cs" Inherits="HRMOD_AdditionalAllowanceDeduction" %>

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
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            //             add by poomalar 
            function PrtDiv() {
                var panel = document.getElementById("<%=divspread.ClientID %>");
                var printWindow = window.open('', '', 'height=auto,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
                printWindow.document.write('</head><body>');
                printWindow.document.write('<form>');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write(' </form>');
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Additional Allowance And Deduction
                            Report</span></div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="updatecollege" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="300px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_dept" runat="server" Text="Department" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight1" ReadOnly="true"
                                            Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
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
                                            Style="font-weight: bold; width: 150px; font-family: book antiqua; font-size: medium;">--Select--</asp:TextBox>
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
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_staffc" runat="server" Text="Category" Style="font-weight: bold;
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
                            <td>
                                <%-- <fieldset>--%>
                                <div style="border-color: white; height: 24px; width: 204px; border-style: solid;
                                    border-width: 1px;">
                                    <asp:RadioButtonList ID="radFormat" runat="server" OnSelectedIndexChanged="radFormat_SelectedIndexChanged"
                                        AutoPostBack="true" RepeatDirection="Horizontal" Width="200px" Height="10px">
                                        <asp:ListItem Selected="True" Value="0" Text="Allowance"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Deduction"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <%-- </fieldset>--%>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_Allowdeduc" runat="server" CssClass="textbox  ddlheight3"
                                    OnSelectedIndexChanged="AllowDeducSelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="lbl_amount" runat="server" Text="Amount" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                                <asp:TextBox ID="txt_amount" runat="server" MaxLength="10" Width="88px" Style="font-weight: bold;
                                    font-family: book antiqua; margin-left: 0px; font-size: medium;"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_amount"
                                    FilterType="Numbers" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_go" runat="server" Text="Go" OnClick="btn_go_Click" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;" CssClass="textbox btn1" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" runat="server" Text="Add New" CssClass="textbox btn2"
                                    OnClick="btn_addnew_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaff" runat="server" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                    AutoPostBack="true" Visible="true" CssClass="ddlheight1 textbox textbox1">
                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                    AutoPostBack="True" Visible="true" CssClass="txtheight3 textbox textbox1" Width="200px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txt_search1" runat="server" OnTextChanged="txt_search1_TextChanged"
                                    AutoPostBack="True" Visible="false" CssClass="txtheight3 textbox textbox1" Width="200px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search1"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="ermsg" Font-Bold="true" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <div id="divspread" runat="server" visible="false" style="width: 1000px;">
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="true" BorderWidth="1px"
                            BorderColor="#0CA6CA" Style="height: auto; width: auto; overflow: auto; background-color: White;"
                            CssClass="spreadborder" OnButtonCommand="Fpspread1_ButtonCommand" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <center>
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
                            <asp:Button ID="btnsavespread" runat="server" Text="Save" CssClass="textbox textbox1 btn2"
                                OnClick="btnsavespread_Click" />
                            <asp:Button ID="btnprintcell" Visible="true" runat="server" Text="Print" CssClass="textbox textbox1 btn2"
                                OnClick="btnprintcell_click" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <center>
                        <div id="popaddnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0;">
                            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 23px; margin-left: 341px;"
                                OnClick="imagebtnpopclose_Click" />
                            <br />
                            <br />
                            <div class="subdivstyle" style="background-color: White; overflow: auto; width: 700px;
                                height: 400px;" align="center">
                                <br />
                                <center>
                                    <asp:Label ID="lbl_AddAllowDeduc" runat="server" class="fontstyleheader" Style="color: Green;"
                                        Text="Allowance and Deduction"></asp:Label>
                                </center>
                                <br />
                                <div align="left" style="overflow: auto; width: 500px; height: 280px; border-radius: 10px;
                                    border: 1px solid Gray;">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:DropDownList ID="ddl_college1" runat="server" CssClass="textbox ddlheight5"
                                                        Width="250px" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_allowance" runat="server" Text="Allowance"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox btn" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_plus_Click" />
                                                    <asp:DropDownList ID="ddl_popAllowDeduc" CssClass="textbox ddlheight3" runat="server"
                                                        OnSelectedIndexChanged="PopAllowDeducSelectedIndexChanged" AutoPostBack="true"
                                                        onfocus="return myFunction(this)">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_minus_Click" />
                                                    <span style="color: Red;">*</span>
                                                    <asp:Label ID="Label2" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
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
                                            </tr>
                                            <tr>
                                                <td>
                                                    Cheque/DD No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtchqno" runat="server" MaxLength="15" CssClass="textbox textbox1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Cheque / Challan / DD Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtchqdt" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="cal_chqdt" runat="server" TargetControlID="txtchqdt" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Challan No/Transfer voucher
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtchallonNoTransferVoucher" runat="server" MaxLength="15" CssClass="textbox textbox1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <br />
                                                    <center>
                                                        <asp:Button ID="btn_popdelete" runat="server" Text="Delete" CssClass="textbox btn2"
                                                            Visible="false" OnClick="btn_popdelete_Click" />
                                                        <asp:Button ID="btn_allowsave" runat="server" CssClass="textbox btn2" Text="Save"
                                                            Visible="false" OnClick="btn_allowsave_Click" />
                                                        <asp:Button ID="btn_popexit" runat="server" CssClass="textbox btn2" Text="Exit" OnClick="btn_popexit_Click" />
                                                    </center>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </div>
                        </div>
                    </center>
                    <center>
                        <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="panel_reason" runat="server" visible="false" class="table" style="background-color: White;
                                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    margin-top: 200px; border-radius: 10px;">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lbl_add" runat="server" Text="Allow Name" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txt_AllowDeducname" runat="server" Width="200px" Style="text-transform: capitalize;
                                                    font-family: 'Book Antiqua'; margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" MaxLength="35"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderhouse" runat="server" TargetControlID="txt_AllowDeducname"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <br />
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btn_add_Allowname" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_add_Allowname_Click" />
                                                <asp:Button ID="btn_exit_Allowname" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_exit_Allowname_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </center>
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
                                                        <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                            OnClick="btn_errorclose_Click" Text="ok" runat="server" />
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
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
