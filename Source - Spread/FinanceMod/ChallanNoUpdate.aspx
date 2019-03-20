<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ChallanNoUpdate.aspx.cs" Inherits="ChallanNoUpdate" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            function checkDate() {
                var fromDate = "";
                var toDate = "";
                var date = ""
                var date1 = ""
                var month = "";
                var month1 = "";
                var year = "";
                var year1 = "";
                var empty = "";
                fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
                toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);

                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
                var today = new Date();
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

                if (year == year1) {
                    if (month == month1) {
                        if (date == date1) {
                            empty = "";
                        }
                        else if (date < date1) {
                            empty = "";
                        }
                        else {
                            empty = "e";
                        }
                    }
                    else if (month < month1) {
                        empty = "";
                    }
                    else if (month > month1) {
                        empty = "e";
                    }
                }
                else if (year < year1) {
                    empty = "";
                }
                else if (year > year1) {
                    empty = "e";
                }
                if (empty != "") {
                    alert("To date should be greater than from date ");
                    return false;
                }
            }
        </script>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Challan No Update</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true"
                                        Width="227px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                        Height="28px" OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_roll" runat="server" Style="margin-left: 5px; height: 18px;
                                        width: 100px;" placeholder="Roll no" AutoPostBack="true" OnTextChanged="txt_roll_Changed"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_name" runat="server" CssClass="txtheight5 txtcaps" placeholder="Name"
                                        AutoPostBack="true" OnTextChanged=" txt_name_Changed">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetName" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txt_name"
                                        FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .-@,">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divdatewise" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From" Style="margin-top: 10px;
                                                        margin-left: 6px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px; margin-left: 15px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px; margin-left: 6px;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                        OnClientClick="return checkDate()" OnClick="btnsearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <div id="divspread" runat="server" visible="false" style="width: 961px; overflow: auto;
                                            background-color: White; border-radius: 10px;">
                                            <FarPoint:FpSpread ID="FpSpreadbase" runat="server" Visible="true" BorderStyle="Solid"
                                                BorderWidth="0px" Width="950px" Style="overflow: auto; border: 0px solid #999999;
                                                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                class="spreadborder">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnsave" runat="server" Visible="false" Style="margin-left: 420px;
                                            margin-top: 15px;" CssClass="textbox btn2" Text="Save" OnClick="btnsave_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
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
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
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
                                            <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
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
