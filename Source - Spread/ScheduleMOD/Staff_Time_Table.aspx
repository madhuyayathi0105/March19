<%@ Page Title="" Language="C#" MasterPageFile="~/ScheduleMOD/ScheduleSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Staff_Time_Table.aspx.cs" Inherits="Staff_Time_Table"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .Grid
        {
            border: 2px solid #999999;
            background-color: White;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
            overflow: auto;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green">Staff Time Table </span>
        <br />
        <br />
        <div class="maindivstyle">
            <br />
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                College
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="200px" OnSelectedIndexChanged="ddlcollege_change"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upddept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                            Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="p1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Designation
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDesig" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                            Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbDesig" runat="server" Text="Select All" OnCheckedChanged="cbDesig_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cblDesig" runat="server" OnSelectedIndexChanged="cblDesig_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDesig"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Staff Type
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtStfType" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                            Style="width: 100px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                            Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                            position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                            height: 200px;">
                                            <asp:CheckBox ID="cbStfType" runat="server" Text="Select All" OnCheckedChanged="cbStfType_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cblStfType" runat="server" OnSelectedIndexChanged="cblStfType_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtStfType"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Staff Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlStfName" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlStfName_OnSelectedIndexChanged"
                                            CssClass="textbox1 ddlheight5" Width="200px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Search By
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlSearchOption" runat="server" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" CssClass="textbox1 ddlheight5" Width="144px" OnSelectedIndexChanged="ddlSearchOption_Change"
                                            AutoPostBack="true">
                                            <asp:ListItem Selected="True" Text="Staff Code" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Staff Name" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td id="tdStfCode" runat="server" visible="false">
                                Staff Code
                            </td>
                            <td id="tdStfCodeAuto" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td id="tdStfName" runat="server" visible="false">
                                Staff Name
                            </td>
                            <td id="tdStfNameAuto" runat="server" visible="false">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="txtsearchpan">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" Height="35px" Width="50px" OnClick="btnGo_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
            </td> </tr> </table>
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
                            <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                        </td>
                        <td colspan="3" align="right">
                            <span id="spStaffName" class="headerDisp1" runat="server"></span>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="3" align="left">
                            <span id="spProgremme" class="headerDisp1" runat="server"></span>
                        </td>
                        <td colspan="3" align="right">
                            <span id="spSection" class="headerDisp1" runat="server"></span>
                        </td>
                    </tr>--%>
                </table>
                <asp:GridView ID="grdStf_TT" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                    HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnRowDataBound="grdStf_TT_OnRowDataBound">
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
                            <HeaderTemplate>
                                <asp:Label ID="lbl1" runat="server" Text="Period 1" />
                                <br />
                                <asp:Label ID="Label1" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 2" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl2" runat="server" Text="Period 2" />
                                <br />
                                <asp:Label ID="Label2" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 3" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl3" runat="server" Text="Period 3" />
                                <br />
                                <asp:Label ID="Label3" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 4" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl4" runat="server" Text="Period 4" />
                                <br />
                                <asp:Label ID="Label4" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 5" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl5" runat="server" Text="Period 5" />
                                <br />
                                <asp:Label ID="Label5" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 6" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl6" runat="server" Text="Period 6" />
                                <br />
                                <asp:Label ID="Label6" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 7" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl7" runat="server" Text="Period 7" />
                                <br />
                                <asp:Label ID="Label7" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 8" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl8" runat="server" Text="Period 8" />
                                <br />
                                <asp:Label ID="Label8" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 9" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl9" runat="server" Text="Period 9" />
                                <br />
                                <asp:Label ID="Label9" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 10" Visible="false">
                            <HeaderTemplate>
                                <asp:Label ID="lbl10" runat="server" Text="Period 10" />
                                <br />
                                <asp:Label ID="Label10" runat="server" />
                            </HeaderTemplate>
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
            <br />
            <br />
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
        </ContentTemplate> </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel9">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
