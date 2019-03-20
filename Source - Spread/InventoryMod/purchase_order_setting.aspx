<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="purchase_order_setting.aspx.cs" Inherits="purchase_order_setting" %>

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
        <%-- <link href="Styles/css/checkboxcss.css" rel="stylesheet" type="text/css" />--%>
        <style type="text/css">
            .maindivstylesize
            {
                height: 550px;
                width: 1000px;
            }
            .sty1
            {
                -moz-border-left-colors: none;
                -moz-border-right-colors: none;
                -moz-border-top-colors: none;
                border-bottom: 5px solid #0ca6ca;
                border-color: #0ca6ca;
                border-image: none;
                border-left: 5px solid #0ca6ca;
                border-radius: 10px;
                border-style: solid;
                border-top: 30px solid #0ca6ca;
                height: 400px;
                width: 750px;
            }
        </style>
    </head>
    <body>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <br />
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green" cssclass="fontstyleheader">Purchase
                                Order Setting</span>
                        </div>
                        <br />
                    </center>
                </div>
            </div>
            <center>
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <center>
                        <div>
                            <fieldset style="border: 1px solid #0CA6CA; width: 845px; height: 24px; border-radius: 10px;
                                background-color: #0CA6CA; font-size: medium;" class="maintable1">
                                <table cellspacing="20px;" style="height: 10px; margin-top: -20px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_collegename" Text="College" runat="server" CssClass="txtheight"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1 ddlheight5"
                                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdo_mess" runat="server" Text="Mess" GroupName="b" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdo_inventory" runat="server" Text="Inventory" GroupName="b"
                                                AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdo_Library" runat="server" Text="Book" GroupName="b" AutoPostBack="true" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_new" runat="server" Text="Add New" CssClass="textbox btn2" OnClick="btn_new_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </center>
                    <br />
                    <br />
                    <center>
                        <div>
                            <asp:Label ID="lbl_error" runat="server" ForeColor="red"></asp:Label>
                        </div>
                    </center>
                    <center>
                        <div id="divPopper" runat="server" visible="false" class="popupstyle popupheight">
                            <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 29px; margin-left: 439px;"
                                OnClick="imagebtnpopclose1_Click" />
                            <br />
                            <br />
                            <center>
                                <div class="sty1" style="background-color: White; overflow: auto; width: 900px; height: 421px;"
                                    align="center">
                                    <br />
                                    <center>
                                        <asp:Label ID="lblHeader2" runat="server" Style="color: Green;" CssClass="fontstyleheader"
                                            Text="Purchase Order Setting"></asp:Label>
                                    </center>
                                    <br />
                                    <div align="center" style="overflow: auto; height: 320px; width: 850px; border-radius: 10px;
                                        border: 1px solid Gray;" class="spreadborder">
                                        <br />
                                        <fieldset style="border: 1px solid #0CA6CA; width: 680px; height: 24px; border-radius: 10px;
                                            background-color: #0CA6CA; font-size: medium;" class="maintable1 ">
                                            <table cellspacing="20px;" style="height: 10px; margin-top: -20px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_clg" Text="College" runat="server" CssClass="txtheight"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_clg" runat="server" CssClass="textbox1 ddlheight5" OnSelectedIndexChanged="ddl_clg_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbpopmess" runat="server" Text="Mess" GroupName="a" AutoPostBack="true" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbpopinv" runat="server" Text="Inventory" GroupName="a" AutoPostBack="true" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdbpoplib" runat="server" Text="Book" GroupName="a"
                                                            AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <br />
                                        <br />
                                        <center>
                                            <center>
                                                <div id="div1" runat="server">
                                                    <fieldset style="width: 810px; margin-top: -12px;" class="maintable1">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rd_user_approval" runat="server" GroupName="select" Text="User approved vendor for indiviual Item" />
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rd_requestapproved" runat="server" GroupName="select" Text="Select vendor for request and Approved" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rd_direct_po" runat="server" GroupName="select" Text="User direct purchase order" />
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="rd_request_po" runat="server" GroupName="select" Text="Use request and purchase order" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rd_quatation_po" runat="server" GroupName="select" Text="Use quotation and purchase order" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                            </center>
                                        </center>
                                        <br />
                                        <br />
                                        <center>
                                            <center>
                                                <div id="div2" runat="server">
                                                    <table cellspacing="7px" class="maintable1">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chk_purchase" runat="server" Text="Purchase" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chk_service" runat="server" Text="Service" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight">
                                                                </asp:TextBox>
                                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight">
                                                                </asp:TextBox>
                                                                <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" CssClass="textbox btn2" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_update" runat="server" Visible="false" Text="Update" OnClick="btn_update_Click"
                                                                    CssClass="textbox btn2" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btn_delete" runat="server" Visible="false" Text="Delete" OnClick="btn_delete_Click"
                                                                    CssClass="textbox btn2" />
                                                                <asp:Button ID="btn_exit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btn_exit_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <center>
                                                        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 20px;
                                                            left: 0px;">
                                                            <center>
                                                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
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
                                                                                            Text="Ok" runat="server" OnClick="btnerrclose_Click" />
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
                                        </center>
                                    </div>
                                </div>
                            </center>
                        </div>
                    </center>
                    <center>
                        <asp:Label ID="errorlable" runat="server" Text="No Records Found" ForeColor="Red"
                            Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        <div>
                            <div id="spreaddiv" runat="server" visible="false" style="width: 850px; height: 350px;
                                overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;"
                                class="spreadborder">
                                <br />
                                <FarPoint:FpSpread ID="FpSpread1" Visible="false" OnCellClick="Cell_Click" OnPreRender="FpSpread1_render"
                                    runat="server" Width="750px" Height="350px">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
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
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
