<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="inv_purchaseorder_request.aspx.cs" Inherits="inv_purchaseorder_request" %>

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
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <asp:Label ID="lbl_header" runat="server" Style="color: Green;" Text="Purchase Order Request"
                    CssClass="fontstyleheader"></asp:Label>
                <br />
                <br />
            </center>
        </div>
        <div>
            <center>
                <div class="maindivstyle" style="height: 500px; width: 1000px;">
                    <br />
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="Rad_item" AutoPostBack="true" runat="server" Checked="true"
                                        Text="Item" GroupName="d" OnCheckedChanged="Rad_item_Click" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="Rad_book" AutoPostBack="true" runat="server" Text="Book" GroupName="d"
                                        OnCheckedChanged="Rad_book_Click" />
                                </td>
                            </tr>
                        </table>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_vendor" Width="100px" Visible="false" runat="server" Text="Vendor Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_vendorname" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" CssClass="multxtpanel" Visible="false" runat="server" Style="height: 250px;
                                                width: 250px; position: absolute;">
                                                <asp:CheckBox ID="cb_vendor" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_vendor_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_vendor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_vendor_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_vendorname"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblordercode" Width="100px" Visible="false" runat="server" Text="Order Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtordercode" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel2" CssClass="multxtpanel" Visible="false" runat="server" Style="height: 250px;
                                                width: 250px; position: absolute;">
                                                <asp:CheckBox ID="cb_oc" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_oc_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_oc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_oc_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtordercode"
                                                PopupControlID="Panel2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_item" runat="server" Visible="false" Text="Item Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_item" runat="server" Visible="false" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" Width="250px" Visible="false" CssClass="multxtpanel" runat="server"
                                                Style="height: 250px; position: absolute;">
                                                <asp:CheckBox ID="cb_item" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_item_CheckedChange" />
                                                <asp:CheckBoxList ID="cbl_item" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_item_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_item"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                        Width="75px" OnTextChanged="txt_fromdate_TextChanged" ForeColor="Black" Style="top: 31px;
                                        left: 63px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                        Width="75px" OnTextChanged="txt_todate_TextChanged" ForeColor="Black" Style="top: 31px;
                                        left: 194px;"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                        Format="dd/MM/yyyy">
                                        <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                    </asp:CalendarExtender>
                                </td>
                                <%--</tr>
                    <tr>--%>
                                <td colspan="3" class="textbox" style="width: 285px;">
                                    <asp:RadioButton ID="rdb_request" runat="server" Text="Request" AutoPostBack="true"
                                        OnCheckedChanged="rdb_request_CheckedChanged" GroupName="grdo" />
                                    <asp:RadioButton ID="rdb_wait" runat="server" AutoPostBack="true" OnCheckedChanged="rdb_wait_CheckedChanged"
                                        Text="Waiting" GroupName="grdo" />
                                    <asp:RadioButton ID="rdb_approval" runat="server" Text="Approval" OnCheckedChanged="approval_CheckedChanged"
                                        AutoPostBack="true" GroupName="grdo" />
                                    <asp:RadioButton ID="rdb_reject" runat="server" Text="Reject" AutoPostBack="true"
                                        OnCheckedChanged="reject_CheckedChanged" GroupName="grdo" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <div>
                                <asp:Label ID="lbl_error" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                        </center>
                        <center>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Style="height: 370px; overflow: auto; background-color: White;
                                border-radius: 10px; box-shadow: 0px 0px 8px #999999" OnButtonCommand="Fp_btn_Click"
                                ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                    </center>
                    <div id="pop_purchaseitems" runat="server" visible="false" style="height: 48em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 47px; margin-left: 484px;"
                            OnClick="imagebtnpopclose3_Click" />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 521px; width: 990px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <div style="position: absolute; margin-top: -6px; margin-left: 29px;">
                                <center>
                                    <asp:Label ID="lbl_time" runat="server" Font-Names="Viner Hand ITC" ForeColor="#DF95FB"></asp:Label>
                                </center>
                            </div>
                            <center>
                                <span style="color: Green;" class="fontstyleheader ">Purchase Item List</span>
                            </center>
                            <br />
                            <center>
                                <div class="spreadborder" style="height: 380px; width: 970px; background-color: White;">
                                    <FarPoint:FpSpread ID="Fpspread3" runat="server" Visible="true" BorderStyle="NotSet"
                                        BorderWidth="0px" Width="960px">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="LightBlue">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_approval" Text="Approve" Visible="false" runat="server" CssClass="textbox btn2"
                                        OnClick="btn_approval_Click" />
                                    <asp:Button ID="btn_reject" Text="Reject" Visible="false" runat="server" CssClass="textbox btn2"
                                        OnClick="btn_reject_Click" />
                                    <asp:Button ID="btn_poprint" Visible="false" runat="server" Text="PO Print" CssClass="textbox btn2"
                                        AutoPostBack="true" OnClick="btn_poprint_Click" />
                                    <asp:Button ID="btnpopwin1exit" Text="Exit" runat="server" CssClass="textbox btn2"
                                        OnClick="btnpop1winexit_Click" />
                                </div>
                            </center>
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
                                                    <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <center>
                                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn2 comm" Style="height: 28px;
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
        </div>
        </form>
    </body>
    </html>
</asp:Content>
