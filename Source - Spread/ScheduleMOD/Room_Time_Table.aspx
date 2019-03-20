<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Room_Time_Table.aspx.cs" Inherits="Room_Time_Table"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #printdiv
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <script type="text/javascript">
        function printTTOutput() {
            var panel = document.getElementById("<%=printdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green">Room Time Table </span>
            <br />
            <br />
            <div class="maindivstyle">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            College
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="213px" OnSelectedIndexChanged="ddlcollege_change"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Room Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRoomType" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="120px" OnSelectedIndexChanged="ddlRoomType_change"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Room
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRoom" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight1" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset id="fldDates" runat="server" style="border-color: Black; border-radius: 5px;
                                width: auto;">
                                <asp:RadioButton ID="radSemWise" runat="server" Text="Semester Wise" Checked="true"
                                    GroupName="SemDay" OnCheckedChanged="radSemWise_Change" AutoPostBack="true" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="radDayWise" runat="server" Text="Day Wise" GroupName="SemDay"
                                    OnCheckedChanged="radDayWise_Change" AutoPostBack="true" Visible="false" />
                            </fieldset>
                        </td>
                        <td id="tdlbFrm" runat="server" visible="false">
                            From Date
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtFrmDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                                Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                                font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="calFrmDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txtFrmDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="lblToDt" runat="server" Text="To Date" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                                Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                                font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="calToDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txtToDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Button ID="btnGo" runat="server" Text="Go" Height="35px" Width="50px" OnClick="btnGo_Click"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2" />
                                 <asp:RadioButton ID="format1" runat="server" Text="Format1"
                                GroupName="Attendance" AutoPostBack="true" Checked="true" />
                    <asp:RadioButton ID="format2" runat="server" Text="Format2" 
                                GroupName="Attendance" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                    Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua"></asp:Label>
                <br />
                <div id="printdiv" runat="server">
                    <table class="printclass" style="width: 98%; height: auto; margin: 0px;">
                        <tr>
                            <th align="center" colspan="6">
                                <span id="spCollegeName" class="headerDisp" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <th align="center" colspan="6">
                                <span id="spAddr" class="headerDisp1" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <th align="center" colspan="6">
                                <span id="spReportName" class="headerDisp1" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <span id="spRoomType" class="headerDisp1" runat="server"></span>
                            </td>
                            <td colspan="3" align="right">
                                <span id="spRoomNo" class="headerDisp1" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                        HeaderStyle-BackColor="#0CA6CA" BackColor="White">
                        <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                        <Columns>
                            <asp:TemplateField HeaderText="Day">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                                    <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 1" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 2" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 3" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 4" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 5" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 6" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 7" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 8" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 9" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Period 10" Visible="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' ForeColor="Blue"
                                        OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                    <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblTT_10" runat="server" Text='<%#Eval("TT_10") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                </div>
                <button id="btnComPrint" runat="server" visible="false" onclick="return printTTOutput();"
                    style="background-color: LightGreen; font-weight: bold; font-size: medium; font-family: Book Antiqua;">
                    Print
                </button>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
