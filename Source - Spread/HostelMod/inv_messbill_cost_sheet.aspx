<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="inv_messbill_cost_sheet.aspx.cs" Inherits="inv_messbill_cost_sheet" %>

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
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <asp:Label ID="lblhead" Style="color: Green;" runat="server" class="fontstyleheader"
                            Text="Mess bill for the Month"></asp:Label>
                        <br />
                    </div>
                </center>
                <br />
        </center>
        </div> </center>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: auto;">
                <center>
                    <br />
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_messname" runat="server" Text="Mess Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_messname" runat="server" CssClass="textbox  txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel6" runat="server" CssClass="multxtpanel" Style="height: 200px;
                                                width: 160px;">
                                                <asp:CheckBox ID="cb_messname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_messname_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_messname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_messname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_messname"
                                                PopupControlID="Panel6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_year" runat="server" Text="Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox1  ddlheight1">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_month" Visible="false" runat="server" Text="Month"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_month" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_month" runat="server" Visible="false" CssClass="textbox" ReadOnly="true"
                                                Width="60px" Height="20px">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_month" Visible="false" runat="server" CssClass="multxtpanel"
                                                Style="height: 200px; width: 120px;">
                                                <asp:CheckBox ID="cb_month" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_month_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_month" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_month_SelectedIndexChanged">
                                                    <asp:ListItem Value="1">January</asp:ListItem>
                                                    <asp:ListItem Value="2">February</asp:ListItem>
                                                    <asp:ListItem Value="3">March</asp:ListItem>
                                                    <asp:ListItem Value="4">April</asp:ListItem>
                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                    <asp:ListItem Value="6">June</asp:ListItem>
                                                    <asp:ListItem Value="7">July</asp:ListItem>
                                                    <asp:ListItem Value="8">August</asp:ListItem>
                                                    <asp:ListItem Value="9">September</asp:ListItem>
                                                    <asp:ListItem Value="10">October</asp:ListItem>
                                                    <asp:ListItem Value="11">November</asp:ListItem>
                                                    <asp:ListItem Value="12">December</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="Pop_month" runat="server" TargetControlID="txt_month"
                                                PopupControlID="panel_month" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1 textbox1 "
                                        OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        <br />
                    </div>
                </center>
                <center>
                    <div id="spreaddiv" runat="server" visible="false" style="width: 870px; height: 389px;"
                        class="spreadborder">
                        <FarPoint:FpSpread ID="Fpspread1" Visible="false" runat="server" Width="750px" Height="387px"
                            OnButtonCommand="btnType_Click">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <center>
                    <div>
                    </div>
                </center>
                <center>
                    <div id="reportdiv" runat="server" visible="false" style="width: 671px; height: auto;"
                        class="spreadborder">
                        <FarPoint:FpSpread ID="Fpspread2" runat="server" Width=" 667px" Height="1000px">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <center>
                            <div id="rptprint" runat="server" visible="false">
                                <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                    Visible="false"></asp:Label>
                                <br />
                                <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname" runat="server" Width="180px" Height="20px" onkeypress="display()"
                                    CssClass="textbox textbox1"></asp:TextBox>
                                <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                    Width="127px" CssClass="textbox btn1" />
                                <asp:Button ID="btnprintmaster" runat="server" Visible="false" Text="Print" OnClick="btnprintmaster_Click"
                                    CssClass="textbox btn1" Width="60px" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </div>
                        </center>
                        </br>
                    </div>
                </center>
            </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>
