<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ChallanConfirm.aspx.cs" Inherits="ChallanConfirm" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Challan Confirm</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .div
        {
            left: 0%;
            top: 0%;
        }
        .watermark
        {
            color: #999999;
        }
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .popsty3
        {
            height: 600px;
            width: 700px;
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
    </style>
    <body>
        <script type="text/javascript" language="javascript">
            function display() {
                document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
            }
            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=1056');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    //<div id="footer" style="background-color:White;"></div>
                    // <div id="header" style="background-color:White;"></div>
                    //                document.getElementById('header').style.display = 'none';
                    //                document.getElementById('footer').style.display = 'none';
                    printWindow.print();
                }, 500);
                return false;
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Challan Confirm</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 970px; height: 700px;">
                <center>
                    <div>
                        <table class="maintablestyle" style="width: 950px;">
                            <tr>
                                <%--<td>
                      Type
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle ddlheight3"
                            OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>--%>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox  ddlheight2"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_strm" Width="80px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_stream" runat="server" Visible="false"></asp:Label>
                                    <asp:UpdatePanel ID="UP_Type" runat="server" Visible="false">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_stream" runat="server" CssClass="textbox txtheight" ReadOnly="true">Stream</asp:TextBox>
                                            <asp:Panel ID="pnl_stream" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="130px">
                                                <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_stream_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stream_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_stream" runat="server" TargetControlID="txt_stream"
                                                PopupControlID="pnl_stream" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Batch &nbsp;&nbsp;<asp:CheckBox ID="cb_batchDeg" runat="server" OnCheckedChanged="cb_batchDeg_Change"
                                        AutoPostBack="true" />
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight" ReadOnly="true">Batch</asp:TextBox>
                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel multxtpanleheight1"
                                                Width="125px">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="SelectAll" AutoPostBack="True" OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="panel_batch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight" ReadOnly="true">Degree</asp:TextBox>
                                            <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="130px">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="panel_degree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight" ReadOnly="true">Department</asp:TextBox>
                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                                PopupControlID="panel_dept" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPpanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                placeholder="Semester/Year" onfocus="return myFunction1(this)"></asp:TextBox>
                                            <asp:Panel ID="panel_sem" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_sem" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupce_sem" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="panel_sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_header" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_header" runat="server" CssClass="textbox  txtheight" ReadOnly="true">Header</asp:TextBox>
                                            <asp:Panel ID="Panel_header" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_header_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_header"
                                                PopupControlID="Panel_header" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblappNo" runat="server" Visible="false" Text="AppForm No"></asp:Label>
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_regno" runat="server" CssClass="textbox  txtheight1" Width="110px"
                                        AutoPostBack="true" OnTextChanged="txt_chnoreg_OnTextChanged">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_regno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_name" Placeholder="Name" runat="server" CssClass="textbox  txtheight"
                                        Width="180px" AutoPostBack="true" OnTextChanged="txt_chnoName_OnTextChanged">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                        ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    Challan-Acr
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_chaln" runat="server" CssClass="textbox  txtheight">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    Challan-No
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_chno" runat="server" CssClass="textbox  txtheight" AutoPostBack="true"
                                        OnTextChanged="txt_chno_OnTextChanged" Width="100px">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderpa" runat="server" TargetControlID="txt_chno"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_befAftAdmis" runat="server" Visible="false" CssClass="textbox  ddlheight1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_befAftAdmis_OnSelectedIndexChanged">
                                        <asp:ListItem>Before Admission</asp:ListItem>
                                        <%--  <asp:ListItem Selected="True">After Admission</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cb_datewise" runat="server" Text="Datewise" AutoPostBack="true"
                                        OnCheckedChanged="cb_datewise_OnCheckedChanged" />
                                </td>
                                <td colspan="4" runat="server" id="td_challanOption">
                                    <asp:DropDownList ID="ddl_ChallanOption" runat="server" CssClass="textbox  ddlheight"
                                        Width="90px">
                                        <asp:ListItem Selected="True">Confirmed</asp:ListItem>
                                        <asp:ListItem>Unconfirmed</asp:ListItem>
                                        <asp:ListItem>Online</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="cb_fromToDate" runat="server" />
                                    From
                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                        Width="65px" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                    To
                                    <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                        Width="65px" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblDate" runat="server" Text="Confirm Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox  txtheight"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="3">
                                    Name Search
                                    <asp:TextBox ID="txt_Name_Search" runat="server" CssClass="textbox  txtheight3" Width="128px">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="GetSearchname" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_Name_Search"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblfin" runat="server" Text="Financial Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfinyear" runat="server" CssClass="textbox textbox1 ddlheight2"
                                        Style="width: 130px;" AutoPostBack="true" OnSelectedIndexChanged="ddlfinyear_Selected">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                        OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <br />
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="850px"
                                Style="margin-top: -0.1%;">
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="850px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Value="ChallanNo" Enabled="false">Challan No</asp:ListItem>
                                            <asp:ListItem Value="ChallanDate" Enabled="false">Challan Date</asp:ListItem>
                                            <asp:ListItem Value="RollAppNo">Roll/App No</asp:ListItem>
                                            <asp:ListItem Value="Reg_No">Reg No</asp:ListItem>
                                            <asp:ListItem Value="Stud_Name">Name</asp:ListItem>
                                            <asp:ListItem Value="Degree">Degree</asp:ListItem>
                                            <asp:ListItem Value="transcode">Receipt No</asp:ListItem>
                                            <asp:ListItem Value="transdate">Confirm date</asp:ListItem>
                                            <asp:ListItem Value="sem">Semester</asp:ListItem>
                                            <asp:ListItem Value="transdate" Enabled="false">Total</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="~/images/right.jpeg"
                        ExpandedImage="~/images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                </center>
                <div style="padding-left: 249px; text-align: right;">
                    <table runat="server" id="tblBtns" visible="false" class="table">
                        <tr>
                            <td>
                                <asp:Button ID="btnrcptdupl" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Receipt Duplicate" Width="145px" Visible="false" OnClick="btnrcptdupl_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnChlnConfirm" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Confirm" OnClick="btnChlnConfirm_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnChlnCancel" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Width="120px" Text="Challan Cancel" OnClick="btnChlnCancel_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnChlnDelete" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Delete" OnClick="btnChlnDelete_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnChlnDuplicate" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Duplicate" OnClick="btnChlnDuplicate_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnchangeconfirm" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Change Confirm Date" Width="180px" OnClick="btnchangeconfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <center>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" OnUpdateCommand="Fpspread1_Command"
                        OnCellClick="Cell_Click1" OnPreRender="Fpspread_render" Height="450px" Width="950px"
                        Visible="false" CssClass="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <div id="rptprint" runat="server" visible="false">
                    <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                        Visible="false"></asp:Label><br />
                    <asp:Label ID="lbl_rptname" runat="server" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                    <asp:Button ID="btn_excel" runat="server" OnClick="btn_excel_Click" Text="Export To Excel"
                        Width="127px" CssClass="textbox btn2 textbox1" />
                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                        CssClass="textbox btn2 textbox1" Width="60px" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </div>
                <br />
            </div>
        </center>
        <%--Delete Confirmation Popup --%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Delete Selected Challans?"
                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_yes" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_no" CssClass=" textbox textbox1 btn1 " Style="height: 28px; width: 65px;"
                                                OnClick="btn_sureno_Click" Text="no" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- Pop Alert--%>
        <center>
            <div id="imgAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
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
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_alertclose_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- Changeconfirm Alert--%>
        <center>
            <div id="Div1" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblconfirmdate" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_confirmchange_yes" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_confirmchange_yes_Change" Text="Yes" runat="server" />
                                            <asp:Button ID="btn_confirmchange_no" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_confirmchange_no_Change" Text="No" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%-- New Print div--%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
    </body>
    </html>
</asp:Content>
