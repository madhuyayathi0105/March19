<%@ Page Title="" Language="C#" MasterPageFile="~/InventoryMod/inventorysite.master" AutoEventWireup="true"
    CodeFile="Transfer_details.aspx.cs" Inherits="Transfer_details" %>

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
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green">Stock Transfer Details</span></div>
                    </center>
                    <br />
                    <div class="maindivstyle maindivstylesize">
                        <br />
                        <center>
                            <div>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_fromdate" runat="server" OnTextChanged="txt_fromdate_TextChanged"
                                                AutoPostBack="true" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight1"
                                                OnTextChanged="txt_todate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                Format="dd/MM/yyyy">
                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            Search By
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_option" runat="server" CssClass="textbox1 ddlheight1" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_option_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Mess Name" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Store Name" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Department" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_hostelname" runat="server" Visible="false" Text="Transfer Mess Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upp1" Visible="false" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_hosname" runat="server" CssClass="textbox textbox1 txtheight1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="150px" Width="160px">
                                                        <asp:CheckBox ID="cb_hos" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_mess_CheckedChange" />
                                                        <asp:CheckBoxList ID="cbl_hos" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mess_SelectedIndexChange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_hosname"
                                                        PopupControlID="p1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblstorename" runat="server" Visible="false" Text="Transfer Store Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="false">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_basestore" runat="server" CssClass="textbox textbox1 txtheight1"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="150px" Width="180">
                                                        <asp:CheckBox ID="cb_mainstore" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_mainstore_CheckedChange" />
                                                        <asp:CheckBoxList ID="cbl_mainstore" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_mainstore_SelectedIndexChange">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_basestore"
                                                        PopupControlID="Panel1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_degree" Visible="false" Text="Transfer Department" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upp4" Visible="false" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                        ReadOnly="true">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p3" runat="server" CssClass="multxtpanel" Width="250px" Height="180px">
                                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                            OnCheckedChanged="cb_degree_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_degree"
                                                        PopupControlID="p3" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div>
                                <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                <br />
                            </div>
                            <%--   <div id="spreaddiv1" runat="server" visible="false" style="width: 824px; height: 372px;"
                            class="spreadborder">--%>
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="966px" Height="360px" CssClass="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <%--</div>--%>
                            <br />
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                    Width="180px" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                    Text="Export To Excel" Width="127px" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                    </div>
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
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>
