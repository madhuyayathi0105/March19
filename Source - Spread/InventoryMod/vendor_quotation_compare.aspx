<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master"
    AutoEventWireup="true" CodeFile="vendor_quotation_compare.aspx.cs" Inherits="vendor_quotation_compare" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
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
        <script type="text/javascript">
            function display2() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <br />
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: green;">Supplier Quotation Comparative</span>
                        </div>
                        <br />
                    </center>
                </div>
            </div>
            <%--base screen--%>
            <center>
                <div class="maindivstyle maindivstylesize" style="width: 1000px; height: 1495px;">
                    <br />
                    <%-- <asp:UpdatePanel ID="upd" runat="server">
                    <ContentTemplate> </ContentTemplate>
                </asp:UpdatePanel>--%>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="Rad_item" AutoPostBack="true" runat="server" Text="Item" GroupName="d"
                                        OnCheckedChanged="Rad_item_Click" />
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
                                    <asp:Label ID="lbl_reqcomparecode" runat="server" Text="Request Compare Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_reqcompcode" runat="server" CssClass="textbox ddlheight2"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_reqcompcode_selectedIndexchange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_venname" runat="server" Text="SupplierName"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_venname" runat="server" CssClass="textbox textbox1 txtheight1"
                                                ReadOnly="true" Width="127px" Height="18px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 200px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cb_venname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_venname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_venname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_venname_SelectedIndexChange">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_venname"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_basego" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <div>
                                <asp:Label ID="lbl_baseerror" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </div>
                        </center>
                        <br />
                        <center>
                            <div>
                                <div id="spreaddiv" runat="server" visible="false" style="width: 900px; height: 350px;"
                                    class="spreadborder">
                                    <br />
                                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="1px" Width="875px" Height="325px">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </div>
                        </center>
                        <br />
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" Visible="false" CssClass="maintablestyle"
                                    Height="22px" Width="850px" Style="margin-top: -0.1%;">
                                    <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <center>
                            <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="850px">
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
                                            &nbsp;
                                            <asp:TextBox ID="tborder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="false"
                                                Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="6" RepeatDirection="Horizontal">
                                                <%--OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged"--%>
                                                <%-- <asp:ListItem Value="VendorName">Supplier Name</asp:ListItem>--%>
                                                <asp:ListItem Value="ItemName">Item Name</asp:ListItem>
                                                <asp:ListItem Value="itemcode">Item Code</asp:ListItem>
                                                <asp:ListItem Value="quantity">Quantity</asp:ListItem>
                                                <asp:ListItem Value="rpu">Rpu</asp:ListItem>
                                                <asp:ListItem Value="discount">Discount</asp:ListItem>
                                                <asp:ListItem Value="tax">Tax</asp:ListItem>
                                                <asp:ListItem Value="extax">Exercies Tax</asp:ListItem>
                                                <asp:ListItem Value="educationcess">Education Cess</asp:ListItem>
                                                <asp:ListItem Value="hieducationcess">Higher Education Cess</asp:ListItem>
                                                <asp:ListItem Value="othercharge">Other Charges</asp:ListItem>
                                                <asp:ListItem Value="cost">Cost</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                            ExpandedImage="~/images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </center>
                    <center>
                        <br />
                        <div>
                            <asp:Button ID="btn_selectitem" Text="Add" Visible="false" runat="server" CssClass="textbox btn3"
                                Height="30px" OnClick="btn_go1_Click" />
                        </div>
                    </center>
                    <br />
                    <center>
                        <div id="spreaddiv1" runat="server" visible="false" style="width: 900px; height: 350px;"
                            class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread5" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="875px" Height="325px" OnUpdateCommand="FpSpread5_Command">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <center>
                        <div id="Div1" runat="server" visible="false" style="width: 900px; height: 350px;"
                            class="spreadborder">
                            <br />
                            <FarPoint:FpSpread ID="spreadDet1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                Width="980px" Style="overflow: auto; border: 0px solid #999999; border-radius: 10px;
                                background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                                <%--OnCellClick="spreadDet1_OnCellClick" OnPreRender="spreadDet1_Selectedindexchange"  --%>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        <div id="rptprint" runat="server" visible="false">
                                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                Visible="false"></asp:Label>
                                            <br />
                                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                            <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display2()"
                                                CssClass="textbox textbox1"></asp:TextBox>
                                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                                Width="127px" CssClass="textbox btn1" />
                                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                CssClass="textbox btn1" Width="60px" />
                                            <asp:Button ID="btn_purchasereq" Visible="false" Height="30px" Text="Purchase Request"
                                                runat="server" CssClass="textbox btn3" OnClick="btn_purchase_request_Click" />
                                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <center>
                        <div id="Div_supplierDetails" runat="server" visible="false" style="width: 900px;
                            height: 350px;" class="spreadborder">
                            <br />
                            <asp:GridView ID="Showgrid" runat="server" ShowFooter="false" ShowHeader="false"
                                AutoGenerateColumns="true" Font-Names="book antiqua" togeneratecolumns="true"
                                Visible="false" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#0CA6CA"
                                Style="width: 800px;" ShowHeaderWhenEmpty="false">
                            </asp:GridView>
                        </div>
                        <br />
                        <br />
                        <center>
                            <div id="Divgridprint" class="noprint" runat="server" visible="false" style="margin: 0px;
                                margin-bottom: 20px; margin-top: 15px; position: relative;">
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="Button1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel_Click1" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                    Height="35px" CssClass="textbox textbox1" />
                                <asp:Button ID="Button2" runat="server" Text="Print" OnClick="btnprintmaster_Click1"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" />
                                <new:newprintmater runat="server" id="Printcontrol1" visible="false" />
                            </div>
                        </center>
                    </center>
                    <center>
                        <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                            left: 0px;">
                            <center>
                                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
                    </center>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
