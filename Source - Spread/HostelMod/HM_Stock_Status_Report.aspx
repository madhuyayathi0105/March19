<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_Stock_Status_Report.aspx.cs" Inherits="HM_Stock_Status_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .maindivstylesize
            {
                height: auto;
                width: 1010px;
            }
            .watermark
            {
                color: #999999;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <asp:Label ID="Label1" runat="server" Style="color: Green;" class="fontstyleheader"
                            Text="Stock Status Report"></asp:Label>
                        <br />
                        <br />
                    </center>
                </div>
            </center>
            <center>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <center>
                        <div style="width: 600px;">
                            <div class="maindivstyle" style="width: 300px; float: left;">
                                <asp:RadioButton ID="rdb_cumlative" runat="server" Text="Cumulative" GroupName="dd"
                                    OnCheckedChanged="rdb_cumlative_OnCheckedChanged" AutoPostBack="true" />
                                <asp:RadioButton ID="rdb_details" runat="server" Text="Details" GroupName="dd" OnCheckedChanged="rdb_details_OnCheckedChanged"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rdb_datewise" runat="server" Text="Date Wise" GroupName="dd" OnCheckedChanged="rdb_datewise_OnCheckedChanged"
                                    AutoPostBack="true" />
                            </div>
                            <div class="maindivstyle" style="width: 265px; float: right;">
                                <asp:RadioButton ID="rb_store" runat="server" Text="Store" GroupName="stormess" OnCheckedChanged="rb_store_OnCheckedChanged"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rb_mess" runat="server" Text="Mess" GroupName="stormess" OnCheckedChanged="rb_mess_OnCheckedChanged"
                                    AutoPostBack="true" />
                                <asp:RadioButton ID="rb_dept" runat="server" Text="Department" GroupName="stormess"
                                    OnCheckedChanged="rb_dept_OnCheckedChanged" AutoPostBack="true" />
                            </div>
                        </div>
                    </center>
                    <br />
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td colspan="4">
                                <asp:Label ID="lblinventoryusername" runat="server" Text="Inventory User Name:" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_messname" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_messname" runat="server" Visible="false" CssClass="textbox1  ddlheight3"
                                    OnSelectedIndexChanged="ddl_messname_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_store" runat="server" Visible="false" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" Visible="false" CssClass="multxtpanel" Style="height: 200px;
                                            width: 150px;">
                                            <asp:CheckBox ID="cb_storeb" runat="server" OnCheckedChanged="cb_storeb_oncheckedchange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_storeb" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_storeb_onselectedindexchange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_store"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_hosname" runat="server" CssClass="textbox textbox1 txtheight1"
                                            Visible="false">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="150px" Width="160px"
                                            Visible="false">
                                            <asp:CheckBox ID="cb_hos" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_hostel_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hosname"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="upp6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_deptname" runat="server" CssClass="textbox  txtheight2" Visible="false">--Select--</asp:TextBox>
                                        <asp:Panel ID="p6" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 150px;"
                                            Visible="false">
                                            <asp:CheckBox ID="cb_deptname" runat="server" OnCheckedChanged="cb_deptname_oncheckedchange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_deptname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_deptname_onselectedindexchange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_deptname"
                                            PopupControlID="p6" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblitemheadername" runat="server" Text="Item Header Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtheadername" runat="server" Width=" 116px" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cbheadername" runat="server" OnCheckedChanged="cbheadername_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheadername_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtheadername"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_subheadername" Width=" 102px" runat="server" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 190px;">
                                            <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblitemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtitemname" Width=" 102px" runat="server" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="200px" Width="120px">
                                            <asp:CheckBox ID="cbitemname" runat="server" OnCheckedChanged="cbitemname_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblitemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblitemname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtitemname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Search By"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltype" runat="server" CssClass="textbox1  ddlheight2" OnSelectedIndexChanged="ddltype_selectchange"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0">Item Name</asp:ListItem>
                                    <asp:ListItem Value="1">Item Code</asp:ListItem>
                                    <asp:ListItem Value="2">Item Header</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtsearchby" Visible="false" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchby"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <asp:TextBoxWatermarkExtender ID="wateritemname" runat="server" TargetControlID="txtsearchby"
                                    WatermarkText="Search Item Name" WatermarkCssClass="watermark textbox txtheight2">
                                </asp:TextBoxWatermarkExtender>
                                <asp:TextBox ID="txtsearchitemcode" Visible="false" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchitemcode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="searchpanel">
                                </asp:AutoCompleteExtender>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtsearchitemcode"
                                    WatermarkText="Search Item Code" WatermarkCssClass="watermark textbox txtheight2">
                                </asp:TextBoxWatermarkExtender>
                                <asp:TextBox ID="txtsearchheadername" Visible="false" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchheadername"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="searchpanel">
                                </asp:AutoCompleteExtender>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtsearchheadername"
                                    WatermarkText="Search Item Header" WatermarkCssClass="watermark textbox txtheight2">
                                </asp:TextBoxWatermarkExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Quantity" runat="server" Text="Quantity"></asp:Label>
                                <asp:TextBox ID="Txt_Quantity" runat="server" CssClass="textbox  txtheight1" Width="50px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="Txt_Quantity"
                                    FilterType="numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfrom" runat="server" AutoPostBack="true" CssClass="textbox  txtheight1"
                                    ForeColor="Black" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                    Format="dd/MM/yyyy">
                                    <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblto" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtto" runat="server" AutoPostBack="true" CssClass="textbox  txtheight1"
                                    OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <%-- CssClass="cal_Theme1 ajax__calendar_active" --%>
                            </td>
                            <%-- <td>
                            <asp:CheckBox ID="chkhos" runat="server" Visible="false" Text="Hostel"  />
                        </td>--%>
                            <%--  <td>
                        </td>--%>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngoclick" CssClass="textbox btn1" />
                            </td>
                        </tr>
                       <%-- <tr>
                            <td colspan="6">
                                <asp:LinkButton ID="lb_ConsumptionReport" runat="server" Visible="true" Font-Bold="false"
                                    Font-Size="Medium" ForeColor="Black" CausesValidation="False" OnClick="lb_ConsumptionReport_Click">Item wise Consumption Report</asp:LinkButton>
                            </td>
                        </tr>--%>
                    </table>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lblerror" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                        </div>
                    </center>
                    <center>
                        <%--<div id="spreaddiv1" runat="server" visible="false" style="width: 684px; height: 350px;"
                        class="spreadborder">--%>
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" Style="width: 667px;
                            overflow: auto; background-color: White;" CssClass="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <%-- </div>--%>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                Font-Size="Medium" CssClass="textbox textbox1"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Width="60px" CssClass="textbox textbox1 btn2" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                        <br />
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
