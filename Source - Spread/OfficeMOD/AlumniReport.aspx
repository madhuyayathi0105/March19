<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="AlumniReport.aspx.cs" Inherits="AlumniReport" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID%>').innerHTML = "";
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
            var fdt = document.getElementById('<%=txt_fromdate.ClientID%>');
            var todt = document.getElementById('<%=txt_todate.ClientID%>');

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
                fdt.value = currentDate.toString();
                todt.value = currentDate.toString();
                alert("To date should be greater than from date ");
                return false;
            }
        }
    </script>
    <br />
     <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Alumini Report</span></div>
        </center>
        <br />
    <div>
        <center>
            <div>
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_collegename" Text="College" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <%-- <td>
                                Batch
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="panel_batch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                            <td colspan="2">
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
                                                <asp:Label ID="lbl_todate" runat="server" Text="To"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" Style="height: 20px; width: 75px;" onchange="return checkDate()"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                Type
                            </td>
                            <td>
                                <asp:UpdatePanel ID="uptype" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txttype" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnltype" runat="server" CssClass="multxtpanel" Style="width: 121px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbtype" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbtype_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbltype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbltype_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pcetype" runat="server" TargetControlID="txttype" PopupControlID="pnltype"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" runat="server" CssClass="textbox btn2" Style="height: 28px;
                                    width: 60px;" Text="Go" OnClick="btnGo_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <table>
                    <tr>
                        <td>
                            <center>
                                <div>
                                    <center>
                                        <asp:Panel ID="Colheader" runat="server" CssClass="cpHeader" Visible="true" Height="22px"
                                            Width="146px" BackColor="#0CA6CA" Style="margin-top: -0.1%; margin-left: -853px;">
                                            <asp:Label ID="lblflt" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                                                Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                        </asp:Panel>
                                    </center>
                                </div>
                                <br />
                                <div>
                                    <asp:Panel ID="Colorder" runat="server" CssClass="maintablestyle" Width="930px">
                                        <div id="divcolumn" runat="server" style="height: 300px; width: 930px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbcolorder" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbcolorder_OnCheckedChanged " />
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -477px;"
                                                            Visible="false" Width="111px">Remove  All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="tborder" Width="867px" TextMode="MultiLine" CssClass="style1" AutoPostBack="true"
                                                            runat="server" Enabled="false">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cblcolorder" runat="server" AutoPostBack="true" Height="43px"
                                                            Width="920px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                            RepeatColumns="4" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolorder_OnSelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </center>
                            <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="Colorder"
                                CollapseControlID="Colheader" ExpandControlID="Colheader" Collapsed="true" TextLabelID="lblflt"
                                CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                                ExpandedImage="~/images/down.jpeg">
                            </asp:CollapsiblePanelExtender>
                        </td>
                    </tr>
                </table>
                <br />
                <%-- <center>--%>
                <table>
                    <tr>
                        <td>
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Style="overflow: auto;
                                border: 0px; solid #999999; border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                class="spreadborder" OnButtonCommand="FpSpread1_OnButtonCommand">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="SheetView1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <div id="print" runat="server" visible="false">
                                    <asp:Button ID="btnprint" runat="server" Text="Detail Print" ont-Names="Book Antiqua"
                                        Font-Size="small" Height="32px" Width="86px" CssClass="textbox textbox1" OnClick="btnprint_Click" />
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display(this)"
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
                        </td>
                    </tr>
                </table>
                <%-- </center>--%>
            </div>
            <%-- </div>--%>
        </center>
    </div>
    <%--popup window--%>
    <center>
        <div id="Errpopup" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
    </form>
</body>
</html>
</asp:Content>

