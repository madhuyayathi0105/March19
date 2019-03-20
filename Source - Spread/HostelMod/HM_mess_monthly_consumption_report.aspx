<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_mess_monthly_consumption_report.aspx.cs" Inherits="HM_mess_monthly_consumption_report" %>

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
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function display1() {
                document.getElementById('<%=Label1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <span style="color: Green;" class="fontstyleheader">Monthly Consumption Report</span>
                <br />
                <br />
            </center>
            <center>
                <div class="maindivstyle" style="height: 530px; width: 1000px;">
                    <br />
                    <table class="maintablestyle" cellspacing="4px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_messname" Text="Mess Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_messname" runat="server" Width="120px" CssClass="textbox textbox1"
                                            Height="20px" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="p5" runat="server" Height="200px" Width="150px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_messname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_messname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_messname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_messname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_messname"
                                            PopupControlID="p5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_itemheadername" runat="server" Text="Item Header Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_headername" runat="server" Height="20px" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="height: 190px;">
                                            <asp:CheckBox ID="cb_headername" runat="server" Width="100px" OnCheckedChanged="cb_headername_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_headername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_headername_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_headername"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_subheadername" runat="server" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Style="height: 190px;">
                                            <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_itemname" runat="server" Text="Item Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_itemname" runat="server" Height="20px" CssClass="textbox textbox1"
                                            ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="height: 190px;">
                                            <asp:CheckBox ID="cb_itemname" runat="server" Width="100px" OnCheckedChanged="cb_itemname_CheckedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_itemname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_itemname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_itemname"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Text=" From Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight1"
                                    Width="80px" AutoPostBack="true" OnTextChanged="txt_fromdateChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight1"
                                    Width="80px" AutoPostBack="true" OnTextChanged="txt_todateChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="caltodate" TargetControlID="txt_todate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblSession" runat="server" Text="Session Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsession" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cbsession" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbsessionCheckedChanged" />
                                            <asp:CheckBoxList ID="cblsession" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsessionSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsession"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_menuname" runat="server" Text="Menu Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_menuname" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pmenuname" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_menuname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_menuname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_menuname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_menuname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_menuname"
                                            PopupControlID="pmenuname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_show" runat="server" OnCheckedChanged="visiblity" Text="Show All"
                                    AutoPostBack="True" />
                                <%--    </td>
                            <td>--%>
                                <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <div>
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </center>
                    <br />
                    <center>
                        <div>
                            <div id="div2" runat="server" visible="false" style="width: 990px; height: 450px;
                                overflow: auto;" class="table">
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Width="990px" Height="420px" OnCellClick="FpSpread1_CellClick"
                                    OnPreRender="FpSpread1_SelectedIndexChange">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </div>
                    </center>
                    <div id="rptprint" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <br />
                        <asp:Label ID="lblrptname" Text="Report Name" runat="server"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                            Width="127px" CssClass="textbox btn1" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Width="60px" CssClass="textbox btn1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </center>
            <center>
                <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 36px; margin-left: 440px;"
                        OnClick="imagebtnpopclose_Click" />
                    <center>
                        <div id="Div3" runat="server" class="table" style="background-color: White; height: 500px;
                            width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 50px;
                            border-radius: 10px;">
                            <center>
                                <span style="color: Green;" class="fontstyleheader">Daily Consumption Report</span>
                                <br />
                                <table width="700px">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="datelable" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblveg" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblnonveg" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="div4" runat="server" visible="false" style="width: 800px; height: 350px;
                                    overflow: auto;" class="table">
                                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Width="800px">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                                <div id="Div1" runat="server" visible="false">
                                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                        Visible="false"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label2" Text="Report Name" runat="server"></asp:Label>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="180px" onkeypress="display1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="Button1" runat="server" OnClick="btnExcel1_Click" Text="Export To Excel"
                                        Width="127px" CssClass="textbox  btn1" />
                                    <asp:Button ID="Button2" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                        Width="60px" CssClass="textbox btn1" />
                                    <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
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
