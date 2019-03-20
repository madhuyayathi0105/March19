<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_ManualAttnd.aspx.cs" Inherits="Staff_ManualAttnd" %>

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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Staff Manual Attendance</span></div>
                </center>
                <div class="maindivstyle" style="width: 1000px; height: auto;">
                    <br />
                    <table id="Table1" class="maintablestyle" runat="server">
                        <tr>
                            <td>
                                College Name
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_change"
                                    AutoPostBack="true" CssClass="textbox1 ddlheight4" Width="204px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Mon & Year
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlmon" runat="server" OnSelectedIndexChanged="ddlmon_change"
                                    AutoPostBack="true" CssClass="textbox1 ddlheight1">
                                    <asp:ListItem Selected="True" Text="Jan" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                    <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlyear" runat="server" OnSelectedIndexChanged="ddlyear_change"
                                    AutoPostBack="true" CssClass="textbox1 ddlheight1">
                                </asp:DropDownList>
                                From :
                                <asp:TextBox ID="txtfrmdate" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                <asp:CalendarExtender ID="calfrmdate" runat="server" TargetControlID="txtfrmdate"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                To :
                                <asp:TextBox ID="txttodate" runat="server" CssClass="textbox txtheight1"></asp:TextBox>
                                <asp:CalendarExtender ID="caltodate" runat="server" TargetControlID="txttodate" CssClass="cal_Theme1 ajax__calendar_active"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox1 ddlheight4" Width="204px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Staff Category
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstfcat" runat="server" CssClass="textbox1 ddlheight3" Width="203px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Staff Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstftype" runat="server" CssClass="textbox1 ddlheight3" Width="235px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Staff Code
                            </td>
                            <td>
                                <asp:TextBox ID="txt_stfcode" runat="server" OnTextChanged="txt_stfcode_change" AutoPostBack="true"
                                    CssClass="textbox txtheight4" Width="194px">
                                </asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stfcode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                Staff Name
                            </td>
                            <td>
                                <asp:TextBox ID="txt_stfname" runat="server" OnTextChanged="txt_stfname_change" AutoPostBack="true"
                                    CssClass="textbox txtheight4" Width="194px">
                                </asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stfname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkmanuallop" runat="server" Text="Manual LOP Staff" />
                                <asp:Button ID="btn_go" runat="server" Text="GO" OnClick="btn_go_click" Font-Bold="true"
                                    CssClass="textbox1 btn2" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" Visible="false" BackColor="LightGreen"
                                    OnClick="btnsave_click" Font-Bold="true" CssClass="textbox1 btn2" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lbldayscount" runat="server" Visible="false" Text="" Font-Bold="true"
                        ForeColor="DarkGreen" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="mainerr" runat="server" Visible="false" Text="" Font-Bold="true" ForeColor="Red"
                        Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    <div id="spdiv" runat="server">
                        <asp:Label ID="lbldatespecify" runat="server" Text="" Style="margin-left: 530px;"
                            Font-Italic="true" Font-Bold="true" ForeColor="Blue" Font-Size="Medium" Visible="false"></asp:Label>
                        <br />
                        <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Style="width: 900px; height: auto;" OnButtonCommand="FpSpread_Command"
                            class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                </div>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 100px;
                            width: 300px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <table style="height: auto; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" OnClick="btnerrclose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
