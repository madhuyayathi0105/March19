<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="HM_RebateMaster.aspx.cs" Inherits="HM_RebateMaster" %>

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
                height: 580px;
                width: 1000px;
            }
        </style>
    </head>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
            }
        </script>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <center>
                    <div>
                        <span style="color: Green;" class="fontstyleheader">Rebate</span>
                    </div>
                </center>
                <br />
                <div class="maindivstyle maindivstylesize">
                    <center>
                        <br />
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_hostelname" Text="Hostel Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" CssClass="multxtpanel" Height="200px">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_hostelname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupExt4" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_month" Text="Month" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_month" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p2" runat="server" Height="150px" Width="130px" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_month" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_month_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_month" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_month_SelectedIndexChanged">
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
                                            <asp:PopupControlExtender ID="PopupEx2" runat="server" TargetControlID="txt_month"
                                                PopupControlID="p2" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_rebate" Text="Rebate Type" runat="server"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rdbdate" Text="Days" runat="server" GroupName="same2" />
                                    <asp:RadioButton ID="rdbfxdamt" Text="Fixed Amount" runat="server" GroupName="same2" /><%--Visible="false"--%>
                                    <%--  <asp:RadioButtonList ID="Radiobtnstype" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                    Style="margin-left: 0px;" Font-Bold="true" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0">Days</asp:ListItem>
                                    <asp:ListItem Value="1">Fixed Amount</asp:ListItem>
                                </asp:RadioButtonList>--%>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_allowallmonth" Text="All Month" runat="server" />
                                    <%--<asp:RadioButton ID="rdb_monthwise" OnCheckedChanged="rdb_monthwise_SelectedIndex"
                                    Text="Monthwise" runat="server" GroupName="r" AutoPostBack="true" />
                                 <asp:RadioButton ID="rdb_allmonth" OnCheckedChanged="rdb_allmonth_SelectedIndex"
                                    Text="All Month" runat="server" GroupName="r" AutoPostBack="true" />
                                <asp:TextBox ID="txt_allmonth" placeholder="Rebate Days" Visible="false" runat="server"
                                    CssClass="textbox txtheight3 textbox1"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txt_allmonth" ValidChars="" FilterType="Numbers"></asp:FilteredTextBoxExtender>--%>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Height="330px" Width="850px"
                            Style="overflow: auto; background-color: White;" class="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <center>
                            <asp:Button ID="btnsave" Text="Save" runat="server" OnClick="btnsave_Click" CssClass="textbox btn2" />
                            <asp:Button ID="btn_reset" Text="Reset" runat="server" OnClick="btn_reset_Click"
                                CssClass="textbox btn2" />
                        </center>
                        <br />
                        <center>
                        </center>
                        <div id="div_report" runat="server" visible="false">
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label>
                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                    CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                                    AutoPostBack="true" OnClick="btnExcel_Click" />
                                <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                                    AutoPostBack="true" OnClick="btn_printmaster_Click" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </center>
                        </div>
                    </center>
                </div>
            </center>
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
        </div>
        </form>
    </body>
    </html>
</asp:Content>
