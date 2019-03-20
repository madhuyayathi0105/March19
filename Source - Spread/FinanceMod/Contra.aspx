<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Contra.aspx.cs" Inherits="Contra" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="../Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
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
                document.getElementById('<%=lblvalidation1.ClientID%>').innerHTML = "";
            }
            function check() {
                var id = "";
                var empty = "";

                var id_acn = "";

                var id_acnm = "";

                var id_bnk = "";
                var id_respns = "";

                id = document.getElementById("<%=Txt_accno.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=Txt_accno.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%= Txt_accname .ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%= Txt_accname .ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%= Txt_bankname.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%= Txt_bankname.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                id = document.getElementById("<%=Txt_depositAmount.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=Txt_depositAmount.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }

                //////            id = document.getElementById("<%=Txt_respnspersornbank.ClientID %>").value;
                //////            if (id.trim() == "") {
                //////                id = document.getElementById("<%=Txt_respnspersornbank.ClientID %>");
                //////                id.style.borderColor = 'Red';
                //////                empty = "E";
                //////            }
                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }

            }

            function checkadd2() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=txt_narrotion.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_narrotion.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }
            }
            function checkpetty() {
                var id = "";
                var empty = "";
                id = document.getElementById("<%=Txtamount.ClientID %>").value;

                if (id.trim() == "") {
                    id = document.getElementById("<%=Txtamount.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=Txt_respnspersorns.ClientID %>").value;
                if (id.trim() == "") {
                    id = document.getElementById("<%=Txt_respnspersorns.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty != "") {
                    return false;
                }
                else {

                    return true;
                }
            }

            function display(x) {
                x.style.borderColor = "#c4c4c4";

            }

            function addmarkss() {
                var finalvalue = 0;
                var txtid;
                var chkidPetty = document.getElementById("<%=rb_petty.ClientID %>");
                if (chkidPetty.checked) {
                    txtid = document.getElementById("<%=Txtamount.ClientID %>").value;
                } else {
                    txtid = document.getElementById("<%=Txt_depositAmount.ClientID %>").value;
                }
                if (txtid.trim() != "") {
                    var varn = document.getElementById("<%=gridView1.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtdeposit") > 1) {
                            if (gridViewControls[i].value != "") {
                                finalvalue += parseFloat(gridViewControls[i].value);
                                if (chkidPetty.checked) {
                                    document.getElementById("<%=Txtamount.ClientID %>").value = finalvalue.toString();
                                } else {
                                    document.getElementById("<%=Txt_depositAmount.ClientID %>").value = finalvalue.toString();
                                }
                            }
                        }
                    }
                }
            }


            function addmarkss1() {
                var finalvalue = 0;
                var txtid = document.getElementById("<%=Txt_depositAmount.ClientID %>").value;
                var txtid1 = document.getElementById("<%=Txtamount.ClientID %>").value;
                if (txtid.trim() != "") {
                    var varn = document.getElementById("<%=gridView1.ClientID %>");
                    var gridViewControls = varn.getElementsByTagName("input");
                    var len = varn.rows.length;
                    for (var i = 0; i < gridViewControls.length; i++) {
                        if (gridViewControls[i].name.indexOf("txtdeposit") > 1) {
                            if (gridViewControls[i].value != "") {
                                finalvalue += parseFloat(gridViewControls[i].value);
                                document.getElementById("<%=Txt_depositAmount.ClientID %>").value = finalvalue.toString();
                                document.getElementById("<%=Txtamount.ClientID %>").value = finalvalue.toString();
                            }
                        }
                    }
                }
            }
            function Wthdrwpety() {
                var check = false;
                var payamt = "";
                var empty = false;
                var avalbal = document.getElementById('<%=txtaval.ClientID%>').value;
                var staff = document.getElementById('<%=txtwith.ClientID%>').value;
                payamt = document.getElementById('<%=txtamt.ClientID%>').value;
                if (parseFloat(avalbal) != "" && parseFloat(payamt) != "") {
                    if (parseFloat(avalbal) != "" && parseFloat(payamt) != "") {
                        if (parseFloat(payamt) > parseFloat(avalbal)) {
                            check = true;
                        }
                    }
                    else {
                        check = true;
                    }
                }
                if (staff.trim() == "") {
                    staff = document.getElementById("<%=txtwith.ClientID %>");
                    staff.style.borderColor = 'Red';
                    empty = true;
                }
                if (payamt.trim() == "") {
                    payamt = document.getElementById('<%=txtamt.ClientID%>');
                    payamt.style.borderColor = 'Red';
                    empty = true;
                }
                if (check == true) {
                    payamt = document.getElementById('<%=txtamt.ClientID%>');
                    payamt.style.borderColor = 'Red';
                    payamt.value = "";
                    return false;
                }
                if (empty == true) {
                    return false;
                }
            }


            function Wthdrwbank() {
                var check = false;
                var empty = false;
                var payamt = "";
                var avalbal = document.getElementById('<%=txtbankaval.ClientID%>').value;
                payamt = document.getElementById('<%=txtbankamt.ClientID%>').value;
                var staff = document.getElementById('<%=txtbankwith.ClientID%>').value;
                var cheqno = document.getElementById('<%=txtche.ClientID%>').value;
                if (parseFloat(avalbal) != "" && parseFloat(payamt) != "") {
                    if (parseFloat(payamt) > parseFloat(avalbal)) {
                        check = true;
                    }
                }
                else {
                    check = true;
                }
                if (staff.trim() == "") {
                    staff = document.getElementById("<%=txtbankwith.ClientID %>");
                    staff.style.borderColor = 'Red';
                    empty = true;
                }
                if (cheqno.trim() == "") {
                    cheqno = document.getElementById("<%=txtche.ClientID %>");
                    cheqno.style.borderColor = 'Red';
                    empty = true;
                }
                if (payamt.trim() == "") {
                    payamt = document.getElementById('<%=txtbankamt.ClientID%>');
                    payamt.style.borderColor = 'Red';
                    empty = true;
                }
                if (check == true) {
                    payamt = document.getElementById('<%=txtbankamt.ClientID%>');
                    payamt.style.borderColor = 'Red';
                    payamt.value = "";
                    return false;
                }
                if (empty == true) {
                    return false;
                }
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
                var spfromdate = fromDate.split("/");

                date = parseInt(spfromdate[0].toString());
                month = parseInt(spfromdate[1].toString());
                year = parseInt(spfromdate[2].toString());

                var totodate = toDate.split("/");
                date1 = parseInt(totodate[0].toString());
                month1 = parseInt(totodate[1].toString());
                year1 = parseInt(totodate[2].toString());

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
                    document.getElementById('<%=txt_fromdate.ClientID%>').value = currentDate;
                    document.getElementById('<%=Txt_Todate.ClientID %>').value = currentDate;
                    return false;
                }
            }


            function amountCheck() {
                var empty = "";
                var check = false;
                var amt = false;
                var pety = document.getElementById('<%=rb_petty.ClientID%>');
                var bank = document.getElementById('<%=rb_bank.ClientID%>');
                var totalamt = document.getElementById('<%=lblamount.ClientID%>').innerHTML;
                if (pety.checked == true) {
                    var txtid1 = document.getElementById('<%=Txtamount.ClientID%>');
                    if (txtid1.value != "") {
                        if (parseFloat(totalamt) < parseFloat(txtid1.value)) {
                            txtid1.value = "";
                            alert("deposite amount should be lesser than or equal to total amount");
                            return false;
                        }
                    }
                    else {
                        alert("Please Enter the amount");
                        return false;
                    }
                }
                else if (bank.checked == true) {
                    var txtid2 = document.getElementById('<%=Txt_depositAmount.ClientID%>');
                    if (txtid2.value != "") {
                        if (parseFloat(totalamt) < parseFloat(txtid2.value)) {
                            txtid2.value = "";
                            alert("deposite amount should be lesser than or equal to total amount");
                            return false;
                        }
                    }
                    else {
                        alert("Please Enter the amount");
                        return false;
                    }
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
                                <span class="fontstyleheader" style="color: Green;">Contra</span></div>
                        </center>
                    </div>
                </center>
                <div class="style" style="height: 550px;">
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <div>
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset style="float: left; height: 15px; width: 158px; border-radius: 10px;">
                                                        <asp:RadioButton ID="rb_dept" runat="server" Text="Deposit" AutoPostBack="true" OnCheckedChanged="rb_dept_OnCheckedChanged"
                                                            GroupName="d1" />
                                                        <asp:RadioButton ID="rb_with" runat="server" Text="Withdraw" AutoPostBack="true"
                                                            OnCheckedChanged="rb_with_OnCheckedChanged" GroupName="d1" />
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="float: left; height: 20px; width: 273px; border-radius: 10px;">
                                                        <%-- <asp:RadioButton ID="rbcuml" runat="server" Checked="true" Text="Cumulative" AutoPostBack="true"
                                                        OnCheckedChanged="rbcuml_OnCheckedChanged" GroupName="dd" />
                                                    <asp:RadioButton ID="rbdetail" runat="server" Text="Detail" AutoPostBack="true" OnCheckedChanged="rbdetail_OnCheckedChanged"
                                                        GroupName="dd" />--%>
                                                        <%--  <asp:RadioButton ID="rbcuml" runat="server" Text="Cumulative" Checked="true" AutoPostBack="true"
                                                        OnCheckedChanged="rbcuml_OnCheckedChanged" GroupName="t1" />
                                                    <asp:RadioButton ID="rbdetail" runat="server" Text="Detail" AutoPostBack="true" OnCheckedChanged="rbdetail_OnCheckedChanged"
                                                        GroupName="t1" />--%>
                                                        <span>Type</span>
                                                        <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddltype_indexChanged"
                                                            CssClass="textbox ddlstyle ddlheight3" Width="114px">
                                                            <%-- <asp:ListItem Text="Cumulative" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Detail" Value="2"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="cbinclude" runat="server" Visible="false" Text="Deposit" />
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <fieldset style="height: 15px; width: 126px; float: left; border-radius: 10px;">
                                                        <asp:RadioButton ID="rb_entry" runat="server" AutoPostBack="true" OnCheckedChanged="rb_entry_OnCheckedChanged"
                                                            GroupName="r1" Text="Entry" />
                                                        <asp:RadioButton ID="rb_report" runat="server" AutoPostBack="true" OnCheckedChanged="rb_report_OnCheckedChanged"
                                                            GroupName="r1" Text="Report" />
                                                    </fieldset>
                                                </td>
                                                <td>
                                                    <div id="divwith" runat="server" visible="false">
                                                        <fieldset style="height: 15px; width: 105px; border-radius: 10px;">
                                                            <asp:RadioButton ID="rb_pety" runat="server" AutoPostBack="true" Visible="false"
                                                                OnCheckedChanged="rb_pety_OnCheckedChanged" GroupName="w1" Text="Pety" />
                                                            <asp:RadioButton ID="rb_banks" runat="server" AutoPostBack="true" Visible="false"
                                                                OnCheckedChanged="rb_banks_OnCheckedChanged" GroupName="w1" Text="Bank" />
                                                        </fieldset>
                                                        <asp:Label ID="lbloutput" runat="server" Visible="true" Text=""></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <table class="table2">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                                    <asp:Label ID="lbldept" runat="server" Visible="false" Text="Department"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlclg_indexChanged"
                                                        CssClass="textbox ddlstyle ddlheight3" Width="114px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblfromdate" runat="server" Text="From Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_fromdate" TextMode="SingleLine" runat="server" Height="20px"
                                                        OnTextChanged="txtfrom_TextChanged" CssClass="textbox textbox1" Width="65px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Style="font-size: large;" Text="To Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_Todate" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                                        Width="65px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="Txt_Todate" runat="server"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Search" runat="server" CssClass="textbox btn2" Text="Search" OnClientClick="return checkDate()"
                                                        Width=" 61px" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <div>
                                <asp:Label ID="lblerror" runat="server" ForeColor="Red" Visible="false">
                                </asp:Label>
                            </div>
                        </center>
                    </center>
                    <br />
                    <center>
                        <center>
                            <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <div id="divfp" runat="server" visible="false" style="width: 950px; height: auto;
                                overflow: auto; background-color: White; border-radius: 10px;">
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" OnCellClick="FpSpread1_CellClick"
                                    OnPreRender="FpSpread1_SelectedIndexChanged" OnButtonCommand="FpSpread1_Command"
                                    BorderStyle="Solid" BorderWidth="0px" Width="930px" Style="overflow: auto; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    class="spreadborder">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </center>
                        <br />
                        <div id="divpety" runat="server" visible="false" style="width: 664px; height: 250px;
                            border-radius: 10px;">
                            <fieldset style="border-radius: 10px;">
                                <br />
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblhead" runat="server" Text="Header"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlhead" runat="server" Height="35px" Width="200px" OnSelectedIndexChanged="ddlhead_SelectedIndexChanged"
                                                    AutoPostBack="true" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblledg" runat="server" Text="Ledger"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlledg" runat="server" Height="35px" Width="200px" OnSelectedIndexChanged="ddlledg_SelectedIndexChanged"
                                                    AutoPostBack="true" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblaval" runat="server" Text=" Available Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtaval" runat="server" CssClass="textbox textbox1" Height="20px"
                                                    Width="187px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" TargetControlID="txtaval"
                                                    FilterType="Numbers,custom" ValidChars=". ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblamt" runat="server" Text=" Amount"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtamt" runat="server" Height="20px" onfocus="return display(this)"
                                                    CssClass="textbox textbox1" Width="187px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtamt"
                                                    FilterType="Numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <asp:Label ID="lblavaler" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbldate" runat="server" Text="Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_date" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                                    Width="187px"></asp:TextBox>
                                                <asp:CalendarExtender ID="calex" runat="server" TargetControlID="txt_date" Format="dd/MM/yyyy"
                                                    CssClass="cal_Theme1 ajax__calendar_active">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_with" runat="server" Text="Withdraw By"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtwith" runat="server" Height="20px" CssClass="textbox textbox1"
                                                    AutoPostBack="true" Width="152px" onfocus="return display(this)"></asp:TextBox>
                                                <%--                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtwith"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars="- ">
                                                </asp:FilteredTextBoxExtender>--%>
                                                <asp:Button ID="btn_with" runat="server" Text="?" OnClick="btn_with_Click" CssClass="textbox textbox1"
                                                    Height="32px" Width="34px" />
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                    Enabled="true" ServiceMethod="GetWithdraw" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtwith"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Label ID="lblnarr" runat="server" Text="Narrotion"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtnarr" runat="server" Height="20px" CssClass="textbox textbox1"
                                                    Width="481px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtnarr"
                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=". ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnsavewith" Visible="true" OnClientClick="return Wthdrwpety()" OnClick="btnsavewith_Click"
                                        runat="server" Text="Save" Font-Size="Small" Font-Bold="true" CssClass="textbox textbox1"
                                        Height="32px" Width="60px" />
                                </center>
                            </fieldset>
                            <br />
                        </div>
                        <div id="divbank" runat="server" visible="false" style="width: 686px; height: 250px;
                            border-radius: 10px;">
                            <br />
                            <fieldset style="border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbank" runat="server" Text="Select Bank"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbank" runat="server" Height="35px" Width="200px" OnSelectedIndexChanged="ddlbank_SelectedIndexChanged"
                                                AutoPostBack="true" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Voucher No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtvochno" runat="server" Enabled="false" CssClass="txtheight1"
                                                Width="80px"> </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbankaval" runat="server" Text=" Available Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankaval" runat="server" CssClass="textbox textbox1" Height="20px"
                                                Width="187px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" TargetControlID="txtbankaval"
                                                FilterType="Numbers,custom" ValidChars=". ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbankamt" runat="server" Text=" Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankamt" runat="server" onfocus="return display(this)" Height="20px"
                                                CssClass="textbox textbox1" Width="187px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtbankamt"
                                                FilterType="Numbers,custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblbankaver" runat="server" Style="color: Red;" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblchedate" runat="server" Text="Cheque Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtchedate" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="187px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtchedate"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblche" runat="server" Text=" Cheque No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtche" runat="server" Height="20px" CssClass="textbox textbox1"
                                                onfocus="return display(this)" Width="187px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" TargetControlID="txtche"
                                                FilterType="Numbers,custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbankamtdate" runat="server" Text="Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankamtdate" TextMode="SingleLine" runat="server" Height="20px"
                                                CssClass="textbox textbox1" Width="187px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtbankamtdate"
                                                Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                            </asp:CalendarExtender>
                                        </td>
                                        <%-- <td>
                                        <asp:Label ID="lblchedate" runat="server" Text="Cheque Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtchedate" TextMode="SingleLine" runat="server" Height="20px" CssClass="textbox textbox1"
                                            Width="187px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtchedate"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>--%>
                                        <td>
                                            <asp:Label ID="lblbankwith" runat="server" Text="Withdraw By"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbankwith" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="152px" onfocus="return display(this)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" TargetControlID="txtbankwith"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=".- ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btnbankwith" runat="server" Text="?" OnClick="btnbankwith_Click"
                                                CssClass="textbox textbox1" Height="32px" Width="34px" />
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                Enabled="true" ServiceMethod="Getwithbank" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtbankwith"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">
                                            <asp:Label ID="lblbanknarr" runat="server" Text="Narration"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtbanknarr" runat="server" Height="20px" CssClass="textbox textbox1"
                                                Width="512px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtbanknarr"
                                                FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=". ">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Button ID="btn_save_bank" Visible="true" OnClientClick="return Wthdrwbank()"
                                    OnClick="btn_save_bank_Click" runat="server" Text="Save" Font-Bold="true" Font-Size="small"
                                    CssClass="textbox textbox1" Height="32px" Width="60px" />
                            </fieldset>
                            <br />
                        </div>
                        <%-- </div>--%>
                        <br />
                        <asp:Button ID="btnDeposit" Visible="false" Font-Bold="true" Font-Size="small" runat="server"
                            Text="Deposit" OnClick="btnDeposit_Click" CssClass="textbox textbox1  btn" />
                    </center>
                    <center>
                        <div id="print" runat="server" visible="false">
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
                    <div id="popwindow" runat="server" visible="false" style="height: 100em; z-index: 1000;
                        width: 100%; background-color: rgba(54,25,25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 100px; margin-left: 399px;"
                            OnClick="imagebtnpopclose_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div id="Deposit" visible="true" runat="server" style="background-color: White; z-index: 50em;
                            height: 480px; width: 841px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px;">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Depositedto" runat="server" Style="font-size: large; color: Green;"
                                                Text="Deposited To"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rb_petty" runat="server" GroupName="same" AutoPostBack="true"
                                                OnCheckedChanged="rb_petty_CheckedChanged" Text="Petty" Checked="true"></asp:RadioButton>
                                            <asp:RadioButton ID="rb_bank" runat="server" GroupName="same" AutoPostBack="true"
                                                OnCheckedChanged="rb_bank_CheckedChanged" Text="Bank"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <div id="Grid" runat="server" style="float: right; width: 50%; height: 250px; overflow: auto;">
                                    <asp:Label ID="lbltext" runat="server" Text="Total Deposite Amount:"></asp:Label>
                                    <asp:Label ID="lblamount" runat="server"></asp:Label>
                                    <asp:GridView ID="gridView1" runat="server" GridLines="Both" AutoGenerateColumns="false"
                                        OnDataBound="bankgrid_pg_DataBound" Width="100px" Style="overflow: auto;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="300px">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_S_no" runat="server" Width="60px" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="300px">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_date" runat="server" Width="80px" Text='<%#Eval("Date") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Deposit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="300px">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:TextBox ID="txtdeposit" runat="server" ReadOnly="true" CssClass="  textbox txtheight1"
                                                            Height="17px" Width="150px" Text='<%#Eval("Deposit") %>'></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdeposit"
                                                            FilterType="Numbers,Custom" ValidChars=" . ">
                                                            <%--onblur="return addmarkss(this)"--%>
                                                        </asp:FilteredTextBoxExtender>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </div>
                                <div id="Petty" runat="server" style="float: left; width: 50%;">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblHeader" runat="server" Text="Header"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_header" runat="server" Height="35px" OnSelectedIndexChanged="ddl_header_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLedger" runat="server" Text="Ledger"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlledger" runat="server" Height="35px" OnSelectedIndexChanged="ddl_Ledger_SelectedIndexChanged"
                                                        AutoPostBack="true" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_dpamount" runat="server" Text="Deposit Amount"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txtamount" runat="server" Height="20px" CssClass="textbox textbox1"
                                                        Width="73px" onblur="return amountCheck()" onfocus="return display(this)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="Txtamount"
                                                        FilterType="Numbers,Custom" ValidChars=". ">
                                                        <%--onblur="return addmarkss(this)"--%>
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_narrotion" runat="server" Text="Narration"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Height="33px"
                                                        Width="35px" />
                                                    <asp:DropDownList ID="ddl_narrotion" runat="server" Height="35px" CssClass="textbox textbox1 ddlstyle ddlheight4">
                                                    </asp:DropDownList>
                                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                        Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                        OnClick="btnminus_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblrespons" runat="server" Text=" Response Person"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_respnspersorns" TextMode="SingleLine" runat="server" Height="20px"
                                                        OnTextChanged="Txt_respnspersorns_OnTextChanged" AutoPostBack="true" CssClass="textbox textbox1"
                                                        Width="190px" onfocus=" return display(this)"></asp:TextBox>
                                                    <asp:Button ID="btn_responspersorn" runat="server" Text="?" Font-Bold="true" Font-Size="Medium"
                                                        Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                        OnClick="btnresponspersorn_Click" />
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtenderres" runat="server" DelimiterCharacters=""
                                                        Enabled="true" ServiceMethod="Getresponsename" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_respnspersorns"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_deptname" runat="server" Text="Department Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_deptname" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_deptname"
                                                        FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_desig" runat="server" Text="Designation Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_desig" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_desig"
                                                        FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_cat" runat="server" Text="Catagory Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_cat" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                        <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_cat"
                                                        FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblptydt" runat="server" Text="Deposit Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtptydt" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtptydt"
                                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="btn_savepetty" runat="server" Visible="true" OnClick="btnsavepetty_Click"
                                            Text="Save" Height="32px" Width="70px" CssClass="textbox textbox1" />
                                    </center>
                                    <br />
                                </div>
                                <div id="Bank" visible="false" runat="server" style="float: left; width: 50%;">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_AccNo" runat="server" Text="Account No"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_accno" onfocus=" return display(this)" TextMode="SingleLine"
                                                        OnTextChanged="Txt_accno_OnTextChanged" AutoPostBack="true" runat="server" Height="20px"
                                                        CssClass="textbox textbox1" Width="120px"></asp:TextBox>
                                                    <span style="color: Red;">*</span>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Accountno" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_accno"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_AccName" runat="server" Text="Account Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_accname" onfocus=" return display(this)" TextMode="SingleLine"
                                                        AutoPostBack="true" ReadOnly="true" runat="server" OnTextChanged="txt_AccName_textChanged"
                                                        Height="20px" Width="205px" CssClass="textbox textbox1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbankname" runat="server" Text="Bank Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_bankname" OnTextChanged="Txtbankname_TextChanged" TextMode="SingleLine"
                                                        runat="server" Height="20px" ReadOnly="true" CssClass="textbox textbox1" Width="205px"
                                                        AutoPostBack="true" onfocus=" return display(this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_branch" runat="server" Text="Branch Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_branch" TextMode="SingleLine" runat="server" Height="20px" ReadOnly="true"
                                                        CssClass="textbox textbox1" Width="205px" AutoPostBack="true" onfocus=" return display(this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_depositammount" runat="server" Text="Deposit Amount"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_depositAmount" TextMode="SingleLine" runat="server" Height="20px"
                                                        CssClass="textbox textbox1" onblur="return amountCheck()" onfocus=" return display(this)"
                                                        Width="70px"></asp:TextBox>
                                                    <%--onblur="return addmarkss(this)"--%>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="Txt_depositAmount"
                                                        FilterType="Numbers,custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_depositeby" runat="server" Text="Deposit BY"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Txt_respnspersornbank" TextMode="SingleLine" runat="server" Height="20px"
                                                        OnTextChanged="Txt_respnspersornbank_OnTextChanged" AutoPostBack="true" CssClass="textbox textbox1"
                                                        Width="120px" onfocus=" return display(this)"></asp:TextBox>
                                                    <asp:Button ID="btn_responspersornbank" runat="server" Text="?" Font-Bold="true"
                                                        Font-Size="Medium" Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                                        OnClick="btnresponspersornbank_Click" />
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getdepositename" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Txt_respnspersornbank"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Department Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_dptbank" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_dptbank"
                                                    FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Designation Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_dptdesg" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                    <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_dptdesg"
                                                    FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text="Catagory Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_dptcat" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                    <%--   <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_dptcat"
                                                    FilterType="LowercaseLetters,UppercaseLetters" ValidChars=" ">
                                                </asp:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblbkdptdt" runat="server" Text="Deposit Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtbkdt" runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtbkdt"
                                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                    </asp:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Button ID="btn_savebank" Visible="true" OnClientClick="return check()" OnClick="btnsavebank_Click"
                                            runat="server" Text="Save" Height="32px" Width="70px" CssClass="textbox textbox1" />
                                        <%--OnClientClick="return check()"--%>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </div>
                    <div id="popupsscode1" runat="server" visible="false" class="popupstyle popupheight">
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 436px;"
                            OnClick="imagebtnpopclose2_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 650px; width: 900px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <asp:Label ID="lbl_selectstaffcode" runat="server" Style="font-size: large; color: Green;"
                                    Text="Select the Staff Name"></asp:Label>
                            </center>
                            <br />
                            <div>
                                <center>
                                    <table class="maintablestyle" style="width: 66%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_college2" runat="server" Text="College"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_college2" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                                    CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_department3" runat="server" Text="Department"></asp:Label>
                                                <asp:DropDownList ID="ddl_department3" Width="180px" Height="30px" runat="server"
                                                    AutoPostBack="true" CssClass="textbox textbox1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_searchby" runat="server" Text="Search By"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_searchby" Width="250px" Height="30px" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddl_searchby_SelectedIndexChanged" CssClass="textbox textbox1">
                                                    <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                    <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_searchby" Visible="true" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_searchby"
                                                    FilterType="uppercaseletters,lowercaseletters,custom" ValidChars=" .">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:TextBox ID="txt_wardencode" Visible="false" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="textbox textbox1" Width="180px"></asp:TextBox>
                                                <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_wardencode"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                                <asp:Button ID="btn_go2" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go2_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <p>
                                            <asp:Label ID="lbl_errorsearch" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                        </p>
                                        <p>
                                            <asp:Label ID="lbl_errorsearch1" runat="server" Visible="false" Font-Bold="true"
                                                ForeColor="Red"></asp:Label>
                                        </p>
                                        <FarPoint:FpSpread ID="Fpstaff" runat="server" Visible="false" Width="700px" Style="overflow: auto;
                                            height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                            box-shadow: 0px 0px 8px #999999;">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0099CC">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                    <br />
                                    <center>
                                        <div>
                                            <asp:Button ID="btn_save1" runat="server" CssClass="textbox textbox1" Text="Save"
                                                OnClick="btn_save1_Click" />
                                            <asp:Button ID="btn_exit2" runat="server" CssClass="textbox textbox1" Text="Exit"
                                                OnClick="btn_exit2_Click" />
                                        </div>
                                    </center>
                                </center>
                                <center>
                                    <asp:Label ID="err" ForeColor="Red" Visible="false" runat="server"></asp:Label></center>
                            </div>
                        </div>
                    </div>
                    <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                height: auto; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_description11" runat="server" Text="Description"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_narrotion" runat="server" Height="25px" onfocus=" return display(this)"
                                                Style="text-transform: capitalize;" CssClass="textbox textbox1" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <br />
                                    <tr>
                                        <td align="center">
                                            <br />
                                            <asp:Button ID="btn_addnarrotion" runat="server" Visible="true" CssClass="textbox textbox1"
                                                Text="Add" OnClientClick="return checkadd2()" OnClick="btnnarrotion_Click" />
                                            <asp:Button ID="btn_exitdesc1" runat="server" Visible="true" CssClass="textbox textbox1"
                                                Text="Exit" OnClick="btndescpopexit_Click" />
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </div>
            </center>
        </div>
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
                                    <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </body>
    </html>
</asp:Content>
