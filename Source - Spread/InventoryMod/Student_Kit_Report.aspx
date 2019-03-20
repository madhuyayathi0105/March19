
<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="Student_Kit_Report.aspx.cs" Inherits="InventoryMod_Student_Kit_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="~/Scripts/jquery-latest.min.js" type="text/javascript"></script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Student Kit Report</span>
            </div>
        </center>
    </div>
    <center>
        <div id="maindiv" runat="server" class="maindivstyle" style="width: 930px; height: auto">
            <div>
                <table>
                    <tr>
                        <td>
                            <center>
                                <div>
                                    <table class="maintablestyle" style="width: 900px; height: auto">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                            ReadOnly="true" Width="100px" Style="font-family: 'Book Antiqua'; "
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px">
                                                            <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_CheckedChanged"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                                            PopupControlID="pbatch" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpnlDegree" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtDegree" style="Width:100px; margin-left:10px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px">
                                                            <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                                            PopupControlID="pnlDegree" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upnlBranch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                                        <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="200px">
                                                            <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                            <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                                runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                                            PopupControlID="pnlBranch" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_kitname" Text="Kit Name" runat="server" Style="float: left" Font-Bold="true"
                                                    Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
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
                                            <td colspan="6">
                                                <asp:Label ID="lbl_RollNo" Text="Roll No" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                                <asp:TextBox ID="txt_roll" placeholder="Search Roll No" runat="server" CssClass="textbox textbox1"
                                                    Width="152px" OnTextChanged="txt_roll_changed" AutoPostBack="true"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetRoll" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="multxt1panel"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_item" Text="Item Name" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_item" placeholder="Search Item name" runat="server" CssClass="textbox textbox1"
                                                    OnTextChanged="txt_item_changed"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetItemname" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_item"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_kit" Text="Kit Name" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_kit" placeholder="Search Kit name" runat="server" CssClass="textbox textbox1"
                                                    OnTextChanged="txt_kit_changed"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetKitname" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_kit"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                                    OnClick="btn_go_Click" Style="float: right;" BackColor="LightGreen" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </center>
    <br />
    <center>
        <div id="columnorder" runat="server" visible="false">
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
                            <%-- <td>
                                <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                    Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" Width="850px"
                                    Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;" RepeatColumns="5"
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem Value="degree">Degree</asp:ListItem>
                                    <asp:ListItem Value="Roll_no">Roll No</asp:ListItem>
                                    <asp:ListItem Value="Stud_Name">Student Name</asp:ListItem>
                                    <asp:ListItem Value="MasterValue">Kit Name</asp:ListItem>
                                    <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                    <asp:ListItem Value="Qty">Alloted Qty</asp:ListItem>
                                    <asp:ListItem Value="Issued">Issued Qty</asp:ListItem>
                                    <asp:ListItem Value="Balance">Balance Qty</asp:ListItem>
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
    </center>
    <br />
    <br />
    <center>
        <FarPoint:FpSpread ID="spreadDet1" runat="server" Visible="false" BorderColor="Black"
            BorderStyle="Solid" BorderWidth="1px" Width="950px" Style="overflow: auto; border: 0px solid #999999;
            border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;"
            ShowHeaderSelection="false" OnUpdateCommand="spreadDet1_UpdateCommand">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
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
            <asp:Button ID="btn_individual_Print" runat="server" Text="Individual Print" OnClick="btn_individual_Print_Click"
                Width="120px" CssClass="textbox textbox1 btn2" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </div>
        <br />
    </center>
    <center>
        <div id="alertimg" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
</asp:Content>
