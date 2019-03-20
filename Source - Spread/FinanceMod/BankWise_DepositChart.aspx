<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="BankWise_DepositChart.aspx.cs" Inherits="BankWise_DepositChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">BankWise Deposit Chart</span>
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="height: 575px; width: 1000px;">
                    <br />
                    <table class="maintablestyle" id="maintable" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_clg" runat="server" Text="College Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_col" runat="server" CssClass="textbox textbox1 ddlheight4"
                                    OnSelectedIndexChanged="ddl_col_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbank" runat="server" Text="Bank Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_bank" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbankname" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Width="150px">
                                            <asp:CheckBox ID="cbbankname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbbankname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblbankname" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cblbankname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_bank"
                                            PopupControlID="pbankname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbltype" runat="server" Text="Type Of Deposit"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_typedep" runat="server" CssClass="textbox textbox1 txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="ptypedep" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Width="150px">
                                            <asp:CheckBox ID="cbtypedep" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbtypedep_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbltypedep" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="cbltypedep_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_typedep"
                                            PopupControlID="ptypedep" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <fieldset style="width: 300px;">
                                    <asp:RadioButton ID="rb_Datewise" runat="server" AutoPostBack="true" Checked="true"
                                        Text="Date Wise" GroupName="date" OnCheckedChanged="rb_Datewise_OnCheckedChanged" />
                                    <asp:RadioButton ID="rd_monthwise" runat="server" AutoPostBack="true" Text="Month Wise"
                                        GroupName="date" OnCheckedChanged="rd_monthwise_OnCheckedChanged" />
                                    <asp:RadioButton ID="rb_yearwise" runat="server" AutoPostBack="true" Text="Year Wise"
                                        GroupName="date" OnCheckedChanged="rb_yearwise_OnCheckedChanged" />
                                </fieldset>
                            </td>
                            <td>
                                <asp:Label ID="lblfrmdate" runat="server" Text="From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_frmdate" runat="server" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="cal_frmdate" runat="server" TargetControlID="txt_frmdate"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddlfrmyear" runat="server" CssClass="textbox textbox1 ddlheight2">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlfrmmonwise" runat="server" CssClass="textbox textbox1 ddlheight">
                                    <asp:ListItem Value="0">Jan</asp:ListItem>
                                    <asp:ListItem Value="1">Feb</asp:ListItem>
                                    <asp:ListItem Value="2">Mar</asp:ListItem>
                                    <asp:ListItem Value="3">Apr</asp:ListItem>
                                    <asp:ListItem Value="4">May</asp:ListItem>
                                    <asp:ListItem Value="5">June</asp:ListItem>
                                    <asp:ListItem Value="6">July</asp:ListItem>
                                    <asp:ListItem Value="7">Aug</asp:ListItem>
                                    <asp:ListItem Value="8">Sep</asp:ListItem>
                                    <asp:ListItem Value="9">Oct</asp:ListItem>
                                    <asp:ListItem Value="10">Nov</asp:ListItem>
                                    <asp:ListItem Value="11">Dec</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlfrmmonyear" runat="server" CssClass="textbox textbox1 ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_todate"
                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:DropDownList ID="ddltoyear" runat="server" CssClass="textbox textbox1 ddlheight2">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddltomonwise" runat="server" CssClass="textbox textbox1 ddlheight">
                                    <asp:ListItem Value="0">Jan</asp:ListItem>
                                    <asp:ListItem Value="1">Feb</asp:ListItem>
                                    <asp:ListItem Value="2">Mar</asp:ListItem>
                                    <asp:ListItem Value="3">Apr</asp:ListItem>
                                    <asp:ListItem Value="4">May</asp:ListItem>
                                    <asp:ListItem Value="5">June</asp:ListItem>
                                    <asp:ListItem Value="6">July</asp:ListItem>
                                    <asp:ListItem Value="7">Aug</asp:ListItem>
                                    <asp:ListItem Value="8">Sep</asp:ListItem>
                                    <asp:ListItem Value="9">Oct</asp:ListItem>
                                    <asp:ListItem Value="10">Nov</asp:ListItem>
                                    <asp:ListItem Value="11">Dec</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddltomonyear" runat="server" CssClass="textbox textbox1 ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox textbox1 btn1"
                                    OnClick="btngo_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Label ID="lblerrgo" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <div>
                        <asp:Chart ID="chart" runat="server" Visible="false">
                            <Series>
                            </Series>
                            <Legends>
                                <asp:Legend Title="Performance Graph" Font="Book Antiqua">
                                </asp:Legend>
                            </Legends>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 15pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS,15pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                            </Legends>
                        </asp:Chart>
                    </div>
                    <div id="div1" runat="server" visible="false" style="width: 800px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" CssClass="spreadborder"
                            Width="700px" Height="300px" OnCellClick="Cellcont_Click" OnPreRender="Fpspread1_render"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <center>
                        <div id="rportprint" runat="server" visible="true">
                            <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            <asp:Label ID="lblrporttname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1"
                                Text="Export To Excel" Width="127px" Height="35px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Width="60px" Height="35px" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </center>
                    <br />
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
