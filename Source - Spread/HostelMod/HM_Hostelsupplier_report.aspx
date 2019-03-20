<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_Hostelsupplier_report.aspx.cs" Inherits="HM_Hostelsupplier_report" %>

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
        <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
        <style type="text/css">
            .div
            {
                left: 0%;
                top: 0%;
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


            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Supplier History Report</span></div>
                <br />
                <br />
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="height: 850px; width: 1000px;">
                <%--maincontent--%>
                <center>
                    <div>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <%--<td>
                                <asp:Label ID="lbl_colgname" runat="server" Style="top: 10px; left: 6px;" Text="College Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight3"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>--%>
                                <td>
                                    <asp:Label ID="lbl_hostelname" runat="server" Style="top: 10px; left: 6px;" Text="Mess Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPHostelName" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_hostelname" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_hostel" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostel_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_hostel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_hostel_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popHostelName" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="panel_hostelname" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_suppliername" runat="server" Style="top: 10px; left: 6px;" Text="Supplier Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_supplier" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_supplier" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_supplier" runat="server" Height="150px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_supplier" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_supplier_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_supplier" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_supplier_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop_supplier" runat="server" TargetControlID="txt_supplier"
                                                PopupControlID="panel_supplier" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_fromdate" runat="server" Style="top: 10px; left: 6px;" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_fromdate" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight" OnTextChanged="txt_fromdate_TextChanged"
                                                AutoPostBack="true"> </asp:TextBox>
                                            <asp:CalendarExtender ID="Cal_fromdate" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Style="top: 10px; left: 6px;" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_todate" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight" AutoPostBack="true"
                                                OnTextChanged="txt_todate_TextChanged"> </asp:TextBox>
                                            <asp:CalendarExtender ID="cal_todate" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%-- CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="906px"
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
                            <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="906px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
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
                                            <asp:TextBox ID="tborder" Visible="false" Width="850px" TextMode="MultiLine" CssClass="style1"
                                                AutoPostBack="true" runat="server" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                                Width="906px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                                <asp:ListItem Value="vendor_name">Supplier Name</asp:ListItem>
                                                <asp:ListItem Value="GI_Date">Supplied Date</asp:ListItem>
                                                <asp:ListItem Value="Order_Date">Purchase Order Date</asp:ListItem>
                                                <asp:ListItem Value="Order_Code">Purchase Order Code</asp:ListItem>
                                                <asp:ListItem Value="itemheader_name">Item Header Name</asp:ListItem>
                                                <asp:ListItem Value="Item_Code">Item Code</asp:ListItem>
                                                <asp:ListItem Value="Item_Name">Item Name</asp:ListItem>
                                                <asp:ListItem Value="Item_unit">Measure</asp:ListItem>
                                                <asp:ListItem Value="order_qty">Ordered QTY</asp:ListItem>
                                                <asp:ListItem Value="OrderedAmt">Ordered QTY Amount</asp:ListItem>
                                                <asp:ListItem Value="Request_qty">Requested QTY</asp:ListItem>
                                                <asp:ListItem Value="Request_Amt">Requested QTY Amount</asp:ListItem>
                                                <asp:ListItem Value="RejQty">Rejected QTY</asp:ListItem>
                                                <asp:ListItem Value="MasterValue">Reject Reason</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                        <div id="div1" runat="server" visible="false" class="reportdivstyle" style="width: 900px;">
                            <br />
                            <br />
                            <FarPoint:FpSpread ID="Fpspread1" runat="server" Width="850px" Style="overflow: auto;
                                height: 350px; border: 0px solid #999999; border-radius: 5px; background-color: White;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" CssClass="textbox textbox1"
                                onkeypress="display()"></asp:TextBox>
                            <%--   theivamani 15.10.15--%>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1"
                                Text="Export To Excel" Width="127px" Height="30px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Width="60px" Height="30px" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </div>
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
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                                <%-- <tr>
                        <td colspan="4">
                            <br />
                            <center>
                                <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="textbox btn1" OnClick="btn_update_Click"
                                    Visible="false" OnClientClick="return Test()" />
                                <asp:Button ID="btn_delete" runat="server" Text="Delete" CssClass="textbox btn1" OnClick="btn_delete_Click"
                                    Visible="false" OnClientClick="return Test()" />
                                <asp:Button ID="btn_save1" Text="Save" Visible="false" runat="server" CssClass="textbox btn1"
                                    OnClientClick="return Test()" OnClick="btn_save1_Click" />
                                <asp:Button ID="btn_exit1" Text="Exit" runat="server" CssClass="textbox btn1" OnClick="btn_exit1_Click" />
                            </center>
                        </td>
                    </tr>--%>
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
