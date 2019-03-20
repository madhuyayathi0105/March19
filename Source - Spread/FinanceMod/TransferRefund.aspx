<%@ Page Title="" Language="C#" MasterPageFile="~/Financemod/FinanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TransferRefund.aspx.cs" Inherits="TransferRefund" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title>Transfer/Refund</title>
    <link rel="Shortcut Icon" href="college/Left_Logo.jpeg" />
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
        .txtcaps
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .maindivstyle
        {
            border: 1px solid #999999;
            background-color: #F0F0F0;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            -moz-box-shadow: 0px 0px 10px #999999;
            -webkit-box-shadow: 0px 0px 10px #999999;
            border: 3px solid #D9D9D9;
            border-radius: 15px;
        }
        .subdivstyle
        {
            border: 5px solid #0CA6CA;
            border-top: 30px solid #0CA6CA;
            border-radius: 10px;
        }
        .maindivstyle1
        {
            border: 1px solid #999999;
            border-radius: 15px;
        }
    </style>
    <body>
        <script type="text/javascript">

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }


            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=816,width=980');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} .classReg12 {   font-size:14px; } </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }



            function checkpaidamount() {

                var totpaidval = 0.00;
                var totbalamt = 0.00;
                var txttotaltobepaid = document.getElementById("<%=lbl_grid2_paid.ClientID %>");
                var txtbalanceamount = document.getElementById("<%=lbl_grid2_bal.ClientID %>");


                var tbl = document.getElementById("<%=gridView2.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 0; i < (gridViewControls.length); i++) {

                    var lbltotal = document.getElementById('MainContent_gridView2_lbl_totamt_' + i.toString());
                    var txtbal = document.getElementById('MainContent_gridView2_txt_bal_' + i.toString());
                    var txtexcess = document.getElementById('MainContent_gridView2_txt_exGrid2_' + i.toString());
                    var txtpaid = document.getElementById('MainContent_gridView2_txt_paid_' + i.toString());


                    if (txtbal != null && txtexcess != null && txtpaid != null && lbltotal != null) {


                        if (txtpaid.value != "") {
                            totpaidval += parseFloat(txtpaid.value);

                            if (txtbal.value != "" && lbltotal.innerHTML != "") {
                                totbalamt += parseFloat(lbltotal.innerHTML) - parseFloat(txtpaid.value);
                                txtbal.value = parseFloat(lbltotal.innerHTML) - parseFloat(txtpaid.value);
                            }
                        }

                    }

                }
                txttotaltobepaid.innerHTML = "Rs." + totpaidval.toString();
                txtbalanceamount.innerHTML = "Rs." + totbalamt.toString();

            }
            function checkrefundamount() {
                var totrefund = 0.00;
                var totalgetvalue = 0.00;
                var txttotalrefamt = document.getElementById("<%=txt_AmtPerc.ClientID %>");
                var ddlPercAmt = document.getElementById("<%=ddl_AmtPerc.ClientID %>");


                var tbl = document.getElementById("<%=gridView3.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 0; i < (gridViewControls.length); i++) {

                    var lbltotal = document.getElementById('MainContent_gridView3_lbl_totamt_' + i.toString());
                    var lblbal = document.getElementById('MainContent_gridView3_lbl_bal_' + i.toString());
                    var lblpaid = document.getElementById('MainContent_gridView3_lbl_paid_' + i.toString());
                    var lblAlrefund = document.getElementById('MainContent_gridView3_lbl_AlRefunded_' + i.toString());
                    var txtrefund = document.getElementById('MainContent_gridView3_txt_refund_' + i.toString());
                    var txtrefundbal = document.getElementById('MainContent_gridView3_txt_refundbal_' + i.toString());
                    if (lblpaid != null && txtrefund != null) {
                        if (lblpaid.innerHTML != "" && txtrefund.value != "") {

                            if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {
                                totrefund += parseFloat(txtrefund.value);

                                if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {
                                    txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                }
                                else {
                                    txtrefundbal.value = "0";
                                }
                            }
                            else {

                                txtrefundbal.value = parseFloat(txtrefund.value) - (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                totalgetvalue += parseFloat(txtrefundbal.value);
                                txtrefund.value = "0";

                            }
                        }
                        else {
                            txtrefundbal.value = "0";
                        }

                    }

                }

                txttotalrefamt.value = totrefund.toString();
                document.getElementById("<%=txt_reamt.ClientID %>").value = totalgetvalue;

            }


            function refundamt() {
                var totrefund = parseFloat(document.getElementById("<%=txt_AmtPerc.ClientID %>").value);

                var txttotalrefamt = document.getElementById("<%=txt_AmtPerc.ClientID %>");
                var ddlPercAmt = document.getElementById("<%=ddl_AmtPerc.ClientID %>");

                var tbl = document.getElementById("<%=gridView3.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                var totalgetvalue = 0;
                for (var i = 0; i < (gridViewControls.length); i++) {

                    var lbltotal = document.getElementById('MainContent_gridView3_lbl_totamt_' + i.toString());
                    var lblbal = document.getElementById('MainContent_gridView3_lbl_bal_' + i.toString());
                    var lblpaid = document.getElementById('MainContent_gridView3_lbl_paid_' + i.toString());
                    var lblAlrefund = document.getElementById('MainContent_gridView3_lbl_AlRefunded_' + i.toString());
                    var txtrefund = document.getElementById('MainContent_gridView3_txt_refund_' + i.toString());
                    var txtrefundbal = document.getElementById('MainContent_gridView3_txt_refundbal_' + i.toString());

                    if (lblpaid != null && txtrefund != null) {
                        if (lblpaid.innerHTML != "") {
                            if (ddlPercAmt.value == "Amount") {

                                if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) <= totrefund) {
                                    totrefund -= (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                    txtrefund.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)).toString();
                                    if ((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) >= parseFloat(txtrefund.value)) {

                                        txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                        totalgetvalue += parseFloat(txtrefundbal.value);
                                    }



                                }
                                else {
                                    txtrefund.value = totrefund.toString();
                                    txtrefundbal.value = parseFloat(txtrefund.value) - (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML));
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                    totrefund = 0;
                                }

                            }
                            else {
                                if (totrefund >= 0 && totrefund < 101) {
                                    txtrefund.value = (((parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) * totrefund) / 100).toString();
                                    txtrefundbal.value = (parseFloat(lblpaid.innerHTML) - parseFloat(lblAlrefund.innerHTML)) - parseFloat(txtrefund.value);
                                    totalgetvalue += parseFloat(txtrefundbal.value);
                                }
                                else {
                                    // totalgetvalue += parseFloat(txtrefundbal.value);
                                    txtrefundbal.value = "0";
                                    txtrefund.value = "0";
                                }

                            }

                        }
                    }

                }
                document.getElementById("<%=txt_reamt.ClientID %>").value = totalgetvalue;

            }


        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Transfer/Refund</span></div>
            </center>
        </div>
        <center>
            <div style="width: 1000px; height: auto;">
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_indexChanged"
                                CssClass="textbox ddlstyle ddlheight3" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table class="maintablestyle" style="width: 1010px; text-align: left;">
                    <tr>
                        <td style="width: 380px;">
                            <div>
                                <asp:RadioButton ID="rb_transfer" runat="server" Text="Transfer" GroupName="s1" Checked="true"
                                    OnCheckedChanged="rb_transfer_Change" AutoPostBack="true" />
                                <asp:RadioButton ID="rb_discont" runat="server" Text="Discontinue" GroupName="s1"
                                    OnCheckedChanged="rb_discont_Change" AutoPostBack="true" />
                                <asp:RadioButton ID="rb_refund" runat="server" Text="Refund" GroupName="s1" OnCheckedChanged="rb_refund_Change"
                                    AutoPostBack="true" />
                                <asp:CheckBox ID="cbdisWithoutFees" Visible="false" runat="server" Text="Without Fees" />
                            </div>
                        </td>
                        <td style="width: 250px;">
                            <asp:RadioButtonList ID="rbl_AdmitTransfer" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbl_AdmitTransfer_OnSelectedIndexChanged">
                                <asp:ListItem Selected="True">Applied</asp:ListItem>
                                <asp:ListItem>Not Applied</asp:ListItem>
                            </asp:RadioButtonList>
                            <td style="width: 250px;">
                                <asp:RadioButtonList ID="rbl_TranSngMul" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbl_TranSngMul_OnSelectedIndexChanged">
                                    <asp:ListItem Selected="True">Single</asp:ListItem>
                                    <asp:ListItem>Multiple</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RadioButtonList ID="rbl_EnrollRefund" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="rbl_EnrollRefund_OnSelectedIndexChanged">
                                    <asp:ListItem Selected="True">Enrolled</asp:ListItem>
                                    <asp:ListItem>Not Enrolled</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td id="tdadmis" runat="server" visible="false" style="width: 250px;">
                                <asp:Label ID="lbldrop" runat="server" Text="Include"></asp:Label>
                                <asp:DropDownList ID="ddladmis" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddladmis_Selected">
                                    <asp:ListItem Text="Before Admission" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="After Admission" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </td>
                        <td style="width: 80px;">
                            <asp:RadioButtonList ID="rb_admit" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table"
                                AutoPostBack="true" OnSelectedIndexChanged="rb_admit_OnSelectedIndexChanged">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" id="trnsledgmap" runat="server" visible="true">
                            <asp:CheckBox ID="cbledgmapp" runat="server" Text="Use Ledger Mapping" />
                            <%-- </td>
                    <td colspan="2">--%>
                            <asp:LinkButton ID="lnkledgmap" runat="server" Text="Ledger Mapping Settings" OnClick="lnkledgmap_Click"></asp:LinkButton>
                            <asp:CheckBox ID="cbwithoutfees" runat="server" Text="With Out Fees" />
                        </td>
                        <td>
                            <%--<asp:CheckBox ID="cbwithoutfees" runat="server" Text="With Out Fees" />--%>
                        </td>
                    </tr>
                </table>
                <br />
                <%--******1st div*******--%>
                <div id="div_transfer" runat="server">
                    <div style="width: 900px">
                        <div id="div1" style="float: left">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rollno" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="rbl_rollno_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll" runat="server" CssClass="textbox txtheight4 textbox1"
                                                OnTextChanged="txt_roll_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_roll"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="txt_reg" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span> <span id="rnomsg"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_name" runat="server" CssClass="textbox txtheight6 textbox1"
                                                onblur="getname(this.value)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                CompletionSetCount="10" ServicePath="" TargetControlID="txt_name" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Date of Transfer
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Updp_date" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_date" runat="server" CssClass="textbox txtheight2 textbox2"
                                                        Width="100px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <div style="float: left; margin-left: 180px;">
                            <asp:Image ID="image2" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                width: 100px;" />
                        </div>
                    </div>
                    <%--*******2nd div*****--%>
                    <br />
                    <div style="width: 900px">
                        <div id="div2" style="float: left">
                            <fieldset style="height: 296px; width: 332px; border: 1px solid #999999;">
                                <legend>From</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_colg" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempfstclg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str1" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_strm" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_batch" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempfstdeg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsem" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sec" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_seattype" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <br />
                            <asp:LinkButton ID="lnkindivmap" runat="server" Enabled="false" Text="Individual Mapping"
                                OnClick="lnkindivmap_Click"></asp:LinkButton>
                        </div>
                        <div style="float: left; margin-left: 150px;">
                            <fieldset id="todivnotAdmit" runat="server" style="height: 350px; width: 250px; border: 1px solid #999999;">
                                <legend>To</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclgs" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_colg" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_colg_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str2" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_strm" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_strm_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_batch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldegs" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_degree" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_degree_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldepts" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_dept" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsems" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_sem" runat="server" CssClass="textbox ddlheight1" Width="80px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_sem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_sec" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddl_sec_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_seattype" runat="server" CssClass="textbox ddlheight1"
                                                Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Roll No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll_no" runat="server" CssClass="txtheight2 txtcaps" MaxLength="25"
                                                OnTextChanged="txt_roll_noNotApp_TextChanged" AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_roll_no"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset id="todivAdmit" runat="server" style="height: 350px; width: 250px; border: 1px solid #999999;">
                                <legend>To</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblrollno1" runat="server" Text="ApplicationNo"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll1" runat="server" CssClass="textbox txtheight4 textbox1"
                                                OnTextChanged="txt_roll1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_roll1"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender21" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="GetAppFormno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_roll1"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblclgss" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_colg1" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempsndclg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_str3" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_strm1" runat="server" CssClass="txtheight3 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_batch1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldegss" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_degree1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lblDegCode" runat="server" Visible="false" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldeptss" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_dept1" runat="server" CssClass="txtheight5 txtcaps">
                                            </asp:TextBox>
                                            <asp:Label ID="lbltempsnddeg" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsemss" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sem1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_sec1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Seat Type
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_seat_type1" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Roll No
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_roll_no1" runat="server" CssClass="txtheight2 txtcaps" MaxLength="25"
                                                OnTextChanged="txt_roll_noApp_TextChanged" AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="fteTxtR1" runat="server" TargetControlID="txt_roll_no1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                    <br />
                    <%--*****3rd div******--%>
                    <br />
                    <div style="float: left; width: 930px; padding-left: 20px;">
                        <br />
                        <div id="div_gridView1" runat="server" visible="false" style="float: left; overflow: auto;
                            border-radius: 10px; border: 1px solid Gray; width: 450px; height: 200px;">
                            <div style="height: 180px; overflow: auto;">
                                <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                    OnRowDataBound="gridView1_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        <asp:Label ID="lblpaym" runat="server" Visible="false" Text='<%#Eval("paymode") %>'></asp:Label>
                                                    </asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                    <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                    </asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                </asp:Label>
                                                <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                </asp:Label>
                                                <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <table id="tblgrid1" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            Total :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid1_tot" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Paid :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid1_paid" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Balance :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid1_bal" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <span style="float: left; display: block;">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <div id="div_gridView2" runat="server" visible="false" style="float: left; overflow: auto;
                            border-radius: 10px; border: 1px solid Gray; width: 460px; height: 200px;">
                            <div style="height: 180px; overflow: auto;">
                                <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                    OnRowDataBound="gridView2_OnRowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                    </asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                    <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                    </asp:Label>
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                </asp:Label>
                                                <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                </asp:Label>
                                                <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_paid" runat="server" placeholder="0.00" CssClass="  textbox txtheight"
                                                    Height="15px" Width="60px" onchange="return checkpaidamount()" Style="text-align: right;"></asp:TextBox></center>
                                                <asp:FilteredTextBoxExtender ID="filterextender21" runat="server" TargetControlID="txt_paid"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_bal" runat="server" placeholder="0.00" onchange="return checkpaidamount()"
                                                    ReadOnly="true" CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                <asp:FilteredTextBoxExtender ID="filterextender22" runat="server" TargetControlID="txt_bal"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Excess" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_exGrid2" runat="server" placeholder="0.00" onchange="return checkpaidamount()"
                                                    ReadOnly="true" CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                <asp:FilteredTextBoxExtender ID="filterextendereex22" runat="server" TargetControlID="txt_exGrid2"
                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <table id="tblgrid2" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            Total :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid2_tot" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Paid :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid2_paid" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Balance :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid2_bal" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            Excess :
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_grid2_excess" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            UnMatchedExcess Amt:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblunmtexcess" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <br />
                        <br />
                        <table id="tblbtmhd" runat="server" visible="false">
                            <tr>
                                <td>
                                    Header
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_trheader" runat="server" CssClass="textbox ddlheight4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_trheader_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Ledger
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_trledger" runat="server" CssClass="textbox ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Transfer Amount
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_tramt" runat="server" placeholder="0.00" BackColor="#81F7D8"
                                        CssClass="txtheight txtcaps" Style="text-align: right;">
                                    </asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtendertr4" runat="server" TargetControlID="txt_tramt"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_transfer" runat="server" CssClass="textbox btn btn2" Text="Transfer"
                                        OnClick="btn_transfer_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="div_transfermulti" runat="server" visible="false">
                    <div style="width: 900px; float: left;">
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege1" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_stream1" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_strm1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_strm1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch1" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_batch1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_batch1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree1" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_degree1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_degree1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_branch1" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_branch1" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_branch1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sem1" runat="server" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_sem1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_sem1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sec1" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stType1" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_stType1_OnIndexChange">
                                        <asp:ListItem>Applied</asp:ListItem>
                                        <asp:ListItem>Shortlisted</asp:ListItem>
                                        <asp:ListItem>Wait to admit</asp:ListItem>
                                        <asp:ListItem>Admitted</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go1" Text="Go" OnClick="btn_go1_Click" CssClass="textbox btn1 textbox1"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div>
                            <asp:Label ID="lbl_errormsg1" Visible="false" Font-Bold="true" runat="server" Text=""
                                ForeColor="Red"></asp:Label>
                            <asp:Label ID="lbl_Total1" Visible="false" Font-Bold="true" runat="server" Text=""
                                ForeColor="Red"></asp:Label>
                        </div>
                        <div>
                            <FarPoint:FpSpread ID="spreadStudAdd" runat="server" Visible="false" ShowHeaderSelection="false"
                                OnUpdateCommand="spreadStudAdd_Command" BorderWidth="0px" Width="850px" Style="overflow: auto;
                                height: 300px; border: 0px solid #999999; border-radius: 10px; background-color: White;
                                box-shadow: 0px 0px 8px #999999;">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#F7BE81" SelectionPolicy="Single">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                        <br />
                        <table class="maintablestyle" id="tblToTransMulti" runat="server" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_stream" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstrm" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_strm_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_batch1_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_degree_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_branch" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_branch" Width="200px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_branch_OnIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Sem" runat="server" Text="Semester"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsem" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <%-- <asp:Label ID="lbl_sec" runat="server" Text="Type"></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stType" Width="100px" Height="30px" runat="server" CssClass="textbox ddlheight"
                                        Visible="false">
                                        <asp:ListItem>Applied</asp:ListItem>
                                        <asp:ListItem>Shortlisted</asp:ListItem>
                                        <asp:ListItem>Wait to admit</asp:ListItem>
                                        <asp:ListItem>Admitted</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btn_TransferMulti" runat="server" CssClass="textbox btn2 textbox1"
                                        Text="Transfer" OnClick="btn_TransferMulti_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%--**********end of transfer div**************--%>
                <center>
                    <div id="div_refund" runat="server" visible="false">
                        <div style="width: 900px">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="rbl_rerollno" runat="server" CssClass="textbox  ddlheight"
                                                AutoPostBack="true" OnSelectedIndexChanged="rbl_rerollno_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rerollno" runat="server" AutoPostBack="true" CssClass="textbox txtheight4 textbox1"
                                                OnTextChanged="txt_rerollno_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rerollno"
                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" ">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rerollno"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                            <asp:TextBox ID="TextBox2" runat="server" Style="display: none" CssClass="textbox txtheight4 textbox1"
                                                onchange="return checkrno(this.value)" onkeyup="return checkrno(this.value)"
                                                onblur="return get(this.value)" onfocus="return myFunction(this)"></asp:TextBox>
                                            <span style="color: Red;">*</span> <span id="Span1"></span>
                                        </td>
                                        <td>
                                            Date
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_rdate" runat="server" CssClass="textbox txtheight textbox2"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_rdate" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td rowspan="5">
                                            <asp:Image ID="image3" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 130px;
                                                width: 100px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Name
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_rename" runat="server" CssClass="textbox txtheight6 textbox1"
                                                onblur="getname(this.value)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rename"
                                                FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rename"
                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListItemCssClass="panelbackground">
                                            </asp:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcoll" runat="server" Text="College"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_recolg" runat="server" CssClass="txtheight6 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Batch
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_rebatch" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_str4" runat="server" Text=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_restrm" runat="server" CssClass="txtheight1 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldegre" runat="server" Text="Degree"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_redegree" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                            &nbsp;
                                            <asp:Label ID="lbldeptms" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txt_redept" runat="server" CssClass="txtheight4 txtcaps">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsemests" runat="server" Text="Semester"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_resem" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            Section
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_resec" runat="server" CssClass="txtheight txtcaps">
                                            </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_AmtPerc" runat="server" CssClass="textbox ddlheight" BackColor="#81F7D8"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_AmtPerc_OnSelectedIndexChanged">
                                                <asp:ListItem Selected="True">Amount</asp:ListItem>
                                                <asp:ListItem>Percent</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txt_AmtPerc" runat="server" BackColor="#81F7D8" CssClass="txtheight textbox"
                                                placeholder="0.00" Style="text-align: right" onchange="return refundamt()">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender44" runat="server" TargetControlID="txt_AmtPerc"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CheckBox ID="chk_refCommon" runat="server" AutoPostBack="true" OnCheckedChanged="chk_refCommon_OnCheckedChanged"
                                                Text="Common" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                        <%--*******2nd div*****--%>
                        <div style="float: left; width: 900px;">
                            <center>
                                <div style="border-radius: 10px; border: 1px solid Gray; width: 900px; height: 200px;
                                    overflow: auto;">
                                    <%-- <div style="float: right; padding-right: 20px;">
                                <asp:LinkButton ID="lnkbtn_viewhistory" runat="server" Visible="true" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" CausesValidation="False"
                                    OnClick="lnkbtn_viewhistory_Click">Fees Paid History</asp:LinkButton>
                            </div>
                            <br />--%>
                                    <div style="height: 170px; overflow: auto;">
                                        <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                            OnRowDataBound="gridView3_OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Width="">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                            <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                            </asp:Label>
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                        </asp:Label>
                                                        <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                        </asp:Label>
                                                        <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Already Refunded" HeaderStyle-BackColor="#0CA6CA"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_AlRefunded" runat="server" Text='<%#Eval("RefundAmt") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Refund Taken" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_refund" runat="server" placeholder="0.00" onchange="return checkrefundamount()"
                                                            CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextender2re2" runat="server" TargetControlID="txt_refund"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Refund" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_refundbal" runat="server" placeholder="0.00" CssClass=" textbox txtheight"
                                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderrebal" runat="server" TargetControlID="txt_refundbal"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div>
                                        <table id="tblgrid3" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    Total :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_tot" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Paid :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_paid" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td>
                                                    Balance :
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_grid3_bal" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td style="text-align: right;">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkbtn_viewhistory" runat="server" Visible="false" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" CausesValidation="False"
                                                        OnClick="btnHistory_Click">Fees Paid History</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_hed" runat="server" Visible="false" Text="Header"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_refheader" Visible="false" runat="server" CssClass="textbox ddlheight4"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_refheader_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_led" runat="server" Visible="false" Text="Ledger"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_refledger" Visible="false" runat="server" CssClass="textbox ddlheight4">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Refund Amount
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_reamt" runat="server" CssClass="txtheight txtcaps" placeholder="0.00"
                                                BackColor="#81F7D8" Style="text-align: right">
                                            </asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterextenderre3" runat="server" TargetControlID="txt_reamt"
                                                FilterType="Numbers,Custom" ValidChars=".">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_refund" runat="server" CssClass="textbox btn btn2" Text="Refund"
                                                OnClick="btn_refund_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </div>
                </center>
                <%--***********end of refund div*********--%>
            </div>
        </center>
        <%-- Popup  History--%>
        <center>
            <div id="div_History" runat="server" visible="false" class="popupstyle popupheight1 ">
                <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 75px; margin-left: 410px;"
                    OnClick="imagebtnpopHistclose_Click" />
                <br />
                <br />
                <br />
                <br />
                <div style="background-color: White; height: 500px; width: 850px; border: 5px solid #0CA6CA;
                    border-top: 30px solid #0CA6CA; border-radius: 10px;">
                    <br />
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: Green;">Student Fee Status Report</span></div>
                    </center>
                    <br />
                    <table class="maintablestyle" style="width: 300px; text-align: left;">
                        <tr>
                            <td>
                                <asp:Label ID="lblheadr3" runat="server" Text="Header"></asp:Label>
                                <%--<asp:DropDownList ID="ddlheadr3" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true">                                                     </asp:DropDownList>--%>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtheadr3" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight1">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cbheadr3" runat="server" OnCheckedChanged="cbheadr3_ChekedChange"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblheadr3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblheadr3_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtheadr3"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbllgr3" runat="server" Text="Ledger"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtlgr3" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                            Width="120px">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cblgr3" runat="server" OnCheckedChanged="cblgr3_ChekedChange" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbllgr3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_lgr3_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtlgr3"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnhisgo" Text="Go" OnClick="btnhisgo_Click" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" CssClass="textbox btn1" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <div id="div4" runat="server" style="width: 850px; height: 320px; overflow: auto;">
                            <asp:GridView ID="gridHist" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                Width="830px" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                            </asp:GridView>
                        </div>
                        <div style="padding-top: 5px; text-align: center;">
                            <%-- <asp:Button ID="Button2" runat="server" OnClick="btnpopLedgersave_Click"  Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Text="Save"
                                CssClass="textbox btn2" />--%>
                        </div>
                    </center>
                </div>
            </div>
        </center>
        <%--reuse rollno Confirmation --%>
        <center>
            <div id="divReuseRoll" runat="server" visible="false" style="height: 100%; z-index: 10000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div9" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label7" runat="server" Text="Do You Want To Reuse Roll Number?" Style="color: Red;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnReuseYes" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btnReuseYes_Click" Text="yes" runat="server" />
                                            <asp:Button ID="btnReuseNo" CssClass=" textbox btn1 textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btnReuseNo_Click" Text="no" runat="server" />
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
                                            <asp:Button ID="btn_alertclose" CssClass=" textbox btn1 comm" Style="height: 28px;
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
        <%--ledgermapping--%>
        <center>
            <div id="divledger" runat="server" visible="false" style="height: 100%; z-index: 100%;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div6" runat="server" class="table" style="background-color: White; height: 550px;
                        width: 900px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 104px;
                        border-radius: 10px;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: -30px; margin-left: 427px;" OnClick="imagebtnpopclose1_Click" />
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td colspan="8">
                                        <br />
                                        <center>
                                            <span style="font-weight: bold; font-family: Book Antiqua; font-size: large; color: Green;">
                                                Ledger Mapping</span>
                                        </center>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>From College</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlfrclg" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlfrclg_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <span>From Ledger</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlfrledg" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlfrledg_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%--OnSelectedIndexChanged="ddlfrledg_OnSelectedIndexChanged"--%>
                                    </td>
                                    <td>
                                        <span>To College</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddltoclg" runat="server" CssClass="textbox  ddlheight" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddltoclg_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txttoledg" runat="server" ReadOnly="true" Height="20px" CssClass="textbox textbox1"
                                                    Width="120px">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                                    <asp:CheckBox ID="cbtoledg" runat="server" OnCheckedChanged="cbtoledg_ChekedChange"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                    <asp:CheckBoxList ID="cbltoledg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbltoledgSelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttoledg"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnledggo" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="btnledggo_Click" Text="Go" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <br />
                                        <center>
                                            <fieldset style="border-radius: 10px; width: 500px;">
                                                <%-- <legend style="font-size: larger; font-weight: bold">Application Header Settings</legend>--%>
                                                <table class="table">
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="lstfrom" runat="server" SelectionMode="Multiple" Height="300px"
                                                                Width="200px"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <table class="table1">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneRt" runat="server" Text=">" CssClass="textbox textbox1 btn1"
                                                                            OnClick="btnMvOneRt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoRt" runat="server" Text=">>" CssClass="textbox textbox1 btn1"
                                                                            OnClick="btnMvTwoRt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvOneLt" runat="server" Text="<" CssClass="textbox textbox1 btn1"
                                                                            OnClick="btnMvOneLt_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnMvTwoLt" runat="server" Text="<<" CssClass="textbox textbox1 btn1"
                                                                            OnClick="btnMvTwoLt_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="lstto" runat="server" SelectionMode="Multiple" Height="300px" Width="200px">
                                                            </asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </fieldset>
                                        </center>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <center>
                                            <asp:Button ID="btnledgsave" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnledgsave_Click" Text="ok" runat="server" />
                                            <asp:Button ID="btnledgcancel" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btnledgcancel_Click" Text="Cancel" runat="server" />
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
            <div id="divindi" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54,25,25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="divind" runat="server" class="table" style="background-color: White; height: 650px;
                        width: 1000px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 104px;
                        border-radius: 10px;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: -30px; margin-left: -46px;" OnClick="ImageButton2_Click" />
                        <div style="float: left; width: 1000px; padding-left: 20px;">
                            <span style="font-family: Book Antiqua; font-weight: bold; color: Green; font-size: large;">
                                Individual Mapping </span>
                            <br />
                            <br />
                            <div id="div5" style="float: left; overflow: auto; border-radius: 10px; border: 1px solid Gray;
                                width: 460px; height: 488px;">
                                <div style="height: 450px; overflow: auto;">
                                    <asp:GridView ID="gridView4" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnRowDataBound="gridView4_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:CheckBox ID="cbsel" runat="server" />
                                                        <asp:Label ID="lblpaymode" runat="server" Visible="false" Text='<%#Eval("paymode") %>'></asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                        <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_paid" runat="server" Text='<%#Eval("Paid") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_bal" runat="server" Text='<%#Eval("Balance") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <table id="Table1" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <span style="float: left; display: block;">&nbsp;&nbsp;&nbsp;&nbsp;</span>
                            <div style="float: left; overflow: auto; border-radius: 10px; border: 1px solid Gray;
                                width: 460px; height: 488px;">
                                <div style="height: 450px; overflow: auto;">
                                    <asp:GridView ID="gridView5" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                        OnRowDataBound="gridView5_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                HeaderStyle-Width="">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_sno" runat="server" Text='<%#Eval("Sno") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:CheckBox ID="cblsell" runat="server" />
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year/Semester" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:Label ID="lbl_yearsem" runat="server" Text='<%#Eval("YearSem") %>'></asp:Label>
                                                        <asp:Label ID="lbl_feecat" runat="server" Visible="false" Text='<%#Eval("FeeCategory") %>'>
                                                        </asp:Label>
                                                    </center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Header" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_hdr" runat="server" Text='<%#Eval("Header") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_hdrid" runat="server" Visible="false" Text='<%#Eval("HeaderFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_lgr" runat="server" Text='<%#Eval("Ledger") %>'>
                                                    </asp:Label>
                                                    <asp:Label ID="lbl_lgrid" runat="server" Visible="false" Text='<%#Eval("LedgerFk") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_feeamt" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_totamt" runat="server" Text='<%#Eval("FeeAmt") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_Concess" runat="server" Text='<%#Eval("Concession") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Paid" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_paid" runat="server" placeholder="0.00" CssClass="  textbox txtheight"
                                                        Height="15px" Width="60px" onchange="return checkpaidamount()" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextender21" runat="server" TargetControlID="txt_paid"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_bal" runat="server" placeholder="0.00" onchange="return checkpaidamount()"
                                                        ReadOnly="true" CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextender22" runat="server" TargetControlID="txt_bal"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Excess" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_exGrid2" runat="server" placeholder="0.00" onchange="return checkpaidamount()"
                                                        ReadOnly="true" CssClass="  textbox txtheight" Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                                    <asp:FilteredTextBoxExtender ID="filterextendereex22" runat="server" TargetControlID="txt_exGrid2"
                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div>
                                    <table id="Table2" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Total :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Paid :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Balance :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                Excess :
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="tdunex" runat="server" visible="false">
                                                UnMatchedExcess Amt:
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td colspan="5">
                                        <asp:Button ID="btnadjust" runat="server" CssClass="textbox btn btn2" Text="Mapping"
                                            OnClick="btnadjust_Click" Font-Bold="true" />
                                        <asp:Button ID="btnmapreset" runat="server" CssClass="textbox btn btn2" Text="Reset"
                                            OnClick="btnmapreset_Click" Font-Bold="true" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        Header
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlhedind" runat="server" CssClass="textbox ddlheight4" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlhedind_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Ledger
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlledind" runat="server" CssClass="textbox ddlheight4">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Transfer Amount
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtamtind" runat="server" placeholder="0.00" BackColor="#81F7D8"
                                            CssClass="txtheight txtcaps" Style="text-align: right;">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtamtind"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btntransind" runat="server" Enabled="false" CssClass="textbox btn btn2"
                                            Text="Transfer" OnClick="btntransind_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </center>
            </div>
        </center>
        <%--alert--%>
        <center>
            <div id="divalert" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div8" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
        <%--yes or no--%>
        <center>
            <div id="div7" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div10" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 287px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label10" runat="server" Text="Do You Want Transfer This Amount" Style="color: Green;"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="Label11" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="Label24" runat="server" Style="color: Green;" Font-Bold="true" Font-Size="Medium"
                                                Text="Excess/Advance:"></asp:Label>
                                            <%-- </td>
                                    <td>--%>
                                            <asp:Label ID="Label24ex" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="buttonok" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="buttonok_Click" Text="OK" runat="server" />
                                            <asp:Button ID="btncancel" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btncancel_Click" Text="Cancel" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
        <%--transfer--%>
        <center>
            <div id="div11" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div12" runat="server" class="table" style="background-color: White; height: 195px;
                        width: 394px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Label ID="Label12" runat="server" Text="Don't Have Paid Amount,Do You Want Continue"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label14" runat="server" Visible="false" Text="Total Amount" Style="color: Gray;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label15" runat="server" Text="" Visible="false" Style="color: Green;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label16" runat="server" Text="Paid Amount" Visible="false" Style="color: Gray;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label17" runat="server" Text="" Visible="false" Style="color: Green;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label18" runat="server" Text="Balance Amount" Visible="false" Style="color: Gray;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label19" runat="server" Text="" Style="color: Green;" Visible="false"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label20" runat="server" Text="Excess Amount" Visible="false" Style="color: Gray;"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label21" runat="server" Text="" Style="color: Green;" Visible="false"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label22" runat="server" Text="Unmatched Excess Amount" Visible="false"
                                                        Style="color: Gray;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label23" runat="server" Text="" Style="color: Green;" Visible="false"
                                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<center>
                                        <asp:Label ID="Label13" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </center>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <asp:Button ID="button1" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="button1_Click" Text="OK" runat="server" />
                                            <asp:Button ID="Button2" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="Button2_Click" Text="Cancel" runat="server" />
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
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </center>
    </body>
    </html>
</asp:Content>
