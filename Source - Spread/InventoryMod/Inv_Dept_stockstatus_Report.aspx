<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="Inv_Dept_stockstatus_Report.aspx.cs" Inherits="Inv_Dept_stockstatus_Report" %>

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
                height: 550px;
                width: 1000px;
            }
            .watermark
            {
                color: #999999;
            }
        </style>
    </head>
    <body>
        <form id="form2">
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
                        <asp:Label ID="Label1" runat="server" Style="color: Green;" class="fontstyleheader "
                            Text="Department Stock Status Report"></asp:Label>
                        <br />
                        <br />
                    </center>
                </div>
            </center>
            <center>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblinventoryusername" runat="server" Text="Inventory User Name:" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_messname" runat="server" Text="Department Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_deptname" runat="server" Visible="false" CssClass="textbox1  ddlheight3"
                                    OnSelectedIndexChanged="ddl_deptname_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:UpdatePanel ID="upp6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_deptname" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="p6" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 200px;">
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
                            <td>
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
                                <asp:TextBox ID="txtsearchby" Visible="false" placeholder="Search Item Name" runat="server"
                                    CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchby"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txtsearchitemcode" Visible="false" placeholder="Search Item Code"
                                    runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getitemcode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchitemcode"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="searchpanel">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txtsearchheadername" Visible="false" placeholder="Search Item Header"
                                    runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getitemheader" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearchheadername"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="searchpanel">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td colspan="2">
                                <asp:RadioButton ID="rdb_deptwise" runat="server" Text="Departmentwise" GroupName="d" />
                                <asp:RadioButton ID="rdb_culmul" runat="server" Text="Culmulative" GroupName="d" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngoclick" CssClass="textbox btn1" />
                                <asp:Label ID="lbl_Quantity" Visible="false" runat="server" Text="Quantity"></asp:Label>
                                <asp:TextBox ID="Txt_Quantity" Visible="false" runat="server" CssClass="textbox  txtheight1"
                                    Width="50px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="Txt_Quantity"
                                    FilterType="numbers,Custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lblfromdate" runat="server" Visible="false" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfrom" runat="server" Visible="false" AutoPostBack="true" CssClass="textbox  txtheight1"
                                    ForeColor="Black" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblto" runat="server" Visible="false" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtto" runat="server" AutoPostBack="true" Visible="false" CssClass="textbox  txtheight1"
                                    OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lblerror" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                        </div>
                    </center>
                    <center>
                        <div id="spreaddiv1" runat="server" visible="false" style="width: 684px; height: 350px;"
                            class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" Visible="false" runat="server" Style="width: 667px;
                                height: 300px; overflow: auto; background-color: White;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Size="Medium" ForeColor="Red"
                                Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                CssClass="textbox textbox1"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Width="60px" CssClass="textbox textbox1 btn1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
