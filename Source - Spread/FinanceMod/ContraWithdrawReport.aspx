<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="ContraWithdrawReport.aspx.cs" Inherits="ContraWithdrawReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .btn
        {
            width: 76px;
            height: 30px;
        }
        .btn1
        {
            width: 30px;
            height: 30px;
        }
        .btn2
        {
            width: 40px;
            height: 30px;
        }
        .style
        {
            height: 500px;
            border: 1px solid #999999;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            -moz-box-shadow: 0px 0px 10px #999999;
            -webkit-box-shadow: 0px 0px 10px #999999;
            border: 3px solid #D9D9D9;
            border-radius: 15px;
        }
        .sty1
        {
            height: 640px;
            width: 900px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .sty2
        {
            height: 500px;
            width: 800px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .table
        {
            background-color: white;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
        }
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .multxtpanel
        {
            background: White;
            border-color: Gray;
            border-style: Solid;
            border-width: 2px;
            position: absolute;
            box-shadow: 0px 0px 4px #999999;
            border-radius: 5px;
            overflow: auto;
        }
        .spreadborder
        {
            border: 2px solid #999999;
            background-color: White;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
            overflow: auto;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
    </style>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
            function checkDate() {

                var fromDate = "";
                var toDate = "";
                var date = ""
                var date1 = ""
                var month = "";
                var month1 = "";
                var year = "";
                var year1 = "";
                var empty = "";
                fromDate = document.getElementById('<%=txt_fromdate.ClientID%>').value;
                toDate = document.getElementById('<%=Txt_Todate.ClientID%>').value;

                date = fromDate.substring(0, 2);
                month = fromDate.substring(3, 5);
                year = fromDate.substring(6, 10);

                date1 = toDate.substring(0, 2);
                month1 = toDate.substring(3, 5);
                year1 = toDate.substring(6, 10);
                var today = new Date();
                var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

                if (year == year1) {
                    if (month == month1) {
                        if (date == date1) {
                            empty = "";
                        }
                        else if (date < date1) {
                            empty = "";
                        }
                        else {
                            empty = "e";
                        }
                    }
                    else if (month < month1) {
                        empty = "";
                    }
                    else if (month > month1) {
                        empty = "e";
                    }
                }
                else if (year < year1) {
                    empty = "";
                }
                else if (year > year1) {
                    empty = "e";
                }
                if (empty != "") {
                    alert("To date should be greater than from date ");
                    //// document.getElementById('<%=Txt_Todate.ClientID %>').value = currentDate;
                    return false;
                }

            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <center>
                    <div>
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: Green;">Contra Withdraw Report </span>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div class="style" style="height: auto;">
                        <fieldset style="width: 166px; float: left;">
                            <asp:RadioButton ID="rbpety" runat="server" AutoPostBack="true" OnCheckedChanged="rbpety_OnCheckedChanged"
                                GroupName="w1" Text="Pety" />
                            <asp:RadioButton ID="rbbanks" runat="server" AutoPostBack="true" OnCheckedChanged="rbbanks_OnCheckedChanged"
                                GroupName="w1" Text="Bank" />
                            <asp:RadioButton ID="rbboth" runat="server" AutoPostBack="true" OnCheckedChanged="rbboth_OnCheckedChanged"
                                GroupName="w1" Text="Both" />
                        </fieldset>
                        <table class="table2">
                            <tr>
                                <td>
                                    <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_fromdate" TextMode="SingleLine" runat="server" Height="20px"
                                        CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbltodate" runat="server" Style="font-size: large;" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txt_Todate" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                        Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="Txt_Todate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <div id="divheader" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Header" Style="width: 50px;"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_studhed" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnl_studhed" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Style="width: 126px; height: 120px;">
                                                                <asp:CheckBox ID="chk_studhed" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="chk_studhed_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_studhed" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studhed_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_studhed"
                                                                PopupControlID="pnl_studhed" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Ledger"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_studled" runat="server" Style="height: 20px; width: 100px;"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnl_studled" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                                Style="width: 126px; height: 120px;">
                                                                <asp:CheckBox ID="chk_studled" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="chk_studled_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chkl_studled" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_studled_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_studled"
                                                                PopupControlID="pnl_studled" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <div id="divbank" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbank" runat="server" Text="Select Bank"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtbank" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel multxtpanleheight" Style="width: 126px;
                                                                height: 120px;">
                                                                <asp:CheckBox ID="chkbank" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="chkbank_OnCheckedChanged" />
                                                                <asp:CheckBoxList ID="chklbank" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chklbank_OnSelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbank"
                                                                PopupControlID="Panel1" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="Search" runat="server" CssClass="textbox btn2" Text="Search" Width=" 61px"
                                        OnClientClick="return checkDate()" OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <div id="divspread" runat="server" visible="false" style="height: auto; width: 950px;
                                            overflow: auto;">
                                            <center>
                                                <asp:Label ID="output" runat="server" Visible="true" Style="color: Blue;"></asp:Label>
                                            </center>
                                            <center>
                                                <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderStyle="Solid" Style="overflow: auto;
                                                    border: 0px solid #999999; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                                    class="spreadborder">
                                                    <Sheets>
                                                        <FarPoint:SheetView SheetName="Sheet1">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                </FarPoint:FpSpread>
                                            </center>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <center>
                                                <div id="print" runat="server" visible="false">
                                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Text="Report Name"></asp:Label>
                                                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                                        InvalidChars="/\">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                                    <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                                        CssClass="textbox textbox1" Width="60px" />
                                                    <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                                </div>
                                            </center>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </center>
            <center>
                <div id="pupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pupdiv1" runat="server" class="table" style="background-color: White; height: 120px;
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
        </div>
    </body>
    </html>
</asp:Content>
