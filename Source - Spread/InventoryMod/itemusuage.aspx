<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="itemusuage.aspx.cs" Inherits="itemusuage" %>

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
            .div
            {
                left: 0%;
                top: 0%;
            }
            .watermark
            {
                color: #999999;
            }
            .table2
            {
                border: 1px solid #0CA6CA;
                border-radius: 10px;
                background-color: #0CA6CA;
                box-shadow: 0px 0px 8px #7bc1f7;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <asp:Label ID="Label1" runat="server" Style="color: Green;" class="fontstyleheader"
                            Text="Item Usage"></asp:Label>
                        <br />
                    </div>
                </center>
                <br />
                <div class="maindivstyle" style="height: 800px; width: 1000px;">
                    <br />
                    <table class="maintablestyle">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbl_Department" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" Text="Department" Checked="true" OnCheckedChanged="rbl_Department_Selected" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbl_Store" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    Text="Store" OnCheckedChanged="rbl_Store_Selected" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight2"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="cext_fromdate" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1  txtheight2"
                                    AutoPostBack="true" OnTextChanged="txt_todate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="cext_todate" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_dept" runat="server" Text="Department"></asp:Label>
                                <asp:Label ID="lbl_store" runat="server" Text="Store" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_store" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_store" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_store_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_store" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_store_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_store"
                                            PopupControlID="Panel5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_itemname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                            width: 200px;">
                                            <asp:CheckBox ID="cb_itemname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_itemname_CheckedChange" />
                                            <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChange">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdb_common" runat="server" Text="Common" GroupName="ns" AutoPostBack="true"
                                    OnCheckedChanged="rdb_common_Checkedchange" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_Individual" runat="server" Text="Individual" GroupName="ns"
                                    AutoPostBack="true" OnCheckedChanged="rdb_Individual_Checkedchange" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_type" runat="server" Text="Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox1 ddlheight2" Width="131px">
                                    <asp:ListItem Value="0">Issued</asp:ListItem>
                                    <asp:ListItem Value="1">Partially Issued</asp:ListItem>
                                    <asp:ListItem Value="2">Not Issued</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                    OnClick="btn_addnew_click" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    <div id="commoncolumnorder" runat="server" visible="false">
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                                    Style="margin-top: -0.1%;">
                                    <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <center>
                            <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="LinkButtonsremove_Click" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="DailyConsDate">Consumption Date</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Department Name</asp:ListItem>
                                                <asp:ListItem Value="Stud_Name">Item Header Name</asp:ListItem>
                                                <asp:ListItem Value="ItemCode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="ConsumptionQty">ConsumptionQty</asp:ListItem>
                                                <asp:ListItem Value="Rpu">Rpu</asp:ListItem>
                                                <asp:ListItem Value="consumValue">Consumption Value</asp:ListItem>
                                                <%-- <asp:ListItem Value="Current_Semester">Requested QTY</asp:ListItem>
                                        <asp:ListItem Value="Sections">Requested Value</asp:ListItem>
                                        <asp:ListItem Value="TotItemQty">Used QTY</asp:ListItem>
                                        <asp:ListItem Value="TotItemQty">Used Value</asp:ListItem>
                                        <asp:ListItem Value="Roll_No">Balance QTY</asp:ListItem>
                                        <asp:ListItem Value="Stud_Name">Balance Value</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/Images/right.jpeg"
                            ExpandedImage="~/Images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </div>
                    <%--18.06.16--%>
                    <div runat="server" id="individualcolumnorder" visible="false">
                        <center>
                            <div>
                                <br />
                                <center>
                                    <asp:Panel ID="pheaderfilter3" runat="server" CssClass="table2" Height="22px" Width="850px"
                                        Style="margin-top: -0.1%;">
                                        <asp:Label ID="Label7" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                        <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                    </asp:Panel>
                                </center>
                                <br />
                            </div>
                            <asp:Panel ID="pcolumnorder3" runat="server" CssClass="table2" Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="LinkButtonsremove_Click3" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder3" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove_Click3">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder3" runat="server" Height="43px" Width="850px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="DailyConsDate">Allot Date</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Roll No</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Reg No</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Student Name</asp:ListItem>
                                                <asp:ListItem Value="ItemCode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Department Name</asp:ListItem>
                                                <asp:ListItem Value="ConsumptionQty">Issued Qty</asp:ListItem>
                                                <asp:ListItem Value="Rpu">Rpu</asp:ListItem>
                                                <%--   <asp:ListItem Value="consumValue">Consumption Value</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pcolumnorder3"
                            CollapseControlID="pheaderfilter3" ExpandControlID="pheaderfilter3" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/Images/right.jpeg"
                            ExpandedImage="~/Images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </div>
                    <asp:Label ID="lblnorecr" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                    <br />
                    <FarPoint:FpSpread ID="Fpmain" runat="server" Visible="false" BorderColor="Black"
                        BorderStyle="Solid" BorderWidth="1px" Width="950px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter the Report Name"
                            Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display()"
                            CssClass="textbox textbox1"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                            Width="127px" CssClass="textbox btn1" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </div>
            <div id="popwindow" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 430px;"
                    OnClick="imagebtnpopclose1_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 900px;
                    height: 600px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="lbl_goodsreturn" runat="server" Style="font-size: large; color: Green;"
                            Text="Issued Items"></asp:Label>
                    </center>
                    <br />
                    <div align="left" style="overflow: auto; width: 845px; height: 500px; border-radius: 10px;
                        border: 1px solid Gray;">
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <fieldset style="width: 200px; height: 15px;">
                                            <asp:RadioButtonList ID="rblissue_Wise" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblissue_Wise_Selected">
                                                <asp:ListItem Text="Department" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Store"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-left: 23px; position: absolute; width: 800px; border-radius: 10px;
                                background-color: #0ca6ca; height: 42px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" CssClass="textbox textbox1 txtheight2"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_deptadd" runat="server" Text="Department"></asp:Label>
                                        <asp:Label ID="lbl_storeadd" runat="server" Text="Store" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_dept_SelectedIndexChange">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_Store" runat="server" CssClass="textbox  ddlheight3" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddl_Store_SelectedIndexChange" Visible="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_itemnamesearch" runat="server" Text="Item Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_popitm" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panelpopitm" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                    width: 200px;">
                                                    <asp:CheckBox ID="cb_popitm" runat="server" Text="Select All" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="True" OnCheckedChanged="cb_popitm_CheckedChange" />
                                                    <asp:CheckBoxList ID="cbl_popitm" runat="server" AutoPostBack="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnSelectedIndexChanged="cbl_popitm_SelectedIndexChange">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_popitm"
                                                    PopupControlID="Panelpopitm" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_itemnamesearch" placeholder="Search Item Name" TextMode="SingleLine"
                                            runat="server" AutoCompleteType="Search" CssClass="textbox  txtheight3" Width="110px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_itemnamesearch"
                                            FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" ">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_itemnamesearch"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go_add" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_add_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <br />
                        <div>
                            <center>
                                <asp:Label ID="lbl_errmessage" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                            </center>
                        </div>
                        <center>
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Height="280px" Width="757px" Style="margin-top: 50px;"
                                CssClass="spreadborder" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                        <br />
                        <div>
                            <center>
                                <asp:Button ID="btn_save" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Save" OnClientClick="return Test()" OnClick="btn_save_Click" />
                                <asp:Button ID="btn_exit" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Exit" OnClick="btn_exit_Click" />
                                <asp:Button ID="btn_update" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Update" />
                                <asp:Button ID="btn_delete" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Delete" />
                            </center>
                            <center>
                                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 20px;
                                    left: 0px;">
                                    <center>
                                        <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                                    Text="Ok" runat="server" OnClick="btnerrclose1_Click" />
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
                </div>
            </div>
        </center>
        <center>
            <div id="Individual_div" runat="server" visible="false" style="height: 50em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 13px; margin-left: 452px;"
                    OnClick="imagebtnpopclose2_Click" />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 930px;
                    height: 778px;" align="center">
                    <br />
                    <center>
                        <asp:Label ID="Label2" runat="server" Style="font-size: large; color: Green;" Text="Issued Student Items"></asp:Label>
                    </center>
                    <br />
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbl_individualstudent" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" Text="Individual" Checked="true" OnCheckedChanged="rbl_individualstudent_Selected" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rbl_kit" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        Text="Kit" OnCheckedChanged="rbl_kit_Selected" />
                                    <asp:RadioButton ID="rbl_retuen" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        Text="Return" OnCheckedChanged="rbl_retuen_Selected" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbatch" Text="Batch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true" Style="margin-left: 25px">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" CssClass="multxtpanel" Width="120px" Height="180px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_batch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" CssClass="multxtpanel" Width="111px" Height="180px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p4" runat="server" CssClass="multxtpanel" Width="200px" Height="200px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="p4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_section" Text="Section" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_section" runat="server" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Width="100px" Height="120px">
                                                <asp:CheckBox ID="cb_section" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_section_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_section" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_section_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_section"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label3" Text="Search By" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddl_searchby" runat="server" CssClass="textbox ddlheight3"
                                        Width="115px" AutoPostBack="true" OnSelectedIndexChanged="ddl_searchby_onselectedindexchange">
                                        <asp:ListItem Value="0">Roll No</asp:ListItem>
                                        <asp:ListItem Value="1">Reg No</asp:ListItem>
                                        <asp:ListItem Value="2">Application No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_roll" placeholder="Search Roll No" Visible="false" runat="server"
                                        CssClass="textbox textbox1" Width="152px" OnTextChanged="txt_roll_changed" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetRoll" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txt_reg" placeholder="Search Reg No" Visible="false" runat="server"
                                        CssClass="textbox textbox1" Width="152px" OnTextChanged="txt_reg_changed" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetReg" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_reg"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                    <asp:TextBox ID="txt_app" Visible="false" placeholder="Search Application No" runat="server"
                                        CssClass="textbox textbox1" Width="152px" OnTextChanged="txt_app_changed" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetApp" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_app"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_studname" Text="Student Name" runat="server"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox txtheight3 " placeholder="Search Student Name"
                                        Width="300px" OnTextChanged="txt_studentname_Changed" AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_studentname"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=". ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_studentname"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="cb_fromto" runat="server" Style="margin-left: 10px;" AutoPostBack="true"
                                        OnCheckedChanged="cb_fromto_CheckedChange" Visible="false" />
                                    <asp:Label ID="Label4" runat="server" Text="From Date"></asp:Label>
                                    <asp:TextBox ID="txt_fromdate1" runat="server" OnTextChanged="txt_fromdate_TextChanged"
                                        AutoPostBack="true" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                    <asp:CalendarExtender ID="calfrodate" TargetControlID="txt_fromdate1" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_todate1" runat="server" CssClass="textbox textbox1 txtheight1"
                                        OnTextChanged="txt_todate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate1" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td id="lbl_kit" runat="server" visible="false">
                                    <asp:Label ID="lbl_kitname" Text="Kit Name" runat="server" Style="float: left"></asp:Label>
                                </td>
                                <td id="kitname" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_kitname" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                            <asp:Panel ID="pan_kit" runat="server" unat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_kitname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_kitname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_kitname" runat="server" AutoPostBack="true" Font-Bold="True"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="cbl_kitname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_kitname"
                                                PopupControlID="pan_kit" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go2" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go2_Click" />
                                </td>
                            </tr>
                        </table>
                        <%--column order--%>
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter1" runat="server" CssClass="table2" Height="22px" Width="850px"
                                    Style="margin-top: -0.1%;">
                                    <asp:Label ID="Label6" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <center>
                            <asp:Panel ID="pcolumnorder1" runat="server" CssClass="table2" Width="850px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="LinkButtonsremove_Click1" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder1" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Visible="false" Width="111px" OnClick="LinkButtonsremove_Click1">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="individualstudentcolumnorder">
                                            <asp:CheckBoxList ID="cblcolumnorder1" runat="server" Height="43px" Width="850px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Roll_no">Roll No</asp:ListItem>
                                                <asp:ListItem Value="reg_no">Reg No</asp:ListItem>
                                                <asp:ListItem Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Value="ItemCode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="Dept_Code">Department Name</asp:ListItem>
                                                <asp:ListItem Value="consumValue">Balance Qty</asp:ListItem>
                                                <asp:ListItem Value="rpu">Rpu</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td runat="server" id="kitstudentcolumnorder" visible="false">
                                            <asp:CheckBoxList ID="cblcolumnorder2" runat="server" Height="43px" Width="850px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Roll_no">Roll No</asp:ListItem>
                                                <asp:ListItem Value="reg_no">Reg No</asp:ListItem>
                                                <asp:ListItem Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Value="MasterValue">Kit Name</asp:ListItem>
                                                <asp:ListItem Value="ItemCode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="consumValue">Avaiable Qty</asp:ListItem>
                                                <asp:ListItem Value="Qty">Alloted Qty</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                        <td runat="server" id="kitstudentreturncolumnorder" visible="false">
                                            <asp:CheckBoxList ID="cblcolumnorder4" runat="server" Height="43px" Width="850px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Roll_no">Roll No</asp:ListItem>
                                                <asp:ListItem Value="reg_no">Reg No</asp:ListItem>
                                                <asp:ListItem Value="Stud_Name">Student Name</asp:ListItem>
                                                <asp:ListItem Value="MasterValue">Kit Name</asp:ListItem>
                                                <asp:ListItem Value="ItemCode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="Qty">Alloted Qty</asp:ListItem>
                                                <asp:ListItem Value="Issued">Issue Qty</asp:ListItem>
                                                <asp:ListItem Value="Balance">Balance Qty</asp:ListItem>
                                                <%--<asp:ListItem Value="Return">Return Qty</asp:ListItem>--%>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pcolumnorder1"
                            CollapseControlID="pheaderfilter1" ExpandControlID="pheaderfilter1" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/Images/right.jpeg"
                            ExpandedImage="~/Images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <asp:Label ID="lbl_erro2" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        <center>
                            <div>
                                <div>
                                    <center>
                                        <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" CssClass="multxtpanel"
                                            ShowHeaderSelection="false" Width="928px" Height="350px" OnUpdateCommand="Fpspread2_UpdateCommand">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </div>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <table>
                                    <tr runat="server" id="saveperson_table">
                                        <td>
                                            <asp:Label ID="lbl_Issuedate" runat="server" Text="Issue Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_issuedate" runat="server" CssClass="textbox txtheight1" ReadOnly="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_issuedate" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_issueperson" runat="server" Text="Issue Person"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_issueperson" runat="server" CssClass="textbox txtheight3" AutoPostBack="true"
                                                Width="400px" OnTextChanged="txt_issueperson_Text_Changed"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_issueperson"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_issuedsave" runat="server" Text="Issued" CssClass="textbox btn2"
                                                OnClick="btn_issuedsave_Click" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="return_row" visible="false">
                                      <td>
                                            <asp:Label ID="lbl_return" runat="server" Text="Return Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_reDate" runat="server" CssClass="textbox txtheight1" ReadOnly="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_reDate" runat="server"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                       <td>
                                            <asp:Button ID="btn_return" runat="server" Text="Return" CssClass="textbox btn2"
                                                OnClick="btn_return_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </center>
                </div>
            </div>
        </center>
        </center>
        <div id="imgdiv2" runat="server" visible="false" class="popupstyle" style="height: 50em;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errclose" CssClass=" textbox btn2 comm" OnClick="btn_errclose_Click"
                                            Text="OK" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <center>
        </form>
    </body>
    </html>
</asp:Content>
