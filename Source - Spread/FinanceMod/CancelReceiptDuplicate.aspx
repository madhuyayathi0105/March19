<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CancelReceiptDuplicate.aspx.cs" Inherits="CancelReceiptDuplicate"
    ClientIDMode="Static" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Duplicate Receipt</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .neu
        {
            visibility: hidden;
        }
    </style>
    <body>
        <script type="text/javascript" language="javascript">

            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=980');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style>body, html {margin:0;padding:0;height:100%;} .classRegular { font-family:Arial; font-size:10px; } .classBold10 { font-family:Arial; font-size:12px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:14px; font-weight:bold;} .classBold { font-family:Arial; font-size:10px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
            function CheckCancelReason() {
                var txt = document.getElementById("<%=txtCancelReason.ClientID %>").value.trim();
                if (txt == "" || txt == "0") {
                    alert('Please Enter Cancel Reason');
                    return false;
                }
                else {
                    return true;
                }
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Receipt Cancel and Duplicate
                    </span>
                </div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 970px; height: 590px;">
                <br />
                <center>
                    <div>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Memtype" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rbl_Memtype" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rbl_Memtype_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="1">Student</asp:ListItem>
                                        <asp:ListItem Value="2">Staff</asp:ListItem>
                                        <asp:ListItem Value="3">Vendor</asp:ListItem>
                                        <asp:ListItem Value="4">Others</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblfyear" Text="FinanceYear" Width="85px" Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlfinyear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlfinyear_Selected"
                                        CssClass="textbox textbox1 ddlheight2" Style="width: 130px;">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rbl_receiptOrVoucher" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true">
                                        <asp:ListItem Selected="True" Value="1">Receipt</asp:ListItem>
                                        <asp:ListItem Value="2">Voucher</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="cbl_smsormail" Visible="false" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem>SMS</asp:ListItem>
                                        <asp:ListItem>Email</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
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
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_batch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel  multxtpanleheight"
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
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UP_degree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
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
                                    <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Up_dept" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel multxtpanleheight"
                                                Width="150px">
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
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_header" runat="server" Width="72px" Text="Header"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_header" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_header" runat="server" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
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
                                <td>
                                    <asp:Label ID="lblappNo" runat="server" Visible="false" Text="AppForm No"></asp:Label>
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight2" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="True">Roll No</asp:ListItem>
                                        <asp:ListItem>Reg No</asp:ListItem>
                                        <asp:ListItem>Admin No</asp:ListItem>
                                        <asp:ListItem>App No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_regno" runat="server" CssClass="textbox  txtheight2">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_regno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="LblRecOrVoucher" runat="server" Width="80px" Text="Receipt Acr"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_chaln" runat="server" CssClass="textbox  txtheight2">
                                    </asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="LblRecOrVouNo" runat="server" Width="80px" Text="Receipt No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_chno" runat="server" CssClass="textbox  txtheight2">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderpa" runat="server" TargetControlID="txt_chno"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_name" runat="server" CssClass="textbox  txtheight5">
                                    </asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                        ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td>
                                                From
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox  txtheight" Width="70px"
                                                    OnTextChanged="checkDate" AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_fromdate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                To
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox  txtheight" OnTextChanged="checkDate"
                                                    Width="70px" AutoPostBack="true"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_todate" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <span>Cancel-Date</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_date" runat="server" CssClass="textbox  txtheight" Width="70px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBefAfteAdm" runat="server" CssClass="textbox  ddlheight3"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlBefAfteAdm_Indexchange">
                                                    <asp:ListItem Selected="True">After Admission</asp:ListItem>
                                                    <asp:ListItem>Before Admission</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_go" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                                    OnClick="btn_go_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <br />
                <center>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" OnUpdateCommand="Fpspread1_Command"
                        OnCellClick="Cell_Click1" OnPreRender="Fpspread_render" Height="300px" Width="900px"
                        Visible="false" CssClass="spreadborder" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <div>
                    <table runat="server" id="tblBtns" visible="false">
                        <tr>
                            <td>
                                <center>
                                    <asp:TextBox ID="txtCancelReason" runat="server" CssClass="textbox textbox2" placeholder="Cancel Reason"
                                        MaxLength="480" Width="500px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteTxtCancel" runat="server" TargetControlID="txtCancelReason"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" ./">
                                    </asp:FilteredTextBoxExtender>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnChlnCancel" runat="server" BackColor="#8199FD" CssClass="textbox textbox1 btn2"
                                    Text="Receipt Cancel" Width="120px" OnClientClick="return CheckCancelReason();"
                                    OnClick="btnChlnCancel_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnChlnDelete" BackColor="#8199FD" runat="server" CssClass="textbox textbox1 btn2"
                                    Text="Delete" OnClick="btnChlnDelete_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnChlnDuplicate" BackColor="#8199FD" runat="server" CssClass="textbox textbox1 btn2"
                                    Text="Duplicate" OnClick="btnChlnDuplicate_Click" />
                                &nbsp;&nbsp;
                                <asp:Button ID="btnChlnOriginal" BackColor="#8199FD" runat="server" CssClass="textbox textbox1 btn2"
                                    Text="Original" OnClick="btnChlnDuplicate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr>
                            <td>
                                <center>
                                    <div id="print" runat="server" visible="false">
                                        <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            ForeColor="Red" Text="" Visible="true"></asp:Label>
                                        <asp:Label ID="lblrptname" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                        <asp:TextBox ID="txtexcelname" runat="server" Visible="true" Width="180px" onkeypress="display()"
                                            CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnExcel" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnExcel_Click" Text="Export To Excel" Width="127px"
                                            Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btnprintmasterhed" runat="server" Visible="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" Height="32px"
                                            Style="margin-top: 10px;" CssClass="textbox textbox1" Width="60px" />
                                        <Insproplus:printmaster runat="server" ID="Printcontrolhed" Visible="false" />
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </center>
        <%--Cancel Confirmation Popup --%>
        <center>
            <div id="surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_sure" runat="server" Text="Do You Want To Cancel Selected Receipt?"
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
        <%--Delete Confirmation Popup --%>
        <center>
            <div id="suredivDelete" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                        <asp:Label ID="lbl_sureDel" runat="server" Text="Do You Want To Delete Selected Receipt?"
                                            Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_sureyesDel" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_sureyesDel_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btn_surenoDel" CssClass=" textbox textbox1 btn1 " Style="height: 28px;
                                                width: 65px;" OnClick="btn_surenoDel_Click" Text="no" runat="server" />
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
            <div id="imgAlert" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
        <%-- New Print div--%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
    </body>
    </html>
</asp:Content>
