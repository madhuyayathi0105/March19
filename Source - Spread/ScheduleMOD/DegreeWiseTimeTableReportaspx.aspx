<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DegreeWiseTimeTableReportaspx.aspx.cs" Inherits="DegreeWiseTimeTableReportaspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
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
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
            printWindow.document.write('</head><body oncontextmenu="return false" onkeypress="return false"> ');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>'); printWindow.document.close();
            setTimeout(function () { printWindow.print(); }, 500); return false;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Detailed
            TimeTable Report</span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <table class="maintablestyle" style="height: auto; width: auto;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                            AutoPostBack="True" Style="">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AssociatedControlID="txtBatch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBatch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBatch" Visible="true" Width="67px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBatch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkBatch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBatch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBatch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                        PopupControlID="pnlBatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Degree" AssociatedControlID="txtDegree"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlDegree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegree" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlDegree" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="140px">
                                        <asp:CheckBox ID="chkDegree" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblDegree" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                        PopupControlID="pnlDegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Branch" AssociatedControlID="txtBranch"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlBranch" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtBranch" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkBranch" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblBranch" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                        PopupControlID="pnlBranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbl_org_sem" Text="Semester" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox  txtheight1 commonHeaderFont" ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                    <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                    <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                    PopupControlID="Panel11" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <</tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblVisiblity" Text="Visiblity Settings" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtStaff" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="pnlstaff" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="chkStaff" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="chkStaff_checkedchange" />
                                    <asp:CheckBoxList ID="cblStaff" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblStaff_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtStaff"
                                    PopupControlID="pnlstaff" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox textbox1 txtheight3"
                                    ReadOnly="true">-- Select--</asp:TextBox>
                                <asp:Panel ID="pnlSubject" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="2px" CssClass="multxtpanel" Height="120px" Style="position: absolute;">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Select All" AutoPostBack="true"
                                        OnCheckedChanged="CheckBox1_checkedchange" />
                                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtSubject"
                                    PopupControlID="pnlSubject" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" Height="29px" Width="62px" OnClick="btnGo_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2" />
                        <button id="btnComPrint" runat="server" visible="true" Height="29px" Width="62px" onclick="return printTTOutput();"
                            style="background-color: LightGreen; font-weight: bold; font-size: medium; font-family: Book Antiqua;">
                            Print
                        </button>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <center>
            <div id="printdiv" runat="server">
                <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                    <tr>
                        <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                            <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                Width="100px" Height="100px" />
                        </td>
                        <th class="marginSet" align="center" colspan="6">
                            <span id="spCollegeName" class="headerDisp" runat="server"></span>
                        </th>
                    </tr>
                    <tr>
                        <th class="marginSet" align="center" colspan="6">
                            <span id="spAddr" class="headerDisp1" runat="server"></span>
                        </th>
                    </tr>
                    <tr>
                        <th class="marginSet" align="center" colspan="6">
                            <span id="spReportName" class="headerDisp1" runat="server"></span>
                        </th>
                    </tr>
                    <tr>
                        <td class="marginSet" colspan="3" align="center">
                            <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                        </td>
                        <td colspan="3" align="right">
                            <span id="spSem" class="headerDisp1" runat="server"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="marginSet" colspan="3" align="left">
                            <span id="spProgremme" class="headerDisp1" runat="server"></span>
                        </td>
                        <td class="marginSet" colspan="3" align="right">
                            <span id="spSection" class="headerDisp1" runat="server"></span>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                    HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnDataBound="OnDataBound">
                    <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Degree">
                            <ItemTemplate>
                                <asp:Label ID="lblDegreeDet" runat="server" Text='<%#Eval("DegDet") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#40e0d0" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Room No">
                            <ItemTemplate>
                                <asp:Label ID="lblRoomNo" runat="server" Text='<%#Eval("RoomNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#ffb6c1" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Day">
                            <ItemTemplate>
                                <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                                <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#d3ffce" />
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
        </center>
    </center>
    <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
    </div>
</asp:Content>
