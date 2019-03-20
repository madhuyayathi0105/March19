<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="PaymentReconciliation.aspx.cs" Inherits="PaymentReconciliation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%@ register src="~/Usercontrols/PrintMaster.ascx" tagname="printmaster" tagprefix="Insproplus" %>
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <%@ register assembly="FarPoint.Web.Spread" namespace="FarPoint.Web.Spread" tagprefix="FarPoint" %>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .maindivstylesize
        {
            height: 900px;
            width: 1000px;
        }
        .lbl
        {
            text-align: center;
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
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
        }
        .tabeltd
        {
            background-color: #79BD9A;
            text-decoration: none;
            color: white;
        }
        .autocomplete_highlightedListItem
        {
            background-color: #EEEE89;
            color: black;
            padding: 1px;
            width: 241px;
        }
    </style>
    <body>
       <%-- <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
          <script type="text/javascript">


        function selectAll() {
            var count = 0;
            var chkselall = document.getElementById('<%=cbl_select.ClientID%>');
            var chksel = document.getElementById('<%=chkselall.ClientID%>');
            var tagname = chkselall.getElementsByTagName("input");
            if (chksel.checked == true) {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = true;
                }
            }
            else {
                for (var i = 0; i < tagname.length; i++) {
                    tagname[i].checked = false;
                }
            }
        }
        function chklall() {
            var count = 0;
            var chkselall = document.getElementById('<%=cbl_select.ClientID%>');
            var chksel = document.getElementById('<%=chkselall.ClientID%>');
            var tagname = chkselall.getElementsByTagName("input");
            for (var i = 0; i < tagname.length; i++) {
                if (tagname[i].checked == true) {
                    count += 1;
                }
            }
            if (tagname.length == count) {
                chksel.checked = true;
            }
            else {
                chksel.checked = false;
            }
        }
    </script>--%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Payment Reconciliation</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle" style="width: 1100px; height: 800px;">
                <br />
                <center>
                    <div>
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
                                <td align="left">
                                    <asp:Label ID="lbl_fromdate" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="Updp_frmdate" runat="server">
                                        <ContentTemplate>
                                            <div style="position: relative;">
                                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                                <asp:CalendarExtender ID="Cal_date" TargetControlID="txt_fromdate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_todate" Text="To Date" runat="server"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:UpdatePanel ID="Updp_todate" runat="server">
                                        <ContentTemplate>
                                            <div style="position: relative;">
                                                <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                                    CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%-- <td align="left">
                                <asp:Label ID="lbl_selectdate" Text="Select Date" runat="server"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div style="position: relative;">
                                            <asp:TextBox ID="txt_selectDate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_selectDate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>--%>
                                <td>
                                    <asp:Label ID="Label1" Text="Select Mode" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_cheque" runat="server" Text="Cheque" Checked="true" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_dd" runat="server" Text="DD" Checked="true" />
                                </td>
                                <td id="tdbk" runat="server" visible="false">
                                    Bank
                                </td>
                                <td id="tdcblbk" runat="server" visible="false">
                                    <asp:UpdatePanel ID="upbk" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbank" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="pnlbk" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cbbank" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbbank_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cblbank" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblbank_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txtbank"
                                                PopupControlID="pnlbk" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <%-- <td>
                                <asp:CheckBox ID="cb_cash" runat="server" Text="Cash" />
                            </td>--%>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <fieldset style="height: 15px; width: 353px;">
                                        <asp:RadioButton ID="rbstud" runat="server" Checked="true" Text="Student" AutoPostBack="true"
                                            OnCheckedChanged="rbstud_OnCheckedChanged" GroupName="grd" />
                                        <asp:RadioButton ID="rbstaff" runat="server" Text="Staff" AutoPostBack="true" OnCheckedChanged="rbstaff_OnCheckedChanged"
                                            GroupName="grd" />
                                        <asp:RadioButton ID="rbvendor" runat="server" Text="Vendor" AutoPostBack="true" OnCheckedChanged="rbvendor_OnCheckedChanged"
                                            GroupName="grd" />
                                        <asp:RadioButton ID="rnother" runat="server" Text="Others" AutoPostBack="true" OnCheckedChanged="rnother_OnCheckedChanged"
                                            GroupName="grd" />
                                    </fieldset>
                                </td>
                                <%-- <td>
                                <asp:CheckBox ID="chkselall" runat="server" Text="Select All" onchange="return selectAll()"
                                    OnCheckedChanged="chkselall_OnCheckedChanged" />
                            </td>
                            <td colspan="5" align="left">
                                <asp:CheckBoxList ID="cbl_select" runat="server" RepeatDirection="Horizontal" onclick="return chklall()">
                                    <asp:ListItem Value="0">ToBe Deposited</asp:ListItem>
                                    <asp:ListItem Value="1">Deposited</asp:ListItem>
                                    <asp:ListItem Value="2">Bounce</asp:ListItem>
                                    <asp:ListItem Value="3">Cleared</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>--%>
                                <td>
                                    <asp:RadioButton ID="rbentry" runat="server" Checked="true" Text="Entry" AutoPostBack="true"
                                        OnCheckedChanged="rbentry_OnCheckedChanged" GroupName="two" />
                                    <%--  <asp:RadioButton ID="rbreport" runat="server" Visible="false" Text="Report" AutoPostBack="true"
                                    OnCheckedChanged="rbreport_OnCheckedChanged" GroupName="two" />--%>
                                </td>
                                <td id="tdtype" runat="server" visible="false">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txttype" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                                height: 200px;">
                                                <asp:CheckBox ID="cbtype" runat="server" Width="100px" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cbtype_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="cbltype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbltype_OnSelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttype"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="5">
                                    <fieldset style="height: 15px; width: 353px;">
                                        <asp:RadioButton ID="rbtodeposit" runat="server" Checked="true" Text="To Be Cleared"
                                            AutoPostBack="true" OnCheckedChanged="rbtodeposit_OnCheckedChanged" GroupName="type" />
                                        <asp:RadioButton ID="rbdeposit" runat="server" Text="Deposited" AutoPostBack="true"
                                            OnCheckedChanged="rbdeposit_OnCheckedChanged" GroupName="type" />
                                        <asp:RadioButton ID="rbbounce" runat="server" Text="Bounce" AutoPostBack="true" OnCheckedChanged="rbbounce_OnCheckedChanged"
                                            GroupName="type" />
                                        <asp:RadioButton ID="rbclear" runat="server" Text="Cleared" AutoPostBack="true" OnCheckedChanged="rbclear_OnCheckedChanged"
                                            GroupName="type" />
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                        OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_rollno" runat="server" CssClass="txtheight3 txtcaps" AutoPostBack="true"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" TargetControlID="txt_rollno"
                                        FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" ValidChars=" .">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Width="80px" Text="Go"
                                        OnClick="btn_go_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divlbl" runat="server" visible="false" style="margin-left: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_dep" runat="server" Text="Deposit" Style="background: #e598ff;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_deptbounce" runat="server" Text=" Bounce" Style="background: #ffbf00;"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_deptcleared" runat="server" Text=" Cleared" Style="background: #bfff00;"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td id="fldtotal" runat="server">
                                    <fieldset id="fldtot" runat="server" visible="false" style="height: 20px; width: 150px;">
                                        <table align="left">
                                            <tr>
                                                <td colspan="2" id="trtobe" runat="server" visible="false">
                                                    <asp:Label ID="lbltodptamt" runat="server" Text="ToBeDeposit Amt" Visible="true"></asp:Label>
                                                    <asp:TextBox ID="txttobe" runat="server" Width="90px" Style="color: Green;" Font-Bold="true"
                                                        Height="20px" Enabled="false" Visible="true"></asp:TextBox>
                                                </td>
                                                <td colspan="2" id="trdept" runat="server" visible="false">
                                                    <asp:Label ID="lbldeptamt" runat="server" Text="Deposit Amt" Visible="true" Style="background: #e598ff;"></asp:Label>
                                                    <asp:TextBox ID="txtdept" runat="server" Style="color: Green;" Font-Bold="true" Width="90px"
                                                        Enabled="false" Visible="true"></asp:TextBox>
                                                </td>
                                                <td colspan="2" id="trboun" runat="server" visible="false">
                                                    <asp:Label ID="lblbounceamt" runat="server" Text="Bounce Amt" Visible="true" Style="background: #ffbf00;"></asp:Label>
                                                    <asp:TextBox ID="txtboun" runat="server" Style="color: Green;" Font-Bold="true" Width="90px"
                                                        Enabled="false" Visible="true"></asp:TextBox>
                                                </td>
                                                <td colspan="2" id="trclr" runat="server" visible="false">
                                                    <asp:Label ID="lblclramt" runat="server" Text="Clear Amt" Visible="true" Style="background: #bfff00;"></asp:Label>
                                                    <asp:TextBox ID="txtclr" runat="server" Style="color: Green;" Font-Bold="true" Width="90px"
                                                        Enabled="false" Visible="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                    <div id="divbtn" runat="server" visible="false">
                                        <fieldset style="left: 10px; top: 10px; width: 580px; height: 25px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_bankname" runat="server" Visible="false" Text="Bank">
                                                        </asp:Label>
                                                        <%-- </td>
                            <td align="left">--%>
                                                        <asp:DropDownList ID="ddl_bankname" runat="server" Visible="false" CssClass="textbox  ddlheight5"
                                                            Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlotherBank" runat="server" Visible="false" CssClass="textbox  ddlheight5"
                                                            Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" id="tdseldt" runat="server" visible="false">
                                                        <asp:Label ID="lbl_selectdate" Text="Date" runat="server"></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdseltxtdt" runat="server" visible="false">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <div style="position: relative;">
                                                                    <asp:TextBox ID="txt_selectDate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_selectDate" runat="server"
                                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                    </asp:CalendarExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btn_save" runat="server" Text="save" OnClick="btn_save_Onclick" CssClass="textbox textbox1"
                                                            Height="32px" Width="90px" />
                                                        <asp:Button ID="btn_bounce" runat="server" Text="Bounce" OnClick="btn_bounce_Onclick" CssClass="textbox textbox1"
                                                            Height="32px" Width="90px" />
                                                        <asp:Button ID="btn_clear" runat="server" Text="Clear" OnClick="btn_clear_Onclick" CssClass="textbox textbox1"
                                                            Height="32px" Width="90px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <%--  *********col order********--%>
                    <center>
                        <asp:Label ID="lblerrmsg" runat="server" Style="color: Red;"></asp:Label></center>
                    <div>
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="table2" Height="22px" Width="940px"
                                Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="table2" Width="975px">
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
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="false" Width="930px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="928px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="7" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <%-- <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="stud_name">Name</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="bankname">Bank Name</asp:ListItem>
                                        
                                        <asp:ListItem Selected="True" Value="paymode">Mode</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="transcode">Receipt No</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="transdate">Receipt Date</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="ddno">DD/Cheque No</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="dddate">DD/Cheque Date</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="depositedDate">Deposited Date</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="BouncedDate">Bounced Date</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="CollectedDate">Cleared Date</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="Amount">Amount</asp:ListItem>--%>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <center>
                        <asp:Label Style="color: Red;" ID="lblerr" Visible="false" Text="Record Not Found"
                            runat="server"></asp:Label>
                    </center>
                    <center>
                        <%-- <div id="div1" runat="server" visible="false" style="width: 850px; height: 500px;
                        overflow: auto; border: 1px solid Gray; background-color: White;">--%>
                        <br />
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="NotSet"
                            BorderWidth="0px" ActiveSheetViewIndex="0" OnButtonCommand="FpSpread1_OnButtonCommand" OnCellClick="FpSpread1_OnCellClick"
                            OnUpdateCommand="FpSpread1_OnUpdateCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <%-- </div>--%>
                    </center>
                    <br />
                    <%--*********col order*******--%>
                    <br />
                    <div id="print" runat="server" visible="false">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" CssClass="textbox textbox1" Text="Export To Excel"
                            Width="127px" Height="35px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </center>
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
        <center>
            <div id="Div2" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20%;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalertmsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnalert" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnalert_Click" Text="ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divsave" runat="server" visible="false" style="height: 70em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div4" runat="server" class="table" style="background-color: White; height: 227px;
                        width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20%;
                        border-radius: 10px;">
                        <br />
                        <br />
                        <center>
                            <table id="tbltot" runat="server" visible="false">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbldtxt" runat="server" Text="Deposit Amount:" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lbldtxtamt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblbkname" runat="server" Text="Bank Name:" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblbkvalue" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table align="center" style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblsave" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="btnsavealert" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btnsavealert_Click" Text="ok" runat="server" />
                                            <%--  </center>
                                </td>
                                <td>
                                    <center>--%>
                                            <asp:Button ID="btncan" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btncan_Click" Text="Cancel" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divbounce" runat="server" visible="false" style="height: 75em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div6" runat="server" class="table" style="background-color: White; height: 150px;
                        width: 474px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20%;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Label ID="Label3" runat="server" Text="Bounce Amount:" Style="color: Green;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Style="color: Blue;"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RadioButton ID="rbcancel" runat="server" Text="Do You Want Clear The Receipt"
                                            GroupName="can" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="center">
                                        <asp:RadioButton ID="rbredept" runat="server" Text="Do You Want Re-Deposit The Receipt"
                                            GroupName="can" />
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label2" runat="server" Text="Do You Want Continue This Process" Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnsavebounce" CssClass=" textbox btn1 comm" Visible="false" Style="height: 28px;
                                                width: 65px;" OnClick="btnsavebounce_Click" Text="ok" runat="server" />
                                            <asp:Button ID="btnsavebn" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnsavebn_Click" Text="ok" Visible="false" runat="server" />
                                            <asp:Button ID="btncancelbounce" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btncancelbounce_Click" Text="Cancel" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <center>
            <div id="divdel" runat="server" visible="false">
                <asp:Button ID="btndelg" runat="server" Text="Delegate" OnClick="btndelg_Click" />
            </div>
            <asp:Table ID="tbl" runat="server" Visible="false">
            </asp:Table>
        </center>
    </body>
    </html>
</asp:Content>
