<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_MenuCostMaster.aspx.cs" Inherits="HM_MenuCostMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
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
                height: 500px;
                width: 1000px;
            }
            .watermark
            {
                color: #999999;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <asp:Label ID="lblmenucost" runat="server" class="fontstyleheader" Style="color: Green;"
                        Text="Menu Cost Master"></asp:Label>
                    <br />    <br />
                </center>
            </div>
            <div class="maindivstyle maindivstylesize">
                <br />
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_menuname" runat="server" Text="Menu Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_menuname" runat="server" Height="20px" CssClass="textbox txtheight3 textbox1 "
                                            ReadOnly="true" Width="120px">Select All</asp:TextBox>
                                        <asp:Panel ID="pmnunm" runat="server" Width="180px" Height="300px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_menuname" runat="server" Width="100px" OnCheckedChanged="cb_menuname_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_menuname"
                                            PopupControlID="pmnunm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_search" Text="Search By" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_search" runat="server" placeholder="Search Menu name" CssClass="textbox txtheight3 textbox1"></asp:TextBox>
                                <%-- <asp:TextBoxWatermarkExtender ID="wateritemname" runat="server" TargetControlID="txt_search"
                                WatermarkText="Search Menu name" WatermarkCssClass="watermark textbox textbox1">
                            </asp:TextBoxWatermarkExtender>--%>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                    OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight2" AutoPostBack="true"
                                    OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_addnew" Text="Add New" runat="server" OnClick="btn_addnew_Click"
                                    CssClass="textbox btn2" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                <div id="div1" runat="server" visible="false" style="width: 640px; height: 300px;
                    overflow: auto; border: 1px solid Gray; border-radius: 10px; background-color: White;
                    box-shadow: 0px 0px 8px #999999;">
                    <br />
                    <%-- <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Width="420px" Style="overflow: auto; height: 180px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder" OnUpdateCommand="Fpspread1_Command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>--%>
                    <FarPoint:FpSpread ID="Fpspread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Width="600px" Style="height: 350px; overflow: auto; background-color: White;"
                        OnUpdateCommand="Fpspread1_Command" ShowHeaderSelection="false" OnCellClick="Cell_Click1"
                        OnPreRender="Fpspread3_render">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                <br />
                <div id="div_report" runat="server" visible="false">
                    <center>
                        <asp:Label ID="lbl_norec" runat="server" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label>
                        <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                            CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btnExcel_Click" />
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                            AutoPostBack="true" OnClick="btn_printmaster_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
                <div>
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow1" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 443px;"
                    OnClick="imagebtnpop1close_Click" />
                <br />
                <br />
                <div style="background-color: White; height: 540px; width: 915px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_menucostmaster" runat="server" class="fontstyleheader" Style="color: Green;"
                                Text="Menu Cost Master"></asp:Label>
                            <%--<span>Menu Cost Master</span>--%>
                        </div>
                    </center>
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pop1menu" runat="server" Text="Menu Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_pop1menu" runat="server" Height="20px" CssClass="textbox txtheight3 textbox1"
                                            ReadOnly="true" Width="120px">Select All</asp:TextBox>
                                        <asp:Panel ID="P2" runat="server" CssClass="multxtpanel" Height="180px" Width="150px">
                                            <asp:CheckBox ID="cb_pop1menu" runat="server" Width="100px" OnCheckedChanged="cb_pop1menu_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_pop1menu" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_pop1menu_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_pop1menu"
                                            PopupControlID="P2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop1search" Text="Search By" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_pop1search" runat="server" placeholder="Search Menu Name" CssClass="textbox txtheight3 textbox1"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop1search"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_pop1date" Text="Date" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_pop1date" runat="server" Width="80px" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_pop1date" runat="server"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_pop1go" Text="Go" runat="server" OnClick="btn_pop1go_Click" CssClass="textbox btn1" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblerror1" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" Width="416px" Style="overflow: auto; height: 280px; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        class="spreadborder" OnUpdateCommand="Fpspread2_Command" ActiveSheetViewIndex="0">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_pop1save" Text="Save" runat="server" OnClick="btn_pop1save_Click"
                        CssClass="textbox btn2" Visible="false" />
                    <asp:Button ID="btn_pop1exit" Text="Exit" runat="server" OnClick="btn_pop1exit_Click"
                        CssClass="textbox btn2" Visible="false" />
                </div>
            </div>
        </center>
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
                                        <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <center>
            <div id="editmenu_div" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 150px; margin-left: 169px;"
                    OnClick="imagebtn_Click" />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 211px; width: 355px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_menu" runat="server" Text="Menu Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_spmenu" runat="server" ForeColor="#0099CC" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblqty" runat="server" Text="Quantity"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_qty" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_qty"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcost" runat="server" Text="Cost"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_cost" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_cost"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbldate" runat="server" Text="Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="date" CssClass="textbox textbox1 txtheight" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="calender" TargetControlID="date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_update" Text="Update" CssClass="textbox btn2" runat="server"
                                        OnClick="btn_update_click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_delete" Text="Delete" CssClass="textbox btn2" runat="server"
                                        OnClick="btn_delete_click" />
                                    <asp:Button ID="btn_exit" Text="Exit" CssClass="textbox btn2" runat="server" OnClick="btn_exit_click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </center>
        <%-- theivamani 6.11.15--%>
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
        </form>
    </body>
    </html>
</asp:Content>
