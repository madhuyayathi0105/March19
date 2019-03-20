<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DepatmentwiseCollectionReport.aspx.cs" Inherits="DepatmentwiseCollectionReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
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
                toDate = document.getElementById('<%=txt_todate.ClientID%>').value;

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
                    document.getElementById('<%=txt_fromdate.ClientID%>').value = currentDate;
                    document.getElementById('<%=txt_todate.ClientID%>').value = currentDate;
                    alert("To date should be greater than from date ");
                    return false;
                }
            }       
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Departmentwise Collection Report</span></div>
            </center>
        </div>
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" Text="College" runat="server"></asp:Label>
                                <asp:Label ID="lbldept" Text="College" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="up_clg" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtclg" runat="server" Style="height: 20px; width: 350px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlclg" runat="server" CssClass="multxtpanel" Style="width: 350px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbclg" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbclg_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblclg_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceclg" runat="server" TargetControlID="txtclg" PopupControlID="pnlclg"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" runat="server" Style="height: 20px; width: 124px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 190px;">
                                            <asp:CheckBox ID="cb_sem" runat="server" Width="124px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_sem_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="panel_sem" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                PaymentMode
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upd_paid" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_paid" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnl_paid" runat="server" CssClass="multxtpanel multxtpanleheight"
                                            Style="width: 126px; height: 160px;">
                                            <asp:CheckBox ID="chk_paid" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="chk_paid_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="chkl_paid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkl_paid_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_paid"
                                            PopupControlID="pnl_paid" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="divdatewise" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_fromdate" runat="server" Text="From"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"
                                                    onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 4px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbbfrecon" runat="server" Checked="true" Text="Before Reconciliation" />
                                            </td>
                                            <td colspan="2">
                                                <asp:CheckBox ID="cbbeforeadm" runat="server" Text="Before Admission" Checked="true" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btngo_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <table id="tblpaymode" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblc2" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="Cash" BackColor="LightCoral"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblc3" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="Cheque" BackColor="LightGray"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblc5" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="DD" BackColor="Orange"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblc1" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="Challan" BackColor="LightGreen"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblc4" runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Text="Online Pay" BackColor="LightGoldenrodYellow"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblcard" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Card" BackColor="white"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="spreadDet" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    class="spreadborder">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="Green">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <div id="print" runat="server" visible="false">
                                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Report Name" visible="false"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                                            CssClass="textbox textbox1 txtheight4" visible="false"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnClick="btnExcel_Click" visible="false" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmasterhed" visible="false" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                                            CssClass="textbox textbox1" Width="60px" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
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
</asp:Content>
